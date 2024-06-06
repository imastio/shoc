import { RouterProvider } from 'react-router-dom';
import router from './router';
import AuthProvider from './providers/session-provider/session-provider';
import { Helmet, HelmetProvider } from 'react-helmet-async';

export default function App() {

    return <>
        <HelmetProvider>
            <Helmet titleTemplate='%s | Shoc Identity'></Helmet>
            <AuthProvider>
                <RouterProvider router={router} />
            </AuthProvider>
        </HelmetProvider>
    </>
};