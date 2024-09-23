import ClusterIcon from "@/components/icons/cluster-icon";
import HomeIcon from "@/components/icons/home-icon";
import UsersIcon from "@/components/icons/users-icon";
import useWorkspaceAccess from "@/providers/workspace-access/use-workspace-access";
import { MixerHorizontalIcon, PlayIcon } from "@radix-ui/react-icons";
import { usePathname } from "next/navigation";
import { useMemo } from "react";
import { useIntl } from "react-intl";
import { WorkspacePermissions } from "@/well-known/workspace-permissions";
import { KeyRoundIcon } from "lucide-react";

export default function useWorkspaceMenu({ name }: { name: string }){

    const intl = useIntl();
    const pathname = usePathname();
    const { hasAny } = useWorkspaceAccess();

    const all = useMemo(() => [
        { 
            path: `/workspaces/${name}`, 
            title: intl.formatMessage({ id: 'workspaces.sidebar.dashboard' }), 
            icon: HomeIcon,
            visible: hasAny([WorkspacePermissions.WORKSPACE_VIEW])
        },
        { 
            path: `/workspaces/${name}/jobs`, 
            title: intl.formatMessage({ id: 'workspaces.sidebar.jobs' }), 
            icon: PlayIcon,
            visible: hasAny([WorkspacePermissions.WORKSPACE_VIEW])
        },
        { 
            path: `/workspaces/${name}/members`, 
            title: intl.formatMessage({ id: 'workspaces.sidebar.members' }), 
            icon: UsersIcon,
            visible: hasAny([WorkspacePermissions.WORKSPACE_LIST_MEMBERS]),
            altPaths: [`/workspaces/${name}/members/invitations`]
        },
        { 
            path: `/workspaces/${name}/clusters`, 
            title: intl.formatMessage({ id: 'workspaces.sidebar.clusters' }), 
            icon: ClusterIcon,
            visible: hasAny([WorkspacePermissions.WORKSPACE_LIST_CLUSTERS])
        },
        { 
            path: `/workspaces/${name}/secrets`, 
            title: intl.formatMessage({ id: 'workspaces.sidebar.secrets' }), 
            icon: KeyRoundIcon,
            visible: hasAny([WorkspacePermissions.WORKSPACE_LIST_SECRETS, WorkspacePermissions.WORKSPACE_LIST_USER_SECRETS])
        },
        { 
            path: `/workspaces/${name}/settings`, 
            title: intl.formatMessage({ id: 'workspaces.sidebar.settings' }), 
            icon: MixerHorizontalIcon,
            visible: hasAny([WorkspacePermissions.WORKSPACE_UPDATE])
        }
    ], [name, hasAny, intl])

    const menu = useMemo(() => all.filter(item => item.visible).map(item => ({
        ...item,
        active: item.path === pathname || item.altPaths?.includes(pathname || '')
    })), [all, pathname])


    return menu
}