
import axios from 'axios';
import dotenv from '@dotenvx/dotenvx';
import https from 'https'

const ALLOW_INSECURE_ALWAYS = true

export function startup() {

    dotenv.config({convention: 'nextjs', quiet: true});

    if (process.env.ALLOW_INSECURE === 'yes' || ALLOW_INSECURE_ALWAYS) {
        const httpsAgent = new https.Agent({
            rejectUnauthorized: false,
        })
        axios.defaults.httpsAgent = httpsAgent
    }

}