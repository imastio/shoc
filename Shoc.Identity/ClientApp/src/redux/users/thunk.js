import { createAsyncThunk } from "@reduxjs/toolkit";
import { identityClient } from "api"
import { rejectable } from "redux/utility";

export const signup = createAsyncThunk(
    'users/signup',
    async ({ input }, thunkAPI) => await rejectable(thunkAPI, async () => {
        const response = await identityClient.signup(input)
        return response.data
    })
)

export const signin = createAsyncThunk(
    'users/signin',
    async ({ input }, thunkAPI) => await rejectable(thunkAPI, async () => {
        const response = await identityClient.signin(input)
        return response.data
    })
)

export const requestConfirmation = createAsyncThunk(
    'users/request-confirmation-code',
    async ({ input }, thunkAPI) => await rejectable(thunkAPI, async () => {
        const response = await identityClient.requestConfirmation(input)
        return response.data
    })
)

export const processConfirmation = createAsyncThunk(
    'users/confirm-account',
    async ({ input }, thunkAPI) => await rejectable(thunkAPI, async () => {
        const response = await identityClient.processConfirmation(input)
        return response.data
    })
)
export const signout = createAsyncThunk(
    'users/signout',
    async ({ input }, thunkAPI) => await rejectable(thunkAPI, async () => {
        const response = await identityClient.signout(input)
        return response.data
    })
)

