import { useAuth } from "auth/useAuth";
import { UserManager } from "oidc-client-ts";
import { Button, Result, Row, Col } from "antd";
import Helmet from "react-helmet";

const IndexPage = () => {

    const auth = useAuth();

    return (
        <>
            <Helmet>
                <title>Welcome</title>
            </Helmet>
            <Row type="flex" justify="center" align="middle" style={{ minHeight: '100vh' }}>
                <Col>
                    <Result
                        status="success"
                        title="Welcome"
                        subTitle={`Welcome, ${auth?.user?.profile?.name || "N/A"}`}
                        extra={
                            <Button type="primary" onClick={() => {

                                const mgr = new UserManager({ ...auth.getUserManager().settings, monitorAnonymousSession: false, monitorSession: false });
                                mgr.signoutRedirect()

                            }}>
                                Sign Out
                            </Button>
                        }
                    />
                </Col>
            </Row>
        </>

    )
}

export default IndexPage;