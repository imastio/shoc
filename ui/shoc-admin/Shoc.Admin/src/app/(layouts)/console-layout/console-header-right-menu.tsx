import React from "react"
import { Dropdown, Button, Space } from "antd"
import { LogoutOutlined, UserOutlined } from "@ant-design/icons"
import { useSession } from "next-auth/react";
import { useRouter } from "next/navigation";
import { rpc, rpcDirect } from "@/server-actions/rpc";
import { ItemType } from "antd/es/menu/interface";

export default function ConsoleHeaderRightMenu() {
    const session = useSession()
    const router = useRouter();
    const name = session?.data?.user?.name || "Unknown User";
    const firstName = name.split(" ")[0];

    const items: ItemType[] = [
        {
            key: "welcome-group",
            label: `Welcome, ${firstName}`,
            type: "group",
            children: [
                {
                    key: "my-profile",
                    icon: <UserOutlined />,
                    label: <span onClick={() => router.push("/myself/profile")}>My Profile</span>
                },
                {
                    key: "sign-out",
                    icon: <LogoutOutlined />,
                    label: <span onClick={async () => {
                        const { data, errors } = await rpc('auth/signleSignOut', { postLogoutRedirectUri: new URL('/', window.location.href).toString() })

                        if (!errors) {
                            await rpcDirect('auth/signOut', { endSessionUri: data.redirectUri })
                            window.location.href = data.redirectUri
                        }
                    }}>Sign out</span>
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


