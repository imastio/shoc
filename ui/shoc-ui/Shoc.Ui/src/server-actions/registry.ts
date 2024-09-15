import { serverActions as index } from './actions'
import { serverActions as auth } from './actions/auth'
import { serverActions as userWorkspaces } from './actions/workspace/user-workspaces'
import { serverActions as userWorkspaceMembers } from './actions/workspace/user-workspace-members'
import { serverActions as userWorkspaceInvitations } from './actions/workspace/user-workspace-invitations'
import { serverActions as userInvitations } from './actions/workspace/user-invitations'
import { serverActions as workspaceClusters } from './actions/cluster/workspace-clusters'

const allRpc = {
    ...index,
    ...auth,
    ...userWorkspaces,
    ...userWorkspaceMembers,
    ...userWorkspaceInvitations,
    ...userInvitations,
    ...workspaceClusters
}

export default allRpc;
