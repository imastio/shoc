import { loadConfig } from "@/services/config";
import { logger } from "@/services/logger";
import { createCommand } from "commander";
import { asyncHandler } from "../common";

const configViewCommand = createCommand('view')

configViewCommand
    .description('View the configuration')
    .action(asyncHandler(async () => {

        const config = await loadConfig();

        if(!config){
            logger.warn('No configuration found. Run "shoc config init" to create one.');
            return;
        }

        logger.just('Available providers');
        (config.providers || []).forEach(item => {
            logger.just(`  - Name: ${item.name}, Url: ${item.url}`)
        });

        logger.break();

        logger.just('Available contexts');
        (config.contexts || []).forEach(item => {
            const isDefault = item.name === config.defaultContext; 
            const message = `  - Name: ${item.name}, Provider: ${item.provider}, Workspace: ${item.workspace} ${isDefault ? '(default)' : ''}`;
            const writer = isDefault ? logger.success : logger.just;
            writer(message);
        })
    }));


export default configViewCommand;
