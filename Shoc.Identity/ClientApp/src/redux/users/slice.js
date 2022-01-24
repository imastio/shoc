import { createSlice } from '@reduxjs/toolkit';
import * as thunk from './thunk' 

const initialState = { 
    signup: { progress: false },
    nothing: true
};

const slice = createSlice({
    name: 'users',  
    initialState,  
    reducers: {    
        doNothing: (state) => state.nothing = false
    },
    extraReducers: builder => {
    }
});

export const actions = {...slice.actions, ...thunk};
export const reducer = slice.reducer;