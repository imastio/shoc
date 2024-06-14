import { App, Avatar as AntdAvatar, Spin } from "antd";
import { Button, Upload } from "antd";
import AntdImgCrop from "antd-img-crop";

const maxUploadSize = 2024 * 1000;

export default function Avatar(
    { 
        progress,
        uploading,
        pictureUri,
        meta = {}, 
        editable = false, 
        size, 
        style = {},
        onUpload
    }) {

    const { notification } = App.useApp();

    const avatar = () => <AntdAvatar src={pictureUri} size={size}>
        {!pictureUri && <div>
            {meta}
        </div>
        }
    </AntdAvatar>


    return <Spin spinning={progress || uploading} style={{ ...style }}>
        {!editable && avatar()}
        {editable && <>

            <AntdImgCrop aspect={1} quality={1}>
                <Upload type="select" accept="image/*" showUploadList={false} beforeUpload={async (file) => {

                    const isJpgOrPng = file.type === 'image/jpeg' || file.type === 'image/png';

                    if (file.size > maxUploadSize) {
                        notification.error({ message: 'Image must be smaller than 2MB!' });
                        return false;
                    }

                    if (!isJpgOrPng) {
                        notification.error({ message: 'You can only upload JPG/PNG file!' });
                        return false;
                    }

                    const size = 512;
                    const canvas = document.createElement('canvas')
                    const ctx = canvas.getContext('2d')

                    canvas.width = size
                    canvas.height = size

                    const bitmap = await createImageBitmap(file)
                    const { width, height } = bitmap

                    const ratio = Math.max(size / width, size / height)

                    const x = (size - (width * ratio)) / 2
                    const y = (size - (height * ratio)) / 2

                    ctx.drawImage(bitmap, 0, 0, width, height, x, y, width * ratio, height * ratio);

                    const base64Uri = await canvas.toDataURL('image/jpeg', 0.4);

                    onUpload(base64Uri)
                }}>
                    <Button style={{height: '100%'}} type="link">
                        {avatar()}
                    </Button>
                </Upload>
            </AntdImgCrop>
        </>
        }
    </Spin>
}