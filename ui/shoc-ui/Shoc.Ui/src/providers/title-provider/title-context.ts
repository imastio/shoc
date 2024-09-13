import { createContext } from "react";

const TitleContext = createContext<TitleContextValueType | any>({});

export type TitleContextValueType = {
    title: string,
    setTitle: (title: string) => void,
}

export default TitleContext;