import { Col, Row, Space } from "antd";
import { PrivilegeAccessRecords } from "./privilege-access-records";

export default function PrivilegeDetailsTab({ privilegeId, loading = false }: any) {

    return <Row gutter={16}>
        <Col span={12}>
            <Space direction="vertical" style={{ width: '100%' }}>
                <PrivilegeAccessRecords privilegeId={privilegeId} loading={loading} />
            </Space>
        </Col>
    </Row>
}