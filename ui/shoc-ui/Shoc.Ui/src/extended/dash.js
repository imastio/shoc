export function debounce(func, timeout = 300) {
  let timer;
  return (...args) => {
    clearTimeout(timer);
    timer = setTimeout(() => { func.apply(this, args); }, timeout);
  };
}

export function sleeper(ms) {
    return new Promise(resolve => setTimeout(() => resolve(), ms));
}
