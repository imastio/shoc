import { Config } from '@/core/types';
import { promises as fs } from 'fs';
import * as path from 'path';
import * as YAML from 'yaml';

const SHOC_FOLDER_NAME = '.shoc';
const CONFIG_FILE_NAME = 'config.yaml';
const CONFIG_PATH = path.join(process.env.HOME || '', SHOC_FOLDER_NAME, CONFIG_FILE_NAME);

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