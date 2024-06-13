import React from "react"
import ConsoleHeaderRightMenu from "./console-header-right-menu"
import { Button, Col, Row } from "antd"
import { MenuOutlined } from "@ant-design/icons"

export default function ConsoleHeader({isFlyout, onMenuOpen}) {
   return <Row>
      <Col span={isFlyout ? 4 : 0}>
         <Button onClick={onMenuOpen}>
            <MenuOutlined />
         </Button>
      </Col>
      <Col span={isFlyout ? 20 : 24}>
         <ConsoleHeaderRightMenu />
      </Col>
   </Row>
}
