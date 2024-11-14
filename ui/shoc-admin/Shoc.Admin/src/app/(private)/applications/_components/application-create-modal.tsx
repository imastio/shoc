import { selfClient } from "@/clients/shoc";
import ApplicationsClient from "@/clients/shoc/identity/applications-client";
import StandardAlert from "@/components/general/standard-alert";
import { useApiAuthentication } from "@/providers/api-authentication/use-api-authentication";
import { Form, Input, Modal, Switch } from "antd";
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

export default function ApplicationCreateModal(props: any) {

    const { visible, onClose, onSuccess } = props;

    const [form] = Form.useForm();
    const { withToken } = useApiAuthentication();
    const [progress, setProgress] = useState(false);
    const [errors, setErrors] = useState([]);

    const onCloseWrapper = () => {
        form.resetFields();
        setErrors([]);
        onClose();
    }

    const onSubmit = async (values: any) => {
        setErrors([]);
        setProgress(true);

        const result = await withToken((token: string) => selfClient(ApplicationsClient).create(token, {
            ...values
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
            open={visible}
            onCancel={onCloseWrapper}
            destroyOnClose={true}
            title="Create Application"
            confirmLoading={progress}
            onOk={() => form.submit()}
        >
            <StandardAlert style={{ marginBottom: "8px" }} message="Unable to create an application" errors={errors} optional={true} />
            <Form form={form} onFinish={onSubmit} layout="horizontal" {...formItemLayout} initialValues={{enabled: false}}>
                <Form.Item name="name" label="Name" rules={[{ required: true, min: 2, message: 'Please enter valid name' }]}>
                    <Input placeholder="User friendly name for application" />
                </Form.Item>
                <Form.Item name="applicationClientId" label="Client Id" rules={[{ required: true, min: 2, message: 'Please enter a valid and unique client id' }]}>
                    <Input placeholder="A unique client id" />
                </Form.Item>
                <Form.Item name="description" label="Description" rules={[{ required: true, min: 2, message: 'Please enter the description' }]}>
                    <Input.TextArea placeholder="Describe the application" />
                </Form.Item>
                <Form.Item name="enabled" label="Enabled" valuePropName="checked" rules={[{required: true}]}>
                    <Switch checkedChildren="Enabled" unCheckedChildren="Disabled" />
                </Form.Item>
            </Form>
        </Modal>
    );
}
