import { promises as fs } from 'fs';
import * as path from 'path';
import * as YAML from 'yaml';

const CONFIG_PATH = path.join(process.env.HOME || '', '.shoc', 'config.yaml');

export interface Config {
  providers: { name: string, url: string }[];
  contexts: { name: string; provider: string; workspace: string }[];
  defaultContext: string;
}

export async function loadConfig(): Promise<Config | null> {
  try {
    const file = await fs.readFile(CONFIG_PATH, 'utf8');
    return YAML.parse(file) as Config;
  } catch (error) {
    return null;
  }
}

export async function saveConfig(config: Config): Promise<void> {
  const dir = path.dirname(CONFIG_PATH);
  await fs.mkdir(dir, { recursive: true });
  await fs.writeFile(CONFIG_PATH, YAML.stringify(config), 'utf8');
}