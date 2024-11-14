import { selfClient } from "@/clients/shoc";
import WorkspaceInvitationsClient from "@/clients/shoc/workspace/workspace-invitations-client";
import StandardAlert from "@/components/general/standard-alert";
import { UserSelector } from "@/components/user-management/user-selector";
import { useApiAuthentication } from "@/providers/api-authentication/use-api-authentication";
import { workspaceRoles } from "@/well-known/workspaces";
import { Form, Input, Modal, Select} from "antd";
import { useEffect, useState } from "react";

export default function WorkspaceInvitationsAddModal({workspaceId, open, onClose = () => {}, onSuccess = () => {}}: any){
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
            email: values.email,
            role: values.role,
            invitedBy: values.invitedBy,
        }

        const result = await withToken((token: string) => selfClient(WorkspaceInvitationsClient).create(token, workspaceId, input));

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
            email: null,
            role: null,
            invitedBy: null
        });

    }, [open, form])

    return (
        <Modal
            closable={false}
            cancelButtonProps={{ disabled: progress }}
            open={open}
            onCancel={onCloseWrapper}
            destroyOnClose={true}
            forceRender
            title="Invite new member to the workspace"
            confirmLoading={progress}
            onOk={() => form.submit()}
        >
            <StandardAlert style={{ marginBottom: "8px" }} message={`Could not invite user to the workspace`} errors={errors} optional={true} />
            <Form
                form={form}
                preserve={false}
                onFinish={onSubmit}
                layout="horizontal"
            >
                <Form.Item name="email" label="Email" rules={[{ required: true, type: 'email', message: 'Please enter a valid email' }]}>
                    <Input placeholder="john@example.com" />
                </Form.Item>
                <Form.Item name="role" label="Role" rules={[{ required: true, message: 'Please select a valid role' }]}>
                    <Select placeholder="Select the role ">
                        {workspaceRoles.filter(entry => entry.key !== 'owner').map(entry => <Select.Option key={entry.key} value={entry.key}>{entry.display}</Select.Option>)}
                    </Select>
                </Form.Item>  
                <Form.Item name="invitedBy" label="From" rules={[{ required: true, message: 'Please select a valid user' }]}>
                    <UserSelector placeholder="Select the inviting user" />
                </Form.Item>
            </Form>
        </Modal>
    );
}