import resolveContext, { getWellKnownEndpoints } from "@/services/context-resolver";
import { createCommand } from "commander";
import { asyncHandler, getRootOptions } from "@/commands/common";
import { authorize } from "../../../services/authorize";
import { storeSession } from "@/services/session-service";
import { logger } from "@/services/logger";

const authLoginCommand = createCommand('login')

authLoginCommand
    .description('Authenticate the user in the provider')
    .action(asyncHandler(async (_, cmd) => {

        const rootOptions = getRootOptions(cmd);
        const context = await resolveContext(rootOptions.context, rootOptions.workspace);

        const { idp } = await getWellKnownEndpoints(context.providerUrl);

        const { accessToken, refreshToken } = await authorize({ idp })

        storeSession(context.providerUrl.toString(), { accessToken, refreshToken })
        logger.success(`You have been successfully authenticated.`);
    }));


export default authLoginCommand;
