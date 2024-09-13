import ErrorDefinitions from "@/addons/error-handling/error-definitions";
import { defineServerAction } from "../define";
import { ServerActionFunction } from "../types";

export const noServerAction: ServerActionFunction = defineServerAction(() => {
    throw ErrorDefinitions.unknown();
})

export const serverActions = {
    'index/noServerAction': noServerAction
}