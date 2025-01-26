// https://nuxt.com/docs/api/configuration/nuxt-config
export default defineNuxtConfig({
  compatibilityDate: "2024-04-03",
  devtools: { enabled: true },
  css: ["~/assets/css/main.css"],
  modules: [
    "nuxt-auth-utils",
    "nuxt-authorization",
    "@nuxtjs/i18n",
    "@nuxt/icon",
    "nuxt-open-fetch",
    "nuxt-time",
  ],
  postcss: {
    plugins: {
      tailwindcss: {},
      autoprefixer: {},
    },
  },
  i18n: {
    vueI18n: "./i18n.config.ts",
  },
  openFetch: {
    openAPITS: {
      enum: true,
    },
    clients: {
      api: {
        baseURL: "/api",
      },
    },
  },
  routeRules: {
    "/back/**": { proxy: process.env.NUXT_API_URL + "/**" },
  },
});
