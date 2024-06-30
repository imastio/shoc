import { CacheStorage } from "@/addons/cache";
import { getRefreshRequestLifetime, getTokenCacheLifetime } from "./config";

export const tokenRefreshRequests = new CacheStorage({
    prefix: 'refresh-requests',
    defaultTtl: getRefreshRequestLifetime(),
    checkEvery: 60
});

export const sessionAccessTokenCache = new CacheStorage({
    prefix: 'session-access-tokens.',
    defaultTtl: getTokenCacheLifetime(),
    checkEvery: 60
})

