import errorsEn from "./messages/en/errors.json";
import authEn from "./messages/en/auth.json";
import localeConfig from "./locale-config";

export const localeMessages = {
    en: {
        ...errorsEn,
        ...authEn
    }
};


declare global {
    namespace FormatjsIntl {
        interface Message {
            ids: keyof typeof localeMessages.en
        }
    }
}

export function getLocaleSource(locale: string) {

    if(!localeConfig.availableLocales.some(item => item === locale)){
        locale = localeConfig.defaultLocale;
    }

    return {
        locale,
        defaultLocale: localeConfig.defaultLocale,
        messages: Object.fromEntries(Object.entries(localeMessages))[locale]
    }
}