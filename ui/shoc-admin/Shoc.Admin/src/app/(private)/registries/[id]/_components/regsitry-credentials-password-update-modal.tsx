import { selfClient } from "@/clients/shoc";
import RegistryCredentialsClient from "@/clients/shoc/registry/registry-credentials-client";
import StandardAlert from "@/components/general/standard-alert";
import { useApiAuthentication } from "@/providers/api-authentication/use-api-authentication";
import { Form, Input, Modal} from "antd";
import { useEffect, useState } from "react";

export default function RegistryCredentialsPasswordUpdateModal({registryId, existing, open, onClose = () => {}, onSuccess = () => {}}: any){
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
            password: values.password
        }

        const result = await withToken((token: string) => selfClient(RegistryCredentialsClient).updatePasswordById(token, registryId, existing?.id, input));

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
            password: ''
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
            title="Edit credential's password"
            confirmLoading={progress}
            onOk={() => form.submit()}
        >
            <StandardAlert style={{ marginBottom: "8px" }} message={`Could not edit the credential's password`} errors={errors} optional={true} />
            <Form
                form={form}
                preserve={false}
                onFinish={onSubmit}
                labelCol={{ span: 6 }}
                wrapperCol={{ span: 18 }}
                layout="horizontal"
            >
                 <Form.Item name="password" label="Password" rules={[{ required: true, max: 256, message: 'Please enter valid password' }]} >
                    <Input type="password" placeholder="Please enter a password" />
                </Form.Item>
            </Form>
        </Modal>
    );
}