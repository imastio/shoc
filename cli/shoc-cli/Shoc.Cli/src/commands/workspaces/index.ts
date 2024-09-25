import { createCommand } from 'commander';
import workspacesListCommand from './list';

const workspacesCommand = createCommand('workspaces');

workspacesCommand.addCommand(workspacesListCommand);

export default workspacesCommand;