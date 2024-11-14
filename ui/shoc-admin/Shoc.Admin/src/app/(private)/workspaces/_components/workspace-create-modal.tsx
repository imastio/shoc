import { selfClient } from "@/clients/shoc";
import WorkspacesClient from "@/clients/shoc/workspace/workspaces-client";
import StandardAlert from "@/components/general/standard-alert";
import { UserSelector } from "@/components/user-management/user-selector";
import { useApiAuthentication } from "@/providers/api-authentication/use-api-authentication";
import { workspaceNamePattern, workspaceTypes } from "@/well-known/workspaces";
import { Form, Input, Modal, Select } from "antd";
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

export default function WorkspaceCreateModal(props: any){

    const { open, onClose, onSuccess } = props;

    const [form] = Form.useForm();
    const { withToken } = useApiAuthentication();
    const [progress, setProgress] = useState(false);
    const [errors, setErrors] = useState([]);

    const onCloseWrapper = () => {
        form.resetFields();
        setErrors([]);
        if(onClose){
            onClose();
        }
    }

    const onSubmit = async (values: any) => {
        setErrors([]);
        setProgress(true);

        const result = await withToken((token: string) => selfClient(WorkspacesClient).create(token, { ...values }));

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
    }, [form, open])

    return (
        <Modal
            closable={false}
            cancelButtonProps={{ disabled: progress }}
            open={open}
            forceRender
            onCancel={onClose}
            title="Create Workspace"
            confirmLoading={progress}
            onOk={() => form.submit()}
        >
            <StandardAlert style={{ marginBottom: "8px" }} message="Could not create a workspace" errors={errors} optional={true} />
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
                <Form.Item name="type" label="Type" rules={[{ required: true, message: 'Please select a valid type' }]}>
                    <Select placeholder="Select the type ">
                        {workspaceTypes.map(entry => <Select.Option key={entry.key} value={entry.key}>{entry.display}</Select.Option>)}
                    </Select>
                </Form.Item>
                <Form.Item name="createdBy" label="Creator" rules={[{ required: true, message: 'Please select a creator' }]}>
                    <UserSelector placeholder="Select the creator" />
                </Form.Item>
            </Form>
        </Modal>
    );

}
