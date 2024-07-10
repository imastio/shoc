import { ReactNode } from "react";

export interface ModalDialogProps<TResult = any> {
    open?: boolean,
    trigger?: ReactNode,
    onClose?: () => void,
    onSuccess?: (result: TResult) => void,
}
