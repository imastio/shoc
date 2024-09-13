import { clusterStatusesMap } from "@/well-known/clusters";
import { Tag } from "antd";

export default function ClusterStatus({ status }: { status: string }){
    return <Tag color={status === 'active' ? 'success' : 'default'}>{clusterStatusesMap[status] || status}</Tag>
}