import React from "react";
import Helmet from "react-helmet";
import { Row } from "antd";
import SignInMain from "components/auth/signin-main";

const SignInPage = () => {
  
  return (
    <>
      <Helmet>
        <title>Sign in</title>
      </Helmet>
      <Row>
        <SignInMain />
      </Row>
    </>
  );
};

export default SignInPage;