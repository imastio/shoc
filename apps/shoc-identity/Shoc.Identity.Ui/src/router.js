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
                ]
            }
        ]
    }
]



const router = createBrowserRouter(allRoutes);
export default router;