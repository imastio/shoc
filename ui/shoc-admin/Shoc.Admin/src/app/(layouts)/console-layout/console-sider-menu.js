import React, { useMemo } from "react"
import { Menu } from "antd"
import { menuDefinitions, resolveByPath } from "./menu-definitions"
import ConsoleSiderLogo from "./console-sider-logo"
import { usePathname } from "next/navigation"
import useRouteAccess from "@/access/use-route-access"

export default function ConsoleSiderMenu({collapsed}){

   const pathname = usePathname();
   const { isAllowed } = useRouteAccess();
   
   const selectedItems = useMemo(() => {
      return resolveByPath(pathname).map(it => it.key)
   }, [pathname]);   

   const menuItems = menuDefinitions.filter(item => isAllowed(item.path)).map(item => ({
      key: item.key,
      label: item.label,
      title: item.title,
      icon: item.icon,
      children: item.children ? item.children.filter(child => isAllowed(child.path)).map(nested => ({
         key: nested.key, 
         title: nested.title,   
         label: nested.label 
      })) : undefined
   }))

   return (
      <>
         <ConsoleSiderLogo small={collapsed} />
         <Menu
            theme="light"
            selectedKeys={selectedItems}
            defaultOpenKeys={collapsed ? undefined : selectedItems}
            activeKey={collapsed ? undefined : selectedItems}
            forceSubMenuRender={true}
            mode="inline"
            items={menuItems}
         >
         </Menu>
      </>
   )
}
