import useRouteAccess from "@/access/use-route-access";
import { CloudOutlined, DashboardOutlined, DatabaseOutlined, FolderOutlined, SettingOutlined, TeamOutlined } from "@ant-design/icons";
import { MenuDataItem } from "@ant-design/pro-layout";

const menuPrototypes: MenuDataItem[] = [
    {
        path: '/',
        name: 'Dashboard',
        icon: <DashboardOutlined />,
    },
    {
        path: '/workspaces',
        name: 'Workspaces',
        icon: <FolderOutlined />,
    },
    {
        path: '/registries',
        name: 'Registries',
        icon: <DatabaseOutlined />,
    },
    {
        path: '/clusters',
        name: 'Clusters',
        icon: <CloudOutlined />,
    },
    {
        name: 'User Management',
        icon: <TeamOutlined />,
        children: [
            {
                path: '/users',
                name: 'Users'
            },
            {
                path: '/user-groups',
                name: 'User Groups'
            },
            {
                path: '/roles',
                name: 'Roles'
            },
            {
                path: '/privileges',
                name: 'Privileges'
            }
        ]
    },
    {
        name: 'Settings',
        icon: <SettingOutlined />,
        children: [
            {
                path: '/applications',
                name: 'Applications'
            },
            {
                path: '/mailing-profiles',
                name: 'Mailing Profiles'
            }
        ]
    }
]

function buildMenu(items: MenuDataItem[], isAllowed: (path: string | null) => boolean): MenuDataItem[]{

    const allowedOnly = items.filter(item => (item.path && isAllowed(item.path) || !item.path)).map(({ icon, children, ...item }) => ({
        ...item,
        icon: icon,
        children: children && buildMenu(children, isAllowed),
      }));

      return allowedOnly;
}

function filterEmpty(items: MenuDataItem[]): MenuDataItem[]{

    return items.filter(item => item.path || item.children).map(({ icon, children, ...item }) => ({
        ...item,
        icon: icon,
        children: children && filterEmpty(children),
      }));
}

export default function useMenu(){
    const { isAllowed } = useRouteAccess();

    const allowedOnly =  buildMenu(menuPrototypes, isAllowed);

    return  filterEmpty(allowedOnly)
}

