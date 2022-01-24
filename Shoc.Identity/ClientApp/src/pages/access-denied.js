import { Button, Result } from 'antd';
import React from 'react';
import Helmet from 'react-helmet';
import { useHistory } from 'react-router-dom';

const ErrorAccessDenied = () => {

  const history = useHistory();

  return (
    <>
      <Helmet>
        <title>Access Denied</title>
      </Helmet>
      <Result
        status="403"
        title="Access Denied"
        subTitle="You do not have access to this page"
        extra={
          <Button type="primary" onClick={() => history.goBack()}>
            Go Back
          </Button>
        }
      />
    </>
  )
};

export default ErrorAccessDenied;