import i18n from 'i18next';
import { initReactI18next } from 'react-i18next';

import EN from './locales/en/translation.json';
import PT from './locales/pt/translation.json';

const resources = {
  en: {
    translation: EN
  },
  pt: {
    translation: PT
  }
};

i18n
  .use(initReactI18next)
  .init({
    resources,
    fallbackLng: 'en',
    debug: true,

    interpolation: {
      escapeValue: false, // not needed for react as it escapes by default
    }
  });


export default i18n;