"use client"

import { Separator } from "@/components/ui/separator"
import { SidebarTrigger } from "@/components/ui/sidebar"
import { ReactNode } from "react"

export default function WorkspacePageHeader({ breadcrumb, actions }: { breadcrumb: ReactNode, actions?: ReactNode }) {
  return (
    <header className="flex h-14 shrink-0 items-center gap-2">
          <div className="flex flex-1 items-center gap-2 px-3">
            <SidebarTrigger />
            <Separator orientation="vertical" className="mr-2 !h-4" />
            {breadcrumb}
          </div>
          {actions && <div className="ml-auto px-3">
            {actions}
          </div>}
        </header>
  )
}
