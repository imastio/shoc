import { Drawer, Layout } from "antd"

export default function ConsoleSiderDrawer({ children, onClose, open, width }) {

    return <Drawer
        closable={false}
        styles={{
            body: {width: '100%', padding: 0}
        }}
        onClose={onClose}
        forceRender
        width={width}
        destroyOnClose={false}
        open={open}
        placement="left"
    >
        <Layout style={{ minHeight: "100vh" }} >
            <Layout.Sider theme="dark" width={width}>
                {children}
            </Layout.Sider>
        </Layout>
    </Drawer>

}