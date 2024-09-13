import { selfClient } from "@/clients/shoc";
import RolePrivilegesClient from "@/clients/shoc/identity/role-privileges-client";
import StandardAlert from "@/components/general/standard-alert";
import { PrivilegeSelector } from "@/components/user-management/privilege-selector";
import { useApiAuthentication } from "@/providers/api-authentication/use-api-authentication";
import { Form, Modal} from "antd";
import { useEffect, useMemo, useState } from "react";

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

export function RolePrivilegeRecordsAddModal({ roleId, open, granted, onClose = () => {}, onSuccess = () => {}}: any){
    const [form] = Form.useForm();
    const { withToken } = useApiAuthentication();
    const [progress, setProgress] = useState(false);
    const [errors, setErrors] = useState([]);

    const filterGranted = useMemo(() => granted.map((item: any) => item.id), [granted]);

    const onCloseWrapper = () => {
        setErrors([])
        onClose();
    }

    const onSubmit = async (values: any) => {
        setErrors([]);
        setProgress(true);

        const input = {
            roleId: roleId,
            privilegeId: values.privilegeId
        }

        const result = await withToken((token: string) => selfClient(RolePrivilegesClient).create(token, input));
        
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
            privilegeId: null
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
            title="Add privilege to role"
            confirmLoading={progress}
            onOk={() => form.submit()}
        >
            <StandardAlert style={{ marginBottom: "8px" }} message={`Could not add the privilege to role`} errors={errors} optional={true} />
            <Form
                {...formItemLayout}
                form={form}
                preserve={false}
                onFinish={onSubmit}
                layout="horizontal"
            >           
                <Form.Item name="privilegeId" label="Privilege" rules={[{ required: true, message: 'Please select the privilege' }]}>
                    <PrivilegeSelector filter={filterGranted} placeholder="Select the privileges to add"/>
                </Form.Item>
            </Form>
        </Modal>
    );
}