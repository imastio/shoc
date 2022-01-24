import React, { useEffect, useState } from "react";
import { useAuth } from "auth/useAuth";
import { useHistory } from "react-router";
import { Route } from "react-router-dom";
import loadable from '@loadable/component'
import Loader from "components/loader";
import LoadingPage from "pages/_loading";


const whereAmI = (pathname) => {

    switch(pathname){
        case "/sign-in":
            return "sign-in";
        case "/signed-out":
            return "signed-out";
        case "/signed-in":
            return "signed-in"
        case "/sign-out":
            return "sign-out";
        default:
            return "page";
    }
}

const render = (props, ready) => {

    const DynamicComponent = loadable(props.component);

    return (
        <Route key={props.key} exact={props.exact} path={props.path}>
            {
                !ready ? <LoadingPage /> : <DynamicComponent fallback={<Loader />} />
            }
        </Route>
    );
}

const MaybeProtectedRoute = (props) => {

    const history = useHistory();
    const auth = useAuth();
  
    const [where, setWhere] = useState("page");

    // update page once location is changed
    useEffect(() => setWhere(whereAmI(history.location.pathname)), [history.location.pathname])

    // need to signin
    const needSignin = !auth.isLoading && !auth.isAuthenticated && props.protected;

    // ready if no need for signin and no authentication error
    const ready = !needSignin && !auth.error;

    // enable auto sign-in only if protected page and not in sign-in process
    useEffect(() => {
        auth.setAutoSignin(!needSignin && props.protected);
    }, [auth, needSignin, props.protected]);

    // monitor to see if need to redirect to sign- in page
    useEffect(() => {

        // if signin is not required or just signed out do not do anything
        if(!needSignin || auth.signout === "pending"){
            return;
        }
    
        // go and signin otherwise
        auth.getUserManager()
            .signinRedirect({ state: document.location.pathname })
            .catch(_ => {
                history.push("/access-denied")
            });

    }, [auth, needSignin, history]);

    
    // monitor to see if logout happens go to signed-out page
    useEffect(() => {

        // if sign-in is required or page is public do not redirect to signed-out page
        if(!props.protected){
            return;
        }

        // handle if signed out (but not completed sign-out yet)
        if (auth.signout === "pending" && where !== "signed-out") {
            history.replace("/signed-out");
        }
    }, [auth.signout, history, needSignin, where, props.protected])

    return render(props, ready)
}

export default MaybeProtectedRoute;