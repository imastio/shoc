import 'server-only';

import { defineServerAction } from '../define';
import { signIn as authSignIn, signOut as authSignOut } from '@/addons/auth';

export const signIn = defineServerAction(({ }) => {
    return authSignIn('shoc', { redirect: true });
});

export const signOut = defineServerAction(() => {
    return authSignOut();
});

export const serverActions = {
    'auth/signIn': signIn,
    'auth/signOut': signOut
}