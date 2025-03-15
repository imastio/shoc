import { jobTaskStatusesMap } from "@/well-known/jobs";
import { Tag } from "antd";

const colors: Record<string, string> = {
    'created': 'default',
    'pending': 'geekblue',
    'running': 'processing',
    'succeeded': 'success',
    'failed': 'error',
    'cancelled': 'purple'
}

export default function JobTaskStatus({ status }: { status: string }){
    return <Tag color={colors[status] || 'default'}>{jobTaskStatusesMap[status] || status}</Tag>
}