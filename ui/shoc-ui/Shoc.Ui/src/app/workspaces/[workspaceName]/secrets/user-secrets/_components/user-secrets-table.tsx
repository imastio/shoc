"use client"

import DataTable from "@/components/data-table"
import { type ColumnDef } from "@tanstack/react-table"
import DataTableColumnHeader from "@/components/data-table/data-table-column-header"
import { useIntl } from "react-intl"
import { localDate } from "@/extended/format"
import { KeyRoundIcon, MoreHorizontal } from "lucide-react"
import { Button } from "@/components/ui/button"
import { DropdownMenu, DropdownMenuContent, DropdownMenuItem, DropdownMenuLabel, DropdownMenuTrigger } from "@/components/ui/dropdown-menu"
import { rpc } from "@/server-actions/rpc"
import { useCallback, useEffect, useMemo, useState } from "react"
import useWorkspaceAccess from "@/providers/workspace-access/use-workspace-access"
import DataTableToolbar from "@/components/data-table/data-table-toolbar"
import { WorkspacePermissions } from "@/well-known/workspace-permissions"
import UserSecretCreateDialog from "./user-secret-create-dialog"
import UserSecretUpdateDialog from "./user-secret-update-dialog"
import UserSecretValueUpdateDialog from "./user-secret-value-update-dialog"
import UserSecretDeleteDialog from "./user-secret-delete-dialog"

