'use client'

import { useIntl } from "react-intl";
import { Button } from "@/components/ui/button";
import { rpcDirect } from "@/server-actions/rpc";
import { useState } from "react";

export default function LogoutButton() {
    const intl = useIntl();
    const [progress, setProgress] = useState(false);

    return <Button variant="ghost" disabled={progress} onClick={async () => {
        setProgress(true);
        await rpcDirect('auth/signOut');
    }}>
        {intl.formatMessage({ id: 'logout' })}
    </Button>
}