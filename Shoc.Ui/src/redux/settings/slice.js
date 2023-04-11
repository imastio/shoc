import { createSlice } from '@reduxjs/toolkit';

const initialState = {   
};

const slice = createSlice({
    name: 'settings',  
    initialState,  
    reducers: {    
    }
});

export const actions = {...slice.actions};
export const reducer = slice.reducer;