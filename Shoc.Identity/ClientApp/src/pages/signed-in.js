import Loader from "components/loader";
import Helmet from "react-helmet";
import { useAuth } from "auth/useAuth";
import { useHistory } from "react-router";
import { useEffect } from "react";

const SignedInPage = () => {
    
    const auth = useAuth();
    const history = useHistory();

    useEffect(() => {
        auth.getUserManager().signinRedirectCallback().then(user => {
            history.replace(user?.state || "/")
        }).catch(error => {
            history.replace("/")
            console.error("Could not get back from signin page", error)
        })
    }, [auth, history])
        
    return (
        <>
            <Helmet title="Signed in" />
            <Loader />
        </>
    )
}

export default SignedInPage;