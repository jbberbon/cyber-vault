import {createRouter, createWebHistory} from "vue-router";
import {ROUTES} from "@/lib/constants/routes.ts";
import {checkAuthAsync} from "@/lib/services/auth/check-auth-async.ts";

const router = createRouter({
  routes: ROUTES,
  history: createWebHistory()
});

// Protect routes
router.beforeEach(async (to, from, next) => {
  // Authenticated but wants to go to log in / register page
  if (to.path === '/login' || to.path === '/register') {
    const isAuthenticated = await checkAuthAsync();
    if (isAuthenticated) return next('/home');
  }

  // Authenticated and wants to go to a protected path
  if (to.meta.requireAuth) {
    const isAuthenticated = await checkAuthAsync();
    if (isAuthenticated) return next();
  }

  // Unauthenticated but wants to go to a protected path
  if (to.meta.requireAuth && to.path !== '/login') {
    const isAuthenticated = await checkAuthAsync();
    if (!isAuthenticated) return next('/login');
  }



  next();
})

export default router;
