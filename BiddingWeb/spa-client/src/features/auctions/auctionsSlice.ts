import { createSlice, PayloadAction } from "@reduxjs/toolkit";

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
    auctions: Auction[]
}
  
const initialState: AuctionsState = {
    selectedAuctionId: null,
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

export const auctionsSlice = createSlice({
    'name': 'auctions',
    initialState, 
    reducers: {
        selectAuction: (state, action: PayloadAction<string>) => {
            state.selectedAuctionId = action.payload;
        }
    }
});

export const { selectAuction } = auctionsSlice.actions;

export default auctionsSlice.reducer;