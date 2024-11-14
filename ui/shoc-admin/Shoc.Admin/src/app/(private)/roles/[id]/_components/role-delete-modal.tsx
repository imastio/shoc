import { selfClient } from "@/clients/shoc";
import RolesClient from "@/clients/shoc/identity/roles-client";
import StandardAlert from "@/components/general/standard-alert";
import { useApiAuthentication } from "@/providers/api-authentication/use-api-authentication";
import { Modal } from "antd";
import { useState } from "react";

export default function RoleDeleteModal(props: any){

    const { open, onClose, onSuccess, roleId } = props;

    const { withToken } = useApiAuthentication();
    const [progress, setProgress] = useState(false);
    const [errors, setErrors] = useState([]);

    const onOk = async () => {
        setErrors([]);
        setProgress(true);

        const result = await withToken((token: string) => selfClient(RolesClient).deleteById(token, roleId));

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
            title="Delete Role"
            confirmLoading={progress}
            okText="Delete"
            cancelText="Cancel"
            onOk={() => onOk()}
        >
            <StandardAlert 
                style={{ marginBottom: "8px" }} 
                message="Could not delete the role" 
                errors={errors} 
                optional={true} />
           Are you sure you want to delete the role?
        </Modal>
    );

}
