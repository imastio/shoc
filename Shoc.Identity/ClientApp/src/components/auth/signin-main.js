import React, { useState } from "react";
import { Row, Col, Typography, Button } from "antd";
import { useAuth } from "react-oidc-context";
import AlreadyLoggedIn from "./already-logged-in";
import SignInForm from "./signin-form";
import { resolveError } from "common/errors";
import { useSearchParams } from "react-router-dom";
import { CloseOutlined } from "@ant-design/icons";

const SignInMain = () => {

  const auth = useAuth();
  const isAuthenticated = auth.isAuthenticated;
  const [errors, setErrors] = useState([]);
  const [searchParams] = useSearchParams();
  
  // the return url from query
  const returnUrl = searchParams.get('returnUrl') || searchParams.get('return_url');

  const alertErrors = errors => errors.map((error, i) => renderError(error, i))

  const renderError = (error, i) => {

    return (  
      <Typography.Paragraph key={i}>
        <CloseOutlined /> {resolveError(error.code)}
      </Typography.Paragraph>
    )
  }

  const googleHandler = () => {
    const params = new URLSearchParams();
    params.set('returnUrl', returnUrl)
    window.location.href = '/sign-in/google?' + params.toString()
  };

  return (
    <>
          <Row type="flex" justify="center" align="middle" style={{minHeight: '100vh'}}>
            <Col>
              <h1>Sign in</h1>
                
              { !isAuthenticated && <p>
                      Welcome back!
                  </p>
              }
              
              { isAuthenticated && <AlreadyLoggedIn /> }

              {errors && errors.length > 0 && alertErrors(errors)}

                  <SignInForm isAuthenticated={isAuthenticated} returnUrl={returnUrl} onError={setErrors} />
                  <Button onClick={googleHandler}>Google</Button>
            </Col>
          </Row>
    </>
  );
};

export default SignInMain;