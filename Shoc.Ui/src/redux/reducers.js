import { combineReducers } from 'redux'
import { reducer as settings } from './settings/slice'

const rootReducer = combineReducers({
  settings
})

export default rootReducer