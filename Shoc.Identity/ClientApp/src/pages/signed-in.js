import Loader from "components/loader";
import Helmet from "react-helmet";

const SignedInPage = () => {        
    return (
        <>
            <Helmet title="Signed in" />
            <Loader />
        </>
    )
}

export default SignedInPage;