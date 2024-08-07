import { ReactNode } from "react";

export default function BasicHeader( { title,  actions } : { title: string, actions?: ReactNode[] } ) {
    return <div className="flex">
        <div className="flex-1 items-center">
            <h1 className="text-lg truncate font-semibold md:text-2xl">{title}</h1>
        </div>
        <div className="flex items-center md:ml-4">
            {actions}
        </div>
    </div>
}