import { useContext } from "react";
import AccessContext, { AccessContextValueType } from "./access-context";

export default function useAccess(){
    return useContext<AccessContextValueType>(AccessContext);
}