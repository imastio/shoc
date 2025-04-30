"use client"

import { Button } from "@/components/ui/button";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Sheet, SheetContent, SheetDescription, SheetHeader, SheetTitle, SheetTrigger } from "@/components/ui/sheet";
import Link from "next/link";
import useWorkspaceMenu from "@/app/workspaces/[workspaceName]/use-workspace-menu";
import { cn } from "@/lib/utils";
import MenuIcon from "../icons/menu-icon";
import { useIntl } from "react-intl";
import { useState } from "react";

export default function WorkspaceMobileSidebar({ name }: { name: string }) {

  const menu = useWorkspaceMenu({ name });
  const intl = useIntl();
  const [open, setOpen] = useState(false);


  return <Sheet open={open} onOpenChange={setOpen}>
    <SheetTrigger asChild>
      <Button variant="outline" size="icon" className="shrink-0 md:hidden">
        <MenuIcon className="h-5 w-5" />
        <span className="sr-only">Toggle navigation menu</span>
      </Button>
    </SheetTrigger>
    <SheetContent side="left" className="flex flex-col">
      <SheetHeader>
        <SheetTitle>{intl.formatMessage({ id: 'workspaces.workspace' })}</SheetTitle>
        <SheetDescription>
        </SheetDescription>
      </SheetHeader>
      <nav className="grid gap-2 text-lg font-medium">
        {menu.map((item, index) => <Link
          key={index}
          href={item.path}
          onClick={() => setOpen(false)}
          className={cn("mx-[-0.65rem] flex items-center gap-4 rounded-xl px-3 py-2 transition-all hover:text-foreground", item.active ? "bg-muted text-primary" : "text-muted-foreground")}
          prefetch={false}
        >
          <item.icon className="h-5 w-5" />
          {item.title}
        </Link>
        )}
      </nav>
      <div className="mt-auto">
        <Card>
          <CardHeader>
            <CardTitle>Version Alpha</CardTitle>
            <CardDescription>The application is still in a very early testing phase. If you face any problems while using it, just let us know.</CardDescription>
          </CardHeader>
          <CardContent>
          </CardContent>
        </Card>
      </div>
    </SheetContent>
  </Sheet>

}