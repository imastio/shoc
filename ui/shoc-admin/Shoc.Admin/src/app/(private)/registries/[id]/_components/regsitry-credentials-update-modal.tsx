import { selfClient } from "@/clients/shoc";
import RegistryCredentialsClient from "@/clients/shoc/registry/registry-credentials-client";
import StandardAlert from "@/components/general/standard-alert";
import { useApiAuthentication } from "@/providers/api-authentication/use-api-authentication";
import { Form, Input, Modal, Switch} from "antd";
import { useEffect, useState } from "react";

export default function RegistryCredentialsUpdateModal({registryId, existing, open, onClose = () => {}, onSuccess = () => {}}: any){
    const [form] = Form.useForm();
    const { withToken } = useApiAuthentication();
    const [progress, setProgress] = useState(false);
    const [errors, setErrors] = useState([]);

    const onCloseWrapper = () => {
        setErrors([])
        onClose();
    }

    const onSubmit = async (values: any) => {
        setErrors([]);
        setProgress(true);

        const input = {
            registryId: registryId,
            username: values.username,
            email: values.email,
            pullAllowed: values.pullAllowed,
            pushAllowed: values.pushAllowed
        }

        const result = await withToken((token: string) => selfClient(RegistryCredentialsClient).updateById(token, registryId, existing?.id, input));

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

        if(!open){
            return;
        }

        form.setFieldsValue({
            username: existing?.username,
            email: existing?.email,
            pullAllowed: existing?.pullAllowed,
            pushAllowed: existing?.pushAllowed
        });

    }, [open, existing, form])

    return (
        <Modal
            closable={false}
            cancelButtonProps={{ disabled: progress }}
            open={open}
            onCancel={onCloseWrapper}
            destroyOnClose={true}
            forceRender
            title="Edit registry credential"
            confirmLoading={progress}
            onOk={() => form.submit()}
        >
            <StandardAlert style={{ marginBottom: "8px" }} message={`Could not edit the credential`} errors={errors} optional={true} />
            <Form
                form={form}
                preserve={false}
                onFinish={onSubmit}
                labelCol={{ span: 6 }}
                wrapperCol={{ span: 18 }}
                layout="horizontal"
            >
                <Form.Item name="username" label="Username" rules={[{ required: true, max: 256, message: 'Please enter valid username' }]}>
                    <Input placeholder="Please enter a username " />
                </Form.Item>
                <Form.Item name="email" label="Email" rules={[{ required: false, max: 256, type: 'email', message: 'Please enter valid Email' }]}>
                    <Input placeholder="Please enter an email (optional)" />
                </Form.Item> 
                <Form.Item name="pullAllowed" label="Pull Access" valuePropName="checked">
                    <Switch checkedChildren="Allow" unCheckedChildren="Disallow" />
                </Form.Item>
                <Form.Item name="pushAllowed" label="Push Access" valuePropName="checked">
                    <Switch checkedChildren="Allow" unCheckedChildren="Disallow" />
                </Form.Item>
            </Form>
        </Modal>
    );
}