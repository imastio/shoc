"use client"

import useTitle from "@/providers/title-provider/use-title";
import { PageContainer as AntPageContainer, PageContainerProps } from "@ant-design/pro-layout"
import { useRouter } from "next/navigation"
import { useEffect } from "react";

export default function PageContainer({ fluid = false, title, onBack, children, ...rest } : { fluid: boolean } & PageContainerProps){

    const router = useRouter();
    const { title: pageTitle, setTitle } = useTitle();

    useEffect(() => {
        if(title as string){
            setTitle(title as string)
        }
    }, [title])


    return <>
        <AntPageContainer {...rest} title={ title as string ? pageTitle : title } onBack={onBack ? onBack : () => router.back()}>
            { children }
        </AntPageContainer>
    </>

}