import net, { AddressInfo } from 'net';

export async function getFreePort() {
    return new Promise(res => {
        const srv = net.createServer();
        srv.listen(0, () => {
            const port = (srv.address() as AddressInfo).port
            srv.close(() => res(port))
        });
    })
}