import { Col, Row, Space } from "antd";
import ApplicationSecretsCard from "./application-secrets-card";
import ApplicationClaimsCard from "./application-claims-card";
import ApplicationUrisCard from "./application-uris-card";

export default function ApplicationExtendedTab({ application = {}, loading = false, onUpdate = () => { } }: any) {

    return <Row gutter={16}>
        <Col lg={12} md={24}>
            <Space direction="vertical" style={{ width: '100%' }}>
                <ApplicationSecretsCard applicationId={application?.id} loading={loading} onUpdate={onUpdate} />
                <ApplicationClaimsCard applicationId={application?.id} loading={loading} onUpdate={onUpdate} />
            </Space>
        </Col>
        <Col lg={12} md={24}>
            <Space direction="vertical" style={{ width: '100%' }}>
                <ApplicationUrisCard applicationId={application?.id} loading={loading} onUpdate={onUpdate} />
            </Space>
        </Col>
    </Row>

}