import { selfClient } from "@/clients/shoc";
import UserGroupsClient from "@/clients/shoc/identity/user-groups-client";
import { useApiAuthentication } from "@/providers/api-authentication/use-api-authentication";
import { Select } from "antd";
import { useEffect } from "react";
import { useState } from "react";
import { useCallback } from "react";

export function UserGroupSelector(props: any) {

    const { withToken } = useApiAuthentication();
    const [data, setData] = useState<any[] | null>(null);
    const [progress, setProgress] = useState(false);

    const load = useCallback(async () => {
        setProgress(true);

        const result = await withToken((token: string) => selfClient(UserGroupsClient).getAll(token));

        setProgress(false);

        if (result.error) {
            setData([]);
            return;
        }

        setData(result.payload.map((obj: any) => ({ label: obj.name, value: obj.id })));

    }, [withToken]);

    useEffect(() => {

        if (data === null) {
            load();
        }
    }, [load, data])

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