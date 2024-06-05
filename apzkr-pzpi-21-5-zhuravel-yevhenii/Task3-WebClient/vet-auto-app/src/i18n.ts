import { initReactI18next } from "react-i18next";
import i18n from "i18next";
import ns1 from "./locales/en/ns1.json";
import ns2 from "./locales/uk/ns2.json";

export const defaultNS = "ns1";
export const resources = {
  en: {
    translation: ns1,
  },
  uk: {
    translation: ns2,
  },
} as const;

i18n.use(initReactI18next).init({
  lng: "uk",
  resources,
});

export default i18n;
