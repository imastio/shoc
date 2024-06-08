import BaseAxiosClient from "./base-axios-client";

/**
 * Thhe session client
 */
export default class SessionClient extends BaseAxiosClient {
  /**
   * Creates new instance of the client
   *
   * @param {*} config The client configuration
   */
  constructor(config) {
    super("shoc-identity", config);
  }

  get() {
    const url = this.urlify({
      api: "api-auth/session"
    });

    return this.webClient.get(url, {
      headers: {
      }
    });
  }
}

