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

    const menuClickHandler = async ({ key }: { key: string }) => {
        switch (key) {
            case "sign-out":
                const { data, errors } = await rpc('auth/signleSignOut', { postLogoutRedirectUri: new URL('/', window.location.href).toString() })

                if (!errors) {
                    await rpcDirect('auth/signOut', { endSessionUri: data.redirectUri })
                    window.location.href = data.redirectUri
                }
                break;
            default:
                return;
        }
    }

    const items: ItemType[] = [
        {
            key: "welcome-group",
            label: `Welcome, ${firstName}`,
            type: "group",
            children: [
                {
                    key: 'sign-out',
                    icon: <LogoutOutlined />,
                    label: 'Sign out'
                }
            ]
        }
    ]

    return (
        <div style={{
            float: "right",
        }}>
            <Space direction="horizontal">
                <Dropdown menu={{ items, onClick: menuClickHandler }} trigger={['click']} placement="bottomRight">
                    <Button shape="circle" type="default" size="large"><UserOutlined /></Button>
                </Dropdown>
            </Space>
        </div>
    )
}


