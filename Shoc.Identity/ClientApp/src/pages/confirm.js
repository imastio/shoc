import React, { useState, useEffect } from "react";
import { useHistory } from "react-router";
import { Row, Col, Form, Input, Button, Typography, Popconfirm, notification } from "antd";
import { Helmet } from "react-helmet";
import { useDispatch } from "react-redux";
import { RiCloseCircleLine } from "react-icons/ri";
import { actions as userActions } from 'redux/users/slice';
import momentTz from 'moment-timezone';
import qs from "qs";
import _ from "lodash";
import { validateEmail } from "utility/input";
import { resolveError } from "common/errors";

const ConfirmPage = () => {

    const dispatch = useDispatch();
    const history = useHistory();
    const query = qs.parse(history.location.search, { ignoreQueryPrefix: true });
    const [progress, setProgress] = useState(false);
    const [errors, setErrors] = useState([]);
    const [validEmail, setValidEmail] = useState(false);
    const [currentEmail, setCurrentEmail] = useState(false);

    useEffect(() => {
        setValidEmail(validateEmail(currentEmail))
    }, [currentEmail])

    const returnUrl = query.return_url || query.returnUrl;

    // request a one-time password
    const requestCode = async () => {
        setErrors([])
        const result = await dispatch(userActions.requestConfirmation({
            input: {
                email: currentEmail,
                returnUrl: returnUrl,
            }
        }))

        if (result.error) {
            setErrors(result?.payload?.errors || [])
            return;
        }

        // get payload
        const payload = result?.payload || { sent: false };

        if(payload.sent){
            notification.success({description: "Confirmation email is was sent!"})
        }      
    }

    const onFinish = async (values) => {
        setProgress(true)
        setErrors([])
        const result = await dispatch(userActions.processConfirmation({
            input: {
                email: values.email,
                code: values.code,
                returnUrl: returnUrl,
                timezone: momentTz.tz.guess(true)
            }
        }))
        setProgress(false)

        if (result.error) {
            setErrors(result?.payload?.errors || [])
            return;
        }

        // get payload
        const payload = result?.payload || {};

        if (_.isEmpty(payload.returnUrl)) {
            history.push("/");
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
                <RiCloseCircleLine className="remix-icon enl-text-color-danger-1 enl-mr-8" /> {resolveError(error.code)}
            </Typography.Paragraph>
        )
    )

    return (<>
        <Helmet>
            <title>Confirm your email</title>
        </Helmet>
        <Row gutter={[32, 0]} >

            <Col md={12}>
                <Row className="enl-h-100" align="middle" justify="center">
                    <Col
                        xxl={11}
                        xl={15}
                        lg={20}
                        md={20}
                        sm={24}
                    >
                        <h1>Confirm email</h1>
                        <p>
                            Please confirm you email
                        </p>
                        {errors && errors.length > 0 && alertErrors(errors)}

                        <Form
                            layout="vertical"
                            name="basic"
                            onFinish={onFinish}
                        >
                            <Form.Item
                                required
                                name="email"
                                label="Email:"
                                rules={[{ required: true, type: "email", message: "Enter a valid email" }]} >
                                <Input onChange={ (e) => setCurrentEmail(e.target.value)} />
                            </Form.Item>

                            <Form.Item
                                required
                                name="code"
                                label="Code:"
                                rules={[{ required: true, min: 6, message: "Enter valid confirmation code" }]}>
                                <Input />
                            </Form.Item>

                            <Row>
                                <Popconfirm
                                    okType="default"
                                    okButtonProps={{ disabled: !validEmail }}
                                    cancelButtonProps={{ style: { display: "none" } }}
                                    title="Send again?"
                                    onConfirm={requestCode}
                                    cancelText={"No"}
                                    okText={"Yes"}
                                >
                                    <Button type="link">
                                        Send another code
                                    </Button>
                                </Popconfirm>
                            </Row>
                            <Form.Item>
                                <Button block type="primary" htmlType="submit" loading={progress}>
                                    Confirm
                                </Button>
                            </Form.Item>
                        </Form>

                    </Col>
                </Row>
            </Col>
        </Row>
    </>
    )
}

export default ConfirmPage;