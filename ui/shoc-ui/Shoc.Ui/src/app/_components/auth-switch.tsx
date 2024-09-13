import { auth } from "@/addons/auth";
import LoginButton from "./login-button";
import LogoutButton from "./logout-button";

export default async function AuthSwitch(){
    const session = await auth();

    if(session){
        return <LogoutButton />
    }

    return <LoginButton />
}