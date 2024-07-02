"use client"

import * as React from "react"
import { WorkspaceMember } from "./types"
import DataTable from "@/components/data-table"
import useWorkspaceMembersColumns from "./workspace-members-columns"

export default function WorkspaceMembersTable({ members, className }: { members: WorkspaceMember[], className?: string }) {

  const columns = useWorkspaceMembersColumns()

  return (
    <DataTable
      className={className}
      data={members}
      columns={columns}
    >
    </DataTable>
  )
}