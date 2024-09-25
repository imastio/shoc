import ApiConfig, { shocApiConfig } from "../api-config";
import BaseAxiosClient from "../base-axios-client";

type ClientConstructor<TClient> = new (config: ApiConfig) => TClient; 

export function shocClient<TClient extends BaseAxiosClient>(apiRoot: string, client: ClientConstructor<TClient>){
    return new client(shocApiConfig(apiRoot));
}