import dayjs from "@/extended/time";

export const localDateTime = (dateStr: string) => dateStr ? dayjs.utc(dateStr).local().format("YYYY-MM-DD HH:mm") : "";
export const localDateTimeWithSec = (dateStr: string) => dateStr ? dayjs.utc(dateStr).local().format("YYYY-MM-DD HH:mm:ss") : "";
export const localDate = (dateStr: string) => dateStr ? dayjs.utc(dateStr).local().format("YYYY-MM-DD") : "";
export const localYear = (dateStr: string) => dateStr ? dayjs.utc(dateStr).local().format("YYYY") : "";

export const utcDateTime = (dateStr: string) => dateStr ? dayjs.utc(dateStr).format("YYYY-MM-DD HH:mm") : "";
export const utcDate = (dateStr: string) => dateStr ? dayjs.utc(dateStr).format("YYYY-MM-DD") : "";
export const utcYear = (dateStr: string) => dateStr ? dayjs.utc(dateStr).format("YYYY") : "";

export const padZero = (number: string | number, size: number) => String(number).padStart(size, '0')

export const nowUtc = () => dayjs.utc().toDate();

export function durationBetween(fromStr: string, toStr?: string) {

    const from = dayjs.utc(fromStr).toDate();
    const to = toStr ? dayjs.utc(toStr).toDate() : nowUtc();


    let totalSeconds = Math.floor((to.getTime() - from.getTime()) / 1000);
    const sign = totalSeconds < 0 ? "-" : "";
    totalSeconds = Math.abs(totalSeconds);

    const days = Math.floor(totalSeconds / 86400);
    const hours = Math.floor((totalSeconds % 86400) / 3600);
    const minutes = Math.floor((totalSeconds % 3600) / 60);
    const seconds = totalSeconds % 60;

    const timeString = `${String(hours).padStart(2, '0')}:${String(minutes).padStart(2, '0')}:${String(seconds).padStart(2, '0')}`;
    
    return days > 0 ? `${sign}${days}.${timeString}` : `${sign}${timeString}`
}