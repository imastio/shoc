import React from "react"
import { Layout } from "antd"
import { Outlet } from "react-router-dom"

export default function MainLayout() {

   return (
      <>
		<Layout style={{backgroundColor: 'white', minHeight: "100vh"}}>
		   <Layout.Content style={{
			  margin: '18px 18px',
			  padding: '8px 8x',
			  backgroundColor: 'white'
		   }}>
			  <Outlet />
		   </Layout.Content>
		</Layout>
      </>
   )
}