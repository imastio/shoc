import resolveContext from "@/services/context-resolver";
import { createCommand } from "commander";
import { asyncHandler, getRootOptions } from "@/commands/common";
import { logger } from "@/services/logger";
import { anonymousClientGuard } from "@/services/client-guard";
import { shocClient } from "@/clients/shoc";
import TemplatesClient from "@/clients/shoc/package/templates-client";

const templatesListCommand = createCommand('list')

templatesListCommand
    .description('List available templates')
    .action(asyncHandler(async (_, cmd) => {
        
        const context = await resolveContext(getRootOptions(cmd));

        const result: any[] = await anonymousClientGuard(context, (ctx) => shocClient(ctx.apiRoot, TemplatesClient).getAll());
    
        logger.just('Available templates:');
        result.forEach(item => {
            logger.just(`  - ${item.name}:[${item.variants.join(', ')}]`)
        });
    }));

export default templatesListCommand;
