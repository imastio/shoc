import { serverActions as index } from './actions'
import { serverActions as auth } from './actions/auth'

const allRpc = {
    ...index,
    ...auth
}

export default allRpc;
