import { createSlice, PayloadAction, createAsyncThunk } from "@reduxjs/toolkit";

function delay(n: number){
    return new Promise(function(resolve){
        setTimeout(resolve,n*1000);
    });
}

export interface ConfigState {
    apiBaseUrl: string | null,
    status: 'idle' | 'loading' | 'failed' | 'succeeded',
}

const initialState: ConfigState = {
    apiBaseUrl: null,
    status: 'idle'
}

export const fetchConfig = createAsyncThunk('config/fetchConfig', async (): Promise<string> => {
    //fetch the config from the server:
    const response = await fetch('/config');
    const responseBody = await response.json();
    // TODO: remove later, keep in here to make sure state is loading in right order:
    await delay(2);
    return responseBody.apiUrl;
});

export const configSlice = createSlice({
    'name': 'config',
    initialState, 
    reducers: {
        configLoaded: (state, action: PayloadAction<string>) => {
            state.apiBaseUrl = action.payload;
        }
    },
    extraReducers: (builder) => {
        builder.addCase(fetchConfig.fulfilled, (state, action) => {
            return {
                ...state,
                status: 'succeeded',
                apiBaseUrl: <string> action.payload
            };
        })
    }
});

export const { configLoaded } = configSlice.actions;

export default configSlice.reducer;