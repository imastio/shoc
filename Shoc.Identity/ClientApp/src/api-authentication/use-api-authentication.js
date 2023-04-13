import { useContext } from "react";
import { ApiAuthenticationContext } from "./api-authentication-context";

export const useApiAuthentication = () => {
    const context = useContext(ApiAuthenticationContext);

    if (!context) {
        throw new Error("ApiAuthenticationContext context is undefined, please verify you are calling useApiAuthentication() as child of a <ApiAuthenticationProvider> component.");
    }

    return context;
};