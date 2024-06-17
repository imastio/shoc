import { selfClient } from "@/clients/shoc";
import UsersClient from "@/clients/shoc/identity/users-client";
import TableContainer from "@/components/general/table-container";
import { localDateTime } from "@/extended/format";
import { useApiAuthentication } from "@/providers/api-authentication/use-api-authentication";
import { Card, ConfigProvider, Descriptions, Empty, Skeleton, Table } from "antd";
import { useCallback, useEffect, useState } from "react";

export default function UserSignInHistoryCard(props: any) {

    const columns = [
        {
            title: 'Provider',
            dataIndex: 'provider',
            key: 'provider',
            width: '70px'
        },
        {
            title: 'IP Address',
            dataIndex: 'ip',
            key: 'ip',
            ellipsis: true,
            width: '150px',
        },
        {
            title: 'User Agent',
            dataIndex: 'userAgent',
            key: 'userAgent',
            width: '200px',
            ellipsis: true,
            showTooltip: true
        },
        {
            title: 'Session',
            dataIndex: 'sessionId',
            key: 'sessionId',
            width: '100px',
            render: (sessionId: string) => sessionId || "N/A",
            ellipsis: true
        },
        {
            title: 'Time',
            dataIndex: 'time',
            key: 'time',
            width: '200px',
            ellipsis: true,
            render: (time: string) => localDateTime(time)
        }
    ];

    const { withToken } = useApiAuthentication();
    const [paging, setPaging] = useState({ page: 0, size: 10 });
    const [progress, setProgress] = useState(false);
    const [signinHistory, setSigninHistory] = useState({ items: [], totalCount: 0 });
    const [errors, setErrors] = useState([]);

    const { userId, loading } = props;

    const loadHistory = useCallback(async () => {

        if (!userId) {
            return;
        }

        setProgress(true);

        const result = await withToken((token: string) => selfClient(UsersClient).getSigninHistoryById(token, userId, paging.page, paging.size));

        setProgress(false);

        if (result.error) {
            setErrors(result.payload.errors);
            return;
        }

        setSigninHistory({
            items: result.payload?.items || [],
            totalCount: result.payload?.totalCount || 0
        });
    }, [withToken, paging, userId])

    useEffect(() => {
        loadHistory();
    }, [loadHistory]);

    return (
        <>
            <Card>
                <Skeleton loading={progress} paragraph={{ rows: 5 }}>
                    <Descriptions size="small" column={1} layout="horizontal" bordered={false} labelStyle={{}} title="Sign-in history" />
                    <ConfigProvider theme={{ components: { List: { paddingContentHorizontal: 0 } } }} renderEmpty={() => <Empty description={"No sign-in records to display"}></Empty>}>
                        <TableContainer errors={errors}>
                            <Table
                                rowKey="id"
                                tableLayout="fixed"
                                columns={columns}
                                dataSource={signinHistory.items}
                                loading={progress || loading}
                                scroll={{ x: '100%' }}
                                pagination={{
                                    onChange: (page, size) => setPaging({ page: page - 1, size }),
                                    total: signinHistory.totalCount,
                                    hideOnSinglePage: false,
                                    showPrevNextJumpers: true,
                                    showLessItems: false,
                                    defaultPageSize: paging.size,
                                    current: paging.page + 1,
                                    pageSize: paging.size
                                }}
                            />
                        </TableContainer>

                    </ConfigProvider>
                </Skeleton>
            </Card>
        </>
    )
}
