import { PageHeader } from "@ant-design/pro-layout";
import { useNavigate } from "react-router-dom";

const PageHeaderExpended = props => {

    const navigate = useNavigate();
    const { title, style, tags, extra, ...rest } = props;

    return (

        <PageHeader
            onBack={() => navigate(-1)}
            title={title}
            style={{ paddingLeft: 0, ...style }}
            extra={extra}
            tags={tags}
            {...rest}

        />
    )
}

export default PageHeaderExpended;