import { Col, Row, Space } from "antd";
import UserProfileCard from "./user-profile-card";
import { UserGroupRecordsCard } from "./user-group-records-card";
import { UserRoleRecordsCard } from "./user-role-records-card";
import { UserAccessRecords } from "./user-access-records";
import UserSignInHistoryCard from "./user-sign-in-history-card";

export default function UserDetailsTab({ userId, loading = false } : { userId: string, loading: boolean }) {

    return <Row gutter={16}>
        <Col lg={8} md={24}>
            <Space direction="vertical" style={{ width: '100%' }}>
                <UserProfileCard userId={userId} loading={loading} />
                <UserRoleRecordsCard userId={userId} loading={loading} />
                <UserGroupRecordsCard userId={userId} loading={loading} />
            </Space>
        </Col>
        <Col lg={16} md={24}>
            <Space direction="vertical" style={{ width: '100%' }}>
                <UserSignInHistoryCard userId={userId} loading={loading} />
                <UserAccessRecords userId={userId} loading={loading} />
            </Space>
        </Col>
    </Row>

}