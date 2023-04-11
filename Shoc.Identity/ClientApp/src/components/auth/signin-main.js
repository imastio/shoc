import React, { useState, useEffect } from "react";
import { Link } from "react-router-dom";
import { useHistory } from "react-router";
import { Row, Col, Typography, Button } from "antd";
import { RiCloseCircleLine } from "react-icons/ri";
import { useAuth } from "auth/useAuth";
import qs from 'qs';
import AlreadyLoggedIn from "./already-logged-in";
import SignInForm from "./signin-form";
import { resolveError } from "common/errors";

const SignInMain = () => {

  const auth = useAuth();
  const history = useHistory();
  const isAuthenticated = auth.isAuthenticated;
  const [errors, setErrors] = useState([]);
  
  // get the query string
  const query = qs.parse(history.location.search, { ignoreQueryPrefix: true });

  // the return url from query
  const returnUrl = query.return_url || query.returnUrl;

  const alertErrors = errors => errors.map((error, i) => renderError(error, i))

  const renderError = (error, i) => {

    return (  
      <Typography.Paragraph key={i}>
        <RiCloseCircleLine className="remix-icon" /> {resolveError(error.code)}</Typography.Paragraph>
    )
  }

  // handle specific errors
  useEffect(() => {

    // no errors
    if(!errors || !errors.length || errors.length === 0){
      return;
    }

    // handle if need verification
    if(errors.some(e => e.code === "CONNECT_UNVERIFIED_EMAIL")){
      history.push({
        pathname: "confirm",
        search: "?" + new URLSearchParams({...query}).toString()
      })
    }

  }, [errors, query, history]);


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
              <Col>
                <span>
                  Do not have an account?
                </span>
                <Link
                  to={{
                    pathname: "/sign-up",
                    search: "?" + new URLSearchParams({...query})
                  }}
                >
                  Sign up
                </Link>
               
              </Col>
            </Col>
          </Row>
    </>
  );
};

export default SignInMain;