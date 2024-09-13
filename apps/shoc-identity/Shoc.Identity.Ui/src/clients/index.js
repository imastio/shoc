import AuthClient from "./auth-client";
import getApiConfig from "./client-config";
import SessionClient from "./session-client";

// build configuration
const config = getApiConfig();

export const sessionClient = new SessionClient(config);
export const authClient = new AuthClient(config);

export const clientGuard = async (fn) => {
    try{
        const result = await fn();
        return { payload: result.data, error: null };
    }
    catch(error){
        if(error && error.response && error.response.data){
            return { payload: error.response.data, error }
        }
        return { payload: { errors: [{ code: "UNKNOWN_ERROR", kind: 1}] }, error };
    }
}