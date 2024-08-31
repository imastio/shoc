import { registryStatusesMap } from "@/well-known/registries";
import { Tag } from "antd";

export default function RegistryStatus({ status }: { status: string }){
    return <Tag color={status === 'active' ? 'success' : 'default'}>{registryStatusesMap[status] || status}</Tag>
}