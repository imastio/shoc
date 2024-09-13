import { createContext } from "react";

const AccessContext = createContext<AccessContextValueType | any>({});

export type AccessContextValueType = {
    progress: boolean,
    accesses: Set<string>,
    hasAny: (requirements: string[]) => boolean,
    hasAll: (requirements: string[]) => boolean
}

export default AccessContext;