import React, { lazy as reactLazy, Suspense } from 'react';
import { createBrowserRouter } from 'react-router-dom';
import LoadingPage from '@/pages/loading';

const lazy = (action) => {
    return reactLazy(() => {
        return action();
    });
}

const page = (elem) => (
    <Suspense fallback={ <LoadingPage /> }>
        {elem}
    </Suspense>
);
const GlobalLayout = lazy(() => import('@/layouts/global-layout'));
const InitLayout = lazy(() => import('@/layouts/init-layout'));

const IndexPage = lazy(() => import('@/pages/index'));
const SignInPage = lazy(() => import('@/pages/sign-in'));
const SignUpPage = lazy(() => import('@/pages/sign-up'));
const ConfirmPage = lazy(() => import('@/pages/confirm'));
const RecoverPasswordPage = lazy(() => import('@/pages/recover-password'));
const SignOutPage = lazy(() => import('@/pages/sign-out'));

export const allRoutes = [
    {
        element: <GlobalLayout />,
        children: [
            {
                element: <InitLayout />,
                children: [
                    { index: true, element: page(<IndexPage />) },
                    { path: '/sign-in', element: page(<SignInPage />) },
                    { path: '/sign-up', element: page(<SignUpPage />) },
                    { path: '/confirm', element: page(<ConfirmPage />) },
                    { path: '/recover-password', element: page(<RecoverPasswordPage />) },
                    { path: '/sign-out', element: page(<SignOutPage />) },
                ]
            }
        ]
    }
]

const router = createBrowserRouter(allRoutes);
export default router;