"use client"

import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import Link from "next/link";
import { cn } from "@/lib/utils";
import useWorkspaceMenu from "@/app/workspaces/[name]/use-workspace-menu";

export default function WorkspaceSidebar({ name }: { name: string }) {

    const menu = useWorkspaceMenu({ name });

    return <div className="hidden border-r bg-muted/40 md:block w-[280px]">
        <div className="flex h-full max-h-screen flex-col pt-8 gap-2">
            <div className="flex-1">
                <nav className="grid items-start px-2 text-sm font-medium lg:px-4">
                    {menu.map((item, index) => <Link
                        key={index}
                        href={item.path}
                        className={cn("flex items-center gap-3 rounded-lg px-3 py-2 transition-all hover:text-primary", item.active ? "bg-muted text-primary" : "text-muted-foreground")}
                        prefetch={false}
                    >
                        <item.icon className="h-4 w-4" />
                        {item.title}
                    </Link>
                    )}
                </nav>
            </div>
            <div className="mt-auto p-4">
                <Card x-chunk="dashboard-02-chunk-0">
                    <CardHeader className="p-2 pt-0 md:p-4">
                        <CardTitle>Upgrade to Pro</CardTitle>
                        <CardDescription>Unlock all features and get unlimited access to our support team.</CardDescription>
                    </CardHeader>
                    <CardContent className="p-2 pt-0 md:p-4 md:pt-0">
                        <Button size="sm" className="w-full">
                            Upgrade
                        </Button>
                    </CardContent>
                </Card>
            </div>
        </div>
    </div>
}