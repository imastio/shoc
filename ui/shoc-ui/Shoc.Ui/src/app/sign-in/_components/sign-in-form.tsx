"use client"

import SpinnerIcon from "@/components/icons/spinner-icon"
import { Button } from "@/components/ui/button"
import {
    Card,
    CardContent,
    CardDescription,
    CardHeader,
    CardTitle,
} from "@/components/ui/card"
import { rpcDirect } from "@/server-actions/rpc"
import { useCallback, useEffect, useState } from "react"
import { useIntl } from "react-intl"

export default function SignInForm({ expired, next = '/' }: { expired?: boolean, next: string }) {

    const intl = useIntl();
    const [progress, setProgress] = useState(false);

    const submit = useCallback(async () => {

        setProgress(true);
        await rpcDirect('auth/signIn', { redirectTo: next.startsWith('/sign-in') ? '/' : next })
    }, [next]);

    useEffect(() => {

        if(!expired){
            submit()
        }

    }, [expired])

    return <Card className="mx-auto my-auto max-w-sm">
        <CardHeader>
            <CardTitle className="text-xl">{intl.formatMessage({ id: expired ? 'auth.sessionExpired' : 'auth.signIn' })}</CardTitle>
            <CardDescription>
                {intl.formatMessage({ id: expired ? 'auth.notice.expired' : 'auth.notice.signIn' })}
            </CardDescription>
        </CardHeader>
        <CardContent>
            <form>
                <div className="grid gap-4">
                    <Button type="submit" onClick={() => submit()} className="w-full" disabled={progress}>
                    {progress && <SpinnerIcon className="mr-2 h-4 w-4 animate-spin" />}
                    {intl.formatMessage({ id: 'global.actions.continue' })}
                    </Button>
                </div>
            </form>
        </CardContent>
    </Card>
} 