import { version } from '../package.json';
import configCommand from './commands/config';
import authCommand from './commands/auth';
import { createCommand } from 'commander';

process.env.NODE_TLS_REJECT_UNAUTHORIZED = "0";

const program = createCommand();

program
  .name('shoc')
  .description('A command-line interface for managing your job on Shoc Platform')
  .version(version, '-v, --version')
  .option('--context <context>', 'Use the mentioned context')
  .option('--workspace <workspace>', 'Use the mentioned workspace')
  .enablePositionalOptions();

program.addCommand(configCommand);
program.addCommand(authCommand);

program.parse(process.argv);

