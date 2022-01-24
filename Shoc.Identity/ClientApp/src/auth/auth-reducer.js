/**
 * Handles how that state changes in the `useAuth` hook.
 */
const authReducer = (state, action) => {
    switch (action.type) {
        case "init":
        case "loaded":
            return {
                ...state,
                user: action.user,
                isLoading: false,
                isAuthenticated: action.user ? !action.user.expired : false,
                error: undefined,
            };
        case "unloaded":
            return {
                ...state,
                user: undefined,
                isAuthenticated: false,
            };
        case "session_changed":
            return {
                ...state,
                sessionChanged: action.sessionChanged,
            };
        case "toggle_autosignin":
                return {
                    ...state,
                    autoSignin: action.autoSignin,
                };
        case "sign_out":
            return {
                ...state,
                signout: action.signout,
                user: undefined,
                isAuthenticated: false,
            };
        case "signed_out_completed":
            return {
                ...state,
                signedOutCompleted: true
            };
        case "error":
            return {
                ...state,
                isLoading: false,
                error: action.error,
            };
        default: 
            return state;
    }
};

export default authReducer;