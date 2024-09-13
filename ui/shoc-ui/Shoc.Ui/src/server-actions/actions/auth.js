import 'server-only';

import { defineServerAction } from '../define';
import { signIn as authSignIn, signOut as authSignOut } from '@/addons/auth';
import { singleSignOut as authSingleSignOut } from '@/addons/auth/actions';

export const signIn = defineServerAction(({ redirectTo }) => {
    return authSignIn('shoc', { redirect: true, redirectTo });
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