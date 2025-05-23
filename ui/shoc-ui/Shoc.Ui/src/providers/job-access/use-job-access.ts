import { useContext } from "react";
import JobAccessContext, { JobAccessContextValueType } from "./job-access-context";


export default function useJobAccess(){
    return useContext<JobAccessContextValueType>(JobAccessContext);
}