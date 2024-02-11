import React from "react"
import { Button, Col, Row } from "antd"
import { HomeOutlined, MenuOutlined } from "@ant-design/icons"
import HeaderRightMenu from "./header-right-menu"
import { Link } from "react-router-dom"

export default function LayoutHeader({isFlyout, onMenuOpen}) {
   return <Row>
      <Col span={isFlyout ? 4 : 0}>
         <Button onClick={onMenuOpen}>
            <MenuOutlined />
         </Button>
      </Col>
      <Col span={4}>
         <Link to="/" style={{ marginRight: 0, marginLeft: 0 }}><HomeOutlined /> Shoc</Link>
      </Col>
      <Col span={isFlyout ? 16 : 20}>
         <HeaderRightMenu />
      </Col>
   </Row>
}
