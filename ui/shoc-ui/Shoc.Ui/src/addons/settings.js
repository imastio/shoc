export function baseUrl(){
    return process.env.SHOC_BASE_URL;
}

export function shocApi(){
    return {
        root: process.env.SHOC_API_ROOT,
        gateway: process.env.SHOC_API_GATEWAY
    };
}