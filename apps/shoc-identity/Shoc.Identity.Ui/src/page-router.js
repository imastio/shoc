import React, { lazy as reactLazy, Suspense } from 'react';
import { createBrowserRouter, RouterProvider } from 'react-router-dom';
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

const IndexPage = lazy(() => import('@/pages/index'));
const SignInPage = lazy(() => import('@/pages/sign-in'));

export const allRoutes = [
    {
        element: <GlobalLayout />,
        children: [
            { index: true, element: page(<IndexPage />) },
            { path: '/sign-in', element: page(<SignInPage />) },
        ]
    }
]

const router = createBrowserRouter(allRoutes);
export default function PageRouter() {
    return <RouterProvider router={router} />;
}