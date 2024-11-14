import { ConfigProvider, Result } from "antd";
import { ReactNode } from "react";

function getErrorEmpty(errors: any[]){
    
    errors = errors || [];

    if(errors.some(error => error.code === 'ACCESS_ERROR')){
        return <Result title="Access Denied" subTitle="You don't have sufficient permissions for this operation!" status="403" />
    }

    if(errors.some(error => error.code === 'NOT_FOUND_ERROR')){
        return <Result title="Not Found" subTitle="The data you are requesting could not be found!" status="404" />
    }

    if(errors.some(error => error.code === 'VALIDATION_ERROR') || errors.length > 0){
        return <Result title="Invalid Request" subTitle="Seems, your request is not valid to process!" status="error" />
    }

    if(errors.some(error => error.code === 'UNKNOWN_ERROR') || errors.length > 0){
        return <Result title="System Error" subTitle="There is a problem with our systems, please try again a bit later!" status="500" />
    }

    return undefined;
}

export default function TableContainer({ errors, children }: { errors: any[], children: ReactNode }){

    const emptyComponent = getErrorEmpty(errors);
    const emptyRenderProps = emptyComponent ? { renderEmpty: () => emptyComponent } : {};

    return <ConfigProvider {...emptyRenderProps}>
        {children}
    </ConfigProvider>
}