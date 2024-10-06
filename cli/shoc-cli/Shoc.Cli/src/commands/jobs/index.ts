import { createCommand } from 'commander';
import jobsBuildCommand from './build';

const jobsCommand = createCommand('jobs');

jobsCommand.addCommand(jobsBuildCommand);

export default jobsCommand;