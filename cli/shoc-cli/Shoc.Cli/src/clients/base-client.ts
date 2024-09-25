import ApiConfig from "./api-config";

/**
 * The base client module
 */
export default class BaseClient {

  /**
   * The service name
   */
  service: string;

  /**
   * The api configuration
   */
  config: ApiConfig;

  /**
   * Creates new instance of base client
   *
   * @param {string} service The service to connect
   * @param {object} apiConfig The API configuration
   */
  constructor(service: string, apiConfig: ApiConfig) {
    this.service = service;
    this.config = apiConfig;
  }

  /**
   * The auth bearer header
   *
   * @param {string} token The token
   */
  authBearer(token: string) {
    return { Authorization: `Bearer ${token}` };
  }

  /**
   * The auth basic header
   *
   * @param {string} username The username
   * @param {string} password The password
   */
  authBasic(username: string, password: string) {
    return { Authorization: `Basic ${Buffer.from(`${username}:${password}`).toString("base64")}` };
  }

  /**
   * Allow CORS header
   *
   */
  allowCORS() {
    return { "Access-Control-Allow-Origin": "*" };
  }


  /**
   * The content type header
   *
   * @param {string} value The content type value
   */
  contentType(value?: string) {
    // use value if given
    if (value) {
      return { "Content-Type": value };
    }

    // json by default
    return { "Content-Type": "application/json" };
  }

  /**
   * Converts data into form object
   *
   * @param {object} data The data to convert to form
   */
  form(data: {[key: string]: any}): FormData {
    // new form object
    const formData = new FormData();

    // traverse keys and append to form
    Object.keys(data).forEach((key) => formData.append(key, data[key]));

    // return form data
    return formData;
  }

  /**
   * Escape the string for URI
   *
   * @param {string} string The string to URI encode
   */
  esc(string: string): string {
    return encodeURIComponent(string);
  }

  /**
   * Build the param value
   *
   * @param {any} value The value to normalize
   */
  paramValue(value: string | string[]) {

    // in case of array
    if (Array.isArray(value)) {
      return value.map(this.esc).join(",");
    } 
    
    return this.esc(value);
  }

  /**
   *
   * @param {object} params The parameters to pass
   */
  queryParams(params: {[key: string]: string} = {}) {
    return Object.keys(params)
      .map((k: string) => this.mapQueryParam(k, params[k]))
      .join("&");
  }

  /**
   * Map the query parameter
   *
   * @param {string} key The key
   * @param {any} value The value
   */
  mapQueryParam(key: string, value: string) {
    if (value === null || value === undefined || value === "") {
      return `${this.esc(key)}`;
    }

    return `${this.esc(key)}=${this.paramValue(value)}`;
  }

  /**
   * Get the URL for the API endpoint
   *
   * @param {string} api The API endpoint
   */
  getApi(api: string) {

    const root = this.config.root;
    const gateway = this.config.gateway;
    const gatewayEnding = gateway === "" ? "" : "/";

    if (this.config.serviceRouting) {
      return `${root}${gateway}${gatewayEnding}${this.service}/${api}`;
    }

    return `${root}${api}`;
  }

  /**
   * The URL Builder
   *
   * @param {object} address The address details to build URL
   */
  urlify(address: {api: string, query?: {[key: string]: string}}): string {
    // build endpoint
    let url = this.getApi(address.api);

    // if query params are given
    if (address.query) {
      url = `${url}?${this.queryParams(address.query)}`;
    }

    return url;
  }

  /**
   * Check if object is error
   *
   * @param {object} object The object to check
   */
  isError(object: Error) {
    return (
      object &&
      object.stack &&
      object.message &&
      typeof object.stack === "string" &&
      typeof object.message === "string"
    );
  }
}