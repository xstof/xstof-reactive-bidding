import { createSlice, PayloadAction, createAsyncThunk } from "@reduxjs/toolkit";
import { Auctions as auctionClient } from "../../biddingclient/Auctions";
import { start } from "repl";

export interface Lot {
    name: string,
    id: string,
    price: number
}

export interface Auction {
    name: string,
    id: string,
    category: string, 
    lots: Lot[]
}

export interface AuctionsState {
    selectedAuctionId: string | null,
    status: 'idle' | 'loading' | 'failed' | 'succeeded',
    auctions: Auction[]
}
  
const initialState: AuctionsState = {
    selectedAuctionId: null,
    status: 'idle',
    auctions: [
        {
            name: 'Art Auction',
            id: '0',
            category: 'Art',
            lots: [
                {
                    name: 'Medieval Painting',
                    id: '0',
                    price: 1.2
                },
                {
                    name: 'Late 19th Century Painting',
                    id: '1',
                    price: 1.3
                }
            ]
        },
        {
            name: 'Antiquity Auction',
            id: '1',
            category: 'Antiquity',
            lots: [
                {
                    name: 'Chair',
                    id: '0',
                    price: 2.2
                },
                {
                    name: 'Table',
                    id: '1',
                    price: 2.3
                }
            ]
        },
        {
            name: 'Fruits Auction',
            id: '2',
            category: 'Fruits',
            lots: [
                {
                    name: 'Apple',
                    id: '0',
                    price: 3.2
                },
                {
                    name: 'Banana',
                    id: '1',
                    price: 3.3
                }
            ]
        }
    ]
};

export const fetchAuctions = createAsyncThunk('auctions/fetchAuctions', async () => {

    // use swagger-generated typescript client, generated in /src/biddingclient/
    const client = new auctionClient({
        baseUrl: 'https://localhost:7294' // TODO GET THIS FROM CONFIG
    });

    const response = await client.getAuctions();
    const auctionsWithoutLots = response.data.map(auction => {
        return <Auction> {
            name: auction.name,
            id: auction.id,
            category: auction.category,
            lots: <Lot[]>[
            ]};
    });

    const auctions = await Promise.all(
        auctionsWithoutLots.map(async (auction) => {
            var lotsResponse = await client.getAuctionLots(auction.id);
            let lots = lotsResponse.data.map(lot => <Lot>{
                name: lot.name,
                id: lot.id,
                price: lot.price
            });
            auction.lots = lots;
            return auction;
        })
    );

   return auctions;
});

export const auctionsSlice = createSlice({
    'name': 'auctions',
    initialState, 
    reducers: {
        auctionSelected: (state, action: PayloadAction<string>) => {
            state.selectedAuctionId = action.payload;
        },
        auctionAdded: (state, action: PayloadAction<Auction>) => {
            state.auctions.push(action.payload);
        }
    },
    extraReducers: (builder) => {
        builder.addCase(fetchAuctions.fulfilled, (state, action) => {
            return {
                ...state,
                status: 'succeeded',
                auctions: action.payload
            };
        })
    }
});

export const { auctionSelected, auctionAdded } = auctionsSlice.actions;

export default auctionsSlice.reducer;