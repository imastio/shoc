"use client"

import ErrorScreen from "@/components/error/error-screen"

export default function Error({ error, reset }: {
    error: Error & { digest?: string, errors?: any[] }
    reset: () => void
}) {

    return <div className="grid min-h-screen w-full">
        <ErrorScreen errors={[]} kind='unknown' />
    </div>
}