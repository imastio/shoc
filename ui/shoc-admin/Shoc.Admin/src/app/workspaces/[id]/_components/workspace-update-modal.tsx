import { selfClient } from "@/clients/shoc";
import WorkspacesClient from "@/clients/shoc/workspace/workspaces-client";
import StandardAlert from "@/components/general/standard-alert";
import { useApiAuthentication } from "@/providers/api-authentication/use-api-authentication";
import { workspaceNamePattern, workspaceStatuses } from "@/well-known/workspaces";
import { Form, Input, Modal, Select } from "antd";
import { useEffect } from "react";
import { useState } from "react";

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

export default function WorkspaceUpdateModal(props: any) {

    const { onClose, onSuccess, existing = {}, open } = props;

    const [form] = Form.useForm();
    const { withToken } = useApiAuthentication();
    const [progress, setProgress] = useState(false);
    const [errors, setErrors] = useState([]);

    const onCloseWrapper = () => {

        setErrors([]);

        if (onClose) {
            onClose();
        }
    }

    const onSubmit = async (values: any) => {
        setErrors([]);
        setProgress(true);

        const result = await withToken((token: string) => selfClient(WorkspacesClient).updateById(token, existing.id, { ...values }));

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

        if (!open) {
            return;
        }

        form.setFieldsValue({
            ...existing
        });

    }, [form, existing, open])

    return (
        <Modal
            closable={false}
            cancelButtonProps={{ disabled: progress }}
            open={open}
            forceRender
            onCancel={onCloseWrapper}
            destroyOnClose={true}
            title="Update Workspace"
            confirmLoading={progress}
            onOk={() => form.submit()}
        >
            <StandardAlert style={{ marginBottom: "8px" }} message="Could not update the workspace" errors={errors} optional={true} />
            <Form form={form} onFinish={onSubmit} layout="horizontal" {...formItemLayout}>
                <Form.Item name="name" label="Name" rules={[
                    { required: true, message: 'Please enter valid name' },
                    { pattern: workspaceNamePattern, message: 'The name is invalid' }
                    ]}>
                    <Input placeholder="Please enter the name" />
                </Form.Item>          
                <Form.Item name="description" label="Description" rules={[{ required: true, min: 2, max: 128, message: 'Please enter valid description' }]}>
                    <Input placeholder="Please enter the description" />
                </Form.Item>  
                <Form.Item name="status" label="Status" rules={[{ required: true, message: 'Please select a valid status' }]}>
                    <Select placeholder="Select the status ">
                        {workspaceStatuses.map(entry => <Select.Option key={entry.key} value={entry.key}>{entry.display}</Select.Option>)}
                    </Select>
                </Form.Item>       
            </Form>
        </Modal>
    );

}
