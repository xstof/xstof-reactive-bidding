import { createSlice } from "@reduxjs/toolkit";

export interface Lot {
    name: string,
    price: number
}

export interface Auction {
    name: string,
    category: string, 
    lots: Lot[]
}

export interface AuctionsState {
    auctions: Auction[]
}
  
const initialState: AuctionsState = {
    auctions: [
        {
            name: 'Art Auction',
            category: 'Art',
            lots: [
                {
                    name: 'Medieval Painting',
                    price: 1.2
                },
                {
                    name: 'Late 19th Century Painting',
                    price: 1.3
                }
            ]
        },
        {
            name: 'Antiquity Auction',
            category: 'Antiquity',
            lots: [
                {
                    name: 'Chair',
                    price: 2.2
                },
                {
                    name: 'Table',
                    price: 2.3
                }
            ]
        },
        {
            name: 'Fruits Auction',
            category: 'Fruits',
            lots: [
                {
                    name: 'Apple',
                    price: 3.2
                },
                {
                    name: 'Banana',
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
        // TODO
    }
});

export default auctionsSlice.reducer;