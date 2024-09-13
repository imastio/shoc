"use client"

import ErrorScreen from "@/components/error/error-screen"

export default function NotFound({ error }: {
  error: Error & { digest?: string }
  reset: () => void
}) {

  return <div className="grid min-h-screen w-full">
    <ErrorScreen errors={[]} kind='not_found' />
  </div>
}