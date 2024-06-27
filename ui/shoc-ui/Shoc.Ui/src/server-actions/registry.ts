import { serverActions as index } from './actions'
import { serverActions as auth } from './actions/auth'
import { serverActions as userWorkspaces } from './actions/workspace/user-workspaces'

const allRpc = {
    ...index,
    ...auth,
    ...userWorkspaces
}

export default allRpc;
