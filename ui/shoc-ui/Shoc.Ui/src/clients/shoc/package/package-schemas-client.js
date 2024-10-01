import BaseAxiosClient from "@/clients/base-axios-client";

/**
 * The client for "secret" service
 */
export default class PackageSchemasClient extends BaseAxiosClient {
  
  /**
   * Creates new instance of the client
   *
   * @param {*} config The client configuration
   */
  constructor(config) {
    super("shoc-package", config);
  }

  getAll() {
    const url = this.urlify({
      api: `api/schemas`
    });
    
    return this.webClient.get(url);
  }

  get(name) {
    const url = this.urlify({
      api: `api/schemas/${name}`
    });
    
    return this.webClient.get(url);
  }
}