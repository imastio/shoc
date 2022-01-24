import { Button, Result } from 'antd';
import React from 'react';
import { useHistory } from 'react-router-dom';
import Helmet from 'react-helmet';

const ErrorNotFound = () => {

  const history = useHistory();

  return (
    <>
      <Helmet>
        <title>Not Found</title>
      </Helmet>
      <Result
        status="404"
        title="Not Found"
        subTitle="We could not find a page for you!"
        extra={
          <Button type="primary" onClick={() => history.goBack()}>
            Go Back
          </Button>
        }
      />
    </>
  )
};

export default ErrorNotFound;