import { Col, Row, Space } from "antd";
import ApplicationGeneralCard from "./application-general-card";
import ApplicationTokensCard from "./application-tokens-card";

export default function ApplicationDetailsTab({ application = {}, loading = false, onUpdate = () => { } }: any) {

    return <Row gutter={16}>
        <Col lg={8} md={24}>
            <Space direction="vertical" style={{ width: '100%' }}>
                <ApplicationGeneralCard application={application} loading={loading} onUpdate={onUpdate} />
            </Space>
        </Col>
        <Col lg={8} md={24}>
            <Space direction="vertical" style={{ width: '100%' }}>
                <ApplicationTokensCard application={application} loading={loading} onUpdate={onUpdate} />
            </Space>
        </Col>
    </Row>

}