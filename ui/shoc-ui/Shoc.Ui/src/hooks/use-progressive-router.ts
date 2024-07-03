import { AppRouterInstance, NavigateOptions, PrefetchOptions } from 'next/dist/shared/lib/app-router-context.shared-runtime';
import { useRouter } from 'next/navigation'
import { useCallback, useMemo, useState } from 'react'

export default function useProgressiveRouter(): AppRouterInstance & { progress: boolean } {

  const [progressMap, setProgressMap] = useState<Map<string, boolean>>(new Map());
  const router = useRouter();

  const buildProgress = useCallback((prev: Map<string, boolean>, key: string, value: boolean) => {
    const clone = new Map(prev.entries())
    
    if(value){
      clone.set(key, value);
    }
    else{
      clone.delete(key);
    }

    return clone;
  }, [setProgressMap]);
  
  const push = useCallback((href: string, options?: NavigateOptions) => {
    const key = crypto.randomUUID();
    setProgressMap(prev => buildProgress(prev, key, true));
    router.push(href, options);
    setProgressMap(prev => buildProgress(prev, key, false));
  }, [router])

  const replace = useCallback((href: string, options?: NavigateOptions) => {
    const key = crypto.randomUUID();
    setProgressMap(prev => buildProgress(prev, key, true));
    router.replace(href, options);
    setProgressMap(prev => buildProgress(prev, key, false));
  }, [router])

  const refresh = useCallback(() => {
    const key = crypto.randomUUID();
    setProgressMap(prev => buildProgress(prev, key, true));
    router.refresh();
    setProgressMap(prev => buildProgress(prev, key, false));
  }, [router])

  const back = useCallback(() => {
    const key = crypto.randomUUID();
    setProgressMap(prev => buildProgress(prev, key, true));
    router.back();
    setProgressMap(prev => buildProgress(prev, key, false));
  }, [router])

  const forward = useCallback(() => {
    const key = crypto.randomUUID();
    setProgressMap(prev => buildProgress(prev, key, true));
    router.forward();
    setProgressMap(prev => buildProgress(prev, key, false));
  }, [router])

  const prefetch = useCallback((href: string, options?: PrefetchOptions) => {
    const key = crypto.randomUUID();
    setProgressMap(prev => buildProgress(prev, key, true));
    router.prefetch(href, options);
    setProgressMap(prev => buildProgress(prev, key, false));
  }, [router])


  const progress = useMemo(() => progressMap.size > 0, [progressMap])

  const value: AppRouterInstance & { progress: boolean } = useMemo(() => ({
    push,
    replace,
    prefetch,
    back,
    forward,
    refresh,
    progress
  }), [push, replace, prefetch, back, forward, refresh, progress]);


  return value
}
