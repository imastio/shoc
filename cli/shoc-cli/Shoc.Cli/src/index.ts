import program from './commands';
import { logger } from './services/logger';
import { startup } from './services/startup';

startup()

try {
    program.parse(process.argv);
}
catch (e) {
    logger.error((e as Error).message || 'Command failed due to an unexpected error')
    process.exit(1)
}

