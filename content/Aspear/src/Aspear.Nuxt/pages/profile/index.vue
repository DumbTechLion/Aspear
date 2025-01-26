<script setup>
  const { $api, $toast } = useNuxtApp();
  const { loggedIn, user, session, fetch, clear } = useUserSession();
  const { $authApi } = useAuthApiFetch("/api");
  
  const call = async () => {
    const response = await $api("/weatherforecast");
    console.log(response);
  };
</script>

<template>
  <div v-if="loggedIn">
    <h1>Welcome {{ user.login }}!</h1>
    <p>Logged in since {{ session.loggedInAt }}</p>
    <button @click="clear">Logout</button>
  </div>
  <div v-else>
    <h1>Not logged in</h1>
    <a href="/auth/keycloak">Login with Keycloak</a>
  </div>
  <button @click="call()">Test Secure API Call</button>
</template>
