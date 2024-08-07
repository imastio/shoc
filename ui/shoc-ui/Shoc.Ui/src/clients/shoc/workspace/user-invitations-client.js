import BaseAxiosClient from "@/clients/base-axios-client";

/**
 * The client for "workspace" service
 */
export default class UserInvitationsClient extends BaseAxiosClient {
  /**
   * Creates new instance of the client
   *
   * @param {*} config The client configuration
   */
  constructor(config) {
    super("shoc-workspace", config);
  }

  getAll(token) {
    const url = this.urlify({
      api: `api/user-invitations`
    });

    return this.webClient.get(url, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

  countAll(token) {
    const url = this.urlify({
      api: `api/user-invitations/count`
    });

    return this.webClient.get(url, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

  accept(token, input) {

    const url = this.urlify({
      api: `api/user-invitations/accept`
    });

    return this.webClient.put(url, input, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

  decline(token, input) {

    const url = this.urlify({
      api: `api/user-invitations/decline`
    });

    return this.webClient.put(url, input, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }
}