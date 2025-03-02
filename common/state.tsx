import {NativeModules} from 'react-native';

export interface State {
  setData: (key: string, value: string) => string;
  getData: (key: string) => Promise<string>;
  clearKey: (key: string) => void;
  clearData: () => void;
}

const nativeStateModule: State = NativeModules.MMKVModule;

export function setData<T>(key: string, value: T) {
  const stringify = JSON.stringify(value);
  nativeStateModule.setData(key, stringify);
}

export async function getData<T>(key: string) {
  const value = await nativeStateModule.getData(key);
  if (!value || value === '') return null;
  return JSON.parse(value);
}

export function clearKey(key: string) {
  nativeStateModule.clearKey(key);
}

export function clearData() {
  nativeStateModule.clearData();
}


//
// Example JSON data to test the methods
export const json1 = '{"name": "John", "age": "30"}';
export const json2 = '{"city": "New York", "country": "USA"}';
export const json3 = '{"product": "Laptop", "price": "1000"}';
export const json4 = '{"user": "Jane", "email": "jane@example.com"}';
