
import axios from 'axios';
import dotenv from '@dotenvx/dotenvx';
import https from 'https'

export function startup() {

    dotenv.config({convention: 'nextjs', quiet: true});

    if (process.env.ALLOW_INSECURE === 'yes') {
        const httpsAgent = new https.Agent({
            rejectUnauthorized: false,
        })
        axios.defaults.httpsAgent = httpsAgent
    }

}