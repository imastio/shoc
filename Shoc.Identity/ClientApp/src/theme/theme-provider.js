import { ConfigProvider } from "antd";

export default function ThemeProvider({children}) {

    return <ConfigProvider theme={{
        token: {
        }
    }}>
        {children}
    </ConfigProvider>
}