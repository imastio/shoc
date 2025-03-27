import { selfClient } from "@/clients/shoc";
import OidcProvidersClient from "@/clients/shoc/identity/oidc-providers-client";
import StandardAlert from "@/components/general/standard-alert";
import { useApiAuthentication } from "@/providers/api-authentication/use-api-authentication";
import { Modal } from "antd";
import { useState } from "react";

export default function OidcProviderDeleteModal(props: any){

    const { open, onClose, onSuccess, providerId } = props;

    const { withToken } = useApiAuthentication();
    const [progress, setProgress] = useState(false);
    const [errors, setErrors] = useState([]);

    const onOk = async () => {
        setErrors([]);
        setProgress(true);

        const result = await withToken((token: string) => selfClient(OidcProvidersClient).deleteById(token, providerId));

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
            title="Delete Provider"
            confirmLoading={progress}
            okText="Delete"
            cancelText="Cancel"
            onOk={() => onOk()}
        >
            <StandardAlert 
                style={{ marginBottom: "8px" }} 
                message="Could not delete the provider" 
                errors={errors} 
                optional={true} />
           Are you sure you want to delete the provider?
        </Modal>
    );

}
