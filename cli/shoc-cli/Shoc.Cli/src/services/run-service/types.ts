import { AuthSession } from "@/core/types"

export interface RunContext {
    workspace: string,
    dir: string,
    buildFile: string,
    scope: string,
    runFile: string
}