import { createCommand } from 'commander';
import templatesListCommand from './list';
import templatesInitCommand from './init';

const templatesCommand = createCommand('templates');

templatesCommand.addCommand(templatesListCommand);
templatesCommand.addCommand(templatesInitCommand);

export default templatesCommand;