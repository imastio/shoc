import React, { lazy as reactLazy, Suspense } from 'react';
import MainLayout from 'layouts/main-layout';
import { Route, Routes } from 'react-router-dom';
import LoadingPage from 'pages/_loading';
import PrivateLayout from 'layouts/private-layout';

const lazy = (action) => {
    return reactLazy(() => {
        return action();
    });
} 

const page = (elem) => (
    <Suspense fallback={<LoadingPage />}>
        {elem}
    </Suspense>
);

const IndexPage = lazy(() => import('pages/index'));
const SignedInPage = lazy(() => import('pages/signed-in'));
const AccessDeniedPage = lazy(() => import('pages/access-denied'));
const NotFoundPage = lazy(() => import('pages/not-found'));

const PageRouter = () => {
    return (
        <Routes>
            <Route element={<PrivateLayout />}>
                <Route element={<MainLayout />}>
                    <Route index element={page(<IndexPage />)} />
                </Route>
            </Route>
            <Route path="/signed-in" element={page(<SignedInPage />)} />
            <Route path="/access-denied" element={page(<AccessDeniedPage />)} />
            <Route path="/not-found" element={page(<NotFoundPage />)} />
            <Route path="*" element={page(<NotFoundPage />)} />
        </Routes>
    )
}

export default PageRouter;
