import { createSlice, PayloadAction, createAsyncThunk } from "@reduxjs/toolkit";

export interface ConfigState {
    apiBaseUrl: string | null,
    status: 'idle' | 'loading' | 'failed' | 'succeeded',
}

const initialState: ConfigState = {
    apiBaseUrl: null,
    status: 'idle'
}

export const fetchConfig = createAsyncThunk('config/fetchConfig', async () => {
    //fetch the config from the server:
    const response = await fetch('/config');
    const responseBody = await response.json();
    return responseBody.apiUrl;
});

export const configSlice = createSlice({
    'name': 'config',
    initialState, 
    reducers: {
        configLoaded: (state, action: PayloadAction<string>) => {
            state.apiBaseUrl = action.payload;
        }
    }
});

export const { configLoaded } = configSlice.actions;

export default configSlice.reducer;