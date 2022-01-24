const getApiConfig = () => ({
    root: process.env.REACT_APP_API_ROOT || "",
    gateway: process.env.REACT_APP_API_GATEWAY || "",
    client: process.env.REACT_APP_API_CLIENT || "identity",
    timeout: parseInt(process.env.REACT_APP_API_TIMEOUT) || 0    
});

export default getApiConfig;
