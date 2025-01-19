export default defineNuxtRouteMiddleware(async ({ path }) => {
  const { loggedIn, user } = useUserSession();

  // if (!loggedIn.value && path !== "/auth/auth0") {
  //   return navigateTo("/auth/auth0", { external: true });
  // }
});
