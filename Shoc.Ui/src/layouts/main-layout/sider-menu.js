import React, { useMemo } from "react"
import { Menu } from "antd"
import { menuDefinitions, resolveByPath } from "./menu-definitions"
import { useLocation } from "react-router-dom"

export default function SiderMenu({collapsed}){

   const location = useLocation();
   
   const selectedItems = useMemo(() => {
      return resolveByPath(location.pathname).map(it => it.key)
   }, [location.pathname]);   

   const menuItems = menuDefinitions.map(item => ({
      key: item.key,
      label: item.label,
      title: item.title,
      icon: item.icon,
      children: item.children ? item.children.map(nested => ({
         key: nested.key, 
         title: nested.title,   
         label: nested.label 
      })) : undefined
   }))

   return (
      <>
         <Menu
            theme="light"
            selectedKeys={selectedItems}
            defaultOpenKeys={collapsed ? undefined : selectedItems}
            forceSubMenuRender={true}
            mode="inline"
            items={menuItems}
         >
         </Menu>
      </>
   )
}
