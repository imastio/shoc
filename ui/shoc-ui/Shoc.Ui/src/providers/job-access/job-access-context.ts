import { createContext } from "react";

const JobAccessContext = createContext<JobAccessContextValueType | any>({});

export type JobAccessContextValueType = {
    progress: boolean,
    accesses: Set<string>,
    hasAny: (requirements: string[]) => boolean,
    hasAll: (requirements: string[]) => boolean
}

export default JobAccessContext;