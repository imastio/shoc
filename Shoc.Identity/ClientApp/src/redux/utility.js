
export const rejectable = async (thunkAPI, fn) => {
    try{
        return await fn()
    }
    catch(error){
        if(error && error.response && error.response.data){
            return thunkAPI.rejectWithValue(error.response.data)
        }
        return thunkAPI.rejectWithValue(error);
    }
}

