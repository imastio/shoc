import useAuth from "@/providers/auth-provider/use-auth"

export default function IndexPage(){
    const auth = useAuth();
    return<> <div>
        Hello, {auth?.user?.fullName || 'Anonymous'}
    </div>
    <pre>
    {JSON.stringify(auth, null, 4)}
    </pre>
    </>
}