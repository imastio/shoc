"use client"

import { useEffect, useMemo, useState } from "react";
import TitleContext from "./title-context";
import { usePathname } from "next/navigation";

export default function TitleProvider({ children } : { children: React.ReactNode }){
    
    const [title, setTitle] = useState('');
    const pathname = usePathname();

    useEffect(() => {
        setTitle('')
    }, [pathname])

    const value = useMemo(() => ({
        title,
        setTitle
    }), [title])


    return <TitleContext.Provider value={value}>
        {children}
    </TitleContext.Provider>
}