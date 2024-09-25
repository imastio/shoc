import resolveContext from "@/services/context-resolver";
import { createCommand } from "commander";
import { asyncHandler, getRootOptions } from "@/commands/common";
import { clearSession } from "@/services/session-service";
import { logger } from "@/services/logger";

const authLogoutCommand = createCommand('logout')

authLogoutCommand
    .description('Logout from the provider')
    .action(asyncHandler(async (_, cmd) => {

        const context = await resolveContext(getRootOptions(cmd));

        await clearSession(context.providerUrl.toString());
        logger.success(`You have been logged out successfully!`);

    }));


export default authLogoutCommand;
