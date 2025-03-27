import { RouterProvider } from 'react-router-dom';
import router from './router';
import SessionProvider from './providers/session-provider/session-provider';
import { Helmet, HelmetProvider } from 'react-helmet-async';

export default function App() {

    return <>
        <HelmetProvider>
            <Helmet titleTemplate='%s | Shoc Platform'></Helmet>
            <SessionProvider>
                <RouterProvider router={router} />
            </SessionProvider>
        </HelmetProvider>
    </>
};