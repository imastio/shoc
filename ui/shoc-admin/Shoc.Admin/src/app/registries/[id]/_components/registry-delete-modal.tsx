import { selfClient } from "@/clients/shoc";
import RegistriesClient from "@/clients/shoc/registry/registries-client";
import StandardAlert from "@/components/general/standard-alert";
import { useApiAuthentication } from "@/providers/api-authentication/use-api-authentication";
import { Modal } from "antd";
import { useState } from "react";

export default function RegistryDeleteModal(props: any){

    const { open, onClose, onSuccess, registryId } = props;

    const { withToken } = useApiAuthentication();
    const [progress, setProgress] = useState(false);
    const [errors, setErrors] = useState([]);

    const onOk = async () => {
        setErrors([]);
        setProgress(true);

        const result = await withToken((token: string) => selfClient(RegistriesClient).deleteById(token, registryId));

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
            title="Delete Registry"
            confirmLoading={progress}
            okText="Delete"
            cancelText="Cancel"
            onOk={() => onOk()}
        >
            <StandardAlert 
                style={{ marginBottom: "8px" }} 
                message="Could not delete the registry" 
                errors={errors} 
                optional={true} />
           Are you sure you want to delete the registry?
        </Modal>
    );

}
