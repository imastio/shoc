"use client"

import * as React from "react"
import { type ColumnDef } from "@tanstack/react-table"
import { Badge } from "@/components/ui/badge"
import { WorkspaceMember } from "./types"
import DataTableColumnHeader from "@/components/data-table/data-table-column-header"
import { useIntl } from "react-intl"
import KeyIcon from "@/components/icons/key-icon"
import { workspaceRolesMap } from "@/app/workspaces/(chooser)/_components/well-known"
import { localDate } from "@/extended/format"
import { MoreHorizontal, TrashIcon } from "lucide-react"
import { Button } from "@/components/ui/button"
import { DropdownMenu, DropdownMenuContent, DropdownMenuItem, DropdownMenuLabel, DropdownMenuSeparator, DropdownMenuTrigger } from "@/components/ui/dropdown-menu"
import WorkspaceMemberDeleteDialog from "./workspace-member-delete-dialog"


export default function useWorkspaceMembersColumns(): ColumnDef<WorkspaceMember>[] {
  const intl = useIntl();
  return React.useMemo(() => [
    {
      accessorKey: "fullName",
      header: ({ column }) => (
        <DataTableColumnHeader column={column} title={intl.formatMessage({ id: 'global.labels.name' })} />
      ),
      cell: ({ row }) => <div>{row.getValue("fullName")}</div>,
      enableSorting: true,
      enableHiding: false,
      size: 100
    },
    {
      accessorKey: "email",
      header: ({ column }) => (
        <DataTableColumnHeader column={column} title={intl.formatMessage({ id: 'global.labels.email' })} />
      ),
      cell: ({ row }) => <div>{row.getValue("email")}</div>,
      enableSorting: false,
      enableHiding: false,
    },
    {
      accessorKey: "role",
      header: ({ column }) => (
        <DataTableColumnHeader column={column} title={intl.formatMessage({ id: 'workspaces.labels.role' })} />
      ),
      cell: ({ row }) => <Badge variant="secondary">
        <KeyIcon className="w-4 h-4 mr-1" />
        {intl.formatMessage({ id: 'workspaces.labels.role' })}: {intl.formatMessage({ id: workspaceRolesMap[row.getValue('role') as string] })}
      </Badge>,
      enableSorting: true,
      enableHiding: false,
    },
    {
      accessorKey: "created",
      header: ({ column }) => (
        <DataTableColumnHeader column={column} title={intl.formatMessage({ id: 'workspace.members.labels.since' })} />
      ),
      cell: ({ row }) => localDate(row.getValue('created')),
      enableSorting: true,
      enableHiding: false,
    },
    {
      id: "actions",
      enableHiding: false,
      enableSorting: false,
      header: ({ column }) => (
        <DataTableColumnHeader column={column} title={intl.formatMessage({ id: 'global.labels.actions' })} />
      ),
      cell: function Cell({ row }){
        const workspace = row.original
        const [deleteOpen, setDeleteOpen] = React.useState(false);

        return (<>
          <WorkspaceMemberDeleteDialog
            open={deleteOpen}
            onClose={() => setDeleteOpen(false)}
            item={row.original}
          />
          <DropdownMenu>
            <DropdownMenuTrigger asChild>
              <Button variant="ghost" className="h-8 w-8 p-0">
                <span className="sr-only">Open menu</span>
                <MoreHorizontal className="h-4 w-4" />
              </Button>
            </DropdownMenuTrigger>
            <DropdownMenuContent align="end">
              <DropdownMenuLabel>{intl.formatMessage({ id: 'global.labels.actions' })}</DropdownMenuLabel>
              <DropdownMenuItem
                onClick={() => navigator.clipboard.writeText(workspace.id)}
              >
                Copy payment ID
              </DropdownMenuItem>
              <DropdownMenuSeparator />
              <DropdownMenuItem>View customer</DropdownMenuItem>
              <DropdownMenuItem className="text-red-600 hover:!text-red-600 hover:!bg-red-100" onClick={() => setDeleteOpen(true)}>
                Delete
              </DropdownMenuItem>
            </DropdownMenuContent>
          </DropdownMenu>
        </>

        )
      }
    }
  ], [intl])
}