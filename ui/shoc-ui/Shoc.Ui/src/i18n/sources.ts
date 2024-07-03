import globalEn from "./messages/en/global.json";
import componentsEn from "./messages/en/components.json";
import errorsEn from "./messages/en/errors.json";
import workspacesEn from "./messages/en/workspaces.json";
import localeConfig from "./locale-config";

export const localeMessages = {
   
    en: {
        ...globalEn,
        ...componentsEn,
        ...errorsEn,
        ...workspacesEn
    }
};

export type IntlMessageId = keyof typeof localeMessages.en

declare global {
    namespace FormatjsIntl {
        interface Message {
            ids: IntlMessageId
        }
    }
}


export function getLocaleSource(locale?: string) {

    if(!localeConfig.availableLocales.some(item => item === locale)){
        locale = localeConfig.defaultLocale;
    }

    locale = locale || localeConfig.defaultLocale;

    return {
        locale,
        defaultLocale: localeConfig.defaultLocale,
        messages: Object.fromEntries(Object.entries(localeMessages))[locale]
    }
}
