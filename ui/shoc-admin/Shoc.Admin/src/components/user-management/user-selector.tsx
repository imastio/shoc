import { selfClient } from "@/clients/shoc";
import UsersClient from "@/clients/shoc/identity/users-client";
import useDebounce from "@/hooks/useDebounce";
import { useApiAuthentication } from "@/providers/api-authentication/use-api-authentication";
import { Empty, Select, Spin } from "antd";
import { useEffect } from "react";
import { useState } from "react";
import { useCallback } from "react";

export function UserSelector(props: any) {

    const { withToken } = useApiAuthentication();
    const [data, setData] = useState<any[] | null>(null);
    const [progress, setProgress] = useState(false);
    const [search, setSearch] = useState(null);
    const debouncedSearch = useDebounce(search);

    const load = useCallback(async (term: string | null) => {
        setProgress(true);
        setData([])
        const result = await withToken((token: string) => selfClient(UsersClient).getAllReferentialValues(token, {
            search: term
        }, 0, term ? 100 : 20));

        setProgress(false);

        if (result.error) {
            setData([]);
            return;
        }

        const { items } = result.payload;

        setData(items.map((obj: any) => ({ label: obj.fullName, value: obj.id })));

    }, [withToken]);

    useEffect(() => {
        load(debouncedSearch);
    }, [load, debouncedSearch])


    return <Select
        {...props}
        filterOption={false}
        showSearch
        searchValue={search}
        onSearch={setSearch}
        notFoundContent={progress ? <Spin size="small" /> : <Empty />}
        options={data || []}
        disabled={props.disabled}
    />

}