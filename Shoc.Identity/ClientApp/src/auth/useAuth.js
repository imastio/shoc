import React from "react";
import { AuthContext } from "./auth-context";

export const useAuth = () => {
    const context = React.useContext(AuthContext);

    if (!context) {
        throw new Error("AuthProvider context is undefined, please verify you are calling useAuth() as child of a <AuthProvider> component.");
    }

    return context;
};