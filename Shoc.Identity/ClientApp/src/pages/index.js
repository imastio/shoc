import { useAuth } from "auth/useAuth";
import { UserManager } from "oidc-client-ts";
import { Button, Result } from "antd";
import Helmet from "react-helmet";
import { RiCheckboxCircleFill } from "react-icons/ri";

const IndexPage = () => {

    const auth = useAuth();

    return (
        <>
     <Helmet>
        <title>Welcome</title>
      </Helmet>
    <Result
        status="success"
        title="Welcome"
        subTitle={`Welcome, ${auth?.user?.profile?.name || "N/A" }`}
        icon={<RiCheckboxCircleFill className="remix-icon" />}
        extra={
        <Button type="primary" onClick={() => {
            
            const mgr = new UserManager({ ...auth.getUserManager().settings, monitorAnonymousSession: false, monitorSession: false });
            mgr.signoutRedirect()

            }}>
            Sign Out
        </Button>
        }
    />
    </>

    )
}

export default IndexPage;