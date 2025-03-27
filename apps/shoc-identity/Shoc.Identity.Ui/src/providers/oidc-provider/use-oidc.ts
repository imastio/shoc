import { useContext } from "react";
import OidcContext from "./oidc-context";

interface OidcContextType {
    progress: boolean,
    loginHint: string,
}

export default function useOidc(){
    return useContext<OidcContextType>(OidcContext);
}