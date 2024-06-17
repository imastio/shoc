import { selfClient } from "@/clients/shoc";
import MailingProfilesClient from "@/clients/shoc/settings/mailing-profiles-client";
import StandardAlert from "@/components/general/standard-alert";
import { useApiAuthentication } from "@/providers/api-authentication/use-api-authentication";
import { Modal } from "antd";
import { useState } from "react";

export default function MailingProfileDeleteModal(props: any){

    const { open, onClose, onSuccess, profileId } = props;

    const { withToken } = useApiAuthentication();
    const [progress, setProgress] = useState(false);
    const [errors, setErrors] = useState([]);


    const okOk = async () => {
        setErrors([]);
        setProgress(true);

        const result = await withToken((token: string) => selfClient(MailingProfilesClient).deleteById(token, profileId));

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
            destroyOnClose={true}
            title="Delete Mailing Profile"
            confirmLoading={progress}
            okText="Delete"
            cancelText="Cancel"
            onOk={() => okOk()}
        >
            <StandardAlert 
                style={{ marginBottom: "8px" }} 
                message="Could not delete the profile" 
                errors={errors} 
                optional={true} />
           Are you sure you want to delete the profile?
        </Modal>
    );

};