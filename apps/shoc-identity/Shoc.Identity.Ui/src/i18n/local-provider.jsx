import { useSearchParams } from "react-router-dom";
import localeConfig from "./locale-config";
import { getLocaleSource } from "./sources";
import { IntlProvider as ReactIntlProvider } from 'react-intl';

export default function LocaleProvider({ children }) {

    const [searchParams] = useSearchParams();
    const locale = searchParams.get('lang') || localeConfig.defaultLocale;
    const source = getLocaleSource(locale);

    return <ReactIntlProvider locale={source.locale} defaultLocale={source.defaultLocale} messages={source.messages} onError={() => { }}>
        {children}
    </ReactIntlProvider>
}