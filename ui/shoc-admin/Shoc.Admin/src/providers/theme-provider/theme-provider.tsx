import { App, ConfigProvider } from "antd";
import { ReactNode } from "react";

export default function ThemeProvider({ children }: { children: ReactNode }) {

    return <ConfigProvider locale={{locale: 'en-US'}} theme={{
        token: {
            colorPrimary: '#1e293b',
        },
    }}>
        <App>
            {children}
        </App>
    </ConfigProvider>
}
