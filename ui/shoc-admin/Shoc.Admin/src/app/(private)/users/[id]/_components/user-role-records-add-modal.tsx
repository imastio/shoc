import { selfClient } from "@/clients/shoc";
import RoleMembersClient from "@/clients/shoc/identity/role-members-client";
import StandardAlert from "@/components/general/standard-alert";
import { RoleSelector } from "@/components/user-management/role-selector";
import { useApiAuthentication } from "@/providers/api-authentication/use-api-authentication";
import { Form, Modal} from "antd";
import { useEffect, useState } from "react";

export function UserRoleRecordAddModal({userId, open, onClose = () => {}, onSuccess = () => {}}: any){
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
            userId: userId,
            roleId: values.roleId
        }

        const result = await withToken((token: string) => selfClient(RoleMembersClient).create(token, input));

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
            groupId: null
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
            title="Add user to the role"
            confirmLoading={progress}
            onOk={() => form.submit()}
        >
            <StandardAlert style={{ marginBottom: "8px" }} message={`Could not add the user to selected role`} errors={errors} optional={true} />
            <Form
                form={form}
                preserve={false}
                onFinish={onSubmit}
                layout="horizontal"
            >
                <Form.Item name="roleId" label="Role" rules={[{ required: true, message: 'Please select a valid role' }]}>
                    <RoleSelector placeholder="Select the role" />
                </Form.Item>
            </Form>
        </Modal>
    );
}