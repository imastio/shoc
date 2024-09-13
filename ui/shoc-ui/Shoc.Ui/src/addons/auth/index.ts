import NextAuth from "next-auth"
import DuendeIDS6Provider from "next-auth/providers/duende-identity-server6"
import { getAuthSecret, getClientId, getClientSecret, getIssuer, getOpenIdScopes } from "./config";
import { getProfileUser, jwtCallback, sessionCallback } from "./lifecycle";

export const { handlers, auth, signIn, signOut } = NextAuth({
    providers: [
        DuendeIDS6Provider({
            id: 'shoc',
            name: 'Shoc Identity',
            clientId: getClientId(),
            clientSecret: getClientSecret(),
            issuer: getIssuer(),
            profile: getProfileUser,
            authorization: { params: { scope: getOpenIdScopes() } }
        })
    ],
    pages: {
        signIn: '/sign-in',
        signOut: '/',
        error: '/',
        verifyRequest: '/',
        newUser: '/'
    },
    trustHost: true,
    secret: getAuthSecret(),
    session: {
        strategy: 'jwt'
    },
    callbacks: {
        session: sessionCallback,
        jwt: jwtCallback
    }
})
