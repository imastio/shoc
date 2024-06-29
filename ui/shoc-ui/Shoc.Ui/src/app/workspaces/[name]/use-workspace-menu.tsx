import ClusterIcon from "@/components/icons/cluster-icon";
import HomeIcon from "@/components/icons/home-icon";
import UsersIcon from "@/components/icons/users-icon";
import useWorkspaceAccess from "@/providers/workspace-access/use-workspace-access";
import { LayersIcon, MixerHorizontalIcon, PlayIcon } from "@radix-ui/react-icons";
import { usePathname } from "next/navigation";
import { useMemo } from "react";
import { useIntl } from "react-intl";

export default function useWorkspaceMenu({ name }: { name: string }){

    const intl = useIntl();
    const pathname = usePathname();
    const { hasAny } = useWorkspaceAccess();

    const all = useMemo(() => [
        { 
            path: `/workspaces/${name}`, 
            title: intl.formatMessage({ id: 'workspaces.sidebar.dashboard' }), 
            icon: HomeIcon,
            visible: hasAny(['workspace_view'])
        },
        { 
            path: `/workspaces/${name}/jobs`, 
            title: intl.formatMessage({ id: 'workspaces.sidebar.jobs' }), 
            icon: PlayIcon,
            visible: hasAny(['workspace_view'])
        },
        { 
            path: `/workspaces/${name}/members`, 
            title: intl.formatMessage({ id: 'workspaces.sidebar.members' }), 
            icon: UsersIcon,
            visible: hasAny(['workspace_edit'])
        },
        { 
            path: `/workspaces/${name}/clusters`, 
            title: intl.formatMessage({ id: 'workspaces.sidebar.clusters' }), 
            icon: ClusterIcon,
            visible: hasAny(['workspace_edit'])
        },
        { 
            path: `/workspaces/${name}/registries`, 
            title: intl.formatMessage({ id: 'workspaces.sidebar.registries' }), 
            icon: LayersIcon,
            visible: hasAny(['workspace_edit'])
        },
        { 
            path: `/workspaces/${name}/settings`, 
            title: intl.formatMessage({ id: 'workspaces.sidebar.settings' }), 
            icon: MixerHorizontalIcon,
            visible: hasAny(['workspace_edit'])
        }
    ], [name, hasAny, intl])

    const menu = useMemo(() => all.filter(item => item.visible).map(item => ({
        ...item,
        active: item.path === pathname
    })), [all, pathname])


    return menu
}