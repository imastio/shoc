import {
   DashboardOutlined,
   SettingOutlined
} from "@ant-design/icons"
import { deepFind, injectParents } from "extended/collections";
import { Link, matchPath } from "react-router-dom";

const mapLink = (label, path) => (
   <Link to={path}>{label}</Link>
)
  
export const menuDefinitions = [
   {
      key: "dashboard-page",
      label: mapLink("Dashboard", "/dashboard"),
      title: "Dashboard",
      icon: <DashboardOutlined />,
      path: "/dashboard",
      pathSelectors: ["/dashboard"]
   },
   {
      key: "settings-page",
      label: "Settings",
      title: "settings",
      icon: <SettingOutlined />,
      path: "/settings",
      children: [
         {
            key: "registeries-page",
            label: mapLink("Registries", "/settings/registries"),
            title: "Registries",
            path: "/settings/registries",
            pathSelectors: ["/settings/registries", "/settings/registries/:id"]
         }
      ],
   },
]

injectParents(null, menuDefinitions);

export const resolveByPath = pathSelector => {

   let found = deepFind(
      { children: menuDefinitions },
      obj => (obj.pathSelectors || []).some(p =>  matchPath(p, pathSelector))
   );

   let items = [];

   while (found) {

      items = [found, ...items]
      found = found.parent;
   }

   return items
}

