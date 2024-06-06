import { useContext } from "react";
import SessionContext from "./session-context";

export default function useSession(){
    return useContext(SessionContext);
}