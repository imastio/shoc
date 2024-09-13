import { useIntl } from "react-intl";

export default function PrivacyNotice() {
    const intl = useIntl();

    return <p className="px-8 text-center text-sm text-muted-foreground">
        {intl.formatMessage({id: 'auth.privacy.notice'})}
    </p>
}