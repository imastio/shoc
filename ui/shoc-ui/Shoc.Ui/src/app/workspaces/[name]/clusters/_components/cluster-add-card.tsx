"use client"

import { Button } from "@/components/ui/button";
import { useIntl } from "react-intl";

export default function ClusterAddCard(){
    const intl = useIntl();

    return <div className="flex flex-1 items-center justify-center rounded-lg border border-dashed shadow-sm min-h-[160px] max-w-[250px] w-full">
    <div className="flex flex-col items-center gap-1 text-center">
        <Button variant="outline" className="mt-5">Add new cluster</Button>
    </div>
</div>
}