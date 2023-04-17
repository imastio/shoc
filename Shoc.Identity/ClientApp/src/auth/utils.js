import { InMemoryWebStorage } from 'oidc-client-ts';

const resolveStorage = (type) => {
    switch (type) {
    case 'local':
        return localStorage;
    case 'session':
        return sessionStorage;
    case 'in-memory':
        return new InMemoryWebStorage();
    default:
        return sessionStorage;
    }
};

export default resolveStorage;
