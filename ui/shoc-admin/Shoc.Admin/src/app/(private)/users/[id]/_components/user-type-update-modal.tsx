import { selfClient } from "@/clients/shoc";
import UsersClient from "@/clients/shoc/identity/users-client";
import StandardAlert from "@/components/general/standard-alert";
import { useApiAuthentication } from "@/providers/api-authentication/use-api-authentication";
import { userTypes } from "@/well-known/user-types";
import { Form, Modal, Radio } from "antd";
import { useEffect } from "react";
import { useState } from "react";

export default function UserTypeUpdateModal(props: any) {

    const { user, open, onClose, onSuccess } = props;

    const [form] = Form.useForm();
    const { withToken } = useApiAuthentication();
    const [progress, setProgress] = useState(false);
    const [errors, setErrors] = useState([]);

    const onCloseWrapper = () => {

        setErrors([]);

        if(onClose){
            onClose();
        }
    }

    const onSubmit = async (values: any) => {
        setErrors([]);
        setProgress(true);

        const result = await withToken((token: string) => selfClient(UsersClient).updateTypeById(token, user.id, {type: values.type}));

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

        form.setFieldsValue({ type: user.type })
    }, [open, user.type, form])

    return (
        <Modal
            closable={false}
            cancelButtonProps={{ disabled: progress }}
            open={open}
            onCancel={onCloseWrapper}
            title="Update Type"
            confirmLoading={progress}
            onOk={() => form.submit()}
        >
            <StandardAlert style={{ marginBottom: "8px" }} message="Error while updating type" errors={errors} optional={true} />
            <Form form={form} layout="vertical" onFinish={onSubmit}>

            <Form.Item name="type">
                <Radio.Group>
                    {userTypes.map(t => <Radio.Button key={t.key} value={t.key}>{t.display}</Radio.Button>)}
                </Radio.Group>
            </Form.Item>

            </Form>
        </Modal>
    );

}
