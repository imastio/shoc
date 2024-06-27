"use client"

import { Badge } from "@/components/ui/badge";
import { Button } from "@/components/ui/button";
import { Card, CardDescription, CardFooter, CardHeader, CardTitle } from "@/components/ui/card";
import { ArrowRightIcon } from "@radix-ui/react-icons";
import { useIntl } from "react-intl";
import { workspaceRolesMap, workspaceTypesMap } from "./well-known";
import Link from "next/link";
import UsersIcon from "@/components/icons/users-icon";
import OrganizationIcon from "@/components/icons/organization-icon";
import KeyIcon from "@/components/icons/key-icon";

export default function WorkspaceCard({ workspace }: any) {
    const intl = useIntl();
    return <Card className="w-full">
        <div className="flex">
            <div className="flex-1">
                <CardHeader>
                    <Link href={`/workspaces/${workspace.name}`}><CardTitle className="truncate leading-normal hover:underline">{workspace.name}</CardTitle></Link>
                    <CardDescription className="text-balance">{workspace.description}</CardDescription>
                </CardHeader>

                <CardFooter>
                    <Badge variant="secondary">
                        {workspace.type === 'individual' && <UsersIcon className="w-4 h-4 mr-1" />}
                        {workspace.type === 'organization' && <OrganizationIcon className="w-4 h-4 mr-1" />}
                        {intl.formatMessage({ id: 'workspaces.labels.type' })}: {intl.formatMessage({ id: workspaceTypesMap[workspace.type] })}
                    </Badge>
                    <Badge variant="secondary" className="ml-2">
                        <KeyIcon className="w-4 h-4 mr-1" />
                        {intl.formatMessage({ id: 'workspaces.labels.role' })}: {intl.formatMessage({ id: workspaceRolesMap[workspace.role] })}
                    </Badge>
                </CardFooter>
            </div>
            <div className="flex p-6">
                <div className="flex m-auto p-0">
                    <Link href={`/workspaces/${workspace.name}`}><Button variant="default">{intl.formatMessage({ id: 'global.actions.select' })} <ArrowRightIcon className="w-4 h-4 ml-2" /></Button></Link>
                </div>
            </div>
        </div>

    </Card>

}