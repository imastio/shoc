import { selfClient } from "@/clients/shoc";
import RegistryCredentialsClient from "@/clients/shoc/registry/registry-credentials-client";
import StandardAlert from "@/components/general/standard-alert";
import { useApiAuthentication } from "@/providers/api-authentication/use-api-authentication";
import { Modal} from "antd";
import { useState } from "react";

export default function RegistryCredentialsDeleteModal({registryId, existing, open, onClose = () => {}, onSuccess = () => {}}: any){
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

        const result = await withToken((token: string) => selfClient(RegistryCredentialsClient).deleteById(token, registryId, existing?.id ));

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
            cancelButtonProps={{ disabled: progress }}
            okButtonProps={{ danger: true, disabled: progress || !existing }}
            closable={false}
            open={open}
            onCancel={onCloseWrapper}
            destroyOnClose={true}
            forceRender
            title="Delete credential from the registry"
            okText="Delete"
            cancelText="Cancel"
            confirmLoading={progress}
            onOk={() => onOk()}
        >
            <StandardAlert 
                style={{ marginBottom: "8px" }} 
                message={`Could not delete the credential from the registry`} 
                errors={errors} 
                optional={true} />
            
            Are you sure you want to delete the credential from the registry?
        </Modal>
    );
}