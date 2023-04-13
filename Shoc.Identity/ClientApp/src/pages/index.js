import { Button, Result, Row, Col } from "antd";
import Helmet from "react-helmet";
import { useAuth } from "react-oidc-context"

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
                            <Button type="primary" onClick={() => auth.signoutRedirect()}>
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