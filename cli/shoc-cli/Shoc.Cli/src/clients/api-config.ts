
type ApiConfig = {
    root: string;
    gateway: string;
    serviceRouting: boolean;
    client: string;
    timeout: number;  
}

export function shocApiConfig(apiRoot: string): ApiConfig {
    return {
        root: apiRoot,
        gateway: 'api/fwd-direct',
        serviceRouting: true,
        client: 'cli',
        timeout: 60000
    }
}

export default ApiConfig;