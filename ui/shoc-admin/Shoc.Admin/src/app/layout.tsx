import type { Metadata } from "next";
import "./globals.css";
import { AntdRegistry } from "@ant-design/nextjs-registry";
import NextTopLoader from 'nextjs-toploader'
import { SessionProvider } from "next-auth/react";
import { DEFAULT_FONT } from '@/addons/fonts'
import ProtectedLayout from "./components/protected-layout";

export const metadata: Metadata = {
  title: "Create Next App",
  description: "Generated by create next app",
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
          <ProtectedLayout>
            <AntdRegistry>
              {children}
            </AntdRegistry>
          </ProtectedLayout>
        </SessionProvider>
      </body>
    </html>
  );
}
