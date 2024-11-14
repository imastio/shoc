import { selfClient } from "@/clients/shoc";
import UserGroupsClient from "@/clients/shoc/identity/user-groups-client";
import StandardAlert from "@/components/general/standard-alert";
import { useApiAuthentication } from "@/providers/api-authentication/use-api-authentication";
import { Modal } from "antd";
import { useState } from "react";

export default function UserGroupDeleteModal(props: any){

    const { open, onClose, onSuccess, groupId } = props;

    const { withToken } = useApiAuthentication();
    const [progress, setProgress] = useState(false);
    const [errors, setErrors] = useState([]);

    const okOk = async () => {
        setErrors([]);
        setProgress(true);

        const result = await withToken((token: string) => selfClient(UserGroupsClient).deleteById(token, groupId));

        setProgress(false);

        if (result.error) {
            setErrors(result.payload.errors);
            return;
        }

        if (onSuccess) {
            onSuccess(result.payload);
        }

        onClose();
    }
 
    return (
        <Modal
            closable={false}
            cancelButtonProps={{ disabled: progress }}
            okButtonProps={{ danger: true }}
            open={open}
            onCancel={onClose}
            forceRender
            title="Delete Group"
            confirmLoading={progress}
            okText="Delete"
            cancelText="Cancel"
            onOk={() => okOk()}
        >
            <StandardAlert 
                style={{ marginBottom: "8px" }} 
                message="Could not delete the group" 
                errors={errors} 
                optional={true} />
           Are you sure you want to delete the group?
        </Modal>
    );

}
