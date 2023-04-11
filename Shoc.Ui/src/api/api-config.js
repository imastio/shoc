const getApiConfig = () => ({
    root: process.env.REACT_APP_API_ROOT,
    gateway: process.env.REACT_APP_API_GATEWAY || "",
    serviceRouting: process.env.REACT_APP_API_SERVICE_ROUTING || true,
    client: process.env.REACT_APP_API_CLIENT || "shoc-ui",
    timeout: parseInt(process.env.REACT_APP_API_TIMEOUT) || 0    
});

export default getApiConfig;
