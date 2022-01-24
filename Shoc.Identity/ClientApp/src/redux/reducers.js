import { combineReducers } from 'redux'
import { reducer as users } from './users/slice'


const rootReducer = combineReducers({
  users
})

export default rootReducer