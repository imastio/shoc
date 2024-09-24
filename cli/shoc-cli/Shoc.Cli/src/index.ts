import { Command } from 'commander';
import { version } from '../package.json'; // Adjust the path based on your structure
import configCommand from './commands/config';

const program = new Command();

program
  .name('shoc')
  .description('CLI tool for SHOC')
  .version(version)
  .option('--context <context>', 'Override the default context')
  .option('--workspace <workspace>', 'Override the default workspace');

// Add commands
program.addCommand(configCommand);

// Parse the command-line arguments
program.parse(process.argv);

// Access global options
const options = program.opts();
if (options.context) {
  console.log(`Using context: ${options.context}`);
}
if (options.workspace) {
  console.log(`Using workspace: ${options.workspace}`);
}