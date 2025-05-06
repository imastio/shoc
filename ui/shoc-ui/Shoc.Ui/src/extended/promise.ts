export function delay(millisec: number) {
    return new Promise(resolve => {
        setTimeout(() => { resolve('') }, millisec);
    })
}