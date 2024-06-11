import { auth, getJwt } from "@/addons/auth"
import { getToken } from "@auth/core/jwt";
import { cookies } from "next/headers"

export default async function Home() {

  const session = await auth();

  return <div>
    <p>Server Side Session: {JSON.stringify(session, null, 4)}</p>
    <p>Server Side JWT: {JSON.stringify(await getJwt(), null, 4)}</p>
    </div>
}
