import { selfClient } from "@/clients/shoc";
import UsersClient from "@/clients/shoc/identity/users-client";
import StandardAlert from "@/components/general/standard-alert";
import { useApiAuthentication } from "@/providers/api-authentication/use-api-authentication";
import countries from "@/well-known/countries";
import timezones, { guessTimezone } from "@/well-known/timezones";
import { genders } from "@/well-known/user-details";
import { userTypes } from "@/well-known/user-types";
import { Form, Input, Modal, Switch, Tabs, Radio, Select } from "antd";
import { useState } from "react";

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

export default function UserCreateModal(props: any) {

    const { visible, onClose, onSuccess } = props;

    const [form] = Form.useForm();
    const { withToken } = useApiAuthentication();
    const [progress, setProgress] = useState(false);
    const [errors, setErrors] = useState([]);

    const withoutRoot = userTypes.filter(e => e.key !== 'root')

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

        const result = await withToken((token: string) => selfClient(UsersClient).create(token, { ...values }));

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
            closable={false}
            cancelButtonProps={{ disabled: progress }}
            open={visible}
            forceRender
            onCancel={onCloseWrapper}
            destroyOnClose={true}
            title="Create User"
            confirmLoading={progress}
            onOk={() => form.submit()}
        >
            <StandardAlert style={{ marginBottom: "8px" }} message="Could not create a user" errors={errors} optional={true} />
            <Form form={form} onFinish={onSubmit} layout="horizontal" {...formItemLayout} initialValues={{
                country: 'Armenia',
                type: 'external',
                timezone: guessTimezone(),
                emailVerified: true,
                phoneVerified: false
            }}>

                <Tabs defaultActiveKey="1" items={[
                    {
                        key: "1",
                        label: "Definition",
                        children: <>
                            <Form.Item name="fullName" label="Name" rules={[{ required: true, min: 2, message: 'Please enter valid name' }]}>
                                <Input placeholder="Please enter your Full name" />
                            </Form.Item>
                            <Form.Item name="type" label="User Type" rules={[{ required: true, message: 'Please select a user type' }]}>
                                <Radio.Group>
                                    {withoutRoot.map(t => <Radio.Button key={t.key} value={t.key}>{t.display}</Radio.Button>)}
                                </Radio.Group>
                            </Form.Item>
                            <Form.Item name="email" label="Email" rules={[{ required: true, type: 'email', message: 'Please enter valid Email' }]}>
                                <Input placeholder="Please enter your Email" />
                            </Form.Item>
                            <Form.Item name="emailVerified" label="Email Verified" valuePropName="checked">
                                <Switch checkedChildren="Verified" unCheckedChildren="Unverified" />
                            </Form.Item>
                            <Form.Item name="username" label="Username" rules={[{ required: true, pattern: /^[\w\\._]+$/, message: 'Please enter valid username' }]}>
                                <Input placeholder="Please enter your username " />
                            </Form.Item>
                            <Form.Item name="password" label="Password" rules={[{ required: true, min: 6, message: 'Please enter valid password' }]} >
                                <Input type="password" placeholder="Please enter your password" />
                            </Form.Item>
                            <Form.Item name="timezone" label="Timezone">
                                <Select>
                                    {timezones.map(tz => <Select.Option key={tz} value={tz}>{tz}</Select.Option>)}
                                </Select>
                            </Form.Item>
                        </>
                    },
                    {
                        key: "2",
                        label: "Details",
                        forceRender: true,
                        children: <>
                            <Form.Item name="firstName" label="First name">
                                <Input placeholder="Please enter your first name " />
                            </Form.Item>
                            <Form.Item name="lastName" label="Last name">
                                <Input placeholder="Please enter your last name" />
                            </Form.Item>
                            <Form.Item name="gender" label="Gender">
                                <Radio.Group>
                                    {genders.map(g => <Radio.Button key={g.key} value={g.key}>{g.display}</Radio.Button>)}
                                </Radio.Group>
                            </Form.Item>
                            <Form.Item name="phone" label="Phone">
                                <Input placeholder="Please enter your phone number " />
                            </Form.Item>
                            <Form.Item name="phoneVerified" label="Phone Verified" valuePropName="checked">
                                <Switch checkedChildren="Verified" unCheckedChildren="Unverified" />
                            </Form.Item>
                        </>
                    },
                    {
                        key: "3",
                        label: "Address",
                        forceRender: true,
                        children: <>
                            <Form.Item name="country" label="Country">
                                <Select>
                                    {countries.map(country => <Select.Option key={country} value={country}>{country}</Select.Option>)}
                                </Select>
                            </Form.Item>
                            <Form.Item name="state" label="State">
                                <Input placeholder="State" />
                            </Form.Item>
                            <Form.Item name="city" label="City">
                                <Input placeholder="City" />
                            </Form.Item>
                            <Form.Item name="postal" label="Postal">
                                <Input placeholder="Please enter your postal code" />
                            </Form.Item>
                            <Form.Item name="address1" label="First address">
                                <Input placeholder="Please enter your first address" />
                            </Form.Item>
                            <Form.Item name="address2" label="Second address">
                                <Input placeholder="Please enter your second address" />
                            </Form.Item>
                        </>
                    }
                ]} />
            </Form>
        </Modal>
    );

}
