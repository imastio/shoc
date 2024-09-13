export function baseUrl(){
    return process.env.SHOC_UI_BASE_URL;
}

export function shocApi(){
    return {
        root: process.env.SHOC_UI_API_ROOT,
        gateway: process.env.SHOC_UI_API_GATEWAY
    };
}