import BaseAxiosClient from "@/clients/base-axios-client";

/**
 * The client for "identity" service
 */
export default class UsersClient extends BaseAxiosClient {
  /**
   * Creates new instance of the client
   *
   * @param {*} config The client configuration
   */
  constructor(config) {
    super("shoc-identity", config);
  }

  /**
   * Get all users
   * 
   * @returns {object} The users
   */
  getAll(token, filter, page, size) {

    const url = this.urlify({
      api: "api/users",
      query: { ...filter, page, size }
    });

    return this.webClient.get(url, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

  /**
   * Get all user referential values
   * 
   * @returns {object} The users
   */
  getAllReferentialValues(token, filter, page, size) {

    const url = this.urlify({
      api: "api/users/referential-values",
      query: { ...filter, page, size }
    });

    return this.webClient.get(url, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

  /**
   * Get the user by id
   * 
   * @returns {object} The user
   */
  getById(token, id) {

    const url = this.urlify({
      api: `api/users/${id}`,
    });

    return this.webClient.get(url, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

  /**
   * Get the user profile by id
   * 
   * @returns {object} The user profile
   */
  getProfileById(token, id) {

    const url = this.urlify({
      api: `api/users/${id}/profile`,
    });

    return this.webClient.get(url, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }


  
  /**
   * Get the user by id
   * 
   * @returns {object} The user
   */
  getPictureById(token, id) {

    const url = this.urlify({
      api: `api/users/${id}/picture`,
    });

    return this.webClient.get(url, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

  /**
   * Get the user's sign-in history
   * 
   * @returns {object} The user's sign-in history
   */
  getSigninHistoryById(token, userId, page, size) {

    const url = this.urlify({
      api: `api/users/${userId}/sign-in-history`,
      query: { page, size }
    });

    return this.webClient.get(url, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

  /**
   * Get the user's groups
   * 
   * @returns {object} The user's group references
   */
  getGroupsById(token, userId) {

    const url = this.urlify({
      api: `api/users/${userId}/groups`,
    });

    return this.webClient.get(url, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

   /**
   * Get the user's groups
   * 
   * @returns {object} The user's group references
   */
   getRolesById(token, userId) {

    const url = this.urlify({
      api: `api/users/${userId}/roles`,
    });

    return this.webClient.get(url, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

  /**
   * Create a user
   * 
   * @returns {object} The user
   */
   create(token, input) {

    const url = this.urlify({
      api: `api/users`,
    });

    return this.webClient.post(url, input, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

  /**
   * Updates the user by id
   * 
   * @returns {object} The user profile
   */
  updateById(token, id, input) {

    const url = this.urlify({
      api: `api/users/${id}`,
    });

    return this.webClient.put(url, input, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

  /**
   * Updates the user profile
   * 
   * @returns {object} The user
   */
  updateProfileById(token, id, input) {

    const url = this.urlify({
      api: `api/users/${id}/profile`,
    });

    return this.webClient.put(url, input, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

  /**
   * Change the user picture by id
   * 
   * @returns {object} The user
   */
  updatePictureById(token, id, input) {

    const url = this.urlify({
      api: `api/users/${id}/picture`,
    });

    return this.webClient.put(url, input, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }
  
  /**
   * Change the user password
   * 
   * @returns {object} The user
   */
  updatePasswordById(token, id, input) {

    const url = this.urlify({
      api: `api/users/${id}/password`,
    });

    return this.webClient.put(url, input, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

  /**
   * Change the user type
   * 
   * @returns {object} The user
   */
  updateTypeById(token, id, input) {

    const url = this.urlify({
      api: `api/users/${id}/type`,
    });

    return this.webClient.put(url, input, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

  /**
   * Change the user state
   * 
   * @returns {object} The user update result
   */
  updateUserStateById(token, id, input) {

    const url = this.urlify({
      api: `api/users/${id}/state`,
    });

    return this.webClient.put(url, input, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

  /**
    * Change the user lockout
    * 
    * @returns {object} The user
    */
  updateLockoutById(token, id, input) {

    const url = this.urlify({
      api: `api/users/${id}/lockout`,
    });

    return this.webClient.put(url, input, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }

  /**
   * Deletes the user by id
   * 
   * @returns {object} The deleted user
   */
   deleteById(token, id) {

    const url = this.urlify({
      api: `api/users/${id}`,
    });

    return this.webClient.delete(url, {
      headers: {
        ...this.authBearer(token)
      }
    });
  }
}