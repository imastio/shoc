import { Select } from "antd";
import { accessAreas } from "./well-known";

export function AccessAreaSelector(props: any) {
    return <Select {...props}>
        {accessAreas.map(entry => <Select.Option key={entry.key} value={entry.key}>{entry.display}</Select.Option>)}
    </Select>
}