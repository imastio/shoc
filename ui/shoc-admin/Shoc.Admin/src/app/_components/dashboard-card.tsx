import { RightOutlined } from "@ant-design/icons";
import { Card } from "antd";
import Link from "next/link";
import { CSSProperties } from "react";

export default function DashboardCard({style = {}, title, link = '/', description} : {
    style?: CSSProperties,
    title: string,
    link: string,
    description: string
}){
    return <Card
    style={{ ...style }}
    actions={[
      <Link href={link}><RightOutlined key="icon" /></Link>,
    ]}
  >
    <Card.Meta
      title={title}
      description={description}
    />
  </Card>
}