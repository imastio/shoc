import { DatePicker, Form, Input, Modal, Select } from "antd";
import dayjs from "dayjs";
import { useEffect, useState } from "react";
import { applicationSecretTypes } from "@/well-known/applications";
import { useApiAuthentication } from "@/providers/api-authentication/use-api-authentication";
import { selfClient } from "@/clients/shoc";
import ApplicationSecretsClient from "@/clients/shoc/identity/application-secrets-client";
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

export default function ApplicationSecretSaveModal({ applicationId, edit = false, existing, open, onSuccess, onClose }: any) {

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
            ...values,
            expiration: values.expiration ? dayjs.utc(values.expiration) : null
        }

        const action = edit ? 
            (token: string) => selfClient(ApplicationSecretsClient).updateById(token, applicationId, existing.id, input) : 
            (token: string) => selfClient(ApplicationSecretsClient).create(token, applicationId, input);

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
            description: existing?.description,
            expiration: existing?.expiration ? dayjs.utc(existing.expiration).local() : null
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
            title={`${edit ? "Update" : "Add"} application secret`}
            confirmLoading={progress}
            onOk={() => form.submit()}
        >
            <StandardAlert style={{ marginBottom: "8px" }} message={`Could not ${edit ? "update" : "add"} a secret record`} errors={errors} optional={true} />
            <Form
                form={form}
                preserve={false}
                onFinish={onSubmit}
                layout="horizontal"
                {...formItemLayout}
            >
                <Form.Item name="type" label="Type" rules={[{ required: true, message: 'Please select a valid type' }]}>
                    <Select placeholder="Select the type of the secret">
                        {applicationSecretTypes.map(entry => <Select.Option key={entry.key} value={entry.key}>{entry.display}</Select.Option>)}
                    </Select>
                </Form.Item>
                <Form.Item name="value" label="Value" rules={[{required: true, message: 'Please enter a valid value'}]}>
                    <Input placeholder="Enter secret value" />
                </Form.Item>
                <Form.Item name="description" label="Description" rules={[{required: true, message: 'Please enter a valid description'}]}>
                    <Input placeholder="Enter description for the secret" />
                </Form.Item>
                <Form.Item name="expiration" label="Expiration Date" rules={[{required: true, message: 'Please enter a valid expiration date'}]}>
                    <DatePicker placeholder="YYYY-MM-DD" style={{width: '100%'}} showTime={true} />
                </Form.Item>
            </Form>
        </Modal>
    );
}