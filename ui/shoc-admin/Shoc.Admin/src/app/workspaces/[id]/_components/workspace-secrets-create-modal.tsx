import { selfClient } from "@/clients/shoc";
import SecretsClient from "@/clients/shoc/secret/secrets-client";
import StandardAlert from "@/components/general/standard-alert";
import { useApiAuthentication } from "@/providers/api-authentication/use-api-authentication";
import { maxDescriptionLength, secretNamePattern } from "@/well-known/secrets";
import { Form, Input, Modal, Switch} from "antd";
import { useEffect, useState } from "react";

export default function WorkspaceSecretsCreateModal({workspaceId, open, onClose = () => {}, onSuccess = () => {}}: any){
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
            workspaceId: workspaceId,
            name: values.name,
            description: values.description,
            disabled: values.disabled,
            encrypted: values.encrypted,
            value: values.value
        }

        const result = await withToken((token: string) => selfClient(SecretsClient).create(token, workspaceId, input));

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
            name: '',
            description: '',
            disabled: false,
            encrypted: false,
            value: ''

        });

    }, [open, form])

    return (
        <Modal
            closable={false}
            cancelButtonProps={{ disabled: progress }}
            open={open}
            onCancel={onCloseWrapper}
            destroyOnClose={true}
            forceRender
            title="Add a workspace secret"
            confirmLoading={progress}
            onOk={() => form.submit()}
        >
            <StandardAlert style={{ marginBottom: "8px" }} message={`Could not add the secret to the workspace`} errors={errors} optional={true} />
            <Form
                form={form}
                preserve={false}
                onFinish={onSubmit}
                labelCol={{span: 6}}
                wrapperCol={{span: 18}}
                layout="horizontal"
            >
                <Form.Item name="name" label="Name" rules={[
                    { required: true, message: 'Please enter valid name' },
                    { pattern: secretNamePattern, message: 'The name is invalid' }
                    ]}>
                    <Input placeholder="Please enter the name" />
                </Form.Item> 
                <Form.Item name="description" label="Description" rules={[
                    { max: maxDescriptionLength, message: 'The description is too long' }
                    ]}>
                    <Input.TextArea placeholder="Please enter the description" />
                </Form.Item> 
                <Form.Item name="disabled" label="Disabled" valuePropName="checked">
                    <Switch checkedChildren="Yes" unCheckedChildren="No" />
                </Form.Item> 
                <Form.Item name="encrypted" label="Encrypted" valuePropName="checked">
                    <Switch checkedChildren="Yes" unCheckedChildren="No" />
                </Form.Item> 
                <Form.Item name="value" label="Value">
                    <Input.TextArea rows={4} placeholder="Please enter the value" />
                </Form.Item> 
            </Form>
        </Modal>
    );
}