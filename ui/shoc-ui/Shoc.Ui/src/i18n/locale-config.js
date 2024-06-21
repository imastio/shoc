const availableLocales = ["en"];
const defaultLocale = availableLocales[0];
const localeCookie = 'ENLIGHT_LOCALE';

const localeLabels = [
    {
        locale: "en",
        text: "English"
    }
]

const localeConfig = { 
    availableLocales, 
    defaultLocale, 
    localeLabels, 
    localeCookie,
    prefixDefault: true,
    basePath: ''
};

export default localeConfig;