"use client"

import { Button } from "@/components/ui/button"
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from "@/components/ui/dialog"
import { zodResolver } from "@hookform/resolvers/zod"
import { useIntl } from "react-intl"
import { z } from "zod"
import ErrorAlert from "@/components/general/error-alert"
import { useEffect, useState } from "react"
import { FieldValues, SubmitHandler, useForm } from "react-hook-form"
import { Form, FormControl, FormDescription, FormField, FormItem, FormLabel, FormMessage } from "@/components/ui/form"
import SpinnerIcon from "@/components/icons/spinner-icon"
import { rpc } from "@/server-actions/rpc"
import { ModalDialogProps } from "@/components/general/component-types"
import { Textarea } from "@/components/ui/textarea"
import { Checkbox } from "@/components/ui/checkbox"

interface DialogProps extends ModalDialogProps {
  workspaceId: string,
  item: any
}

export default function UserSecretValueUpdateDialog({ item, workspaceId, open, trigger, onClose, onSuccess }: DialogProps) {

  const intl = useIntl();
  const [errors, setErrors] = useState<any[]>([]);
  const [progress, setProgress] = useState(false);

  const formSchema = z.object({
    encrypted: z.boolean(),
    value: z.string().optional()
  });

  const form = useForm({
    resolver: zodResolver(formSchema),
    shouldUseNativeValidation: false
  })

  const onOk: SubmitHandler<FieldValues> = async (values) => {

    setErrors([]);
    setProgress(true);

    const { data, errors } = await rpc('secret/workspace-user-secrets/updateValueById', {
      workspaceId: workspaceId,
      id: item.id,
      input: {
        encrypted: values.encrypted,
        value: values.value
      }
    });

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

  useEffect(() => {

    if (!open) {
      return;
    }

    if (item) {
      form.reset({
        ...item,
        value: item.encrypted ? '' : item.value
      })
    }
    else {
      form.reset()
    }
  }, [form, open, item]);

  return <Dialog open={open} onOpenChange={onOpenChangeWrapper} modal>
    <DialogTrigger asChild>
      {trigger}
    </DialogTrigger>
    <DialogContent className="w-4/5 md:w-1/2">
      <DialogHeader>
        <DialogTitle>{intl.formatMessage({ id: 'secrets.updateValue.title.userSecret' })}</DialogTitle>
        <DialogDescription>
          {intl.formatMessage({ id: 'secrets.updateValue.notice.userSecret' })}
        </DialogDescription>
      </DialogHeader>
      <ErrorAlert errors={errors} title={intl.formatMessage({ id: 'secrets.update.error' })} />
      <Form {...form}>
        <form onSubmit={form.handleSubmit(onOk)}>
          <div className="grid gap-2">
            <div className="grid gap-1">
              <FormField
                control={form.control}
                name="encrypted"
                render={({ field }) => (
                  <FormItem >
                    <FormLabel>
                      {intl.formatMessage({ id: 'global.labels.encrypted' })}
                    </FormLabel>
                    <div className="flex flex-row items-start space-x-3 space-y-0 rounded-md border p-4">
                      <FormControl>
                        <Checkbox
                          checked={field.value}
                          onCheckedChange={field.onChange}
                        />
                      </FormControl>
                      <div className="space-y-1 leading-none">
                        <FormDescription>
                          {intl.formatMessage({ id: 'secrets.descriptions.encrypted' })}
                        </FormDescription>
                      </div>
                    </div>
                  </FormItem>
                )}
              />
            </div>
            <div className="grid gap-1">
              <FormField
                control={form.control}
                name="value"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>{intl.formatMessage({ id: 'global.labels.value' })}</FormLabel>
                    <FormControl>
                      <Textarea
                        placeholder={intl.formatMessage({ id: 'secrets.placeholders.value' })}
                        disabled={progress}
                        {...field}
                      />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />
            </div>
            <DialogFooter>
              <Button type="submit" disabled={progress}>
                {progress && (
                  <SpinnerIcon className="mr-2 h-4 w-4 animate-spin" />
                )}
                {intl.formatMessage({ id: 'global.actions.update' })}
              </Button>
            </DialogFooter>
          </div>
        </form>
      </Form>
    </DialogContent>
  </Dialog>
}