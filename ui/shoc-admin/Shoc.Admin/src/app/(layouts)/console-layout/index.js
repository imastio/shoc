import React from "react"
import ConsoleSiderMenu from './console-sider-menu'
import { Layout, Grid } from "antd"
import { useState } from "react"
import ConsoleHeader from "./console-header"
import ConsoleSiderDrawer from "./console-sider-drawer"

const ConsoleLayout = ({ children }) => {
   const [collapsed, setCollapsed] = useState(false)
   const [drawerOpen, setDrawperOpen] = useState(false);
   const breakpoint = Grid.useBreakpoint();
   const isFlyout = !breakpoint.xxl && !breakpoint.xl && !breakpoint.lg && !breakpoint.md;

   return (
      <>
         <Layout style={{ minHeight: "100vh" }} hasSider>
            {!isFlyout && <Layout.Sider
               width="250px"
               theme="light"
               breakpoint="lg"
               collapsed={collapsed}
               collapsedWidth={'60px'}
               collapsible
               zeroWidthTriggerStyle={{ top: '12px' }}
               onCollapse={setCollapsed}
            >
               <ConsoleSiderMenu collapsed={collapsed && !drawerOpen} />
            </Layout.Sider>
            }
            {
               isFlyout && <ConsoleSiderDrawer width="250px" open={drawerOpen} onClose={() => setDrawperOpen(false)}>
                        <ConsoleSiderMenu collapsed={collapsed && !drawerOpen} />
               </ConsoleSiderDrawer>
            }
            <Layout style={{ backgroundColor: 'white' }}>
               <Layout.Header style={{ width: '100%', padding: '0 16px', backgroundColor: 'white', borderBottom: '1px solid #e4e9f0' }}>
                  <ConsoleHeader isFlyout={isFlyout} onMenuOpen={() => setDrawperOpen(true)} />
               </Layout.Header>
               <Layout.Content style={{
                  margin: '18px 18px',
                  padding: '8px 8x',
                  backgroundColor: 'white'
               }}>
                  {children}
               </Layout.Content>
            </Layout>
         </Layout>

      </>
   )
}

export default ConsoleLayout
