'server-only';

import { createIntl } from '@formatjs/intl';
import { IntlShape } from 'react-intl';
import { getLocaleSource } from './sources';

export default async function getIntl(locale: string): Promise<IntlShape> {
    const localeSource = getLocaleSource(locale)
    return createIntl({
        locale: localeSource.locale,
        defaultLocale: localeSource.locale,
        messages: localeSource.messages
    });
}