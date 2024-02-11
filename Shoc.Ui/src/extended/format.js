import dayjs from "extended/time";

export const localDateTime = dateStr =>  dateStr ? dayjs.utc(dateStr).local().format("YYYY-MM-DD HH:mm") : "";
export const localDate = dateStr =>  dateStr ? dayjs.utc(dateStr).local().format("YYYY-MM-DD") : "";
export const localYear = dateStr =>  dateStr ? dayjs.utc(dateStr).local().format("YYYY") : "";
