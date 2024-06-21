import 'server-only';

import { defineServerAction } from '../define';
import { signIn as authSignIn, signOut as authSignOut, singleSignOut as authSingleSignOut } from '@/addons/auth';

export const signIn = defineServerAction(({ }) => {
    return authSignIn('shoc', { redirect: true });
});

export const signOut = defineServerAction(({ endSessionUri }) => {
    return authSignOut({ redirect: true, redirectTo: endSessionUri });
});

export const singleSignOut = defineServerAction(({ postLogoutRedirectUri, state }) => {
    return authSingleSignOut({ postLogoutRedirectUri, state });
});

export const serverActions = {
    'auth/signIn': signIn,
    'auth/signOut': signOut,
    'auth/signleSignOut': singleSignOut
}