import type { Metadata } from "next";
import "./globals.css";
import { AntdRegistry } from "@ant-design/nextjs-registry";
import NextTopLoader from 'nextjs-toploader'
import { SessionProvider } from "next-auth/react";
import { DEFAULT_FONT } from '@/addons/fonts'
import ThemeProvider from "@/providers/theme-provider/theme-provider";
import ApiAuthenticationProvider from "@/providers/api-authentication/api-authentication-provider";
import TitleProvider from "@/providers/title-provider/title-provider";

export const metadata: Metadata = {
  title: "Shoc Platform",
  description: "The administrative space of Shoc Platform",
};

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {

  return (
    <html lang="en" suppressHydrationWarning className={DEFAULT_FONT.className}>
      <body suppressHydrationWarning>
        <NextTopLoader color='rgb(39 39 42)' height={2} shadow={false} showSpinner={false} />
        <SessionProvider>
          <ApiAuthenticationProvider>
            <AntdRegistry>
              <TitleProvider>
                <ThemeProvider>
                    {children}
                </ThemeProvider>
              </TitleProvider>
            </AntdRegistry>
          </ApiAuthenticationProvider>
        </SessionProvider>
      </body>
    </html>
  );
}
