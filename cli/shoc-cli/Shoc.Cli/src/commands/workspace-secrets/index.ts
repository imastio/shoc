import { createCommand } from 'commander';
import workspaceSecretsListCommand from './list';

const workspaceSecretsCommand = createCommand('workspace-secrets');

workspaceSecretsCommand.addCommand(workspaceSecretsListCommand);

export default workspaceSecretsCommand;