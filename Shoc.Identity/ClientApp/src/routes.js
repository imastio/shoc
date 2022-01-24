import pMinDelay from "p-min-delay";

const page = imported => pMinDelay(imported, 200)

const routes = [
    {
        key: "index-page",
        path: "/",
        component: () => page(import("pages/")),
        exact: true,
        protected: true
    },
    {
        key: "signin-page",
        path: "/sign-in",
        component: () => page(import("pages/sign-in")),
        exact: true,
        protected: false
    },
    {
        key: "signed-in-page",
        path: "/signed-in",
        component: () => page(import("pages/signed-in")),
        exact: true,
        protected: false
    },
    {
        key: "signup-page",
        path: "/sign-up",
        component: () => page(import("pages/sign-up")),
        exact: true,
        protected: false
    },
    {
        key: "confirm-page",
        path: "/confirm",
        component: () => page(import("pages/confirm")),
        exact: true,
        protected: false
    },
    {
        key: "sign-out-page",
        path: "/sign-out",
        component: () => page(import("pages/sign-out")),
        exact: true,
        protected: false
    },
    {
        key: "signed-out-page",
        path: "/signed-out",
        component: () => page(import("pages/signed-out")),
        exact: true,
        protected: false
    },
    {
        key: "access-denied-page",
        path: "/access-denied",
        component: () => page(import("pages/access-denied")),
        exact: true,
        protected: false
    },
    {
        key: "any-unknown-path",
        path: undefined,
        component: () => page(import("pages/not-found"))
    }
]

export default routes;