import { createCommand } from 'commander';
import configInitCommand from './init';
import configViewCommand from './view';

const configCommand = createCommand('config');

configCommand.addCommand(configInitCommand);
configCommand.addCommand(configViewCommand);

export default configCommand;