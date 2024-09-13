"use client"

import { LoadingOutlined } from "@ant-design/icons";
import { Button, Result, Spin } from "antd";
import { useRouter } from "next/navigation";
import { ReactNode } from "react";

const getErrorStatus = (error: { statusCode: number }) => {

    if (error.statusCode === 403) {
        return "403";
    }

    if (error.statusCode === 404) {
        return "404";
    }

    if (error.statusCode === 500) {
        return "500";
    }

    return "error";
}

const getErrorTitle = (error: { statusCode: number }) => {

    if (error.statusCode === 403) {
        return "Access Denied";
    }

    if (error.statusCode === 404) {
        return "Not Found";
    }

    if (error.statusCode === 500) {
        return "Server Error";
    }

    return "Unknown Error";
}

const getErrorSubtitle = (error: { statusCode: number }) => {

    if (error.statusCode === 403) {
        return "You don't have access to the requested resource.";
    }

    if (error.statusCode === 404) {
        return "The resource you are looking for does not exist.";
    }

    if (error.statusCode === 500) {
        return "The resource is temporarily not available.";
    }

    return "There resource is not available due to an internal error.";
}

export function ObjectContainer({ children, loading = false, fatalError } : { children: ReactNode, loading: boolean, fatalError: any  }) {

    const router = useRouter();

    return <>
        {fatalError && <Result
            status={getErrorStatus(fatalError)}
            title={getErrorTitle(fatalError)}
            subTitle={getErrorSubtitle(fatalError)}
            extra={
                <Button type="primary" onClick={() => router.back()}>
                    Go Back
                </Button>
            }
        />}
        {
            !fatalError && <Spin spinning={loading} indicator={<LoadingOutlined />}>
                {children}
            </Spin> 
        }
    </>
}
