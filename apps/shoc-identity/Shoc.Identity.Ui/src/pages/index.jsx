import useSession from "@/providers/session-provider/use-session"
import { Helmet } from "react-helmet-async";

export default function IndexPage(){
    const session = useSession();
    return <>
        <Helmet title="Welcome" />
     <div>
        Hello, {session.user?.fullName || 'Anonymous'}
    </div>
    <pre>
    {JSON.stringify(session, null, 4)}
    </pre>
    </>
}