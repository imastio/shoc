import { selfClient } from "@/clients/shoc";
import ApplicationsClient from "@/clients/shoc/identity/applications-client";
import StandardAlert from "@/components/general/standard-alert";
import { useApiAuthentication } from "@/providers/api-authentication/use-api-authentication";
import { Form, Input, Modal, Switch } from "antd";
import { useEffect, useState } from "react";

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

export default function ApplicationUpdateModal(props: any) {

    const { open, onClose = () => { }, onSuccess, application } = props;

    const [form] = Form.useForm();
    const { withToken } = useApiAuthentication();
    const [progress, setProgress] = useState(false);
    const [errors, setErrors] = useState([]);

    const onCloseWrapper = () => {
        setErrors([]);
        onClose()
    }

    const onSubmit = async (values: any) => {
        setErrors([]);
        setProgress(true);

        const result = await withToken((token: any) => selfClient(ApplicationsClient).updateById(token, application.id, {
            ...application,
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

    useEffect(() => {

        form.setFieldsValue({
            ...application
        });

    }, [form, application])

    return (
        <Modal
            closable={false}
            forceRender
            cancelButtonProps={{ disabled: progress }}
            open={open}
            onCancel={onCloseWrapper}
            destroyOnClose={true}
            title="Update Application"
            confirmLoading={progress}
            onOk={() => form.submit()}
        >
            <StandardAlert style={{ marginBottom: "8px" }} message="Could not update the application" errors={errors} optional={true} />
            <Form form={form} preserve={true} onFinish={onSubmit} layout="horizontal" {...formItemLayout}>
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
