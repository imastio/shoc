import NodeCache from "node-cache"
import { CacheStorageOptions } from "./types";

export class CacheStorage {
    innerCache: NodeCache;
    prefix: string;
    
    constructor(options: CacheStorageOptions){
        this.innerCache = new NodeCache({
            stdTTL: options.defaultTtl,
            checkperiod: options.checkEvery,
            useClones: false,
            forceString: false,
            errorOnMissing: false,
            deleteOnExpire: true
        });
        this.prefix = options.prefix;
    }

    get(key: string): any{
        return this.innerCache.get(this._buildKey(key));
    }

    async computeIfAbsent<ResultType>(key: string, supplier: () => Promise<ResultType>, options?: any): Promise<ResultType> {
        const existing = this.get(key);
        if(existing === undefined || options?.forceCompute){
            const fresh = await supplier();
            this.set(key, fresh, options?.ttl);
            return fresh;
        }

        return existing;
    }

    has(key: string): boolean{
        return this.innerCache.has(this._buildKey(key));
    }

    set(key: string, value: any, ttl: number): boolean{
        return this.innerCache.set(this._buildKey(key), value, ttl);
    }

    del(key: string): number {
        return this.innerCache.del(this._buildKey(key));
    }

    delAll(keys: string[]): number {
        return this.innerCache.del(keys.map(key => this._buildKey(key)));
    }

    _buildKey(key: string): string {
        return `${this.prefix}.${key}`;
    }
}