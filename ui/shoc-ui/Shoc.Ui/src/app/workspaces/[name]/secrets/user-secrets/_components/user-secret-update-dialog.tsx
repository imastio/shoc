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
import { secretDescriptionMaxLength, secretNamePattern } from "../../_components/well-known"
import { Textarea } from "@/components/ui/textarea"
import { Input } from "@/components/ui/input"
import { Checkbox } from "@/components/ui/checkbox"

interface DialogProps extends ModalDialogProps {
  workspaceId: string,
  item: any
}

export default function UserSecretUpdateDialog({ item, workspaceId, open, trigger, onClose, onSuccess }: DialogProps) {

  const intl = useIntl();
  const [errors, setErrors] = useState<any[]>([]);
  const [progress, setProgress] = useState(false);

  const formSchema = z.object({
    name: z.string().regex(secretNamePattern, intl.formatMessage({ id: 'secrets.validation.invalidName' })),
    description: z.string().max(secretDescriptionMaxLength, intl.formatMessage({ id: 'secrets.validation.longDescription' }))
      .min(2, intl.formatMessage({ id: 'secrets.validation.invalidDescription' })),
    disabled: z.boolean(),
  });

  const form = useForm({
    resolver: zodResolver(formSchema),
    shouldUseNativeValidation: false
  })

  const onOk: SubmitHandler<FieldValues> = async (values) => {

    setErrors([]);
    setProgress(true);

    const { data, errors } = await rpc('secret/workspace-user-secrets/updateById', {
      workspaceId: workspaceId,
      id: item.id,
      input: {
        name: values.name,
        description: values.description,
        disabled: values.disabled,
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
      form.reset({ ...item })
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
        <DialogTitle>{intl.formatMessage({ id: 'secrets.update.title.userSecret' })}</DialogTitle>
        <DialogDescription>
          {intl.formatMessage({ id: 'secrets.update.notice.userSecret' })}
        </DialogDescription>
      </DialogHeader>
      <ErrorAlert errors={errors} title={intl.formatMessage({ id: 'secrets.update.error' })} />
      <Form {...form}>
        <form onSubmit={form.handleSubmit(onOk)}>
          <div className="grid gap-2">
            <div className="grid gap-1">
              <FormField
                control={form.control}
                name="name"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>{intl.formatMessage({ id: 'global.labels.name' })}</FormLabel>
                    <FormControl>
                      <Input
                        autoFocus
                        placeholder={intl.formatMessage({ id: 'secrets.placeholders.name.userSecret' })}
                        type="text"
                        autoCapitalize="none"
                        autoComplete="off"
                        aria-autocomplete="none"
                        autoCorrect="off"
                        disabled={progress}
                        {...field}
                      />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />
            </div>
            <div className="grid gap-1">
              <FormField
                control={form.control}
                name="description"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>{intl.formatMessage({ id: 'global.labels.description' })}</FormLabel>
                    <FormControl>
                      <Textarea
                        placeholder={intl.formatMessage({ id: 'secrets.placeholders.description' })}
                        disabled={progress}
                        {...field}
                      />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />
            </div>
            <div className="grid gap-1">
              <FormField
                control={form.control}
                name="disabled"
                render={({ field }) => (
                  <FormItem >
                    <FormLabel>
                      {intl.formatMessage({ id: 'global.labels.disabled' })}
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
                        {intl.formatMessage({ id: 'secrets.descriptions.disabled' })}
                        </FormDescription>
                      </div>
                    </div>
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