import { useContext } from "react";
import JobAccessContext, { JobAccessContextValueType } from "./job-access-context";


export default function useClusterAccess(){
    return useContext<JobAccessContextValueType>(JobAccessContext);
}