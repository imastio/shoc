import { selfClient } from "@/clients/shoc";
import UsersClient from "@/clients/shoc/identity/users-client";
import StandardAlert from "@/components/general/standard-alert";
import { useApiAuthentication } from "@/providers/api-authentication/use-api-authentication";
import timezones from "@/well-known/timezones";
import { Form, Input, Modal, Switch, Select } from "antd";
import { useEffect } from "react";
import { useState } from "react";

const formItemLayout = {
    labelCol: {
        xs: { span: 24 },
        sm: { span: 6 },
    },
    wrapperCol: {
        xs: { span: 24 },
        sm: { span: 18 },
    },
};

export default function UserUpdateModal(props: any) {

    const { onClose, onSuccess, user = {}, open } = props;

    const [form] = Form.useForm();
    const { withToken } = useApiAuthentication();
    const [progress, setProgress] = useState(false);
    const [errors, setErrors] = useState([]);

    const onCloseWrapper = () => {

        setErrors([]);

        if (onClose) {
            onClose();
        }
    }

    const onSubmit = async (values: any) => {
        setErrors([]);
        setProgress(true);

        const result = await withToken((token: string) => selfClient(UsersClient).updateById(token, user.id, { ...values }));

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

    useEffect(() => {

        if (!open) {
            return;
        }

        form.setFieldsValue({
            ...user
        });

    }, [form, user, open])

    return (
        <Modal
            closable={false}
            cancelButtonProps={{ disabled: progress }}
            open={open}
            forceRender
            onCancel={onCloseWrapper}
            destroyOnClose={true}
            title="Update User"
            confirmLoading={progress}
            onOk={() => form.submit()}
        >
            <StandardAlert style={{ marginBottom: "8px" }} message="Could not update the user" errors={errors} optional={true} />
            <Form form={form} onFinish={onSubmit} layout="horizontal" {...formItemLayout}>
                <Form.Item name="fullName" label="Name" rules={[{ required: true, min: 2, message: 'Please enter valid name' }]}>
                    <Input placeholder="Please enter your Full name" />
                </Form.Item>
                <Form.Item name="email" label="Email" rules={[{ required: true, type: 'email', message: 'Please enter valid Email' }]}>
                    <Input placeholder="Please enter your Email" />
                </Form.Item>
                <Form.Item name="emailVerified" label="Email Verified" valuePropName="checked">
                    <Switch checkedChildren="Verified" unCheckedChildren="Unverified" />
                </Form.Item>
                <Form.Item name="timezone" label="Timezone">
                    <Select showSearch>
                        {timezones.map(tz => <Select.Option key={tz} value={tz}>{tz}</Select.Option>)}
                    </Select>
                </Form.Item>
            </Form>
        </Modal>
    );

}
