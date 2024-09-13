import { Col, Row } from "antd";

export default function Container({children, fluid}){

    const colProps = fluid ? {span: 22, offset: 1} : {span: 18, offset: 3};

    return (
        <Row>
            <Col {...colProps}>
                {children}
            </Col>
        </Row>
    )
}


