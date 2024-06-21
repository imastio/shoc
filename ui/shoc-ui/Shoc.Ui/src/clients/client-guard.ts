export default async function clientGuard(action: Function){
    const result = await action();
    return result.data;
}