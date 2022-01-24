import React, { useCallback } from "react";
import { AuthContext } from "./auth-context";
import authReducer from "./auth-reducer";

const authState = {
    user: null,
    isLoading: true,
    isAuthenticated: false,
    signout: 'no',
    sessionChanged: null,
    autoSignin: false,
};

export const AuthProvider = (props) => {

    const {children, userManager } = props;
    const [state, dispatch] = React.useReducer(authReducer, authState);

    const setAutoSignin = useCallback(() => autoSignin => {
        dispatch({ type: "toggle_autosignin", autoSignin });
    }, [dispatch])

    const processSignout = useCallback((signout) => {
        dispatch({ type: "sign_out", signout })
    }, [dispatch])

    React.useEffect(() => {
        if (!userManager) {
            return;
        }

        void (async () => {
            try {
                const user = await userManager.getUser();
                dispatch({ type: "init", user });
            } catch (error) {
                dispatch({ type: "error", error: error });
            }
        })();
    }, [userManager]);


    const silentSignin = useCallback(() => {

        if(!state.autoSignin){
            return;
        }

        userManager.signinSilent().then(user => {
            dispatch({type: "loaded", user})
        }).catch(error => dispatch({type: "error", error}))
    }, [userManager, dispatch, state.autoSignin])


    // register to user manager events
    React.useEffect(() => {
        
        // event UserLoaded (e.g. initial load, silent renew success)
        const handleUserLoaded = (user) => {
            dispatch({ type: "loaded", user });
        };

        // event UserUnloaded (e.g. userManager.removeUser)
        const handleUserUnloaded = () => {
            dispatch({ type: "unloaded" });
        };
       
        // event SilentRenewError (silent renew error)
        const handleSilentRenewError = (error) => {
            dispatch({ type: "error", error });
        };

        // event session changed
        const handleSessionChanged = () => {
        };

        // event signed out
        const handleUserSignedOut = () => {
            processSignout("pending")
        };

        const handleAccessTokenExpiring = () => {
            silentSignin()
        }

        userManager.events.addUserLoaded(handleUserLoaded);
        userManager.events.addUserUnloaded(handleUserUnloaded);
        userManager.events.addSilentRenewError(handleSilentRenewError);
        userManager.events.addUserSignedOut(handleUserSignedOut)
        userManager.events.addUserSessionChanged(handleSessionChanged);
        userManager.events.addAccessTokenExpiring(handleAccessTokenExpiring);

        return () => {
            userManager.events.removeUserLoaded(handleUserLoaded);
            userManager.events.removeUserUnloaded(handleUserUnloaded);
            userManager.events.removeSilentRenewError(handleSilentRenewError);
            userManager.events.removeUserSignedOut(handleUserSignedOut)
            userManager.events.removeUserSessionChanged(handleSessionChanged);
            userManager.events.removeAccessTokenExpiring(handleAccessTokenExpiring);
        };
    }, [userManager, silentSignin, processSignout]);


    return <AuthContext.Provider value={{ ...state, getUserManager: () => userManager, setAutoSignin, processSignout }}>
        {children}
    </AuthContext.Provider>
}