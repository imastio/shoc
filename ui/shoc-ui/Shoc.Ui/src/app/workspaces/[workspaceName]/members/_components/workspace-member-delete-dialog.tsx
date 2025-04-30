import { AlertDialog, AlertDialogAction, AlertDialogCancel, AlertDialogContent, AlertDialogDescription, AlertDialogTitle, AlertDialogTrigger } from "@/components/ui/alert-dialog";
import { WorkspaceMember } from "./types";
import { AlertDialogFooter, AlertDialogHeader } from "@/components/ui/alert-dialog";
import { ReactNode, useState } from "react";
import ErrorAlert from "@/components/general/error-alert";
import { useIntl } from "react-intl";
import { rpc } from "@/server-actions/rpc";
import { cn } from "@/lib/utils";
import { buttonVariants } from "@/components/ui/button";
import SpinnerIcon from "@/components/icons/spinner-icon";

type DialogProps = {
    item: WorkspaceMember,
    open?: boolean,
    onClose?: () => void,
    trigger?: ReactNode,
    onSuccess?: (result: any) => void,
}

export default function WorkspaceMemberDeleteDialog({ item, open, onClose, trigger, onSuccess }: DialogProps) {

    const intl = useIntl();
    const [errors, setErrors] = useState<any[]>([]);
    const [progress, setProgress] = useState(false);

    const onOk = async () => {

        setErrors([]);
        setProgress(true);
        const { data, errors } = await rpc('workspace/user-workspace-members/deleteById', { workspaceId: item.workspaceId, id: item.id });
        setProgress(false);

        if (errors) {
            setErrors(errors);
            return;
        }

        if (onSuccess) {
            onSuccess(data)
        }

        onOpenChangeWrapper(false)
    }

    const onOpenChangeWrapper = (openValue: boolean): void => {
        setErrors([]);

        if (!openValue && onClose) {
            onClose();
        }
    }

    return (
        <AlertDialog open={open} onOpenChange={onOpenChangeWrapper}>
            {trigger && <AlertDialogTrigger asChild>
                {trigger}
            </AlertDialogTrigger>
            }
            <AlertDialogContent>
                <AlertDialogHeader>
                    <AlertDialogTitle>{intl.formatMessage({id: 'global.messages.sure'})}</AlertDialogTitle>
                    <ErrorAlert errors={errors} />
                    <AlertDialogDescription>
                    {intl.formatMessage({id: 'global.messages.noUndoneNotice'})} 
                    {' '}
                    {intl.formatMessage({id: 'global.messages.permanentRemove'})}     
                    </AlertDialogDescription>
                </AlertDialogHeader>
                <AlertDialogFooter>
                    <AlertDialogCancel disabled={progress}>
                        {intl.formatMessage({ id: 'global.actions.cancel' })}
                    </AlertDialogCancel>
                    <AlertDialogAction
                        type="submit"
                        className={cn(buttonVariants({ variant: 'destructive' }))}
                        disabled={progress}
                        onClick={async (e) => {
                            e.preventDefault();
                            await onOk()
                        }}
                    >
                        {progress && <SpinnerIcon className="mr-2 h-4 w-4 animate-spin" />}
                        {intl.formatMessage({ id: 'global.actions.continue' })}
                    </AlertDialogAction>
                </AlertDialogFooter>
            </AlertDialogContent>
        </AlertDialog>
    )

}