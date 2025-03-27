import type {RouteRecordRaw} from "vue-router";

export const SIDEBAR_ROUTES: RouteRecordRaw[] = [
  {
    path: "/login",
    name: "Login",
    component: () => import("@/pages/auth/login/Login.vue"),
  },
  {
    path: "/home",
    name: "Home",
    component: () => import("@/pages/protected/home/Home.vue"),
    meta: {requireAuth: true}
  },
  {
    path: "/my-vault",
    name: "My Vault Root",
    component: () => import("@/pages/protected/my-vault/MyVault.vue"),
    meta: {requireAuth: true}
  },
  {
    path: "/my-vault/:id",
    name: "my-vault-subdirectory",
    component: () => import("@/pages/protected/my-vault/MyVault.vue"),
    props: true,
    meta: {requireAuth: true}
  },
  {
    path: "/shared-with-me",
    name: "Shared with me",
    component: () => import("@/pages/protected/shared-with-me/SharedWithMe.vue"),
    meta: {requireAuth: true}
  },

  // ERROR ROUTES
  {
    path: "/not-found",
    name: "Not found",
    component: () => import("@/pages/error/NotFound.vue"),
  },
  {
    path: "/bad-request",
    name: "Bad request",
    component: () => import("@/pages/error/BadRequest.vue"),
  },
]
