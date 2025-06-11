// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.CloneUtils
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Compression.Zip;
using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.Presentation;

internal sealed class CloneUtils
{
  public static int[] CloneIntArray(int[] array)
  {
    if (array == null)
      return (int[]) null;
    int length = array.Length;
    int[] numArray = new int[length];
    for (int index = 0; index < length; ++index)
      numArray[index] = array[index];
    return numArray;
  }

  public static ushort[] CloneUshortArray(ushort[] array)
  {
    if (array == null)
      return (ushort[]) null;
    int length = array.Length;
    ushort[] numArray = new ushort[length];
    for (int index = 0; index < length; ++index)
      numArray[index] = array[index];
    return numArray;
  }

  public static string[] CloneStringArray(string[] array)
  {
    if (array == null)
      return (string[]) null;
    int length = array.Length;
    string[] strArray = new string[length];
    for (int index = 0; index < length; ++index)
      strArray[index] = array[index];
    return strArray;
  }

  public static object[] CloneArray(object[] array)
  {
    if (array == null)
      return (object[]) null;
    int length = array.Length;
    object[] objArray = new object[length];
    for (int index = 0; index < length; ++index)
    {
      object obj = array[index];
      if (obj is ICloneable cloneable)
        obj = cloneable.Clone();
      objArray[index] = obj;
    }
    return objArray;
  }

  public static object CloneCloneable(ICloneable toClone) => toClone?.Clone();

  public static byte[] CloneByteArray(byte[] arr)
  {
    if (arr == null)
      return (byte[]) null;
    int length = arr.Length;
    byte[] numArray = new byte[length];
    for (int index = 0; index < length; ++index)
      numArray[index] = arr[index];
    return numArray;
  }

  public static Dictionary<TKey, TValue> CloneHash<TKey, TValue>(Dictionary<TKey, TValue> hash)
  {
    Dictionary<TKey, TValue> dictionary = hash != null ? new Dictionary<TKey, TValue>(hash.Count) : throw new ArgumentNullException(nameof (hash));
    foreach (KeyValuePair<TKey, TValue> keyValuePair in hash)
    {
      TValue obj = keyValuePair.Value;
      if (obj is ICloneable cloneable1)
        obj = (TValue) cloneable1.Clone();
      TKey key = keyValuePair.Key;
      if (key is ICloneable cloneable2)
        key = (TKey) cloneable2.Clone();
      dictionary.Add(key, obj);
    }
    return dictionary;
  }

  public static Dictionary<int, int> CloneHash(Dictionary<int, int> hash)
  {
    if (hash == null)
      throw new ArgumentNullException(nameof (hash));
    Dictionary<int, int> dictionary = new Dictionary<int, int>();
    foreach (KeyValuePair<int, int> keyValuePair in hash)
      dictionary.Add(keyValuePair.Key, keyValuePair.Value);
    return dictionary;
  }

  public static Stream CloneStream(Stream stream) => ZipArchiveItem.CloneStream(stream);
}
