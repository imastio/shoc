import { selfClient } from "@/clients/shoc";
import WorkspaceInvitationsClient from "@/clients/shoc/workspace/workspace-invitations-client";
import StandardAlert from "@/components/general/standard-alert";
import { useApiAuthentication } from "@/providers/api-authentication/use-api-authentication";
import { workspaceRoles } from "@/well-known/workspaces";
import { DatePicker, Form, Modal, Select } from "antd";
import dayjs from "dayjs";
import { useEffect, useState } from "react";

export default function WorkspaceInvitationsUpdateModal({ workspaceId, existing, open, onClose = () => { }, onSuccess = () => { } }: any) {
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
            id: existing?.id,
            role: values.role,
            expiration: values.expiration ? dayjs.utc(values.expiration) : null
        }

        const result = await withToken((token: string) => selfClient(WorkspaceInvitationsClient).updateById(token, workspaceId, existing?.id, input));

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
            role: existing?.role,
            expiration: existing?.expiration ? dayjs.utc(existing.expiration).local() : null
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
            title="Edit workspace invitation"
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
                <Form.Item name="role" label="Role" rules={[{ required: true, message: 'Please select a valid role' }]}>
                    <Select placeholder="Select the role">
                        {workspaceRoles.map(entry => <Select.Option key={entry.key} disabled={entry.key === 'owner'} value={entry.key}>{entry.display}</Select.Option>)}
                    </Select>
                </Form.Item>
                <Form.Item name="expiration" required label="Expires" rules={[{required: true, message: 'Please enter a valid expiration date'}]}>
                    <DatePicker style={{ width: '100%' }} showTime />
                </Form.Item>
            </Form>
        </Modal>
    );
}