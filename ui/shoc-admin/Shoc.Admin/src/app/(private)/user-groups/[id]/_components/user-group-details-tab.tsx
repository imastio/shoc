import { Col, Row, Space } from "antd";
import { UserGroupMembersCard } from "./user-group-members-card";
import { UserGroupAccessRecords } from "./user-group-access-records";

export default function UserGroupDetailsTab({ groupId, loading = false }: any) {

    return <Row gutter={16}>
        <Col span={12}>
            <Space direction="vertical" style={{ width: '100%' }}>
                <UserGroupMembersCard groupId={groupId} loading={loading} />
            </Space>
        </Col>
        <Col span={12}>
            <UserGroupAccessRecords groupId={groupId} loading={loading} />
        </Col>
    </Row>
}