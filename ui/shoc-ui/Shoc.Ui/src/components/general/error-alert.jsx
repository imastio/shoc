import { ExclamationTriangleIcon } from "@radix-ui/react-icons"
import {
    Alert,
    AlertDescription,
    AlertTitle,
} from "@/components/ui/alert"
import { useIntl } from "react-intl";

export default function ErrorAlert({ title = '', errors = [], className = '' }) {

    const intl = useIntl();

    if (!errors || errors.length === 0) {
        return false;
    }

    const effectiveTitle = title || intl.formatMessage({id: 'errors.' + errors[0].code});
    const descriptionErrors = title ? errors : errors.slice(1);

    return <Alert className={className} variant="destructive">
        <ExclamationTriangleIcon className="h-4 w-4" />
        <AlertTitle>{effectiveTitle}</AlertTitle>
        {descriptionErrors.length > 0 && <AlertDescription>
            {descriptionErrors.length === 1 && intl.formatMessage({ id: 'errors.' + errors[0].code })}
            {descriptionErrors.length > 1 && <ul>
                {descriptionErrors.map((error, index) => <li key={index}>{intl.formatMessage({ id: 'errors.' + error.code })}</li>)}
            </ul>}
        </AlertDescription>
        }
    </Alert>

}