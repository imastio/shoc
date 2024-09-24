import { createCommand } from 'commander';
import authLoginCommand from './login';

const authCommand = createCommand('auth');

authCommand.addCommand(authLoginCommand);

export default authCommand;