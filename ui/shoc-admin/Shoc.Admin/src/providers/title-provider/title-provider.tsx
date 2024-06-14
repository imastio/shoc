"use client"

import { useMemo, useState } from "react";
import TitleContext from "./title-context";

export default function TitleProvider({ children } : { children: React.ReactNode }){
    
    const [title, setTitle] = useState('');
    const value = useMemo(() => ({
        title,
        setTitle
    }), [title])


    return <TitleContext.Provider value={value}>
        {children}
    </TitleContext.Provider>
}