import { serverActions as index } from './actions'
import { serverActions as auth } from './actions/auth'
import { serverActions as identityCurrentUser } from './actions/identity/current-user'

const allRpc = {
    ...index,
    ...auth,
    ...identityCurrentUser
}

export default allRpc;
