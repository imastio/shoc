import { App, ConfigProvider } from "antd";
import { ReactNode } from "react";

export default function ThemeProvider({ children }: { children: ReactNode }) {

    return <ConfigProvider theme={{
        token: {
            colorPrimary: '#1677ff',
        },
    }}>
        <App>
            {children}
        </App>
    </ConfigProvider>
}
