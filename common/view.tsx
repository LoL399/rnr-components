import React, {useEffect, useRef, forwardRef, Children, useState} from 'react';
import {NativeEventEmitter, NativeModules, View} from 'react-native';

// Forwarded component that takes a ref
export const CustomView = forwardRef(({children, item = [], id}: any, ref) => {
  const localRef = useRef<View>(null);
  useEffect(() => {
    // Combine both refs (the forwarded ref and the internal ref)
    if (!localRef.current) return;
    if (!item || !(item as number[]).length) return;
    NativeModules.Flyout_Item.SetUpCommon(
      (localRef.current as any)._nativeTag,
      item,
      id
    );

    return () => {
      console.log('Component unmounted');
    };
  }, [localRef]); // This effect will run when the ref changes

  return <View ref={localRef}>{children}</View>;
});
