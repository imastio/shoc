'use client'

import { useIntl } from "react-intl";
import { Button } from "@/components/ui/button";
import { rpcDirect } from "@/server-actions/rpc";
import { useState } from "react";

export default function LoginButton() {
    const intl = useIntl();
    const [progress, setProgress] = useState(false);

    return <Button disabled={progress} variant="ghost" onClick={async () => {
        setProgress(true);
        await rpcDirect('auth/signIn', { locale: intl.locale })
    }}>
        {intl.formatMessage({ id: 'login' })}
    </Button>
}