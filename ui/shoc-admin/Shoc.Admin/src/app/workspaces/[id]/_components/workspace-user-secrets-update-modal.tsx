import { selfClient } from "@/clients/shoc";
import UserSecretsClient from "@/clients/shoc/secret/user-secrets-client";
import StandardAlert from "@/components/general/standard-alert";
import { useApiAuthentication } from "@/providers/api-authentication/use-api-authentication";
import { maxDescriptionLength, secretNamePattern } from "@/well-known/secrets";
import { Form, Input, Modal, Switch} from "antd";
import { useEffect, useState } from "react";

export default function WorkspaceUserSecretsUpdateModal({workspaceId, userId, existing, open, onClose = () => {}, onSuccess = () => {}}: any){
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
            userId: userId,
            name: values.name,
            description: values.description,
            disabled: values.disabled
        }

        const result = await withToken((token: string) => selfClient(UserSecretsClient).updateById(token, workspaceId, userId, existing?.id, input));

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
            name: existing?.name,
            description: existing?.description,
            disabled: existing?.disabled
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
            title="Edit a user secret"
            confirmLoading={progress}
            onOk={() => form.submit()}
        >
            <StandardAlert style={{ marginBottom: "8px" }} message={`Could not edit the selected user secret`} errors={errors} optional={true} />
            <Form
                disabled={progress}
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
            </Form>
        </Modal>
    );
}