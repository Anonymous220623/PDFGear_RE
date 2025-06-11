// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.CloneUtils
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Interfaces;
using Syncfusion.OfficeChart.Parser.Biff_Records.Formula;
using Syncfusion.OfficeChart.Parser.Biff_Records.MsoDrawing;
using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records;

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

  [CLSCompliant(false)]
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

  public static List<T> CloneCloneable<T>(List<T> toClone)
  {
    if (toClone == null)
      return (List<T>) null;
    int count = toClone.Count;
    List<T> objList = new List<T>(count);
    for (int index = 0; index < count; ++index)
    {
      T obj = toClone[index] is ICloneable cloneable ? (T) cloneable.Clone() : toClone[index];
      objList.Add(obj);
    }
    return objList;
  }

  public static object CloneCloneable(ICloneable toClone) => toClone?.Clone();

  public static List<BiffRecordRaw> CloneCloneable(List<BiffRecordRaw> toClone)
  {
    if (toClone == null)
      return (List<BiffRecordRaw>) null;
    int count = toClone.Count;
    List<BiffRecordRaw> biffRecordRawList = new List<BiffRecordRaw>(count);
    for (int index = 0; index < count; ++index)
    {
      ICloneable toClone1 = (ICloneable) toClone[index];
      biffRecordRawList.Add((BiffRecordRaw) CloneUtils.CloneCloneable(toClone1));
    }
    return biffRecordRawList;
  }

  public static List<TextWithFormat> CloneCloneable(List<TextWithFormat> toClone)
  {
    if (toClone == null)
      return (List<TextWithFormat>) null;
    int count = toClone.Count;
    List<TextWithFormat> textWithFormatList = new List<TextWithFormat>(count);
    for (int index = 0; index < count; ++index)
    {
      TextWithFormat textWithFormat = toClone[index].TypedClone();
      textWithFormatList.Add(textWithFormat);
    }
    return textWithFormatList;
  }

  public static SortedList<int, int> CloneSortedList(SortedList<int, int> toClone)
  {
    if (toClone == null)
      return (SortedList<int, int>) null;
    int count = toClone.Count;
    SortedList<int, int> sortedList = new SortedList<int, int>(count);
    IList<int> keys = toClone.Keys;
    IList<int> values = toClone.Values;
    for (int index = 0; index < count; ++index)
      sortedList.Add(keys[index], values[index]);
    return sortedList;
  }

  public static SortedList<TKey, TValue> CloneCloneable<TKey, TValue>(SortedList<TKey, TValue> list) where TKey : IComparable
  {
    SortedList<TKey, TValue> sortedList = list != null ? new SortedList<TKey, TValue>(list.Count) : throw new ArgumentNullException(nameof (list));
    foreach (KeyValuePair<TKey, TValue> keyValuePair in list)
    {
      TValue obj = keyValuePair.Value;
      if (obj is ICloneable cloneable1)
        obj = (TValue) cloneable1.Clone();
      TKey key = keyValuePair.Key;
      if (key is ICloneable cloneable2)
        key = (TKey) cloneable2.Clone();
      sortedList.Add(key, obj);
    }
    return sortedList;
  }

  public static List<T> CloneCloneable<T>(IList<T> toClone, object parent)
  {
    if (toClone == null)
      return (List<T>) null;
    int count = toClone.Count;
    List<T> objList = new List<T>(count);
    for (int index = 0; index < count; ++index)
    {
      ICloneParent toClone1 = (ICloneParent) (object) toClone[index];
      objList.Add((T) CloneUtils.CloneCloneable(toClone1, parent));
    }
    return objList;
  }

  public static object CloneCloneable(ICloneParent toClone, object parent)
  {
    return toClone?.Clone(parent);
  }

  [CLSCompliant(false)]
  public static object CloneMsoBase(MsoBase toClone, MsoBase parent)
  {
    return toClone == null ? (object) null : (object) toClone.Clone(parent);
  }

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

  public static Ptg[] ClonePtgArray(Ptg[] arrToClone)
  {
    if (arrToClone == null)
      return (Ptg[]) null;
    int length = arrToClone.Length;
    Ptg[] ptgArray = new Ptg[length];
    for (int index = 0; index < length; ++index)
      ptgArray[index] = (Ptg) arrToClone[index].Clone();
    return ptgArray;
  }

  [CLSCompliant(false)]
  public static ColumnInfoRecord[] CloneArray(ColumnInfoRecord[] arrToClone)
  {
    if (arrToClone == null)
      return (ColumnInfoRecord[]) null;
    int length = arrToClone.Length;
    ColumnInfoRecord[] columnInfoRecordArray = new ColumnInfoRecord[length];
    for (int index = 0; index < length; ++index)
      columnInfoRecordArray[index] = (ColumnInfoRecord) CloneUtils.CloneCloneable((ICloneable) arrToClone[index]);
    return columnInfoRecordArray;
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

  public static Dictionary<TextWithFormat, int> CloneHash(Dictionary<TextWithFormat, int> hash)
  {
    if (hash == null)
      throw new ArgumentNullException(nameof (hash));
    Dictionary<TextWithFormat, int> dictionary = new Dictionary<TextWithFormat, int>();
    foreach (KeyValuePair<TextWithFormat, int> keyValuePair in hash)
    {
      TextWithFormat key = keyValuePair.Key.TypedClone();
      dictionary.Add(key, keyValuePair.Value);
    }
    return dictionary;
  }

  public static Dictionary<object, int> CloneHash(Dictionary<object, int> hash)
  {
    if (hash == null)
      throw new ArgumentNullException(nameof (hash));
    Dictionary<object, int> dictionary = new Dictionary<object, int>();
    foreach (KeyValuePair<object, int> keyValuePair in hash)
    {
      object key = keyValuePair.Key;
      if (key is TextWithFormat textWithFormat)
        key = textWithFormat.Clone();
      dictionary.Add(key, keyValuePair.Value);
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

  public static Dictionary<TKey, TValue> CloneHash<TKey, TValue>(
    Dictionary<TKey, TValue> hash,
    object parent)
  {
    Dictionary<TKey, TValue> dictionary = hash != null ? new Dictionary<TKey, TValue>(hash.Count) : throw new ArgumentNullException(nameof (hash));
    foreach (KeyValuePair<TKey, TValue> keyValuePair in hash)
    {
      TValue obj = keyValuePair.Value;
      if (obj is ICloneParent cloneParent1)
        obj = (TValue) cloneParent1.Clone(parent);
      TKey key = keyValuePair.Key;
      if (key is ICloneParent cloneParent2)
        key = (TKey) cloneParent2.Clone(parent);
      dictionary.Add(key, obj);
    }
    return dictionary;
  }

  public static Stream CloneStream(Stream stream)
  {
    if (stream == null)
      return (Stream) null;
    long position = stream.Position;
    MemoryStream memoryStream = new MemoryStream((int) stream.Length);
    stream.Position = 0L;
    byte[] buffer = new byte[32768 /*0x8000*/];
    int count;
    while ((count = stream.Read(buffer, 0, 32768 /*0x8000*/)) != 0)
      memoryStream.Write(buffer, 0, count);
    stream.Position = position;
    memoryStream.Position = position;
    return (Stream) memoryStream;
  }

  public static bool[] CloneBoolArray(bool[] sourceArray)
  {
    bool[] dst = (bool[]) null;
    if (sourceArray != null)
    {
      int length = sourceArray.Length;
      dst = new bool[length];
      if (length > 0)
        Buffer.BlockCopy((Array) sourceArray, 0, (Array) dst, 0, length);
    }
    return dst;
  }
}
