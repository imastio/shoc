import dayjs from "extended/time";

export const localDateTime = dateStr => dateStr ? dayjs.utc(dateStr).local().format("YYYY-MM-DD HH:mm") : "";
export const localDate = dateStr => dateStr ? dayjs.utc(dateStr).local().format("YYYY-MM-DD") : "";
export const localYear = dateStr => dateStr ? dayjs.utc(dateStr).local().format("YYYY") : "";

export const utcDateTime = dateStr => dateStr ? dayjs.utc(dateStr).format("YYYY-MM-DD HH:mm") : "";
export const utcDate = dateStr => dateStr ? dayjs.utc(dateStr).format("YYYY-MM-DD") : "";
export const utcYear = dateStr => dateStr ? dayjs.utc(dateStr).format("YYYY") : "";

export const formatCurrency = (currency) => {
    switch (currency?.toLowerCase()) {
        case "amd":
            return "֏";
        case "usd":
            return "$";
        case "rub":
        case "rur":
            return "₽";
        case "eur":
            return "€";
        default:
            return currency || "";
    }
}

export const padZero = (number, size) => String(number).padStart(size, '0')

export const toSlug = (text) => text.toLowerCase().replace(/[^\p{L}\p{N}\s]/gu, '').replace(/\s+/gu, ' ').trim().replace(/\s+/gu, '-');
