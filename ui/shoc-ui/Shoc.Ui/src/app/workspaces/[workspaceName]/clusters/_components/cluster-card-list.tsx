"use client"

import { useMemo, useState } from "react";
import { useIntl } from "react-intl";
import ClusterCard from "./cluster-card";
import { Button } from "@/components/ui/button";
import { ChevronLeft, ChevronRight } from "lucide-react";
import ClusterSkeletonCard from "./cluster-skeleton-card";

const DEFAULT_PAGE_SIZE = 6;
const SKELETON_PAGE_SIZE = 6;

export default function ClusterCardList({ workspaceId, workspaceName, items, progress }: { workspaceId: string, workspaceName: string, items: any[], progress: boolean }) {

    const intl = useIntl();
    const [page, setPage] = useState<number>(0);

    const current = useMemo(() => {
        return items.slice(page * DEFAULT_PAGE_SIZE, Math.max((page + 1) * DEFAULT_PAGE_SIZE, items.length - 1))
    }, [page, items])

    return <>
        <div className="flex flex-wrap gap-4 h-min w-full py-4">
            {!progress && current.map((item: any) => <ClusterCard key={item.id} workspaceName={workspaceName} className="w-full lg:w-[calc(50%-1rem)] xl:w-[calc(33%-1rem)] 2xl:w-[calc(25%-1rem)]" cluster={item} />)}
            {progress && Array.from(Array(SKELETON_PAGE_SIZE).keys()).map(idx => <ClusterSkeletonCard className="w-full lg:w-[calc(50%-1rem)] xl:w-[calc(33%-1rem)] 2xl:w-[calc(25%-1rem)]" key={idx} />)}
        </div>
        {items.length > DEFAULT_PAGE_SIZE && <div className="flex mx-auto space-x-2">
            <Button variant="outline" disabled={page === 0} onClick={() => setPage(prev => prev - 1)}>
                <ChevronLeft className="mr-2 w-4 h-4" />
                {intl.formatMessage({ id: 'global.navigation.prev' })}
            </Button>
            <Button variant="outline" disabled={page + 1 > items.length / DEFAULT_PAGE_SIZE} onClick={() => setPage(prev => prev + 1)}>
                {intl.formatMessage({ id: 'global.navigation.next' })}
                <ChevronRight className="ml-2 w-4 h-4" />
            </Button>
        </div>}
    </>
}