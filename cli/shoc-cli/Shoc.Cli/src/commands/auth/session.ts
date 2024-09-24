import resolveContext from "@/services/context-resolver";
import { createCommand } from "commander";
import { asyncHandler, getRootOptions } from "@/commands/common";
import { checkSession } from "@/services/session-service";
import { logger } from "@/services/logger";
import chalk from "chalk";

const authSessionCommand = createCommand('session')

authSessionCommand
    .description('Show details of the current session')
    .action(asyncHandler(async (_, cmd) => {

        const rootOptions = getRootOptions(cmd);
        const context = await resolveContext(rootOptions.context, rootOptions.workspace);

        const session = await checkSession(context.providerUrl.toString());

        if(!session){
            logger.warn('You are not authenticated yet.')
            return;
        }

        const expired = session.expires.getTime() < new Date().getTime()

        logger.just(`Session details as of now (${new Date().toLocaleString()}):`);
        logger.just(`  - Status: ${expired ? chalk.yellow('Expired') : chalk.green('Active')}`);
        logger.just(`  - Valid Until: ${expired ? chalk.yellow(session.expires.toLocaleString()) : chalk.green(session.expires.toLocaleString())}`);
        logger.just(`  - ID: ${session.id}`);
        logger.just(`  - Name: ${session.name}`);
        logger.just(`  - Email: ${session.email}`);
        logger.just(`  - Username: ${session.username}`);
        logger.just(`  - Type: ${session.userType}`);
    }));


export default authSessionCommand;
