import ClusterIcon from "@/components/icons/cluster-icon"
import KubernetesIcon from "@/components/icons/kubernetes-icon"
import { cn } from "@/lib/utils";

export default function ClusterLogo({ type, className }: { type: string, className?: string }){
    const Icon = type === 'k8s' ? KubernetesIcon : ClusterIcon;
    const defaultClassName = type === 'k8s' ? "text-primary" : "";

    return <Icon className={cn(defaultClassName, className)} />
}