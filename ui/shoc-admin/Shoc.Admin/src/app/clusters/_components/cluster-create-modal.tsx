import { selfClient } from "@/clients/shoc";
import ClustersClient from "@/clients/shoc/cluster/clusters-client";
import StandardAlert from "@/components/general/standard-alert";
import { WorkspaceSelector } from "@/components/workspace/workspace-selector";
import { useApiAuthentication } from "@/providers/api-authentication/use-api-authentication";
import { clusterNamePattern, clusterTypes } from "@/well-known/clusters";
import { Form, Input, Modal, Select } from "antd";
import { useEffect, useState } from "react";

const formItemLayout = {
    labelCol: {
      xs: { span: 24 },
      sm: { span: 6 },
    },
    wrapperCol: {
      xs: { span: 24 },
      sm: { span: 18 },
    },
  };

export default function ClusterCreateModal(props: any){

    const { workspaceId, open, onClose, onSuccess } = props;

    const [form] = Form.useForm();
    const { withToken } = useApiAuthentication();
    const [progress, setProgress] = useState(false);
    const [errors, setErrors] = useState([]);

    const onCloseWrapper = () => {
        form.resetFields();
        setErrors([]);
        if(onClose){
            onClose();
        }
    }

    const onSubmit = async (values: any) => {
        setErrors([]);
        setProgress(true);

        const input = {
            workspaceId: workspaceId ? workspaceId : values.workspaceId,
            name: values.name,
            type: values.type,
            configuration: values.configuration
        };

        const result = await withToken((token: string) => selfClient(ClustersClient).create(token, input.workspaceId, input)); 

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

        form.resetFields();
    }, [form, open])

    return (
        <Modal
            closable={false}
            cancelButtonProps={{ disabled: progress }}
            open={open}
            forceRender
            onCancel={onClose}
            title="Create Cluster"
            confirmLoading={progress}
            onOk={() => form.submit()}
        >
            <StandardAlert style={{ marginBottom: "8px" }} message="Could not create a cluster" errors={errors} optional={true} />
            <Form form={form} onFinish={onSubmit} layout="horizontal" {...formItemLayout}>
                <Form.Item name="name" label="Name" rules={[
                    { required: true, message: 'Please enter valid name' },
                    { pattern: clusterNamePattern, message: 'The name is invalid' }
                    ]}>
                    <Input placeholder="Please enter the name" />
                </Form.Item>         
                <Form.Item name="type" label="Type" rules={[{ required: true, message: 'Please select a valid type' }]}>
                    <Select placeholder="Select the type ">
                        {clusterTypes.map(entry => <Select.Option key={entry.key} value={entry.key}>{entry.display}</Select.Option>)}
                    </Select>
                </Form.Item>
                {!workspaceId && <Form.Item name="workspaceId" label="Workspace" rules={[{required: true, message: 'Please select the workspace'}]}>
                    <WorkspaceSelector placeholder="Select the workspace" />
                </Form.Item>}
                <Form.Item name="configuration" label="Configuration">
                    <Input.TextArea placeholder="Please enter the configuration (kubeconfig, etc.)" />
                </Form.Item>  
            </Form>
        </Modal>
    );

}
