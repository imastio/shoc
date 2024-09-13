import { Alert } from "antd";
import { toErrorMessage } from "@/well-known/error-handling";

const StandardAlert = props => {

    const errors = props.errors || [];
    const type = props.type || "error";
    const style = props.style || {};
    const showIcon = props.showIcon || false;

    const visible = errors.length > 0 || (!props.optional && props.message);

    if (!visible) {
        return <></>;
    }

    return <Alert style={{ ...style }}
        type={type}
        showIcon={showIcon}
        message={props.message || "System Error"}
        description={<>
            {errors.length > 1 && <ul>
                {errors.map((err, i) => <li key={i}>{toErrorMessage(err.code)}</li>)}
            </ul>
            }
            {
                errors.length === 1 && toErrorMessage(errors[0].code)
            }
        </>}
    />
};

export default StandardAlert;
