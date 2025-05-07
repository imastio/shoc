"use client"

import {
  ChevronsUpDown,
  LogOut,
} from "lucide-react"

import {
  Avatar,
  AvatarFallback,
  AvatarImage,
} from "@/components/ui/avatar"
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu"
import {
  SidebarMenu,
  SidebarMenuButton,
  SidebarMenuItem,
  useSidebar,
} from "@/components/ui/sidebar"
import { useSession } from "next-auth/react"
import { rpc } from "@/server-actions/rpc"
import { useIntl } from "react-intl"

export default function NavUser() {
  const { isMobile } = useSidebar()
  const session = useSession();
  const intl = useIntl();

  return (
    <SidebarMenu>
      <SidebarMenuItem>
        <DropdownMenu modal={false}>
          <DropdownMenuTrigger asChild>
            <SidebarMenuButton
              disabled={!session || session.status !== 'authenticated'}
              size="lg"
              className="data-[state=open]:bg-sidebar-accent data-[state=open]:text-sidebar-accent-foreground"
            >
              <Avatar className="h-8 w-8 rounded-lg">
                <AvatarImage src={session?.data?.user?.image || undefined} alt={session?.data?.user?.name || ''} />
                <AvatarFallback className="rounded-lg">{session?.data?.user?.name?.charAt(0) || ''}</AvatarFallback>
              </Avatar>
              <div className="grid flex-1 text-left text-sm leading-tight">
                <span className="truncate font-semibold">{session?.data?.user?.name}</span>
                <span className="truncate text-xs">{session?.data?.user?.email}</span>
              </div>
              <ChevronsUpDown className="ml-auto size-4" />
            </SidebarMenuButton>
          </DropdownMenuTrigger>
          <DropdownMenuContent
            className="w-[--radix-dropdown-menu-trigger-width] min-w-56 rounded-lg"
            side={isMobile ? "bottom" : "right"}
            align="end"
            sideOffset={4}
          >
            <DropdownMenuLabel className="p-0 font-normal">
              <div className="flex items-center gap-2 px-1 py-1.5 text-left text-sm">
                <Avatar className="h-8 w-8 rounded-lg">
                  <AvatarImage src={session?.data?.user?.image || undefined} alt={session?.data?.user?.name || ''} />
                  <AvatarFallback className="rounded-lg">{session?.data?.user?.name?.charAt(0) || ''}</AvatarFallback>
                </Avatar>
                <div className="grid flex-1 text-left text-sm leading-tight">
                  <span className="truncate font-semibold">{session?.data?.user?.name}</span>
                  <span className="truncate text-xs">{session?.data?.user?.email}</span>
                </div>
              </div>
            </DropdownMenuLabel>
            <DropdownMenuSeparator />
            <DropdownMenuItem onClick={async () => {
                const { errors, data } = await rpc('auth/signleSignOut', { postLogoutRedirectUri: `${window.location.origin}/signed-out` })
                if(errors || !data){
                  return;
                }
                console.log('redirecting to ', data.redirectUri)
                document.location.href = data.redirectUri
            }}>
              <LogOut />
              {intl.formatMessage({id: 'logout'})}
            </DropdownMenuItem>
          </DropdownMenuContent>
        </DropdownMenu>
      </SidebarMenuItem>
    </SidebarMenu>
  )
}
