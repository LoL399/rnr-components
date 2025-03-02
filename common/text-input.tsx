import {useEffect, useRef, useState} from 'react';
import {NativeModules, Text, TextInput, TextInputProps} from 'react-native';

export const CustomTextInput = ({...prop}: TextInputProps) => {
  const ref = useRef(null);
  const [value, setValue] = useState('');
  const [final, setFinale] = useState('');

  useEffect(() => {
    console.log({ref});
    if (ref.current) NativeModules.Input.Init((ref.current as any)._nativeTag);
  }, []);

  return (
    <>
      <TextInput
        multiline={true}
        ref={ref}
        value={value}
        onChangeText={setValue}
        onSubmitEditing={() => setFinale(value)}
      />
      <Text>{final}</Text>
    </>
  );
};
