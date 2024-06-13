import React from "react"
import Link from "next/link";

export default function ConsoleSiderLogo({ small }){
    
    const linkStyle = { 
        margin: `auto ${small ? 'auto' : '24px'}`,
        display: "block" 
    };

    return (
        <div style={{
            height: "64px",
            display: "flex"
        }}>
            <Link href="/" style={linkStyle}>
                Shoc Platform
            </Link>
        </div>
    )
}


