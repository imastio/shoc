import { Col, Row, Space } from "antd";
import { RolePrivilegeRecordsCard } from "./role-privilege-records-card";
import { RoleMembersCard } from "./role-members-card";

export default function RoleDetailsTab({ roleId, loading = false }: any) {

    return <Row gutter={16}>
        <Col span={12}>
            <Space direction="vertical" style={{ width: '100%' }}>
                <RoleMembersCard roleId={roleId} loading={loading} />
            </Space>
        </Col>
        <Col span={12}>
            <RolePrivilegeRecordsCard roleId={roleId} loading={loading} />
        </Col>
    </Row>
}