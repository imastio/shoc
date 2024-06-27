"use client"

import { ReactNode } from "react";
import { useIntl } from "react-intl"

export default function WorkspacesHeader( { actions } : { actions?: ReactNode[] } ) {
    const intl = useIntl();

    return <div className="flex">
        <div className="flex-1 items-center">
            <h1 className="text-lg truncate font-semibold md:text-2xl">{intl.formatMessage({ id: 'workspaces' })}</h1>
        </div>
        <div className="flex items-center md:ml-4">
            {actions}
        </div>
    </div>
}