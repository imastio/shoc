import BaseAxiosClient from "@/clients/base-axios-client";

/**
 * The client for "package" service
 */
export default class TemplatesClient extends BaseAxiosClient {

  constructor(config) {
    super("shoc-package", config);
  }

  getAll() {
    const url = this.urlify({
      api: `api/templates`
    });
    
    return this.webClient.get(url);
  }

  getBuildSpecInstanceByName(name, variant) {
    const url = this.urlify({
      api: `api/templates/${name}/variants/${variant}/build-spec/instance`
    });
    
    return this.webClient.get(url);
  }
}