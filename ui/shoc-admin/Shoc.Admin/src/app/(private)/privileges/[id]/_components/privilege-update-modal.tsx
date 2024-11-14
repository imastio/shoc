import { selfClient } from "@/clients/shoc";
import PrivilegesClient from "@/clients/shoc/identity/privileges-client";
import StandardAlert from "@/components/general/standard-alert";
import { useApiAuthentication } from "@/providers/api-authentication/use-api-authentication";
import { Form, Input, Modal } from "antd";
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

export default function PrivilegeUpdateModal(props: any) {

    const { onClose, onSuccess, privilege = {}, open } = props;

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

        const result = await withToken((token: string) => selfClient(PrivilegesClient).updateById(token, privilege.id, { ...values }));

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
            ...privilege
        });

    }, [form, privilege, open])

    return (
        <Modal
            closable={false}
            cancelButtonProps={{ disabled: progress }}
            open={open}
            forceRender
            onCancel={onCloseWrapper}
            destroyOnClose={true}
            title="Update Privilege"
            confirmLoading={progress}
            onOk={() => form.submit()}
        >
            <StandardAlert style={{ marginBottom: "8px" }} message="Could not update the privilege" errors={errors} optional={true} />
            <Form form={form} onFinish={onSubmit} layout="horizontal" {...formItemLayout}>
                <Form.Item name="name" label="Name" rules={[{ required: true, min: 2, message: 'Please enter valid name' }]}>
                    <Input placeholder="Please enter valid name" />
                </Form.Item>
                <Form.Item name="category" label="Category" rules={[{ required: true,  min: 2, max: 256, message: 'Please enter valid privilege category' }]}>
                    <Input placeholder="Please enter the category" />
                </Form.Item>          
                <Form.Item name="description" label="Description" rules={[{  min: 2, max: 256, message: 'Please enter valid privilege description' }]}>
                    <Input placeholder="Please enter the description" />
                </Form.Item>         
            </Form>
        </Modal>
    );

}
