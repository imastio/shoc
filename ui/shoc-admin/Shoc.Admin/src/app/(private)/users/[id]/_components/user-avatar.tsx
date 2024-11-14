import { useCallback, useEffect, useState } from "react";
import Avatar from "@/components/general/avatar";
import { useApiAuthentication } from "@/providers/api-authentication/use-api-authentication";
import { selfClient } from "@/clients/shoc";
import UsersClient from "@/clients/shoc/identity/users-client";

export default function UserAvatar({ userId, meta = {} } : { userId: string, meta: any }) {

    const [uploading, setUploading] = useState(false);
    const [progress, setProgress] = useState(true);
    const [pictureUri, setPictureUri] = useState();
    const { withToken } = useApiAuthentication();

    const load = useCallback(async (id: string) => {

        setProgress(true);
        const result = await withToken((token: string) => selfClient(UsersClient).getPictureById(token, id))
        setProgress(false);

        if (result.error) {
            return;
        }

        setPictureUri(result.payload?.pictureUri);

    }, [withToken])

    useEffect(() => {
        if (!userId) {
            return;
        }
        load(userId);
    }, [userId, load])

    const upload = useCallback(async (uri: string) => {
        setUploading(true);
        const result = await withToken((token: string) => selfClient(UsersClient).updatePictureById(token, userId, { pictureUri: uri }))
        setUploading(false);

        if (result.error) {
            return;
        }

        setPictureUri(result.payload?.pictureUri);
    }, [userId, withToken])

   return (
        <Avatar
            progress={progress}
            uploading={uploading}
            pictureUri={pictureUri}
            editable
            size={{xxl: 120, xl: 120, lg: 100, md: 100, sm: 100, xs: 100}} 
            style={{margin: '15px 15px'}} 
            meta={meta} 
            onUpload={upload}
            >
        </Avatar>
   )
}