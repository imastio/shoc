import React, { useState } from "react";
import { Link } from "react-router-dom";
import { useHistory } from "react-router";
import { Row, Col, Form, Input, Button, Typography } from "antd";
import { useAuth } from "auth/useAuth";
import { Helmet } from "react-helmet";
import { useDispatch } from "react-redux";
import { RiCloseCircleLine } from "react-icons/ri";
import { actions as userActions } from 'redux/users/slice';
import momentTz from 'moment-timezone';
import AlreadyLoggedIn from "components/auth/already-logged-in";
import qs from "qs";
import _ from "lodash";
import { resolveError } from "common/errors";

const SignUpPage = () => {

    const dispatch = useDispatch();
    const history = useHistory();
    const auth = useAuth();
    const isAuthenticated = auth.isAuthenticated;
    const query = qs.parse(history.location.search, {ignoreQueryPrefix: true});
    const [progress, setProgress] = useState(false);
    const [errors, setErrors] = useState([]);

    const returnUrl = query.return_url || query.returnUrl;

    const onFinish = async (values) => {
        setProgress(true)
        setErrors([])
        const result = await dispatch(userActions.signup({ input: {
            email: values.email,
            fullName: values.fullName,
            password: values.password,
            returnUrl: returnUrl,
            timezone: momentTz.tz.guess(true)
        } }))
        setProgress(false)

        if(result.error){
            setErrors(result?.payload?.errors || [])
            return;
        }

        // get payload
        const payload = result?.payload || {};

        if (_.isEmpty(payload.returnUrl)) {
            history.push("/");
        }
        else if(!payload.emailVerified){
            history.push({
                pathname: "confirm",
                search: "?" + new URLSearchParams({...query}).toString()
              })
        }
        else if (payload.returnUrl.startsWith("/") && !payload.continueFlow) {
            history.push(payload.returnUrl)
        }
        else {
            window.location.href = payload.returnUrl;
        }
    }

    const alertErrors = errors => errors.map(
        (error, i) => (
            <Typography.Paragraph key={i}>
                <RiCloseCircleLine className="remix-icon" /> {resolveError(error.code)}
            </Typography.Paragraph>
        ) 
    )

    return (<>
        <Helmet>
            <title>Create account</title>
        </Helmet>
        <Row gutter={[32, 0]}>

            <Col md={12}>
                <Row align="middle" justify="center">
                    <Col
                        xxl={11}
                        xl={15}
                        lg={20}
                        md={20}
                        sm={24}
                    >
                        <h1>Create Account</h1>
                        { !isAuthenticated && <p>
                            Please create an account to continue
                        </p> 
                        }
                        { isAuthenticated && <AlreadyLoggedIn /> }

                        {errors && errors.length > 0 && alertErrors(errors)}

                        <Form
                            layout="vertical"
                            name="basic"
                            onFinish={onFinish}
                        >
                            <Form.Item
                                required
                                name="fullName"
                                label="Full Name:"
                                rules={[{ required: true, type: "string", message: "Enter your name" }]} >
                                <Input disabled={progress || isAuthenticated} />
                            </Form.Item>

                            <Form.Item
                                required
                                name="email"
                                label="Email:"
                                rules={[{ required: true, type: "email", message: "Enter a valid email" }]} >
                                <Input disabled={progress || isAuthenticated} />
                            </Form.Item>

                            <Form.Item 
                                required
                                name="password"
                                label="Password"
                                rules={[{ required: true, min: 6, message: "Enter a valid password"}]}>
                                <Input.Password disabled={progress || isAuthenticated} />
                            </Form.Item>

                            <Form.Item 
                                required
                                name="passwordConfirmation"
                                label="Repeat Password:"
                                rules={[
                                    { required: true, min: 6, message: "Enter a matching password" },
                                    ({ getFieldValue }) => ({
                                        validator(_, value) {
                                          if (!value || getFieldValue('password') === value) {
                                            return Promise.resolve();
                                          }
                                          return Promise.reject(new Error("Passwords does not match"));
                                        },
                                      }),
                                ]}>
                                <Input.Password disabled={progress || isAuthenticated} />
                            </Form.Item>

                            <Form.Item>
                                <Button block type="primary" htmlType="submit" disabled={progress || isAuthenticated} loading={progress}>
                                    Sign up
                                </Button>
                            </Form.Item>
                        </Form>

                        <div>
                            <span>
                                Have an account?
                            </span>

                            <Link
                                to={{
                                    pathname: "/sign-in",
                                    search: "?" + new URLSearchParams({...query})
                                  }}
                            >
                                Sign in
                            </Link>
                        </div>
                    </Col>
                </Row>
            </Col>
        </Row>
    </>
    )
}

export default SignUpPage;