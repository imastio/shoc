import { selfClient } from "@/clients/shoc";
import UsersClient from "@/clients/shoc/identity/users-client";
import StandardAlert from "@/components/general/standard-alert";
import { useApiAuthentication } from "@/providers/api-authentication/use-api-authentication";
import { Form, Modal, Input } from "antd";
import { useState } from "react";

export default function UserPasswordUpdateModal(props: any) {

    const { userId, open, onClose, onSuccess } = props;

    const [form] = Form.useForm();
    const { withToken } = useApiAuthentication();
    const [progress, setProgress] = useState(false);
    const [errors, setErrors] = useState([]);

    const onCloseWrapper = () => {
        form.resetFields();

        if (onClose) {
            onClose();
        }
    }

    const onSubmit = async (values: any) => {
        setErrors([]);
        setProgress(true);

        const result = await withToken((token: string) => selfClient(UsersClient).updatePasswordById(token, userId, {
            password: values.password,
            passwordConfirmation: values.passwordConfirmation
        }));

        setProgress(false);

        if (result.error) {
            setErrors(result.payload.errors);
            return;
        }

        if (onSuccess) {
            onSuccess(result.payload);
        }

        onCloseWrapper();
    }

    return (
        <Modal
            closable={false}
            cancelButtonProps={{ disabled: progress }}
            open={open}
            forceRender
            onCancel={onCloseWrapper}
            title="Update Password"
            confirmLoading={progress}
            onOk={() => form.submit()}
        >
            <StandardAlert style={{ marginBottom: "8px" }} message="Error while updating password" errors={errors} optional={true} />
            <Form form={form} layout="vertical" onFinish={onSubmit}>

                <Form.Item
                    required
                    name="password"
                    label="Password:"
                    rules={[{ required: true, min: 5, message: "Password is too weak" }]}>
                    <Input.Password disabled={progress} />
                </Form.Item>

                <Form.Item
                    required
                    name="passwordConfirmation"
                    label="Repeat Password:"
                    rules={[
                        { required: true, min: 5, message: "Password is too weak" },
                        ({ getFieldValue }) => ({
                            validator(_, value) {
                                if (!value || getFieldValue('password') === value) {
                                    return Promise.resolve();
                                }
                                return Promise.reject(new Error("Password does not match the confirmation!"));
                            },
                        }),
                    ]}>
                    <Input.Password disabled={progress} />
                </Form.Item>

            </Form>
        </Modal>
    );

}
