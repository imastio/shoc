import { ExclamationTriangleIcon } from "@radix-ui/react-icons"
import {
    Alert,
    AlertDescription,
    AlertTitle,
} from "@/components/ui/alert"
import { useIntl } from "react-intl";
import { IntlMessageId } from "@/i18n/sources";

interface ErrorAlertProps {
    title?: string, 
    errors: any[], 
    className?: string
} 

export default function ErrorAlert({ title = '', errors = [], className = '' } : ErrorAlertProps) {

    const intl = useIntl();

    if (!errors || errors.length === 0) {
        return false;
    }

    const effectiveTitle = title || intl.formatMessage({id: ('errors.' + errors[0].code) as IntlMessageId});
    const descriptionErrors = title ? errors : errors.slice(1);

    return <Alert className={className} variant="destructive">
        <AlertTitle className="flex mb-0">         
            <ExclamationTriangleIcon className="h-4 w-4 mr-2" />
            {effectiveTitle}
        </AlertTitle>
        {descriptionErrors.length > 0 && <AlertDescription className="mt-1">
            {descriptionErrors.length === 1 && intl.formatMessage({ id: ('errors.' + errors[0].code) as IntlMessageId })}
            {descriptionErrors.length > 1 && <ul>
                {descriptionErrors.map((error, index) => <li key={index}>{intl.formatMessage({ id: ('errors.' + error.code) as IntlMessageId })}</li>)}
            </ul>}
        </AlertDescription>
        }
    </Alert>

}