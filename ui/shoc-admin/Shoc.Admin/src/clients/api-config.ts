import { shocApi } from "@/addons/settings";

type ApiConfig = {
    root: string;
    gateway: string;
    serviceRouting: boolean;
    client: string;
    timeout: number;  
}

export function selfApiConfig(): ApiConfig {
    return {
        root: '/',
        gateway: 'api/fwd-shoc',
        serviceRouting: true,
        client: 'space',
        timeout: 60000
    }
}

export function shocApiConfig(): ApiConfig {
    const options = shocApi();
    return {
        root: options.root || '',
        gateway: options.gateway || '',
        serviceRouting: true,
        client: 'space',
        timeout: 60000
    }
}

export default ApiConfig;