import { deepFind, injectParents } from "@/extended/collections";
import {
   DashboardOutlined,
   TeamOutlined,
} from "@ant-design/icons"
import Link from "next/link";

const mapLink = (label, path) => (
   <Link href={path}>{label}</Link>
)

export const menuDefinitions = [
   {
      key: "dashboard-page",
      label: mapLink("Dashboard", "/dashboard"),
      title: "Dashboard",
      icon: <DashboardOutlined />,
      path: "/dashboard",
      pathSelectors: ["/dashboard", "/"]
   },
   {
      key: "team-page",
      label: "Team",
      title: "Team",
      path: "/team",
      icon: <TeamOutlined />,
      children: [
         {
            key: "users-page",
            label: mapLink("Users", "/team/users"),
            title: "Users",
            path: "/team/users",
            pathSelectors: ["/team/users", "/team/users/:id"]
         },
         {
            key: "groups-page",
            label: mapLink("Groups", "/team/groups"),
            title: "Groups",
            path: "/team/groups",
            pathSelectors: ["/team/groups", "/team/groups/:id"]
         },
         {
            key: "privileges-page",
            label: mapLink("Privileges", "/team/privileges"),
            title: "Privileges",
            path: "/team/privileges",
            pathSelectors: ["/team/privileges", "/team/privileges/:id"]
         },
         {
            key: "roles-page",
            label: mapLink("Roles", "/team/roles"),
            title: "Roles",
            path: "/team/roles",
            pathSelectors: ["/team/roles", "/team/roles/:id"]
         }
      ]
   }
]

injectParents(null, menuDefinitions);

export const resolveByPath = pathSelector => {

   let found = deepFind(
      { children: menuDefinitions },
      obj => (obj.pathSelectors || []).some(p => String(p).startsWith(pathSelector))
   );

   let items = [];

   while (found) {

      items = [found, ...items]
      found = found.parent;
   }

   return items
}

