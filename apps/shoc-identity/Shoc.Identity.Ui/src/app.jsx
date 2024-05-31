import { useEffect, useState } from 'react';
import AuthProvider from '@/providers/auth-provider/auth-provider';
import PageRouter from './page-router';

export default function App() {

    return <>
        <AuthProvider>
            <PageRouter />
        </AuthProvider>
    </>
};