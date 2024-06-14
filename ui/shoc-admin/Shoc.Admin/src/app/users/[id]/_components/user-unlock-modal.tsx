import { selfClient } from "@/clients/shoc";
import UsersClient from "@/clients/shoc/identity/users-client";
import StandardAlert from "@/components/general/standard-alert";
import { useApiAuthentication } from "@/providers/api-authentication/use-api-authentication";
import { Modal } from "antd";
import { useState } from "react";

export default function UserUnlockModal(props: any) {

    const { userId, open, onClose, onSuccess } = props;

    const { withToken } = useApiAuthentication();
    const [progress, setProgress] = useState(false);
    const [errors, setErrors] = useState([]);

    const onCloseWrapper = () => {

        setErrors([])

        if (onClose) {
            onClose();
        }
    }

    const onSubmit = async () => {
        setErrors([]);
        setProgress(true);

        const result = await withToken((token: string) => selfClient(UsersClient).updateLockoutById(token, userId, { lockedUntil: null }));

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
            title="Unlock User"
            confirmLoading={progress}
            onOk={onSubmit}
        >
            <StandardAlert style={{ marginBottom: "8px" }} message="Error while unlocking the user" errors={errors} optional={true} />
            Are you sure you want to unlock the user?
        </Modal>
    );

}
