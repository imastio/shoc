import React from "react"
import { Dropdown, Button, Space } from "antd"
import { LogoutOutlined, UserOutlined } from "@ant-design/icons"
import { useAuth } from "react-oidc-context"

export default function HeaderRightMenu(){
    const auth = useAuth()
    const name = auth?.user?.profile?.name || "Unknown User";
    const firstName = name.split(" ")[0];

    const items = [
        {
            key: "welcome-group",
            label: `Welcome, ${firstName}`,
            type: "group",
            children: [
                {
                    key: "sign-out",
                    icon: <LogoutOutlined />,
                    label: <span onClick={() => auth.signoutRedirect()}>Sign out</span>
                }
            ]
        }
    ]

    return (
        <div style={{
            float: "right",
        }}>
            <Space direction="horizontal">
                <Dropdown menu={{ items }} trigger={['click']} placement="bottomRight">
                    <Button shape="circle" type="default" size="large"><UserOutlined /></Button>
                </Dropdown>
            </Space>
        </div>
    )
}


