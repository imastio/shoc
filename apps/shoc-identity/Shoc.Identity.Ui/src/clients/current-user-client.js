import BaseAxiosClient from "./base-axios-client";

/**
 * Thhe current user client
 */
export default class CurrentUserClient extends BaseAxiosClient {
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
      api: "connect/userinfo"
    });

    return this.webClient.get(url, {
      headers: {
      }
    });
  }
}

