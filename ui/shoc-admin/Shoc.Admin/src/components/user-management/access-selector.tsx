import { selfClient } from "@/clients/shoc";
import ClusterAccessesClient from "@/clients/shoc/cluster/cluster-accesses-client";
import IdentityAccessClient from "@/clients/shoc/identity/identity-accesses-client";
import JobAccessesClient from "@/clients/shoc/job/job-accesses-client";
import PackageAccessesClient from "@/clients/shoc/package/package-accesses-client";
import RegistryAccessesClient from "@/clients/shoc/registry/registry-accesses-client";
import SecretAccessesClient from "@/clients/shoc/secret/secret-accesses-client";
import SettingsAccessesClient from "@/clients/shoc/settings/settings-accesses-client";
import WorkspaceAccessesClient from "@/clients/shoc/workspace/workspace-accesses-client";
import { useApiAuthentication } from "@/providers/api-authentication/use-api-authentication";
import { Select } from "antd";
import { useEffect } from "react";
import { useState } from "react";
import { useCallback } from "react";

const getFetcher = (area: string) => {

    if(area === 'identity'){
        const client = selfClient(IdentityAccessClient);
        return client.getAll.bind(client);
    }

    if(area === 'workspace'){
        const client = selfClient(WorkspaceAccessesClient);
        return client.getAll.bind(client);
    }

    if(area === 'settings'){
        const client = selfClient(SettingsAccessesClient);
        return client.getAll.bind(client);
    }

    if(area === 'registry'){
        const client = selfClient(RegistryAccessesClient);
        return client.getAll.bind(client);
    }

    if(area === 'cluster'){
        const client = selfClient(ClusterAccessesClient);
        return client.getAll.bind(client);
    }

    if(area === 'secret'){
        const client = selfClient(SecretAccessesClient);
        return client.getAll.bind(client);
    }

    if(area === 'package'){
        const client = selfClient(PackageAccessesClient);
        return client.getAll.bind(client);
    }

    if(area === 'job'){
        const client = selfClient(JobAccessesClient);
        return client.getAll.bind(client);
    }

    return () => Promise.resolve(null) ;
}

export function AccessSelector({ area, filter = [], ...rest }: any) {

    const { withToken } = useApiAuthentication();
    const [data, setData] = useState<any[] | null>(null);
    const [progress, setProgress] = useState(false);

    const load = useCallback(async () => {
        setProgress(true);
        const result = await withToken((token: string) => getFetcher(area)(token));
        setProgress(false);

        if (result.error) {
            setData([]);
            return;
        }

        setData(result.payload.filter((obj: any) => !filter.some((item: any) => item === obj)).map((obj: any) => ({ label: obj, value: obj })));

    }, [withToken, area, filter]);

    useEffect(() => {

        if(!area){
            return;
        }

        if(data === null){
            load();
        }
    }, [load, area, data])

    useEffect(() => {
        setData(null);
    }, [area])

    if (!data || data.length === 0 || progress) {
        return <Select {...rest} value={null} disabled />
    }

    return <Select
        {...rest}
        showSearch
        mode='multiple'
        filterOption={(input, option) =>
            (option?.label as string ?? '').toLowerCase().includes(input.toLowerCase())
        }
        options={data}
        disabled={progress || rest.disabled}
    />

}