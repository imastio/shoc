import { signIn } from "@/addons/auth"

export async function GET() {
    
    await signIn('shoc');
   
    return Response.json({ ok: true })
  }