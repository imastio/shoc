"use client"

import { useCallback, useEffect, useState } from "react"
import WorkspacesHeader from "./workspaces-header";
import WorkspaceAddDialogButton from "./workspace-add-dialog-button";
import NoWorkspace from "./no-workspace";
import WorkspaceCard from "./workspace-card";
import ErrorScreen from "@/components/error/error-screen";
import { rpc } from "@/server-actions/rpc";
import WorkspaceSkeletonCard from "./workspace-skeleton-card";
import WorkspaceCardList from "./workspace-card-list";

export default function WorkspacesClientPage() {

    const [progress, setProgress] = useState(true);
    const [items, setItems] = useState<any[]>([]);
    const [errors, setErrors] = useState<any[]>([]);

    const load = useCallback(async () => {

        setProgress(true);

        const { data, errors } = await rpc('workspace/user-workspaces/getAll', {})

        if (errors) {
            setErrors(errors);
            setItems([]);
        } else {
            setErrors([]);
            setItems(data)
        }

        setProgress(false);

    }, []);

    useEffect(() => {
        load();
    }, [load])

    if (errors && errors.length > 0) {
        return <ErrorScreen errors={errors} />
    }

    return <div className="flex mx-auto w-full lg:w-3/5 flex-col gap-4 p-4 lg:gap-6 lg:p-6">
        <WorkspacesHeader actions={items.length > 0 ? [<WorkspaceAddDialogButton key="add-workspace" disabled={progress} onSuccess={() => load()} />] : []} />
        <WorkspaceCardList items={items} progress={progress} />            
    </div>
}