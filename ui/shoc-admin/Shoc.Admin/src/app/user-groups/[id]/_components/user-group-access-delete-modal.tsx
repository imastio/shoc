import { selfClient } from "@/clients/shoc";
import UserGroupAccessesClient from "@/clients/shoc/identity/user-group-accesses-client";
import StandardAlert from "@/components/general/standard-alert";
import { useApiAuthentication } from "@/providers/api-authentication/use-api-authentication";
import { Modal} from "antd";
import { useState } from "react";

export function UserGroupAccessDeleteModal({groupId, open, existing, onClose = () => {}, onSuccess = () => {}}: any){
    const { withToken } = useApiAuthentication();
    const [progress, setProgress] = useState(false);
    const [errors, setErrors] = useState([]);

    const onCloseWrapper = () => {
        setErrors([])
        onClose();
    }

    const onOk = async () => {
        setErrors([]);
        setProgress(true);

        const result = await withToken((token: string) => selfClient(UserGroupAccessesClient).update(token, groupId, {
            revoke: [existing.access] 
        } ));

        setProgress(false);

        if (result.error) {
            setErrors(result.payload.errors);
            return;
        }

        if (onSuccess) {
            onSuccess(existing);
        }

        onCloseWrapper();
    }

    return (
        <Modal
            cancelButtonProps={{ disabled: progress }}
            okButtonProps={{ danger: true, disabled: progress || !existing }}
            closable={false}
            open={open}
            onCancel={onCloseWrapper}
            destroyOnClose={true}
            forceRender
            title="Revoke the access"
            okText="Revoke"
            cancelText="Cancel"
            confirmLoading={progress}
            onOk={() => onOk()}
        >
            <StandardAlert 
                style={{ marginBottom: "8px" }} 
                message={`Could not revoke the access`} 
                errors={errors} 
                optional={true} />
            
            Are you sure you want to revoke the access to {existing?.access}
        </Modal>
    );
}