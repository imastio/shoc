import { useCallback, useMemo } from "react";
import routeAccess from "./route-access";
import useAccess from "@/providers/access-provider/use-access";
import { useSession } from "next-auth/react";

const bypassTypes = ['root', 'admin'];

export default function useRouteAccess(){

    const { hasAny, hasAll } = useAccess();
    const session = useSession();
    const type = (session.data?.user as any).userType;
    
    const isAllowed = useCallback((resolvedPath: string | null) => {

        if(!resolvedPath){
            return false;
        }

        const requirements = routeAccess[resolvedPath];

        if(!requirements){
            return true;
        }

        if(bypassTypes.some(t => t === type)){
            return true;
        }

        if(requirements.oneOf && requirements.oneOf.length > 0){
            if(!hasAny(requirements.oneOf)){
                return false;
            }
        }

        if(requirements.allOf && requirements.allOf.length > 0){
            if(!hasAll(requirements.allOf)){
                return false;
            }
        }

        return true;
    }, [hasAll, hasAny, type])

    const value = useMemo(() => ({
        isAllowed
    }), [isAllowed]);

    return value;
}