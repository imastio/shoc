import { useContext } from "react";
import { TitleContextValueType } from "./title-context";
import TitleContext from "./title-context";

export default function useTitle(){
    return useContext<TitleContextValueType>(TitleContext);
}