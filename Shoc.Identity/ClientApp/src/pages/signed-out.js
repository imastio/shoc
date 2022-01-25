import { Button, Result, Row, Col } from 'antd';
import React, { useEffect } from 'react';
import Helmet from 'react-helmet';
import {useAuth} from "auth/useAuth";
import { useHistory } from 'react-router-dom';

const SignedOutPage = () => {

  const history = useHistory();
  const auth = useAuth();

  useEffect(() => {
    if(auth.signout === "pending"){
      auth.processSignout("no");
    }
  }, [auth])

  return (
    <>
     <Helmet>
        <title>Signed out</title>
      </Helmet>
      <Row type="flex" justify="center" align="middle" style={{ minHeight: '100vh' }}>
                <Col>
    <Result
        status="info"
        title="Signed out"
        subTitle="You were signed out from the system"
        extra={
        <Button type="primary" onClick={() => history.push("/")}>
            Go Home
        </Button>
        }
    />
    </Col></Row>
    </>
  )
};

export default SignedOutPage;