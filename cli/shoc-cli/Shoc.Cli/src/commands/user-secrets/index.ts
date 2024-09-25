import { createCommand } from 'commander';
import userSecretsListCommand from './list';

const userSecretsCommand = createCommand('user-secrets');

userSecretsCommand.addCommand(userSecretsListCommand);

export default userSecretsCommand;