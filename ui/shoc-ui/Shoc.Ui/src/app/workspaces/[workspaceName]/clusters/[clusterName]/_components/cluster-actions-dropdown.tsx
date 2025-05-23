import { Button } from "@/components/ui/button";
import { DropdownMenu, DropdownMenuContent, DropdownMenuItem, DropdownMenuTrigger } from "@/components/ui/dropdown-menu";
import useClusterAccess from "@/providers/cluster-access/use-cluster-access";
import { ClusterPermissions } from "@/well-known/cluster-permissions";
import { Edit, MoreHorizontal, RefreshCcw, Wand2 } from "lucide-react";
import { useIntl } from "react-intl";

export type ClusterActionTypes = 'update' | 'configure' | 'refresh' | 'delete'

type ClusterActionsDropdownProps = {
    disabled?: boolean
    onSelect: (action: ClusterActionTypes) => void
}

export default function ClusterActionsDropdown({ onSelect, disabled }: ClusterActionsDropdownProps) {

    const intl = useIntl();
    const { hasAll } = useClusterAccess();

    return <DropdownMenu>
        <DropdownMenuTrigger asChild disabled={disabled}>
            <Button variant="outline">
                <MoreHorizontal className="ml-auto" /> 
                <span className="sm:inline hidden">{intl.formatMessage({ id: 'global.labels.actions' })}</span>
            </Button>
        </DropdownMenuTrigger>
        <DropdownMenuContent
            side="bottom"
            align="end"
            className="min-w-56 rounded-lg"
        >
            <DropdownMenuItem onClick={() => onSelect('refresh')}>
                <RefreshCcw />
                {intl.formatMessage({ id: 'global.actions.refresh' })}
            </DropdownMenuItem>
            <DropdownMenuItem onClick={() => onSelect('update')} disabled={!hasAll([ClusterPermissions.CLUSTER_UPDATE])}>
                <Edit />
                {intl.formatMessage({ id: 'global.actions.update' })}
            </DropdownMenuItem>
            <DropdownMenuItem onClick={() => onSelect('configure')} disabled={!hasAll([ClusterPermissions.CLUSTER_UPDATE])}>
                <Wand2 />
                {intl.formatMessage({ id: 'global.actions.configure' })}
            </DropdownMenuItem>
        </DropdownMenuContent>
    </DropdownMenu>
}