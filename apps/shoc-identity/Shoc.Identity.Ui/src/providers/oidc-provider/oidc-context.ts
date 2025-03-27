import { createContext } from "react";

const OidcContext = createContext({ progress: false, loginHint: '' });

export default OidcContext;