"use client"

import useRouteAccess from "@/access/use-route-access"
import { Col, Row } from "antd"
import DashboardCard from "./dashboard-card"
import PageContainer from "@/components/general/page-container"

const cards = [
    {
        title: "Workspaces",
        link: "/workspaces",
        content: "Manage your workspaces across all the system.",
    },
    {
        title: "Registries",
        link: "/registries",
        content: "Manage your global and workspace-scoped registries.",
    },
    {
        title: "User Management",
        link: "/users",
        content: "Manage the users, groups, roles and privileges in the system.",
    }
]

export default function DashboardClientPage() {

    const { isAllowed } = useRouteAccess();

    return <PageContainer fluid title="Dashboard">
        <Row gutter={16} style={{ marginTop: 10 }}>
            {
                cards.filter(card => isAllowed(card.link)).map(card => <Col key={card.link} style={{ marginBottom: 16 }} span={8}>
                    <DashboardCard key={card.link} title={card.title} link={card.link} description={card.content} />
                </Col>)
            }
        </Row>
    </PageContainer>
}