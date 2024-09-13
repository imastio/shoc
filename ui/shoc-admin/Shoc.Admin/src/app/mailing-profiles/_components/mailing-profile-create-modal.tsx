import { Form, Input, InputNumber, Modal, Select, Tabs } from "antd";
import { useState } from "react";
import { encryptionTypes, providers } from "@/well-known/mailing";
import { useApiAuthentication } from "@/providers/api-authentication/use-api-authentication";
import { selfClient } from "@/clients/shoc";
import MailingProfilesClient from "@/clients/shoc/settings/mailing-profiles-client";
import StandardAlert from "@/components/general/standard-alert";

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

export default function MailingProfileCreateModal(props: any) {

    const { open, onClose, onSuccess } = props;

    const [form] = Form.useForm();
    const { withToken } = useApiAuthentication();
    const [progress, setProgress] = useState(false);
    const [errors, setErrors] = useState([]);

    const onCloseWrapper = () => {
        setErrors([]);
        form.resetFields();
        onClose();
    }

    const onSubmit = async (values: any) => {
        setErrors([]);
        setProgress(true);

        const result = await withToken((token: string) => selfClient(MailingProfilesClient).create(token, { ...values }));

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
            destroyOnClose={true}
            title="Create Mailing Profile"
            confirmLoading={progress}
            onOk={() => form.submit()}
        >
            <StandardAlert style={{ marginBottom: "8px" }} message="Could not create a profile" errors={errors} optional={true} />
            <Form form={form} onFinish={onSubmit} layout="horizontal" {...formItemLayout} initialValues={{ provider: 'smtp', encryptionType: 'ssl' }}>

                <Tabs defaultActiveKey="1" items={[
                    {
                        key: "1",
                        label: "Definition",
                        forceRender: true,
                        children: <>
                            <Form.Item name="code" label="Code" rules={[{ required: true, pattern: /^[\w-]+$/, message: 'Please enter valid gateway code' }]}>
                                <Input placeholder="Please enter the code " />
                            </Form.Item>
                            <Form.Item name="provider" label="Provider" rules={[{ required: true, message: 'Please select a valid provider' }]}>
                                <Select>
                                    {Object.entries(providers).map(entry => <Select.Option key={entry[0]} value={entry[0]}>{entry[1]}</Select.Option>)}
                                </Select>
                            </Form.Item>
                            <Form.Item name="defaultFromEmail" label="Sender Email" rules={[{ required: true, type: 'email', message: 'Please enter a valid sender email' }]}>
                                <Input placeholder="Please enter the default sender email" />
                            </Form.Item>
                            <Form.Item name="defaultFromSender" label="Sender Name" rules={[{ required: true, message: 'Please enter a valid sender name' }]}>
                                <Input placeholder="Please enter the default sender email" />
                            </Form.Item>
                        </>
                    },
                    {
                        key: "2",
                        label: "SMTP",
                        forceRender: true,
                        children: <>
                            <Form.Item name="server" label="Server">
                                <Input placeholder="smtp.server.com" />
                            </Form.Item>
                            <Form.Item name="port" label="Port" >
                                <InputNumber placeholder="465" />
                            </Form.Item>
                            <Form.Item name="encryptionType" label="Encryption">
                                <Select>
                                    {Object.entries(encryptionTypes).map((entry) => <Select.Option key={entry[0]} value={entry[0]}>{entry[1]}</Select.Option>)}
                                </Select>
                            </Form.Item>
                            <Form.Item name="username" label="Username">
                                <Input placeholder="Your username in the mailing system " />
                            </Form.Item>
                            <Form.Item name="password" label="Password">
                                <Input placeholder="Your password in the mailing system" />
                            </Form.Item>
                        </>
                    }
                ]} />
            </Form>
        </Modal>
    );
};