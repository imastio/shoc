import BaseAxiosClient from "./base-axios-client";

/**
 * The client for "identity" service
 */
 export default class IdentityClient extends BaseAxiosClient {
    /**
     * Creates new instance of the client
     *
     * @param {*} config The client configuration
     */
    constructor(config) {
      super("connect", config);
    }
  
    /**
     * Sign-up with given input
     * 
     * @returns {object} The sign-up result
     */
    signup(input) {
      
      const url = this.urlify({
        api: "api-auth/sign-up"
      });
  
      return this.webClient.post(url, input, {
        headers: {
        }
      });
    } 

    /**
     * Sign-in with given input
     * 
     * @returns {object} The sign-in result
     */
     signin(input) {
      
      const url = this.urlify({
        api: "api-auth/sign-in"
      });
  
      return this.webClient.post(url, input, {
        headers: {
        }
      });
    }

    /**
     * Requests confirmation code
     * 
     * @returns {object} The confirmation code result
     */
     requestConfirmation(input) {
      
      const url = this.urlify({
        api: "api-auth/request-confirmation-code"
      });
  
      return this.webClient.post(url, input, {
        headers: {
        }
      });
    }

    /**
     * Process account confirmation
     * 
     * @returns {object} The confirmation process result
     */
     processConfirmation(input) {
      
      const url = this.urlify({
        api: "api-auth/confirm-account"
      });
  
      return this.webClient.post(url, input, {
        headers: {
        }
      });
    }
    
    /**
     * Sign-out with given input
     * 
     * @returns {object} The sign-out result
     */
     signout(input) {
      
      const url = this.urlify({
        api: "api-auth/sign-out"
      });
  
      return this.webClient.post(url, input, {
        headers: {
        }
      });
    }
}