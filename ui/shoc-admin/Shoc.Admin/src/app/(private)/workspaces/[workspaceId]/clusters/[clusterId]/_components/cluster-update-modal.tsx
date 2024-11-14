import { selfClient } from "@/clients/shoc";
import ClustersClient from "@/clients/shoc/cluster/clusters-client";
import StandardAlert from "@/components/general/standard-alert";
import { useApiAuthentication } from "@/providers/api-authentication/use-api-authentication";
import { clusterNamePattern, clusterStatuses, clusterTypes } from "@/well-known/clusters";
import { Form, Input, Modal, Select} from "antd";
import { useEffect, useState } from "react";

export default function ClusterUpdateModal({existing, open, onClose = () => {}, onSuccess = () => {}}: any){
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
            workspaceId: existing?.workspaceId,
            name: values.name,
            description: values.description,
            type: values.type,
            status: values.status
        }

        const result = await withToken((token: string) => selfClient(ClustersClient).updateById(token, existing?.workspaceId, existing?.id, input));

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
            name: existing?.name,
            description: existing?.description,
            type: existing?.type,
            status: existing?.status
        });

    }, [open, existing, form])

    return (
        <Modal
            closable={false}
            cancelButtonProps={{ disabled: progress }}
            open={open}
            onCancel={onCloseWrapper}
            destroyOnClose={true}
            forceRender
            title="Edit cluster"
            confirmLoading={progress}
            onOk={() => form.submit()}
        >
            <StandardAlert style={{ marginBottom: "8px" }} message={`Could not edit the cluster`} errors={errors} optional={true} />
            <Form
                form={form}
                preserve={false}
                onFinish={onSubmit}
                labelCol={{ span: 6 }}
                wrapperCol={{ span: 18 }}
                layout="horizontal"
            >
                <Form.Item name="name" label="Name" rules={[
                    { required: true, message: 'Please enter valid name' },
                    { pattern: clusterNamePattern, message: 'The name is invalid' }
                    ]}>
                    <Input placeholder="Please enter the name" />
                </Form.Item>
                <Form.Item name="description" label="Description" rules={[
                    { required: true, message: 'Please enter cluster description' },
                    { max: 512, message: 'The description is too long' }
                    ]}>
                    <Input.TextArea placeholder="Please enter the description" />
                </Form.Item>            
                <Form.Item name="type" label="Type" rules={[{ required: true, message: 'Please select a valid type' }]}>
                    <Select placeholder="Select the type ">
                        {clusterTypes.map(entry => <Select.Option key={entry.key} value={entry.key}>{entry.display}</Select.Option>)}
                    </Select>
                </Form.Item>
                <Form.Item name="status" label="Status" rules={[{ required: true, message: 'Please select a valid status' }]}>
                    <Select placeholder="Select the status ">
                        {clusterStatuses.map(entry => <Select.Option key={entry.key} value={entry.key}>{entry.display}</Select.Option>)}
                    </Select>
                </Form.Item>
            </Form>
        </Modal>
    );
}