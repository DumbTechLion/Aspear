import en from "./locales/en";
import fr from "./locales/fr";

export default defineI18nConfig(() => {
  return {
    messages: {
      en,
      fr,
    },
    detectBrowserLanguage: {
      useCookie: true,
      cookieKey: "i18n_redirected",
      redirectOn: "root", // recommended
    },
  };
});
