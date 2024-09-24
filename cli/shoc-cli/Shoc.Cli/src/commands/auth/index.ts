import { createCommand } from 'commander';
import authLoginCommand from './login';
import authSessionCommand from './session';
import authLogoutCommand from './logout';

const authCommand = createCommand('auth');

authCommand.addCommand(authLoginCommand);
authCommand.addCommand(authSessionCommand);
authCommand.addCommand(authLogoutCommand);

export default authCommand;