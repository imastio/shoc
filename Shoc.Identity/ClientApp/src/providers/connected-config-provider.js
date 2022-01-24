import { ConfigProvider } from "antd";

const ConnectedConfigProvider = props => {
    
    return <ConfigProvider children={props.children} />
}

export default ConnectedConfigProvider;