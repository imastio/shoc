import { selfClient } from "@/clients/shoc";
import PrivilegesClient from "@/clients/shoc/identity/privileges-client";
import { useApiAuthentication } from "@/providers/api-authentication/use-api-authentication";
import { Select } from "antd";
import { useEffect, useMemo } from "react";
import { useState } from "react";
import { useCallback } from "react";

export function PrivilegeSelector({ filter = [], ...rest }: any) {
    const { withToken } = useApiAuthentication();
    const [data, setData] = useState<any[] | null>(null);
    const [progress, setProgress] = useState(false);

    const load = useCallback(async () => {
        setProgress(true);

        const result = await withToken((token: string) => selfClient(PrivilegesClient).getAllReferentialValues(token));

        setProgress(false);

        if (result.error) {
            setData([]);
            return;
        }

        setData(result.payload);

    }, [withToken]);

    const filteredData = useMemo(() => {
        if(!data) {
            return null;
        }
        const filteredPrivileges = data.filter(obj => !filter.some((item: any) => item === obj.id));
        const groupedByCategory = Object.groupBy(filteredPrivileges, ({ category }) => category);
        const categories = Object.keys(groupedByCategory);

        return categories.map(category => ({ label: category, title: category, options: groupedByCategory[category]?.map(obj => ({label: obj.name, value: obj.id })) || [] }))
       
    }, [data, filter])

    useEffect(() => {

        if(data === null){
            load();
        }

    }, [load, data])

    if (!filteredData || filteredData.length === 0 || progress) {
        return <Select {...rest} value={null} disabled />
    }

    return <Select
        {...rest}
        showSearch
        filterOption={(input, option) =>
            (option?.label as string ?? '').toLowerCase().includes(input.toLowerCase())
        }
        options={filteredData}
        disabled={progress || rest.disabled}
    />

}