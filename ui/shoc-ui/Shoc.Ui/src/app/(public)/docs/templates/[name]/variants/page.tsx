import { redirect, RedirectType } from "next/navigation";

export default async function TemplatePage({ params: { name } }: any) {

  redirect(`/docs/templates/${name}/variants/default`, RedirectType.replace)
}
