import { Button, Result } from 'antd';
import React from 'react';
import { useNavigate } from 'react-router-dom';
import Helmet from 'react-helmet';

const ErrorNotFound = () => {

  const navigate = useNavigate();

  return (
    <>
      <Helmet>
        <title>Error 404</title>
      </Helmet>
      <Result
        status="404"
        title="Error 404"
        subTitle="The requested page is not found"
        extra={
          <Button type="primary" onClick={() => navigate(-1)}>
            Go Back
          </Button>
        }
      />
    </>
  )
};

export default ErrorNotFound;