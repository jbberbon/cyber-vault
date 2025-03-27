import type {RouteRecordRaw} from "vue-router";
export const ROUTES: RouteRecordRaw[] = [
  {
    path: "/login",
    name: "login",
    component: () => import("@/pages/auth/login/Login.vue"),
    meta: {requireAuth: false},
  },
  {
    path: "/register",
    name: "register",
    component: () => import("@/pages/auth/register/Register.vue"),
    meta: {requireAuth: false},
  },
  {
    path: "/home",
    name: "home",
    component: () => import("@/pages/protected/home/Home.vue"),
    meta: {requireAuth: true},
  },
  {
    path: "/my-vault",
    name: "my-vault-root",
    component: () => import("@/pages/protected/my-vault/MyVault.vue"),
    meta: {requireAuth: true},
  },
  {
    path: "/my-vault/:id",
    name: "my-vault-subdirectory",
    component: () => import("@/pages/protected/my-vault/MyVault.vue"),
    props: true,
    meta: {requireAuth: true},
  },
  {
    path: "/shared-with-me",
    name: "shared-with-me",
    component: () => import("@/pages/protected/shared-with-me/SharedWithMe.vue"),
    meta: {requireAuth: true},
  },

  // ERROR ROUTES
  {
    path: "/not-found",
    name: "not-found",
    component: () => import("@/pages/error/NotFound.vue"),
    meta: {requireAuth: false},
  },
  {
    path: "/bad-request",
    name: "bad-request",
    component: () => import("@/pages/error/BadRequest.vue"),
    meta: {requireAuth: false},
  },
]
