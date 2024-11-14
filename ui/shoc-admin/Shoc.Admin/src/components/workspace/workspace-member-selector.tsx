import { selfClient } from "@/clients/shoc";
import WorkspaceMembersClient from "@/clients/shoc/workspace/workspace-members-client";
import { useApiAuthentication } from "@/providers/api-authentication/use-api-authentication";
import { Select } from "antd";
import { useEffect } from "react";
import { useState } from "react";
import { useCallback } from "react";

export function WorkspaceMemberSelector({ workspaceId, ...rest }: any) {

    const { withToken } = useApiAuthentication();
    const [data, setData] = useState<any[] | null>(null);
    const [progress, setProgress] = useState(false);

    const load = useCallback(async (id: string) => {
        setProgress(true);

        const result = await withToken((token: string) => selfClient(WorkspaceMembersClient).getAllExtended(token, id));

        setProgress(false);

        if (result.error) {
            setData([]);
            return;
        }

        setData(result.payload.map((obj: any) => ({ label: obj.fullName, value: obj.userId })));

    }, [withToken]);

    useEffect(() => {

        if (data === null && workspaceId) {
            load(workspaceId);
        }
    }, [load, data, workspaceId])

    if (!data || data.length === 0 || progress) {
        return <Select {...rest} value={null} disabled />
    }

    return <Select
        {...rest}
        showSearch
        filterOption={(input, option) =>
            (option?.label as string ?? '').toLowerCase().includes(input.toLowerCase())
        }
        options={data}
        disabled={progress || rest.disabled}
    />

}