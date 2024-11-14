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

export default function MailingProfileUpdateApiSecretModal(props: any){

    const { open, onClose, onSuccess, profileId } = props;

    const [form] = Form.useForm();
    const { withToken } = useApiAuthentication();
    const [progress, setProgress] = useState(false);
    const [errors, setErrors] = useState([]);

    const onCloseWrapper = () => {
        form.resetFields();
        setErrors([]);
        onClose();
    }

    const onSubmit = async (values: any) => {
        setErrors([]);
        setProgress(true);

        const result = await withToken((token: string) => selfClient(MailingProfilesClient).updateApiSecretById(token, profileId, { apiSecret: values.apiSecret ? values.apiSecret : null }));

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
            forceRender
            destroyOnClose={true}
            title="Update API Secret of Mailing Profile"
            confirmLoading={progress}
            onOk={() => form.submit()}
        >
            <StandardAlert style={{ marginBottom: "8px" }} message="Could not update the profile's API secret" errors={errors} optional={true} />
            <Form form={form} onFinish={onSubmit} layout="horizontal" {...formItemLayout} >
                <Form.Item name="apiSecret" label="API Secret" rules={[{ required: false, message: 'Please enter a valid API secret' }]}>
                    <Input type="password" placeholder="Please enter new API secret" />
                </Form.Item>
            </Form>
        </Modal>
    );
};