import React, { useState, useEffect } from "react";
import { Link } from "react-router-dom";
import { useHistory } from "react-router";
import { Row, Col, Typography} from "antd";
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

  return (
    <>
        <Col lg={12} span={24}>
          <Row align="middle" justify="center">
            <Col
              xxl={11}
              xl={15}
              lg={20}
              md={20}
              sm={24}
            >
              <h1>Sign in</h1>
                
              { !isAuthenticated && <p>
                      Welcome back!
                  </p>
              }
              
              { isAuthenticated && <AlreadyLoggedIn /> }

              {errors && errors.length > 0 && alertErrors(errors)}

              <SignInForm isAuthenticated={ isAuthenticated } returnUrl={ returnUrl } onError={ setErrors } />

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
        </Col>
    </>
  );
};

export default SignInMain;