"use client"

import { useMemo, useState } from "react";
import WorkspaceSkeletonCard from "./workspace-skeleton-card";
import WorkspaceCard from "./workspace-card";
import NoWorkspace from "./no-workspace";
import { Button } from "@/components/ui/button";
import { ChevronLeft, ChevronRight } from "lucide-react";
import { useIntl } from "react-intl";

const DEFAULT_PAGE_SIZE = 4;

export default function WorkspaceCardList({ progress, items = [] }: { progress?: boolean, items?: any[] }) {

  const [page, setPage] = useState<number>(0);
  const intl = useIntl();

  const current = useMemo(() => {
    return items.slice(page * DEFAULT_PAGE_SIZE, Math.min((page + 1) * DEFAULT_PAGE_SIZE, items.length))
  }, [page, items])

  if (progress) {
    return Array.from(Array(DEFAULT_PAGE_SIZE).keys()).map(idx => <WorkspaceSkeletonCard key={idx} />)
  }
  if (items.length === 0) {
    return <NoWorkspace />
  }

  return <>
    {current.map((item: any) => <WorkspaceCard key={item.id} workspace={item} />)}
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