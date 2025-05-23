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
import { Form, FormControl, FormField, FormItem, FormLabel, FormMessage } from "@/components/ui/form"
import SpinnerIcon from "@/components/icons/spinner-icon"
import { rpc } from "@/server-actions/rpc"
import { ModalDialogProps } from "@/components/general/component-types"
import { Textarea } from "@/components/ui/textarea"

interface DialogProps extends ModalDialogProps {
  workspaceId: string,
  item: any
}

export default function ClusterConfigurationUpdateDialog({ item, workspaceId, open, trigger, onClose, onSuccess }: DialogProps) {

  const intl = useIntl();
  const [errors, setErrors] = useState<any[]>([]);
  const [progress, setProgress] = useState(false);

  const formSchema = z.object({
    configuration: z.string()
  });

  const form = useForm({
    resolver: zodResolver(formSchema),
    shouldUseNativeValidation: false
  })

  const onOk: SubmitHandler<FieldValues> = async (values) => {

    setErrors([]);
    setProgress(true);

    const { data, errors } = await rpc('cluster/workspace-clusters/updateConfigurationById', {
      workspaceId: workspaceId,
      id: item.id,
      input: {
        workspaceId: workspaceId,
        id: item.id,
        configuration: values.configuration
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

    form.reset()
  }, [form, open, item]);

  return <Dialog open={open} onOpenChange={onOpenChangeWrapper} modal>
    <DialogTrigger asChild>
      {trigger}
    </DialogTrigger>
    <DialogContent className="w-4/5 md:w-1/2">
      <DialogHeader>
        <DialogTitle>{intl.formatMessage({ id: 'clusters.configure.title' })}</DialogTitle>
        <DialogDescription>
          {intl.formatMessage({ id: 'clusters.configure.notice' })}
        </DialogDescription>
      </DialogHeader>
      <ErrorAlert errors={errors} title={intl.formatMessage({ id: 'clusters.configure.error' })} />
      <Form {...form}>
        <form onSubmit={form.handleSubmit(onOk)}>
          <div className="grid gap-2">
            <div className="grid gap-1">
              <FormField
                control={form.control}
                name="configuration"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>{intl.formatMessage({ id: 'global.labels.configuration' })}</FormLabel>
                    <FormControl>
                      <Textarea
                        className="min-h-[160px]"
                        placeholder={intl.formatMessage({ id: 'clusters.placeholders.configuration' })}
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
                {intl.formatMessage({ id: 'global.actions.configure' })}
              </Button>
            </DialogFooter>
          </div>
        </form>
      </Form>
    </DialogContent>
  </Dialog>
}