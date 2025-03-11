import { jobStatusesMap } from "@/well-known/jobs";
import { Tag } from "antd";

const colors: Record<string, string> = {
    'created': 'default',
    'pending': 'geekblue',
    'running': 'processing',
    'partially_succeeded': 'gold',
    'succeeded': 'success',
    'failed': 'error',
    'cancelled': 'purple'
}

export default function JobStatus({ status }: { status: string }){
    return <Tag color={colors[status] || 'default'}>{jobStatusesMap[status] || status}</Tag>
}