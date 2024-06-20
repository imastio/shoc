import { Col, Row, Space } from "antd";
import { WorkspaceMembersCard } from "./workspace-members-card";

export default function WorkspaceMembersTab({ workspaceId, loading = false }: any) {

    return <Row gutter={16}>
        <Col span={12}>
            <Space direction="vertical" style={{ width: '100%' }}>
                <WorkspaceMembersCard workspaceId={workspaceId} loading={loading} />
            </Space>
        </Col>
       
    </Row>
}