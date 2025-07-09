import { saveAs as realSaveAs } from 'file-saver';

let saveAsImpl = realSaveAs;

export function saveAs(...args: Parameters<typeof realSaveAs>) {
  return saveAsImpl(...args);
}

// Accept any function for testing purposes
export function setSaveAs(fn: (...args: any[]) => any) {
  // @ts-ignore
  saveAsImpl = fn;
}
