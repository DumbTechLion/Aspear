export default defineNuxtRouteMiddleware(async ({ path }) => {
  const { loggedIn, user } = useUserSession();

  // if (!loggedIn.value && path.startsWith("/profile")) {
  //   return navigateTo("/auth/keycloak", { external: true });
  // }
});
