import { selfClient } from "@/clients/shoc";
import WorkspaceMembersClient from "@/clients/shoc/workspace/workspace-members-client";
import { useApiAuthentication } from "@/providers/api-authentication/use-api-authentication";
import { Select } from "antd";
import { useEffect } from "react";
import { useState } from "react";
import { useCallback } from "react";

export function WorkspaceMemberSelector(props: any) {

    const { withToken } = useApiAuthentication();
    const [data, setData] = useState<any[] | null>(null);
    const [progress, setProgress] = useState(false);

    const load = useCallback(async (workspaceId: string) => {
        setProgress(true);

        const result = await withToken((token: string) => selfClient(WorkspaceMembersClient).getAllExtended(token, workspaceId));

        setProgress(false);

        if (result.error) {
            setData([]);
            return;
        }

        setData(result.payload.map((obj: any) => ({ label: obj.fullName, value: obj.userId })));

    }, [withToken]);

    useEffect(() => {

        if (data === null && props.workspaceId) {
            load(props.workspaceId);
        }
    }, [load, data, props.workspaceId])

    if (!data || data.length === 0 || progress) {
        return <Select {...props} value={null} disabled />
    }

    return <Select
        {...props}
        showSearch
        filterOption={(input, option) =>
            (option?.label as string ?? '').toLowerCase().includes(input.toLowerCase())
        }
        options={data}
        disabled={progress || props.disabled}
    />

}