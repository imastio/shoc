import { createCommand } from 'commander';
import clustersListCommand from './list';

const clustersCommand = createCommand('clusters');

clustersCommand.addCommand(clustersListCommand);

export default clustersCommand;