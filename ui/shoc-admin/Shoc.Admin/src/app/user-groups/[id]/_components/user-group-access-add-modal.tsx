import { selfClient } from "@/clients/shoc";
import UserGroupAccessesClient from "@/clients/shoc/identity/user-group-accesses-client";
import StandardAlert from "@/components/general/standard-alert";
import { AccessAreaSelector } from "@/components/user-management/access-area-selector";
import { AccessSelector } from "@/components/user-management/access-selector";
import { useApiAuthentication } from "@/providers/api-authentication/use-api-authentication";
import { Form, Modal} from "antd";
import { useEffect, useMemo, useState } from "react";

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

export function UserGroupAccessAddModal({ groupId, open, granted, onClose = () => {}, onSuccess = () => {}}: any){
    const [form] = Form.useForm();
    const area = Form.useWatch('area', form);
    const { withToken } = useApiAuthentication();
    const [progress, setProgress] = useState(false);
    const [errors, setErrors] = useState([]);

    const filterGranted = useMemo(() => granted.map((item: any) => item.access), [granted]);

    const onCloseWrapper = () => {
        setErrors([])
        onClose();
    }

    const onSubmit = async (values: any) => {
        setErrors([]);
        setProgress(true);

        const result = await withToken((token: string) => selfClient(UserGroupAccessesClient).update(token, groupId, {
            grant: values.accesses
        }));

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
            area: null,
            accesses: []
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
            title="Add access entry"
            confirmLoading={progress}
            onOk={() => form.submit()}
        >
            <StandardAlert style={{ marginBottom: "8px" }} message={`Could not add the access`} errors={errors} optional={true} />
            <Form
                {...formItemLayout}
                form={form}
                preserve={false}
                onFinish={onSubmit}
                layout="horizontal"
            >
                <Form.Item name="area" label="Area" rules={[{ required: true, message: 'Please select a valid area' }]}>
                    <AccessAreaSelector placeholder="Select the access area" style={{ width: '100%' }} />
                </Form.Item>
                <Form.Item name="accesses" label="Accesses" rules={[{ required: true, message: 'Please select an access modifier' }]}>
                    <AccessSelector placeholder="Select the accesses to grant" filter={filterGranted} area={area} />
                </Form.Item>
            </Form>
        </Modal>
    );
}