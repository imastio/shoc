import getApiConfig from "./client-config";
import CurrentUserClient from "./current-user-client";

// build configuration
const config = getApiConfig();

export const currentUserClient = new CurrentUserClient(config);

export const clientGuard = async (fn) => {
    try{
        const result = await fn();
        return { payload: result.data, error: null };
    }
    catch(error){
        console.log("something is wrong", error)
        if(error && error.response && error.response.data){
            return { payload: error.response.data, error }
        }
        return { payload: { errors: [{ code: "UNKNOWN_ERROR", kind: 1}] }, error };
    }
}