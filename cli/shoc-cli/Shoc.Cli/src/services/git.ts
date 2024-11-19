import git from 'isomorphic-git'
import fs from 'fs'
import gitUrlParse from 'git-url-parse';

interface GitDetails {
    remote: string,
    url: string,
    branch: string | void,
    source: string,
    owner: string,
    name: string,
    repo: string
}

export async function getGitDetails(dir: string): Promise<GitDetails | null> {
    try {
        return await getGitDetailsImpl(dir);
    }
    catch(e){
        return null;
    }
}

async function getGitDetailsImpl(dir: string): Promise<GitDetails | null> {

    const gitFolder = await git.findRoot({ fs, filepath: dir });

    const remotes = await git.listRemotes({ fs, dir: gitFolder });

    if(remotes.length === 0){
        return null;
    }

    let remote = remotes.filter(r => r.remote === 'origin')[0];

    if(!remote){
        remote = remotes[0];
    }

    const branch = await git.currentBranch({fs, dir: gitFolder, fullname: false})

    const parsed = gitUrlParse(remote.url)

    return {
        remote: remote.remote,
        url: remote.url,
        branch: branch,
        source: parsed.source,
        owner: parsed.owner,
        name: parsed.name,
        repo: parsed.full_name
    };
}