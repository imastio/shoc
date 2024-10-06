import { version } from '../../package.json';
import configCommand from './config';
import authCommand from './auth';
import { createCommand } from 'commander';
import workspacesCommand from './workspaces';
import clustersCommand from './clusters';
import userSecretsCommand from './user-secrets';
import workspaceSecretsCommand from './workspace-secrets';
import templatesCommand from './templates';
import jobsCommand from './jobs';

const program = createCommand();

program
  .name('shoc')
  .description('A command-line interface for managing your job on Shoc Platform')
  .version(version, '-v, --version')
  .option('-c, --context <context>', 'Use the mentioned context')
  .option('-w, --workspace <workspace>', 'Use the mentioned workspace')
  .option('-d, --dir <dir>', 'Use the given working directory')
  .enablePositionalOptions();

program.addCommand(configCommand);
program.addCommand(authCommand);
program.addCommand(jobsCommand)
program.addCommand(templatesCommand);
program.addCommand(workspacesCommand);
program.addCommand(clustersCommand);
program.addCommand(userSecretsCommand);
program.addCommand(workspaceSecretsCommand);


export default program;

