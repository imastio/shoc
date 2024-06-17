import BaseAxiosClient from "@/clients/base-axios-client";

/**
 * The client for "settings" service
 */
export default class MailingProfilesClient extends BaseAxiosClient {
  /**
   * Creates new instance of the client
   *
   * @param {*} config The client configuration
   */
  constructor(config) {
    super("shoc-settings", config);
  }

  /**
   * Gets mailing profiles
   * 
   */
  getAll(token){

    const url = this.urlify({
      api: `api/mailing-profiles`
    });

    return this.webClient.get(url, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

  
  /**
   * Gets mailing profile by id
   * 
   */
   getById(token, id){

    const url = this.urlify({
      api: `api/mailing-profiles/${id}`
    });

    return this.webClient.get(url, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

  /**
   * Create a mailing profile
   * 
   */
   create(token, input){

    const url = this.urlify({
      api: `api/mailing-profiles`
    });

    return this.webClient.post(url, input, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }


  /**
  * Updates the mailing profie password by id based on the given input
  * 
  * @returns {object} The profile
  */
   updatePasswordById(token, id, input) {

    const url = this.urlify({
      api: `api/mailing-profiles/${id}/password`
    });

    return this.webClient.put(url, input, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }
  
  /**
  * Updates the mailing profile API Secret by id based on the given input
  * 
  * @returns {object} The mailing profile
  */
   updateApiSecretById(token, id, input) {

    const url = this.urlify({
      api: `api/mailing-profiles/${id}/api-secret`
    });

    return this.webClient.put(url, input, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

  /**
   * Gets mailing profile by id
   * 
   */
   deleteById(token, id){

    const url = this.urlify({
      api: `api/mailing-profiles/${id}`
    });

    return this.webClient.delete(url, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }
  
}