export default function UserSecretsTable({ workspaceId, className }: { workspaceId: string, className?: string }) {

  const intl = useIntl();
  const { hasAny } = useWorkspaceAccess();
  const [progress, setProgress] = useState(true);
  const [data, setData] = useState<any[]>([]);
  const [errors, setErrors] = useState<any[]>([]);
  const [creatingActive, setCreatingActive] = useState<any>(false);
  const [editingItem, setEditingItem] = useState<any>(null);
  const [editingValueItem, setEditingValueItem] = useState<any>(null);
  const [deletingItem, setDeletingItem] = useState<any>(null);

  const load = useCallback(async (workspaceId: string) => {

    setProgress(true);
    const { data, errors } = await rpc('secret/workspace-user-secrets/getAll', { workspaceId })

    if (errors) {
      setErrors(errors);
      setData([]);
    } else {
      setErrors([]);
      setData(data)
    }

    setProgress(false);

  }, []);


  useEffect(() => {
    if (!workspaceId) {
      return;
    }
    load(workspaceId);
  }, [workspaceId, load])

  const columns: ColumnDef<any>[] = useMemo(() => [
    {
      accessorKey: "name",
      header: ({ column }) => (
        <DataTableColumnHeader column={column} title={intl.formatMessage({ id: 'global.labels.name' })} />
      ),
      cell: ({ row }) => <div>{row.getValue("name")}</div>,
      enableSorting: true,
      enableHiding: false,
    },
    {
      accessorKey: "description",
      header: ({ column }) => (
        <DataTableColumnHeader column={column} title={intl.formatMessage({ id: 'global.labels.description' })} />
      ),
      cell: ({ row }) => <div>{row.getValue("description")}</div>,
      enableSorting: true,
      enableHiding: false,
    },
    {
      accessorKey: "disabled",
      header: ({ column }) => (
        <DataTableColumnHeader column={column} title={intl.formatMessage({ id: 'global.labels.disabled' })} />
      ),
      cell: ({ row }) => <div>{row.getValue("disabled") ? 'Disabled' : 'Enabled'}</div>,
      enableSorting: true,
      enableHiding: false,
    },
    {
      accessorKey: "encrypted",
      header: ({ column }) => (
        <DataTableColumnHeader column={column} title={intl.formatMessage({ id: 'global.labels.encrypted' })} />
      ),
      cell: ({ row }) => <div>{row.getValue("encrypted") ? 'Encrypted' : 'Plain Text'}</div>,
      enableSorting: true,
      enableHiding: false,
    },
    {
      accessorKey: "value",
      header: ({ column }) => (
        <DataTableColumnHeader column={column} title={intl.formatMessage({ id: 'global.labels.value' })} />
      ),
      cell: ({ row }) => <div>{row.getValue("value")}</div>,
      enableSorting: false,
      enableHiding: false,
    },
    {
      accessorKey: "created",
      header: ({ column }) => (
        <DataTableColumnHeader column={column} title={intl.formatMessage({ id: 'global.labels.created' })} />
      ),
      cell: ({ row }) => localDate(row.getValue('created')),
      enableSorting: true,
      enableHiding: false,
    },
    {
      accessorKey: "updated",
      header: ({ column }) => (
        <DataTableColumnHeader column={column} title={intl.formatMessage({ id: 'global.labels.updated' })} />
      ),
      cell: ({ row }) => localDate(row.getValue('updated')),
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
      cell: function Cell({ row }) {
        return (<>
          <DropdownMenu>
            <DropdownMenuTrigger asChild>
              <Button variant="ghost" className="h-8 w-8 p-0">
                <MoreHorizontal className="h-4 w-4" />
              </Button>
            </DropdownMenuTrigger>
            <DropdownMenuContent align="end">
              <DropdownMenuLabel>{intl.formatMessage({ id: 'global.labels.actions' })}</DropdownMenuLabel>
              <DropdownMenuItem
                onClick={() => setEditingItem(row.original)}
                disabled={!hasAny([WorkspacePermissions.WORKSPACE_UPDATE_USER_SECRET])}
              >
                {intl.formatMessage({ 'id': 'global.actions.update' })}
              </DropdownMenuItem>
              <DropdownMenuItem
                onClick={() => setEditingValueItem(row.original)}
                disabled={!hasAny([WorkspacePermissions.WORKSPACE_UPDATE_USER_SECRET])}
              >
                {intl.formatMessage({ 'id': 'secrets.updateValue.action' })}
              </DropdownMenuItem>
              <DropdownMenuItem
                className="text-red-600 hover:!text-red-600 hover:!bg-red-100"
                onClick={() => setDeletingItem(row.original)}
                disabled={!hasAny([WorkspacePermissions.WORKSPACE_DELETE_USER_SECRET])}
              >
                {intl.formatMessage({ 'id': 'global.actions.delete' })}
              </DropdownMenuItem>
            </DropdownMenuContent>
          </DropdownMenu>
        </>
        )
      }
    }
  ], [intl, hasAny])

  return (
    <>
      <UserSecretCreateDialog
        workspaceId={workspaceId}
        open={creatingActive}
        onClose={() => setCreatingActive(false)} onSuccess={() => {
          load(workspaceId)
        }} />
      <UserSecretUpdateDialog
        workspaceId={workspaceId}
        item={editingItem}
        open={editingItem}
        onClose={() => setEditingItem(null)} onSuccess={() => {
          load(workspaceId)
        }} />
      <UserSecretValueUpdateDialog
        workspaceId={workspaceId}
        item={editingValueItem}
        open={editingValueItem}
        onClose={() => setEditingValueItem(null)} onSuccess={() => {
          load(workspaceId)
        }} />
      <UserSecretDeleteDialog
        open={deletingItem}
        onClose={() => setDeletingItem(null)}
        item={deletingItem}
        onSuccess={() => {
          load(workspaceId)
        }}
      />

      <DataTable
        className={className}
        data={data}
        columns={columns}
        progress={progress}
        errors={errors}
        toolbar={(table) => <DataTableToolbar table={table}>
          <Button variant="outline" onClick={() => setCreatingActive(true)}>
            <KeyRoundIcon className="mr-2 h-4 w-4" /> {intl.formatMessage({ id: 'secrets.create.action' })}
          </Button>
        </DataTableToolbar>}
      />
    </>
  )
}