/**
 * Sample React Native App
 * https://github.com/facebook/react-native
 *
 * @format
 */

import React, {useCallback, useEffect, useState} from 'react';
import {NativeModules, StyleSheet, Text, TextInput} from 'react-native';
import {
  clearData,
  clearKey,
  getData,
  json1,
  json2,
  json3,
  json4,
  setData,
} from './common/state';
import {CustomTextInput} from './common/text-input';

function App(): React.JSX.Element {
  const [state, setState] = useState<any>();

  useEffect(() => {
    // const obj1 = JSON.parse(json1);
    // setData('user1', obj1.name);
    // setData('age1', obj1.age);
    // console.debug(getData('user1')); // Output: John
    // console.debug(getData('age1')); // Output: 30
    // // Test clearKey
    // clearKey('user1000'); // Output: Key: user1 has been cleared
    // getData('user1000'); // Output: Key: user1 has been cleared
    // console.debug(getData('user1')); // Output: Key: user1 not found
    // // Test clearData
    // const obj2 = JSON.parse(json2);
    // setData('city1', obj2.city);
    // setData('country1', obj2.country);
    // clearData(); // Output: All data has been cleared
    // console.debug(getData('city1')); // Output: Key: city1 not found
    // console.debug(getData('country1')); // Output: Key: country1 not found
    // // Test with different JSON
    // const obj3 = JSON.parse(json3);
    // setData('product1', obj3.product);
    // setData('price1', obj3.price);
    // console.debug(getData('product1')); // Output: Laptop
    // console.debug(getData('price1')); // Output: 1000
    // const obj4 = JSON.parse(json4);
    // setData('user2', obj4.user);
    // setData('email2', obj4.email);
    // console.debug(getData('user2')); // Output: Jane
    // console.debug(getData('email2')); // Output: jane@example.com
  }, []);

  return (
    // <TextInput
    //   value={state}
    //   onChangeText={text => setState(text)}
    //   onSubmitEditing={setNativeData}
    // />
    // <Text>{state.company}</Text>
    <>
      <CustomTextInput />
    </>
  );
}

const styles = StyleSheet.create({
  sectionContainer: {
    marginTop: 32,
    paddingHorizontal: 24,
  },
  sectionTitle: {
    fontSize: 24,
    fontWeight: '600',
  },
  sectionDescription: {
    marginTop: 8,
    fontSize: 18,
    fontWeight: '400',
  },
  highlight: {
    fontWeight: '700',
  },
});

export default App;
