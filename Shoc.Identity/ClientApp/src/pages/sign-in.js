import React from "react";
import Helmet from "react-helmet";
import SignInMain from "components/auth/signin-main";

const SignInPage = () => {
  
  return (
    <>
      <Helmet>
        <title>Sign in</title>
      </Helmet>
      <SignInMain />
    </>
  );
};

export default SignInPage;