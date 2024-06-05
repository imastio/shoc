import BaseAxiosClient from "./base-axios-client";

/**
 * The client for auth services
 */
 export default class AuthClient extends BaseAxiosClient {
    /**
     * Creates new instance of the client
     *
     * @param {*} config The client configuration
     */
    constructor(config) {
      super("shoc-identity", config);
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
     * Resolves the sign-in context with given input
     * 
     * @returns {object} The sign-in context result
     */
     signinContext(input) {
      
      const url = this.urlify({
        api: "api-auth/sign-in-context"
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
     * Requests the sign-in link
     * 
     * @returns {object} The OTP request result
     */
     requestOtp(input) {
      
      const url = this.urlify({
        api: "api-auth/otp/request"
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
        api: "api-auth/confirmation/request"
      });
  
      return this.webClient.post(url, input, {
        headers: {
        }
      });
    }

    /**
     * Requests password recovery code
     * 
     * @returns {object} The recovery code result
     */
     requestPasswordRecovery(input) {
      
      const url = this.urlify({
        api: "api-auth/password-recovery/request"
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
        api: "api-auth/confirmation/confirm"
      });
  
      return this.webClient.post(url, input, {
        headers: {
        }
      });
    }

    /**
     * Process password recovery
     * 
     * @returns {object} The recovery process result
     */
     processPasswordRecovery(input) {
      
      const url = this.urlify({
        api: "api-auth/password-recovery/process"
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