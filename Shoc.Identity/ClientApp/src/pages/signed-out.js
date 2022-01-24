import { Button, Result } from 'antd';
import React, { useEffect } from 'react';
import Helmet from 'react-helmet';
import {useAuth} from "auth/useAuth";
import { RiCheckboxCircleFill } from 'react-icons/ri';
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
    <Result
        status="success"
        title="Signed out"
        subTitle="You were signed out from the system"
        icon={<RiCheckboxCircleFill className="remix-icon" />}
        extra={
        <Button type="primary" onClick={() => history.push("/")}>
            Go Home
        </Button>
        }
    />
    </>
  )
};

export default SignedOutPage;