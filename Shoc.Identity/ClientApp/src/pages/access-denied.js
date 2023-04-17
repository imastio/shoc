import { Button, Result } from 'antd';
import React from 'react';
import Helmet from 'react-helmet';
import { useNavigate } from 'react-router-dom';

const ErrorAccessDenied = () => {

  const navigate = useNavigate();

  return (
    <>
      <Helmet>
        <title>Access Denied</title>
      </Helmet>
      <Result
        status="403"
        title="Access Denied"
        subTitle="You do not have access to the requested page"
        extra={
          <Button type="primary" onClick={() => navigate(-1)}>
            Go Back
          </Button>
        }
      />
    </>
  )
};

export default ErrorAccessDenied;