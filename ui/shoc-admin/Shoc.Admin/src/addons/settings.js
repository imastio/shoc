export function baseUrl(){
    return process.env.SHOC_ADMIN_BASE_URL;
}

export function shocApi(){
    return {
        root: process.env.SHOC_ADMIN_API_ROOT,
        gateway: process.env.SHOC_ADMIN_API_GATEWAY
    };
}