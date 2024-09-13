import { selfClient } from "@/clients/shoc";
import RegistryCredentialsClient from "@/clients/shoc/registry/registry-credentials-client";
import StandardAlert from "@/components/general/standard-alert";
import { UserSelector } from "@/components/user-management/user-selector";
import { WorkspaceSelector } from "@/components/workspace/workspace-selector";
import { useApiAuthentication } from "@/providers/api-authentication/use-api-authentication";
import { registryCredentialSources } from "@/well-known/registries";
import { Form, Input, Modal, Select, Switch} from "antd";
import { useEffect, useState } from "react";

export default function RegistryCredentialsAddModal({registryId, open, onClose = () => {}, onSuccess = () => {}}: any){
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
            workspaceId: values.workspaceId,
            userId: values.userId,
            source: values.source,
            username: values.username,
            password: values.password,
            email: values.email,
            pullAllowed: values.pullAllowed,
            pushAllowed: values.pushAllowed
        }

        const result = await withToken((token: string) => selfClient(RegistryCredentialsClient).create(token, registryId, input));

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
            title="Add registry credential"
            confirmLoading={progress}
            onOk={() => form.submit()}
        >
            <StandardAlert style={{ marginBottom: "8px" }} message={`Could not create a credential for the workspace`} errors={errors} optional={true} />
            <Form
                form={form}
                preserve={false}
                onFinish={onSubmit}
                labelCol={{ span: 6 }}
                wrapperCol={{ span: 18 }}   
                layout="horizontal"
            >
                <Form.Item name="workspaceId" label="Workspace">
                    <WorkspaceSelector allowClear placeholder="Select the workspace (optional)" />
                </Form.Item>
                <Form.Item name="userId" label="User">
                    <UserSelector allowClear placeholder="Select the user (optional)" />
                </Form.Item>
                <Form.Item name="source" required label="Source" rules={[{ required: true, message: 'Please select a valid source' }]}>
                    <Select placeholder="Select the source ">
                        {registryCredentialSources.filter(entry => entry.key === 'manual').map(entry => <Select.Option key={entry.key} value={entry.key}>{entry.display}</Select.Option>)}
                    </Select>
                </Form.Item> 
                <Form.Item name="username" label="Username" rules={[{ required: true, max: 256, message: 'Please enter valid username' }]}>
                    <Input placeholder="Please enter a username " />
                </Form.Item>
                <Form.Item name="password" label="Password" rules={[{ required: true, max: 256, message: 'Please enter valid password' }]} >
                    <Input type="password" placeholder="Please enter a password" />
                </Form.Item>
                <Form.Item name="email" label="Email" rules={[{ required: false, max: 256, type: 'email', message: 'Please enter valid Email' }]}>
                    <Input placeholder="Please enter an email (optional)" />
                </Form.Item> 
                <Form.Item name="pullAllowed" label="Pull Access" valuePropName="checked">
                    <Switch checkedChildren="Allow" unCheckedChildren="Disallow" />
                </Form.Item>
                <Form.Item name="pushAllowed" label="Pull Access" valuePropName="checked">
                    <Switch checkedChildren="Allow" unCheckedChildren="Disallow" />
                </Form.Item>
            </Form>
        </Modal>
    );
}