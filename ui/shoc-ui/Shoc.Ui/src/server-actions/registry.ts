import { serverActions as index } from './actions'
import { serverActions as auth } from './actions/auth'
import { serverActions as userWorkspaces } from './actions/workspace/user-workspaces'
import { serverActions as userWorkspaceMembers } from './actions/workspace/user-workspace-members'

const allRpc = {
    ...index,
    ...auth,
    ...userWorkspaces,
    ...userWorkspaceMembers
}

export default allRpc;
