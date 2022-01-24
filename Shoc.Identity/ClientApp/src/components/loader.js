import {Row, Spin} from 'antd';
import {LoadingOutlined} from '@ant-design/icons';

const Loader = () => (
    <Row type="flex" justify="center" align="middle" style={{minHeight: '100vh'}}>
        <Spin indicator={<LoadingOutlined style={{ fontSize: 24 }} spin />} />
    </Row>
)

export default Loader;