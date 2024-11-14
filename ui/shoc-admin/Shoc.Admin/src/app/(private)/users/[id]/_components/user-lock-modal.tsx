import { DatePicker, Form, Modal } from "antd";
import { useState } from "react";
import dayjs from "@/extended/time";
import { useEffect } from "react";
import { useApiAuthentication } from "@/providers/api-authentication/use-api-authentication";
import { selfClient } from "@/clients/shoc";
import UsersClient from "@/clients/shoc/identity/users-client";
import StandardAlert from "@/components/general/standard-alert";

export default function UserLockModal(props: any) {

    const { userId, open, onClose, onSuccess } = props;

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

        const result = await withToken((token: string) => selfClient(UsersClient).updateLockoutById(token, userId, { lockedUntil: dayjs.utc(values.lockedUntil) }));

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

        form.setFieldsValue({ lockedUntil: dayjs.utc().add(1, 'hour').local() });

    }, [open, form])

    return (
        <Modal
            closable={false}
            cancelButtonProps={{ disabled: progress }}
            forceRender
            open={open}
            onCancel={onCloseWrapper}
            title="Lock User"
            confirmLoading={progress}
            onOk={() => form.submit()}
        >
            <StandardAlert style={{ marginBottom: "8px" }} message="Error while locking user" errors={errors} optional={true} />
            <Form form={form} layout="horizontal" onFinish={onSubmit}>
                <Form.Item name="lockedUntil" label="Lock user until:" rules={[{ required: true, message: "Please enter a valid lock time" }]}>
                    <DatePicker
                        showHour
                        showMinute
                        format="YYYY-MM-DD HH:mm"
                    />
                </Form.Item>
            </Form>
        </Modal>
    );

}
