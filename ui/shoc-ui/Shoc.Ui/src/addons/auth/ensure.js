import { auth } from ".";
import ErrorDefinitions from "../error-handling/error-definitions";

export async function ensureAuthenticated(){
    const authn = await auth();

    if(!authn){
        throw ErrorDefinitions.notAuthenticated();
    }

    return authn;
}