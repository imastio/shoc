import ApiConfig, { selfApiConfig } from "../api-config";
import BaseAxiosClient from "../base-axios-client";

type ClientConstructor<TClient> = new (config: ApiConfig) => TClient; 

export function selfClient<TClient extends BaseAxiosClient>(client: ClientConstructor<TClient>){
    return new client(selfApiConfig());
}