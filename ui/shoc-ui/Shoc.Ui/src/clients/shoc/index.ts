import ApiConfig, { selfApiConfig, shocApiConfig } from "../api-config";
import BaseAxiosClient from "../base-axios-client";

type ClientConstructor<TClient> = new (config: ApiConfig) => TClient; 

export function selfClient<TClient extends BaseAxiosClient>(client: ClientConstructor<TClient>){
    return new client(selfApiConfig());
}

export function shocClient<TClient extends BaseAxiosClient>(client: ClientConstructor<TClient>){
    return new client(shocApiConfig());
}