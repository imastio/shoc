import { useCallback } from "react";
import { useLocation, useSearchParams } from "react-router-dom";

export default function useNavigateSearch(){
    const [searchParams, setSearchParams] = useSearchParams();

    const navigateSearch = useCallback((search, { replace } = { replace: false }) => {
        const nextParams = new URLSearchParams(searchParams);
        
        Object.entries(search).forEach((entry) => {
            nextParams.set(entry[0], entry[1])
        })

        setSearchParams(nextParams, { replace });
    }, [searchParams]);

    return navigateSearch;

}