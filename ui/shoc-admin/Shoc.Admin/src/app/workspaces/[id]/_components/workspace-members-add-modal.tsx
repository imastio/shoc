import { selfClient } from "@/clients/shoc";
import WorkspaceMembersClient from "@/clients/shoc/workspace/workspace-members-client";
import StandardAlert from "@/components/general/standard-alert";
import { UserSelector } from "@/components/user-management/user-selector";
import { useApiAuthentication } from "@/providers/api-authentication/use-api-authentication";
import { workspaceRoles } from "@/well-known/workspaces";
import { Form, Modal, Select} from "antd";
import { useEffect, useState } from "react";

export default function WorkspaceMembersAddModal({workspaceId, open, onClose = () => {}, onSuccess = () => {}}: any){
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
            userId: values.userId,
            role: values.role
        }

        const result = await withToken((token: string) => selfClient(WorkspaceMembersClient).create(token, workspaceId, input));

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
            userId: null
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
            title="Add member to workspace"
            confirmLoading={progress}
            onOk={() => form.submit()}
        >
            <StandardAlert style={{ marginBottom: "8px" }} message={`Could not add the selected user to workspace`} errors={errors} optional={true} />
            <Form
                form={form}
                preserve={false}
                onFinish={onSubmit}
                layout="horizontal"
            >
                <Form.Item name="userId" label="User" rules={[{ required: true, message: 'Please select a valid user' }]}>
                    <UserSelector placeholder="Select the user" />
                </Form.Item>
                <Form.Item name="role" label="Role" rules={[{ required: true, message: 'Please select a valid role' }]}>
                    <Select placeholder="Select the role ">
                        {workspaceRoles.filter(entry => entry.key !== 'owner').map(entry => <Select.Option key={entry.key} value={entry.key}>{entry.display}</Select.Option>)}
                    </Select>
                </Form.Item>  
            </Form>
        </Modal>
    );
}