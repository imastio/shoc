import React, { useState } from "react";
import { Form, Input, Button } from "antd";
import { useDispatch } from "react-redux";
import { actions as userActions } from 'redux/users/slice';
import { useNavigate } from "react-router-dom";
import { isEmpty } from "extensions/string";

const SignInForm = (props) => {

    const dispatch = useDispatch();
    const navigate = useNavigate();
    const [progress, setProgress] = useState(false);
    const { returnUrl, onError, isAuthenticated } = props;
    const [form] = Form.useForm();
    
    const onFinish = async (values) => {
        setProgress(true)
        onError([])
        const result = await dispatch(userActions.signin({
            input: {
                email: values.email,
                password: values.password,
                returnUrl: returnUrl
            }
        }))
        setProgress(false)

        if (result.error) {
            onError(result?.payload?.errors || [])
            return;
        }

        // get payload
        const payload = result?.payload || {};

        if (isEmpty(payload.returnUrl)) {
            navigate("/")
        }
        else if (payload.returnUrl.startsWith("/") && !payload.continueFlow) {
            navigate(payload.returnUrl)
        }
        else {
            window.location.href = payload.returnUrl;
        }
    }
    
    return (
        <>
            <Form
                form={form}
                layout="vertical"
                name="basic"
                onFinish={onFinish}
            >
                <Form.Item 
                    name="email"
                    disabled={progress || isAuthenticated}
                    label="Email:"
                    rules={[{ required: true, type: "email", message: "Enter a valid email" }]}
                >
                    <Input disabled={progress || isAuthenticated} />
                </Form.Item>

                <Form.Item
                    name="password"
                    label="Password"
                    rules={[{ required: true, min: 6, message: "Enter a valid password" }]}>
                    <Input.Password disabled={progress || isAuthenticated} />
                </Form.Item>
                
                <Form.Item>
                    <Button block type="primary" disabled={isAuthenticated} htmlType="submit" loading={progress}>
                        Sign in
                    </Button>
                </Form.Item>
            </Form>
        </>
    );
};

export default SignInForm;