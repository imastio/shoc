import './assets/less/main-theme.less'
import PageRouter from 'page-router';
import { Provider as ReduxProvider } from 'react-redux';
import store from 'redux/store';
import ConnectedConfigProvider from 'providers/connected-config-provider';
import { AuthProvider } from 'auth/auth-provider';
import { userManager } from 'auth';
import Helmet from 'react-helmet';

const App = () => {

  return (
    <>
      <Helmet titleTemplate="%s | Shoc" defer={false}>
        <title>Loading</title>
      </Helmet>

      <AuthProvider userManager={ userManager }>
        <ReduxProvider store={store}>
            <ConnectedConfigProvider>
              <PageRouter />
            </ConnectedConfigProvider>
        </ReduxProvider>
      </AuthProvider>
    </>
  )
};


export default App;
