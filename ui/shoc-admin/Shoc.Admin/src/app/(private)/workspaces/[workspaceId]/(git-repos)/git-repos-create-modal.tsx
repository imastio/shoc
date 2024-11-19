import { selfClient } from "@/clients/shoc";
import GitReposClient from "@/clients/shoc/job/git-repos-client";
import StandardAlert from "@/components/general/standard-alert";
import { useApiAuthentication } from "@/providers/api-authentication/use-api-authentication";
import { Form, Input, Modal } from "antd";
import { useEffect, useState } from "react";

export default function GitReposCreateModal({workspaceId, open, onClose = () => {}, onSuccess = () => {}}: any){
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
            owner: values.owner,
            source: values.source,
            repository: values.repository,
            remoteUrl: values.remoteUrl
        }

        const result = await withToken((token: string) => selfClient(GitReposClient).create(token, workspaceId, input));

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
            name: '',
            owner: '',
            source: '',
            repository: '',
            remoteUrl: ''
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
            title="Add a git repository"
            confirmLoading={progress}
            onOk={() => form.submit()}
        >
            <StandardAlert style={{ marginBottom: "8px" }} message={`Could not add the git repository to the workspace`} errors={errors} optional={true} />
            <Form
                form={form}
                preserve={false}
                onFinish={onSubmit}
                labelCol={{span: 6}}
                wrapperCol={{span: 18}}
                layout="horizontal"
            >
                <Form.Item name="name" label="Name" rules={[
                    { required: true, message: 'Please enter valid name', max: 256 }
                ]}>
                    <Input placeholder="shoc" />
                </Form.Item>  
                <Form.Item name="owner" label="Owner" rules={[
                    { required: true, message: 'Please enter valid owner name', max: 256 }
                ]}>
                    <Input placeholder="shoc-dev" />
                </Form.Item>
                <Form.Item name="source" label="Source" rules={[
                    { required: true, message: 'Please enter valid source', max: 256 }
                ]}>
                    <Input placeholder="github.com" />
                </Form.Item>
                <Form.Item name="repository" label="Source" rules={[
                    { required: true, message: 'Please enter valid repository name', max: 512 }
                ]}>
                    <Input placeholder="shoc-dev/shoc" />
                </Form.Item>
                <Form.Item name="remoteUrl" label="Remote URL" rules={[
                    { required: true, message: 'Please enter valid remote url', max: 768 }
                ]}>
                    <Input placeholder="https://github.com/shoc-dev/shoc.git" />
                </Form.Item>
            </Form>
        </Modal>
    );
}