import axios, { Axios } from "axios";
import BaseClient from "./base-client";
import ApiConfig from "./api-config";

/**
 * The base axios-based client module
 */
export default class BaseAxiosClient extends BaseClient {

    /**
     * The web client instance
     */
    webClient: Axios;

    /**
     * Creates new instance of axios client
     *
     * @param {string} service The service to connect
     * @param {ApiConfig} apiConfig The client configuration
     */
    constructor(service: string, apiConfig: ApiConfig) {
        super(service, apiConfig);

        // create instance of default web client
        this.webClient = axios.create({
            timeout: apiConfig.timeout,
            headers: {
                ...this.contentType(),
                ...this.allowCORS(),
                client: apiConfig.client,
            },
        });
    }

    /**
     * Check if error is an error with number XXX
     *
     * @param {object} object The error object
     * @param {number} ernum The error number
     */
    isXXX(object: any, ernum: number) {
        return object && object.response && object.response.status === ernum;
    }

    /**
     * Check if error is an error with number 404
     *
     * @param {object} object The error object
     */
    is404(object: any) {
        return this.isXXX(object, 404);
    }

    /**
     * Check if error is an error with number 401
     *
     * @param {object} object The error object
     */
    is401(object: any) {
        return this.isXXX(object, 401);
    }
}