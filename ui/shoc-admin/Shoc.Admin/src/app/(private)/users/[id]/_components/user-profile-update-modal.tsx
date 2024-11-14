import { selfClient } from "@/clients/shoc";
import UsersClient from "@/clients/shoc/identity/users-client";
import StandardAlert from "@/components/general/standard-alert";
import { useApiAuthentication } from "@/providers/api-authentication/use-api-authentication";
import countries from "@/well-known/countries";
import { genders } from "@/well-known/user-details";
import { DatePicker, Form, Input, Modal, Select, Switch } from "antd";
import dayjs from "dayjs";
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

export default function UserProfileUpdateModal(props: any) {

    const { open, onClose = () => { }, onSuccess, userId, data } = props;

    const [form] = Form.useForm();
    const { withToken } = useApiAuthentication();
    const [progress, setProgress] = useState(false);
    const [errors, setErrors] = useState([]);

    const onCloseWrapper = () => {
        setErrors([]);
        onClose()
    }

    const onSubmit = async (values: any) => {
        setErrors([]);
        setProgress(true);

        const result = await withToken((token: string) => selfClient(UsersClient).updateProfileById(token, userId, {
            ...values,
            birthDate: values.birthDate ? dayjs.utc(values.birthDate).local().startOf('day').utc() : null
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

        if (!data) {
            return;
        }

        form.setFieldsValue({
            ...data,
            birthDate: data.birthDate ? dayjs.utc(data.birthDate).local() : null
        });

    }, [form, data])

    return (
        <Modal
            closable={false}
            forceRender
            cancelButtonProps={{ disabled: progress }}
            open={open}
            onCancel={onCloseWrapper}
            destroyOnClose={true}
            title="Update user profile"
            confirmLoading={progress}
            onOk={() => form.submit()}
        >
            <StandardAlert style={{ marginBottom: "8px" }} message="Could not update user profile" errors={errors} optional={true} />
            <Form form={form} preserve={true} onFinish={onSubmit} layout="horizontal" {...formItemLayout}>
                <Form.Item name="firstName" label="First Name">
                    <Input placeholder="John" />
                </Form.Item>
                <Form.Item name="lastName" label="Last Name">
                    <Input placeholder="Smith" />
                </Form.Item>
                <Form.Item name="phone" label="Phone" rules={[{ required: false, pattern: /^\+[0-9]{5,15}$/, message: 'Please enter valid phone number' }]}>
                    <Input placeholder="+374XXXXXXXX" />
                </Form.Item>
                <Form.Item name="phoneVerified" label="Phone Verified" valuePropName="checked">
                    <Switch checkedChildren="Verified" unCheckedChildren="Unverified" />
                </Form.Item>
                <Form.Item name="gender" label="Gender">
                    <Select placeholder="Select the gender" allowClear>
                        {genders.map(entry => <Select.Option key={entry.key} value={entry.key}>{entry.display}</Select.Option>)}
                    </Select>
                </Form.Item>
                <Form.Item name="birthDate" label="Birth Date">
                    <DatePicker style={{ width: '100%' }} disabledDate={(current) => current > dayjs()} showTime={false} />
                </Form.Item>
                <Form.Item name="country" label="Country">
                    <Select showSearch placeholder="Select the country" allowClear>
                        {countries.map(entry => <Select.Option key={entry} value={entry}>{entry}</Select.Option>)}
                    </Select>
                </Form.Item>
                <Form.Item name="state" label="State">
                    <Input placeholder="The state, province or region" />
                </Form.Item>
                <Form.Item name="city" label="City">
                    <Input placeholder="The city" />
                </Form.Item>
                <Form.Item name="address1" label="Address 1">
                    <Input placeholder="The address line 1" />
                </Form.Item>
                <Form.Item name="address2" label="Address 2">
                    <Input placeholder="The address line 2" />
                </Form.Item>
                <Form.Item name="postal" label="Postal">
                    <Input placeholder="The postal code" />
                </Form.Item>
            </Form>
        </Modal>
    );

}
