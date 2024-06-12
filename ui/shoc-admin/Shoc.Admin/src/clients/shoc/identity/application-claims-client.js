import BaseAxiosClient from "@/clients/base-axios-client";

/**
 * The client for "identity" applications service
 */
export default class ApplicationClaimsClient extends BaseAxiosClient {
  /**
   * Creates new instance of the client
   *
   * @param {*} config The client configuration
   */
  constructor(config) {
    super("shoc-identity", config);
  }

  getAll(token, applicationId) {

    const url = this.urlify({
      api: `api/applications/${applicationId}/claims`
    });

    return this.webClient.get(url, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

  getById(token, applicationId, id) {

    const url = this.urlify({
      api: `api/applications/${applicationId}/claims/${id}`
    });

    return this.webClient.get(url, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

  create(token, applicationId, input) {

    const url = this.urlify({
      api: `api/applications/${applicationId}/claims`,
    });

    return this.webClient.post(url, input, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

  updateById(token, applicationId, id, input) {

    const url = this.urlify({
      api: `api/applications/${applicationId}/claims/${id}`
    });

    return this.webClient.put(url, input, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

  deleteById(token, applicationId, id) {

    const url = this.urlify({
      api: `api/applications/${applicationId}/claims/${id}`,
    });

    return this.webClient.delete(url, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }
}

