'use client';

import { IntlProvider as ReactIntlProvider } from 'react-intl';

export default function IntlProvider({ messages, defaultLocale, locale, children }) {
  return (
    <ReactIntlProvider messages={messages} defaultLocale={defaultLocale} locale={locale} onError={() => { }}>
      {children}
    </ReactIntlProvider>
  );
}