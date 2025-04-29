import { Select, SelectTrigger, SelectValue, SelectContent, SelectItem } from "@/components/ui/select";
import { JobScope } from "./types";
import { IntlMessageId } from "@/i18n/sources";
import { useIntl } from "react-intl";

const options: { key: JobScope, display: IntlMessageId }[] = [
    {
        key: 'user',
        display: 'jobs.scopes.user'
    },
    {
        key: 'workspace',
        display: 'jobs.scopes.workspace'
    }
]

export default function ScopeSelector({ className, value, onChange }: { className?: string, value?: string, onChange: (value?: string) => void }) {

    const intl = useIntl();

    return <Select
        value={value}
        onValueChange={onChange}
    >
        <SelectTrigger className={className}>
            <SelectValue placeholder={intl.formatMessage({ id: 'jobs.filters.scope.placeholder' })} />
        </SelectTrigger>
        <SelectContent>
            <SelectItem key='all' value='all'>
                {intl.formatMessage({ id: 'jobs.filters.scope.prefix' })} {intl.formatMessage({ id: 'global.filters.all' })}
            </SelectItem>
            {options.map((option) => (
                <SelectItem key={option.key} value={option.key}>
                    {intl.formatMessage({ id: 'jobs.filters.scope.prefix' })} {intl.formatMessage({ id: option.display })}
                </SelectItem>
            ))}
        </SelectContent>
    </Select>
}