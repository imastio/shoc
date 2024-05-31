import { useLocation } from "react-router-dom";

export default function PageTitle({ title }) {
    const location = useLocation();

    useEffect(() => {
        document.title = title;
    }, [location, title]);

    return false;
};
