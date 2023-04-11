import * as React from 'react';
import { Outlet, useLocation } from 'react-router-dom';
import { useAuth } from 'react-oidc-context';
import LoadingPage from 'pages/_loading';

export default function PrivateLayout() {
    const auth = useAuth();
    const location = useLocation();

    // check if loading in progress
    const loading = auth.isLoading || auth.activeNavigator;

    React.useEffect(() => {
        if (!loading && (!auth.isAuthenticated || auth.error)) {
            auth.signinRedirect({ state: `${location.pathname}${location.search}` }).catch((err) => err);
        }
    }, [location.pathname, location.search, loading, auth]);

    React.useEffect(() => {

        auth.clearStaleState();

        return auth.events.addUserSignedOut(() => {
            auth.removeUser();
        })
    }, [auth]);

    if ((loading || !auth.isAuthenticated) && auth.activeNavigator !== 'signinSilent') {
        return <LoadingPage />;
    }

    return <Outlet />;
}
