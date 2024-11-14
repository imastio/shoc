import { selfClient } from "@/clients/shoc";
import MailingProfilesClient from "@/clients/shoc/settings/mailing-profiles-client";
import StandardAlert from "@/components/general/standard-alert";
import { useApiAuthentication } from "@/providers/api-authentication/use-api-authentication";
import { Form, Input, Modal } from "antd";
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

export default function MailingProfileUpdatePasswordModal(props: any){

    const { open, onClose, onSuccess, profileId } = props;

    const [form] = Form.useForm();
    const { withToken } = useApiAuthentication();
    const [progress, setProgress] = useState(false);
    const [errors, setErrors] = useState([]);

    const onCloseWrapper = () => {
        setErrors([]);
        form.resetFields();
        onClose();
    }

    const onSubmit = async (values: any) => {
        setErrors([]);
        setProgress(true);

        const result = await withToken((token: string) => selfClient(MailingProfilesClient).updatePasswordById(token, profileId, {password: values.password ? values.password : null}));

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

    return (
        <Modal
            closable={false}
            cancelButtonProps={{ disabled: progress }}
            open={open}
            onCancel={onCloseWrapper}
            destroyOnClose={true}
            title="Update Mailing Profile Password"
            confirmLoading={progress}
            onOk={() => form.submit()}
        >
            <StandardAlert style={{ marginBottom: "8px" }} message="Could not update the password of mailing profile" errors={errors} optional={true} />
            <Form form={form} onFinish={onSubmit} layout="horizontal" {...formItemLayout} initialValues={{}}>
                <Form.Item name="password" label="Password" rules={[{ required: false, message: 'Please enter a valid password' }]}>
                    <Input type="password" placeholder="Please enter new password" />
                </Form.Item>
            </Form>
        </Modal>
    );
};