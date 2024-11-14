import { selfClient } from "@/clients/shoc";
import WorkspaceMembersClient from "@/clients/shoc/workspace/workspace-members-client";
import StandardAlert from "@/components/general/standard-alert";
import { UserSelector } from "@/components/user-management/user-selector";
import { useApiAuthentication } from "@/providers/api-authentication/use-api-authentication";
import { workspaceRoles } from "@/well-known/workspaces";
import { Form, Modal, Select} from "antd";
import { useEffect, useState } from "react";

export default function WorkspaceMembersUpdateModal({workspaceId, existing, open, onClose = () => {}, onSuccess = () => {}}: any){
    const [form] = Form.useForm();
    const { withToken } = useApiAuthentication();
    const [progress, setProgress] = useState(false);
    const [errors, setErrors] = useState([]);

    const isOwner = existing?.role === 'owner';

    const onCloseWrapper = () => {
        setErrors([])
        onClose();
    }

    const onSubmit = async (values: any) => {
        setErrors([]);
        setProgress(true);

        const input = {
            workspaceId: workspaceId,
            userId: existing?.userId,
            role: values.role
        }

        const result = await withToken((token: string) => selfClient(WorkspaceMembersClient).updateById(token, workspaceId, existing?.id, input));

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
            userId: existing?.userId,
            role: existing?.role
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
            title="Edit member of workspace"
            confirmLoading={progress}
            onOk={() => form.submit()}
        >
            <StandardAlert style={{ marginBottom: "8px" }} message={`Could not add the selected user to workspace`} errors={errors} optional={true} />
            <Form
                disabled={isOwner}
                form={form}
                preserve={false}
                onFinish={onSubmit}
                layout="horizontal"
            >
                <Form.Item name="role" label="Role" rules={[{ required: true, message: 'Please select a valid role' }]}>
                    <Select placeholder="Select the role" disabled={isOwner}>
                        {workspaceRoles.map(entry => <Select.Option key={entry.key} disabled={entry.key === 'owner'} value={entry.key}>{entry.display}</Select.Option>)}
                    </Select>
                </Form.Item>  
            </Form>
        </Modal>
    );
}