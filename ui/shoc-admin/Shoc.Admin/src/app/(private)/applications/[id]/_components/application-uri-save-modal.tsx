import { Form, Input, Modal, Select } from "antd";
import { useEffect, useState } from "react";
import { applicationUriTypes } from "@/well-known/applications";
import { useApiAuthentication } from "@/providers/api-authentication/use-api-authentication";
import { selfClient } from "@/clients/shoc";
import ApplicationUrisClient from "@/clients/shoc/identity/application-uris-client";
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

export default function ApplicationUriSaveModal({ applicationId, edit = false, existing, open, onSuccess, onClose }: any) {

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
            (token: string) => selfClient(ApplicationUrisClient).updateById(token, applicationId, existing.id, input) : 
            (token: string) => selfClient(ApplicationUrisClient).create(token, applicationId, input);

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
            uri: existing?.uri
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
            title={`${edit ? "Update" : "Add"} application uri`}
            confirmLoading={progress}
            onOk={() => form.submit()}
        >
            <StandardAlert style={{ marginBottom: "8px" }} message={`Could not ${edit ? "update" : "add"} a uri record`} errors={errors} optional={true} />
            <Form
                form={form}
                preserve={false}
                onFinish={onSubmit}
                layout="horizontal"
                {...formItemLayout}
            >
                <Form.Item name="type" label="Type" rules={[{ required: true, message: 'Please select a valid type' }]}>
                    <Select placeholder="Select the type of the uri">
                        {applicationUriTypes.map(entry => <Select.Option key={entry.key} value={entry.key}>{entry.display}</Select.Option>)}
                    </Select>
                </Form.Item>
                <Form.Item name="uri" label="Uri" rules={[{required: true, message: 'Please enter a valid uri value'}]}>
                    <Input placeholder="Enter uri value" />
                </Form.Item>
            </Form>
        </Modal>
    );
}