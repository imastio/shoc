import { redirect, RedirectType } from "next/navigation";

export default async function TemplatePage(props: any) {
  const params = await props.params;

  const {
    name
  } = params;

  redirect(`/docs/templates/${name}/variants/default`, RedirectType.replace)
}
