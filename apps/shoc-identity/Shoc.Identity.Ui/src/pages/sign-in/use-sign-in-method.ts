import { useSearchParams } from "react-router-dom";
import { SignInMethods } from "./types";

export default function useSignInMethod(): SignInMethods {
    const [searchParams] = useSearchParams();

    switch(searchParams.get('method')){
        case 'magic-link':
            return 'magic-link';
        default:
            return 'password';
    }
}