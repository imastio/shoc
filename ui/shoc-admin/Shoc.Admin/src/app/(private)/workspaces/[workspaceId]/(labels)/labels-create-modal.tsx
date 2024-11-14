import { selfClient } from "@/clients/shoc";
import LabelsClient from "@/clients/shoc/job/labels-client";
import StandardAlert from "@/components/general/standard-alert";
import { useApiAuthentication } from "@/providers/api-authentication/use-api-authentication";
import { labelNamePattern } from "@/well-known/jobs";
import { Form, Input, Modal } from "antd";
import { useEffect, useState } from "react";

export default function LabelsCreateModal({workspaceId, open, onClose = () => {}, onSuccess = () => {}}: any){
    const [form] = Form.useForm();
    const { withToken } = useApiAuthentication();
    const [progress, setProgress] = useState(false);
    const [errors, setErrors] = useState([]);

    const onCloseWrapper = () => {
        setErrors([])
        onClose();
    }

    const onSubmit = async (values: any) => {
        setErrors([]);
        setProgress(true);

        const input = {
            workspaceId: workspaceId,
            name: values.name,
        }

        const result = await withToken((token: string) => selfClient(LabelsClient).create(token, workspaceId, input));

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

        if(!open){
            return;
        }

        form.setFieldsValue({
            name: ''
        });

    }, [open, form])

    return (
        <Modal
            closable={false}
            cancelButtonProps={{ disabled: progress }}
            open={open}
            onCancel={onCloseWrapper}
            destroyOnClose={true}
            forceRender
            title="Add a label"
            confirmLoading={progress}
            onOk={() => form.submit()}
        >
            <StandardAlert style={{ marginBottom: "8px" }} message={`Could not add the label to the workspace`} errors={errors} optional={true} />
            <Form
                form={form}
                preserve={false}
                onFinish={onSubmit}
                labelCol={{span: 4}}
                wrapperCol={{span: 20}}
                layout="horizontal"
            >
                <Form.Item name="name" label="Name" rules={[
                    { required: true, message: 'Please enter valid name' },
                    { pattern: labelNamePattern, message: 'The name is invalid' }
                    ]}>
                    <Input placeholder="Please enter the name" />
                </Form.Item>  
            </Form>
        </Modal>
    );
}