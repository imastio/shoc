import { redirect, RedirectType } from "next/navigation";

export default async function Home() {

  redirect('/workspaces', RedirectType.push)
}
