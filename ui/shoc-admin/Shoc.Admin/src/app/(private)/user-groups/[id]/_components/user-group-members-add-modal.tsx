import { selfClient } from "@/clients/shoc";
import UserGroupMembersClient from "@/clients/shoc/identity/user-group-members-client";
import StandardAlert from "@/components/general/standard-alert";
import { UserSelector } from "@/components/user-management/user-selector";
import { useApiAuthentication } from "@/providers/api-authentication/use-api-authentication";
import { Form, Modal} from "antd";
import { useEffect, useState } from "react";

export function UserGroupMembersAddModal({groupId, open, onClose = () => {}, onSuccess = () => {}}: any){
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
            groupId: groupId,
            userId: values.userId
        }

        const result = await withToken((token: string) => selfClient(UserGroupMembersClient).create(token, input));

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
            title="Add user to a group"
            confirmLoading={progress}
            onOk={() => form.submit()}
        >
            <StandardAlert style={{ marginBottom: "8px" }} message={`Could not add the selected user to group`} errors={errors} optional={true} />
            <Form
                form={form}
                preserve={false}
                onFinish={onSubmit}
                layout="horizontal"
            >
                <Form.Item name="userId" label="User" rules={[{ required: true, message: 'Please select a valid user' }]}>
                    <UserSelector placeholder="Select the user" />
                </Form.Item>
            </Form>
        </Modal>
    );
}