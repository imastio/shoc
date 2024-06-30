import { AdapterUser } from "@auth/core/adapters"
import { User } from "next-auth"

declare module "next-auth" {
    interface Session {
        user?: User | AdapterUser | null
        error?: "refresh_error" | null
    }
}

declare module "@auth/core/types" {
    interface User {
        emailVerified?: boolean | Date | null,
        username?: string | null,
        userType?: string | null
    }
}

declare module "next-auth/jwt" {
    interface JWT {
        idToken?: string | null,
        accessToken?: string | null,
        expiresAt: number,
        refreshToken?: string | null,
        user?: User | AdapterUser | null,
        error?: "refresh_error" | null
    }
}