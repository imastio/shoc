import PageRouter from 'page-router';
import { Provider as ReduxProvider } from 'react-redux';
import store from 'redux/store';
import ConnectedConfigProvider from 'providers/connected-config-provider';
import { oidcContextConfig } from 'auth';
import { AuthProvider } from 'react-oidc-context';
import Helmet from 'react-helmet';
import { BrowserRouter } from 'react-router-dom';
import ThemeProvider from 'theme/theme-provider';
import { ApiAuthenticationProvider } from 'api-authentication/api-authentication-provider';

const App = () => {

  return (
    <>
      <Helmet titleTemplate="%s | Shoc" defer={false}>
        <title>Loading</title>
      </Helmet>

      <AuthProvider {...oidcContextConfig} >
        <ApiAuthenticationProvider>
          <ReduxProvider store={store}>
            <ConnectedConfigProvider>
              <BrowserRouter>
                <ThemeProvider>
                  <PageRouter />
                </ThemeProvider>
              </BrowserRouter>
            </ConnectedConfigProvider>
          </ReduxProvider>
        </ApiAuthenticationProvider>
      </AuthProvider>
    </>
  )
};


export default App;
