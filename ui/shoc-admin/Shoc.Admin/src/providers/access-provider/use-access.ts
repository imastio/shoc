import { useContext } from "react";
import AccessContext from "./access-context";

export default function useAccess(){
    return useContext(AccessContext);
}