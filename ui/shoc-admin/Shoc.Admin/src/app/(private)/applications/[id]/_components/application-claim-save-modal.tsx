import { selfClient } from "@/clients/shoc";
import ApplicationClaimsClient from "@/clients/shoc/identity/application-claims-client";
import StandardAlert from "@/components/general/standard-alert";
import { useApiAuthentication } from "@/providers/api-authentication/use-api-authentication";
import { Form, Input, Modal } from "antd";
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

export default function ApplicationClaimSaveModal({ applicationId, edit = false, existing, open, onSuccess, onClose }: any) {

    const [form] = Form.useForm();
    const { withToken } = useApiAuthentication();
    const [progress, setProgress] = useState(false);
    const [errors, setErrors] = useState([]);

    const onCloseWrapper = () => {
        setErrors([]);
        onClose();
    }

    const onSubmit = async (values: any) => {
        setErrors([]);
        setProgress(true);

        const input = {
            ...values
        }

        const action = edit ? 
            (token: string) => selfClient(ApplicationClaimsClient).updateById(token, applicationId, existing.id, input) : 
            (token: string) => selfClient(ApplicationClaimsClient).create(token, applicationId, input);

        const result = await withToken(action);

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
            type: existing?.type,
            value: existing?.value,
            valueType: existing?.valueType
        })

    }, [existing, open, form])


    return (
        <Modal
            closable={false}
            cancelButtonProps={{ disabled: progress }}
            open={open}
            onCancel={onCloseWrapper}
            destroyOnClose={true}
            forceRender
            title={`${edit ? "Update" : "Add"} application claim`}
            confirmLoading={progress}
            onOk={() => form.submit()}
        >
            <StandardAlert style={{ marginBottom: "8px" }} message={`Could not ${edit ? "update" : "add"} a claim record`} errors={errors} optional={true} />
            <Form
                form={form}
                preserve={false}
                onFinish={onSubmit}
                layout="horizontal"
                {...formItemLayout}
            >
                <Form.Item name="type" label="Type" rules={[{ required: true, message: 'Please enter a valid claim type' }]}>
                    <Input placeholder="Enter claim type" />
                </Form.Item>
                <Form.Item name="value" label="Value" rules={[{ required: true, message: 'Please enter a valid claim value' }]}>
                    <Input placeholder="Enter claim value" />
                </Form.Item>
                <Form.Item name="valueType" label="Value Type">
                    <Input placeholder="Optional: Value type for the claim" />
                </Form.Item>
            </Form>
        </Modal>
    );
}