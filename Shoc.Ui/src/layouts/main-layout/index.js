import React, { useState } from "react"
import { Grid, Layout } from "antd"
import { Outlet } from "react-router-dom"
import SiderMenu from "./sider-menu";
import SiderDrawer from "./sider-drawer";
import LayoutHeader from "./layout-header";

export default function MainLayout() {
	const [collapsed, setCollapsed] = useState(false)
	const [drawerOpen, setDrawperOpen] = useState(false);
	const breakpoint = Grid.useBreakpoint();
	const isFlyout = !breakpoint.xxl && !breakpoint.xl && !breakpoint.lg && !breakpoint.md;

	return (
		<>
			<Layout style={{ minHeight: "100vh" }}>

				<Layout.Header style={{ width: '100%', padding: '0 16px', backgroundColor: 'white', borderBottom: '1px solid #e4e9f0' }}>
					<LayoutHeader isFlyout={isFlyout} onMenuOpen={() => setDrawperOpen(true)} />
				</Layout.Header>

				<Layout style={{  minHeight: '100vh' }} hasSider>
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
						<SiderMenu collapsed={collapsed && !drawerOpen} />
					</Layout.Sider>
					}
					{
						isFlyout && <SiderDrawer width="250px" open={drawerOpen} onClose={() => setDrawperOpen(false)}>
							<SiderMenu collapsed={collapsed && !drawerOpen} />
						</SiderDrawer>
					}
					<Layout.Content style={{
						margin: '18px 18px',
						padding: '8px 8x',
					}}>
						<Outlet />
					</Layout.Content>
				</Layout>

			</Layout>

		</>
	)
}