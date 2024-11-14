import { selfClient } from "@/clients/shoc";
import RegistrySigningKeysClient from "@/clients/shoc/registry/registry-signing-keys-client";
import StandardAlert from "@/components/general/standard-alert";
import { useApiAuthentication } from "@/providers/api-authentication/use-api-authentication";
import { registrySigningKeyAlgorithms } from "@/well-known/registries";
import { Form, Modal, Select } from "antd";
import { useEffect, useState } from "react";

export default function RegistrySigningKeysAddModal({registryId, open, onClose = () => {}, onSuccess = () => {}}: any){
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
            algorithm: values.algorithm,
        }

        const result = await withToken((token: string) => selfClient(RegistrySigningKeysClient).create(token, registryId, input));

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

        form.resetFields();

    }, [open, form])

    return (
        <Modal
            closable={false}
            cancelButtonProps={{ disabled: progress }}
            open={open}
            onCancel={onCloseWrapper}
            destroyOnClose={true}
            forceRender
            title="Add registry signing key"
            confirmLoading={progress}
            onOk={() => form.submit()}
        >
            <StandardAlert style={{ marginBottom: "8px" }} message={`Could not create a signing key for the registry`} errors={errors} optional={true} />
            <Form
                form={form}
                preserve={false}
                onFinish={onSubmit}
                labelCol={{ span: 4 }}
                wrapperCol={{ span: 20 }}   
                layout="horizontal"
            >
                <Form.Item name="algorithm" required label="Algorithm" rules={[{ required: true, message: 'Please select a valid algorithm' }]}>
                    <Select placeholder="Select the source ">
                        {registrySigningKeyAlgorithms.map(entry => <Select.Option key={entry.key} value={entry.key}>{entry.display}</Select.Option>)}
                    </Select>
                </Form.Item> 
            </Form>
        </Modal>
    );
}