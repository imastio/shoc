import Loader from "components/loader";
import Helmet from "react-helmet";

const LoadingPage = () => {
    
    return (
        <>
            <Helmet title="Loading" />
            <Loader />
        </>
    )
}

export default LoadingPage;