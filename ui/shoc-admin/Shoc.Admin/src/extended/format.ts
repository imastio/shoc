import dayjs from "@/extended/time";

export const localDateTime = (dateStr: string) => dateStr ? dayjs.utc(dateStr).local().format("YYYY-MM-DD HH:mm") : "";
export const localDateTimeWithSec = (dateStr: string) => dateStr ? dayjs.utc(dateStr).local().format("YYYY-MM-DD HH:mm:ss") : "";
export const localDate = (dateStr: string) => dateStr ? dayjs.utc(dateStr).local().format("YYYY-MM-DD") : "";
export const localYear = (dateStr: string) => dateStr ? dayjs.utc(dateStr).local().format("YYYY") : "";

export const utcDateTime = (dateStr: string) => dateStr ? dayjs.utc(dateStr).format("YYYY-MM-DD HH:mm") : "";
export const utcDate = (dateStr: string) => dateStr ? dayjs.utc(dateStr).format("YYYY-MM-DD") : "";
export const utcYear = (dateStr: string) => dateStr ? dayjs.utc(dateStr).format("YYYY") : "";

export const padZero = (number: string | number, size: number) => String(number).padStart(size, '0')
