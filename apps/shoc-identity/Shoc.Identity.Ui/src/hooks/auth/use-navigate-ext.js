import { useCallback } from "react";
import { useNavigate } from "react-router-dom";

export default function useNavigateExt() {
    
    const navigate = useNavigate();
    const baseurl = document.location.href;

    const navigationFunction = useCallback((to, options) => {
        if(typeof to === 'number'){
            navigate(to);
            return;
        }

        const replace = options?.replace || false;
        const state = options?.state || undefined;

        if(typeof to === 'string'){
            const url = new URL(to, baseurl);
            
            const search = new URLSearchParams(url.searchParams);

            navigate({
                pathname: url.pathname,
                search: search.toString(),
                hash: url.hash
            }, {
                replace,
                state
            })

            return;
        }

        if(typeof to === 'object'){ 
            const search = new URLSearchParams(to.search);
            const searchOverrides = to.searchOverrides || {};
                    
            Object.entries(searchOverrides).forEach(([key, value]) => search.set(key, value));

            navigate({
                pathname: to.pathname,
                search: search.toString(),
                hash: to.hash
            }, {
                replace,
                state
            })

            return;
        }

    }, [navigate, baseurl]);


    return navigationFunction;
};