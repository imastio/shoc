import { RouterProvider } from 'react-router-dom';
import router from './router';
import AuthProvider from './providers/auth-provider/auth-provider';

export default function App() {

    return <>
        <AuthProvider>
            <RouterProvider router={router} />
        </AuthProvider>
    </>
};