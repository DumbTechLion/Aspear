export default oauthKeycloakEventHandler({
  async onSuccess(event, { user, tokens }) {
    await setUserSession(event, {
      user: {
        sub: user.sub,
        email: user.email,
        emailVerified: user.email_verified,
        uuid: user["https://schemas.aspear.dog/uuid"] as string,
        roles: user["https://schemas.aspear.dog/roles"] as string[],
        firstName: user["https://schemas.aspear.dog/first_name"] as string,
        lastName: user["https://schemas.aspear.dog/last_name"] as string,
        phoneNumber: user["https://schemas.aspear.dog/phone_number"] as string,
      },
      tokens: {
        accessToken: tokens.access_token,
        refreshToken: tokens.refresh_token,
      },
    });

    return sendRedirect(event, "/");
  },
  // Optional, will return a json error and 401 status code by default
  onError(event, error) {
    console.error("Keycloak OAuth error:", error);
    return sendRedirect(event, "/");
  },
});
