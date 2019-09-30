/*
 COPYRIGHT: https://github.com/azixMcAze/Unity-SerializableDictionary
 */
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class TriggerIntDictionary : SerializableDictionary<Trigger, int> { }
[Serializable]
public class TriggerBoolDictionary : SerializableDictionary<Trigger, bool> { }
[Serializable]
public class StringObjectDictionary : SerializableDictionary<string, System.Object> { }
/*
[Serializable]
public class StringStringDictionary : SerializableDictionary<string, string> {}

[Serializable]
public class ObjectColorDictionary : SerializableDictionary<UnityEngine.Object, Color> {}

[Serializable]
public class ColorArrayStorage : SerializableDictionary.Storage<Color[]> {}

[Serializable]
public class StringColorArrayDictionary : SerializableDictionary<string, Color[], ColorArrayStorage> {}

[Serializable]
public class MyClass
{
    public int i;
    public string str;
}

[Serializable]
public class QuaternionMyClassDictionary : SerializableDictionary<Quaternion, MyClass> {}

#if NET_4_6 || NET_STANDARD_2_0
[Serializable]
public class StringHashSet : SerializableHashSet<string> {}
#endif
*/
