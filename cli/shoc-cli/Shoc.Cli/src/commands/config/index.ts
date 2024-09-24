import { Command } from 'commander';
import promptSync from 'prompt-sync';
import { Config, loadConfig, saveConfig } from '@/services/config';

const configCommand = new Command('config');

configCommand
    .command('view')
    .description('View the configuration')
    .action(async (options) => { // Access global options here
        const config = await loadConfig();
        if (config) {
            console.log(JSON.stringify(config, null, 2));
        } else {
            console.log('No configuration found. Run "shoc config init" to create one.');
        }

        if (options.context) {
            console.log(`Global context: ${options.context}`);
        }
        if (options.workspace) {
            console.log(`Global workspace: ${options.workspace}`);
        }
    });

configCommand
    .command('init')
    .description('Initialize the configuration')
    .action(async (options) => {

        const config: Config = {
            providers: [{ name: 'global', url: 'https://shoc.dev' }],
            contexts: [],
            defaultContext: 'default',
        };

        const workspaceName = options.workspace ? options.workspace : await promptUserForWorkspaceName();

        config.contexts.push({
            name: 'default',
            provider: 'global',
            workspace: workspaceName,
        });

        await saveConfig(config);
        console.log('Configuration initialized.');
    });

async function promptUserForWorkspaceName(): Promise<string> {
    const input = promptSync();
    return input('Enter workspace name: ');
}

export default configCommand;