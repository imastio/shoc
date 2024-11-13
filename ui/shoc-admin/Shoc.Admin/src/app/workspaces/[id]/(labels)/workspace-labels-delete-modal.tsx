import { selfClient } from "@/clients/shoc";
import LabelsClient from "@/clients/shoc/job/labels-client";
import StandardAlert from "@/components/general/standard-alert";
import { useApiAuthentication } from "@/providers/api-authentication/use-api-authentication";
import { Modal} from "antd";
import { useState } from "react";

export default function WorkspaceLabelsDeleteModal({workspaceId, existing, open, onClose = () => {}, onSuccess = () => {}}: any){
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

        const result = await withToken((token: string) => selfClient(LabelsClient).deleteById(token, workspaceId, existing?.id ));

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
            title="Delete the label from the workspace"
            okText="Delete"
            cancelText="Cancel"
            confirmLoading={progress}
            onOk={() => onOk()}
        >
            <StandardAlert 
                style={{ marginBottom: "8px" }} 
                message={`Could not delete the label from the workspace`} 
                errors={errors} 
                optional={true} />
            
            Are you sure you want to delete the label from the workspace?
        </Modal>
    );
}