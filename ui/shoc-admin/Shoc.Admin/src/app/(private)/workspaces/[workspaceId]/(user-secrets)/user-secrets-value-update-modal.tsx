import { selfClient } from "@/clients/shoc";
import UserSecretsClient from "@/clients/shoc/secret/user-secrets-client";
import StandardAlert from "@/components/general/standard-alert";
import { useApiAuthentication } from "@/providers/api-authentication/use-api-authentication";
import { Form, Input, Modal, Switch} from "antd";
import { useEffect, useState } from "react";

export default function UserSecretsValueUpdateModal({workspaceId, userId, existing, open, onClose = () => {}, onSuccess = () => {}}: any){
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
            userId: userId,
            workspaceId: workspaceId,
            encrypted: values.encrypted,
            value: values.value
        }

        const result = await withToken((token: string) => selfClient(UserSecretsClient).updateValueById(token, workspaceId, userId, existing?.id, input));

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
            encrypted: existing?.encrypted,
            value: existing?.encrypted ? '' : existing?.value
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
            title="Edit a user secret value"
            confirmLoading={progress}
            onOk={() => form.submit()}
        >
            <StandardAlert style={{ marginBottom: "8px" }} message={`Could not edit the value of selected user secret`} errors={errors} optional={true} />
            <Form
                disabled={progress}
                form={form}
                preserve={false}
                onFinish={onSubmit}
                labelCol={{span: 6}}
                wrapperCol={{span: 18}}
                layout="horizontal"
            >
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