import { Button } from "@/components/ui/button";
import { DropdownMenu, DropdownMenuContent, DropdownMenuItem, DropdownMenuTrigger } from "@/components/ui/dropdown-menu";
import { Edit, MoreHorizontal, RefreshCcw, Wand2 } from "lucide-react";
import { useIntl } from "react-intl";

export type ClusterActionTypes = 'update' | 'configure' | 'refresh' | 'delete'

type ClusterActionsDropdownProps = {
    disabled?: boolean
    onSelect: (action: ClusterActionTypes) => void
}

export default function ClusterActionsDropdown({ onSelect, disabled }: ClusterActionsDropdownProps) {

    const intl = useIntl();

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
            <DropdownMenuItem onClick={() => onSelect('update')}>
                <Edit />
                {intl.formatMessage({ id: 'global.actions.update' })}
            </DropdownMenuItem>
            <DropdownMenuItem onClick={() => onSelect('update')}>
                <Wand2 />
                {intl.formatMessage({ id: 'global.actions.configure' })}
            </DropdownMenuItem>
        </DropdownMenuContent>
    </DropdownMenu>
}