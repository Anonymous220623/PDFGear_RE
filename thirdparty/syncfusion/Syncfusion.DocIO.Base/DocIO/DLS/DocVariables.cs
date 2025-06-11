// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.DocVariables
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class DocVariables
{
  private Dictionary<string, string> m_variables;

  public string this[string name]
  {
    get => this.m_variables.ContainsKey(name) ? this.m_variables[name] : (string) null;
    set => this.m_variables[name] = value;
  }

  public int Count => this.m_variables.Count;

  internal Dictionary<string, string> Items => this.m_variables;

  public DocVariables() => this.m_variables = new Dictionary<string, string>();

  public void Add(string name, string value)
  {
    if (value == null)
      value = string.Empty;
    this.m_variables.Add(name, value);
  }

  public string GetNameByIndex(int index)
  {
    this.CheckIndex(index);
    return this.FindItem(index, true);
  }

  public string GetValueByIndex(int index)
  {
    this.CheckIndex(index);
    return this.FindItem(index, false);
  }

  public void Remove(string name) => this.m_variables.Remove(name);

  internal void Close()
  {
    if (this.m_variables == null)
      return;
    this.m_variables.Clear();
    this.m_variables = (Dictionary<string, string>) null;
  }

  internal void UpdateVariables(byte[] variables)
  {
    BinaryReader binaryReader = new BinaryReader((Stream) new MemoryStream(variables), Encoding.Unicode);
    int num1 = (int) binaryReader.ReadInt16();
    int length = (int) binaryReader.ReadInt16();
    int num2 = (int) binaryReader.ReadInt16();
    string[] strArray = new string[length];
    for (int index = 0; index < length; ++index)
    {
      int count = (int) binaryReader.ReadInt16();
      char[] chArray = binaryReader.ReadChars(count);
      strArray[index] = new string(chArray);
      binaryReader.ReadInt32();
    }
    for (int index = 0; index < length; ++index)
    {
      int count = (int) binaryReader.ReadUInt16();
      char[] chArray = binaryReader.ReadChars(count);
      this.m_variables.Add(strArray[index], new string(chArray));
    }
  }

  internal byte[] ToByteArray()
  {
    if (this.m_variables.Count == 0)
      return (byte[]) null;
    MemoryStream output = new MemoryStream();
    BinaryWriter binaryWriter = new BinaryWriter((Stream) output, Encoding.Unicode);
    string[] strArray1 = new string[this.m_variables.Count];
    string[] strArray2 = new string[this.m_variables.Count];
    int index1 = 0;
    SortedDictionary<string, string> sortedDictionary = new SortedDictionary<string, string>();
    foreach (string key in this.m_variables.Keys)
      sortedDictionary.Add(key, this.m_variables[key]);
    foreach (string key in (IEnumerable<string>) sortedDictionary.Keys)
    {
      strArray1[index1] = key;
      strArray2[index1] = sortedDictionary[key];
      ++index1;
    }
    binaryWriter.Write(byte.MaxValue);
    binaryWriter.Write(byte.MaxValue);
    short count = (short) this.m_variables.Count;
    binaryWriter.Write(count);
    binaryWriter.Write((short) 4);
    for (int index2 = 0; index2 < (int) count; ++index2)
    {
      binaryWriter.Write((short) strArray1[index2].Length);
      binaryWriter.Write(strArray1[index2].ToCharArray());
      binaryWriter.Write(int.MaxValue);
    }
    for (int index3 = 0; index3 < (int) count; ++index3)
    {
      binaryWriter.Write((short) strArray2[index3].Length);
      binaryWriter.Write(strArray2[index3].ToCharArray());
    }
    return output.ToArray();
  }

  private string FindItem(int index, bool returnName)
  {
    IDictionaryEnumerator enumerator = (IDictionaryEnumerator) this.m_variables.GetEnumerator();
    for (int index1 = 0; index1 <= index; ++index1)
      enumerator.MoveNext();
    return returnName ? (string) enumerator.Entry.Key : (string) enumerator.Entry.Value;
  }

  private void CheckIndex(int index)
  {
    if (index < 0 || index >= this.m_variables.Count)
      throw new ArgumentOutOfRangeException(nameof (index), "Index must be larger than 0 and lower than number of variables in the document");
  }
}
