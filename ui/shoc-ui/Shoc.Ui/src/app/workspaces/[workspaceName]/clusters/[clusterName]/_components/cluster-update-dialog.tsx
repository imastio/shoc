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
import { Input } from "@/components/ui/input"
import { Checkbox } from "@/components/ui/checkbox"
import { clusterNamePattern, clusterStatuses, clusterStatusesMap, clusterTypesMap } from "../../_components/well-known"
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select"

interface DialogProps extends ModalDialogProps {
  workspaceId: string,
  item: any
}

export default function ClusterUpdateDialog({ item, workspaceId, open, trigger, onClose, onSuccess }: DialogProps) {

  const intl = useIntl();
  const [errors, setErrors] = useState<any[]>([]);
  const [progress, setProgress] = useState(false);

  const formSchema = z.object({
      name: z.string().regex(clusterNamePattern, intl.formatMessage({ id: 'clusters.validation.invalidName' })),
      description: z.string().min(2, intl.formatMessage({ id: 'clusters.validation.invalidDescription' })),
      type: z.custom(type => clusterTypesMap[type], intl.formatMessage({ id: 'clusters.validation.invalidType' })),
      status: z.custom(type => clusterStatusesMap[type], intl.formatMessage({ id: 'clusters.validation.invalidStatus' })),
    });

  const form = useForm({
    resolver: zodResolver(formSchema),
    shouldUseNativeValidation: false
  })

  const onOk: SubmitHandler<FieldValues> = async (values) => {

    setErrors([]);
    setProgress(true);

    const { data, errors } = await rpc('cluster/workspace-clusters/updateById', {
      workspaceId: workspaceId,
      id: item.id,
      input: {
        ...item,
        workspaceId: workspaceId,
        id: item.id,
        name: values.name,
        description: values.description,
        status: values.status
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

    if(!open){
      return;
    }

    if(item){
      form.reset({...item})
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
        <DialogTitle>{intl.formatMessage({ id: 'clusters.update.title' })}</DialogTitle>
        <DialogDescription>
          {intl.formatMessage({ id: 'clusters.update.notice' })}
        </DialogDescription>
      </DialogHeader>
      <ErrorAlert errors={errors} title={intl.formatMessage({ id: 'clusters.update.error' })} />
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
                        placeholder={intl.formatMessage({ id: 'clusters.placeholders.name' })}
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
                        placeholder={intl.formatMessage({ id: 'clusters.placeholders.description' })}
                        disabled={progress}
                        {...field}
                      />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />
            </div>
            <div className="grid grap-1">
              <FormField
                control={form.control}
                name="status"
                render={({ field: { ref, ...fieldNoRef } }) => (
                  <FormItem>
                    <FormLabel>{intl.formatMessage({ id: 'global.labels.status' })}</FormLabel>
                    <Select disabled={progress} onValueChange={fieldNoRef.onChange} {...fieldNoRef}>
                      <FormControl>
                        <SelectTrigger className="w-full">
                          <SelectValue placeholder={intl.formatMessage({ id: 'clusters.placeholders.status' })} />
                        </SelectTrigger>
                      </FormControl>
                      <SelectContent>
                        {clusterStatuses.map((item) => <SelectItem key={item.key} value={item.key}>{intl.formatMessage({ id: item.display })}</SelectItem>)}
                      </SelectContent>
                    </Select>
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