// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Biff_Records.ColumnArray
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Biff_Records;

[CLSCompliant(false)]
internal class ColumnArray : List<object>
{
  private const int DEF_DISTANCE_BETWEEN_COLUMNS = 720;
  private SinglePropertyModifierArray m_sprms;

  internal ColumnArray(SinglePropertyModifierArray sprms) => this.m_sprms = sprms;

  internal ColumnDescriptor AddColumn()
  {
    this.m_sprms.SetUShortValue(20491, (ushort) this.Count);
    SinglePropertyModifierRecord propertyModifierRecord1 = new SinglePropertyModifierRecord(61955);
    SinglePropertyModifierRecord propertyModifierRecord2 = new SinglePropertyModifierRecord(61956);
    propertyModifierRecord1.ByteArray = new byte[3];
    propertyModifierRecord2.ByteArray = new byte[3];
    propertyModifierRecord1.ByteArray[0] = (byte) this.Count;
    propertyModifierRecord2.ByteArray[0] = (byte) this.Count;
    this.m_sprms.Add(propertyModifierRecord1);
    this.m_sprms.Add(propertyModifierRecord2);
    ColumnDescriptor columnDescriptor = new ColumnDescriptor(propertyModifierRecord1, propertyModifierRecord2);
    this.Add((object) columnDescriptor);
    return columnDescriptor;
  }

  internal ColumnDescriptor AddEmptyColumn()
  {
    this.m_sprms.SetUShortValue(20491, (ushort) this.Count);
    ColumnDescriptor columnDescriptor = new ColumnDescriptor();
    this.Add((object) columnDescriptor);
    return columnDescriptor;
  }

  internal ColumnDescriptor this[int index] => (ColumnDescriptor) base[index];

  internal bool ColumnsEvenlySpaced
  {
    get => this.m_sprms.GetByte(12293, (byte) 1) == (byte) 1;
    set => this.m_sprms.SetIntValue(12293, value ? 1 : 0);
  }

  internal void Close() => this.m_sprms = (SinglePropertyModifierArray) null;

  internal void ReadColumnsProperties(bool isFromPropertyHash)
  {
    Dictionary<int, SinglePropertyModifierRecord> dictionary = new Dictionary<int, SinglePropertyModifierRecord>();
    if (this.m_sprms.Contain(12857))
    {
      if (isFromPropertyHash)
      {
        for (int index = 0; index < this.m_sprms.Modifiers.Count; ++index)
        {
          SinglePropertyModifierRecord modifier = this.m_sprms.Modifiers[index];
          int typedOptions = modifier.TypedOptions;
          if (modifier.TypedOptions != 12857)
          {
            if (modifier.TypedOptions == 20491)
            {
              dictionary.Add(index, modifier);
              this.m_sprms.Modifiers.Remove(modifier);
            }
          }
          else
            break;
        }
      }
      else
      {
        for (int index = this.m_sprms.Modifiers.Count - 1; index > 0; --index)
        {
          SinglePropertyModifierRecord modifier = this.m_sprms.Modifiers[index];
          int typedOptions = modifier.TypedOptions;
          if (modifier.TypedOptions != 12857)
          {
            if (modifier.TypedOptions == 20491)
            {
              dictionary.Add(index, modifier);
              this.m_sprms.Modifiers.Remove(modifier);
            }
          }
          else
            break;
        }
      }
    }
    this.ReadColumn();
    if (dictionary.Count <= 0)
      return;
    foreach (KeyValuePair<int, SinglePropertyModifierRecord> keyValuePair in dictionary)
    {
      if (this.m_sprms.Count <= keyValuePair.Key)
        this.m_sprms.Modifiers.Add(keyValuePair.Value);
      else
        this.m_sprms.Modifiers.Insert(keyValuePair.Key, keyValuePair.Value);
    }
    dictionary.Clear();
  }

  internal void ReadColumn()
  {
    int num1 = (int) this.m_sprms.GetUShort(20491, (ushort) 0) + 1;
    for (int index = 0; index < num1; ++index)
      this.Add((object) new ColumnDescriptor());
    if (this.ColumnsEvenlySpaced)
    {
      int num2 = (int) this.m_sprms.GetUShort(45087, (ushort) 0) - (int) this.m_sprms.GetUShort(45089, (ushort) 0) - (int) this.m_sprms.GetUShort(45090, (ushort) 0);
      ushort num3 = this.m_sprms.GetUShort(36876, (ushort) 720);
      int num4 = (num2 - (num1 - 1) * (int) num3) / num1;
      for (int index = 0; index < num1; ++index)
      {
        this[index].Width = (ushort) num4;
        this[index].Space = num3;
      }
    }
    else
    {
      List<ushort> ushortList1 = new List<ushort>();
      List<ushort> ushortList2 = new List<ushort>();
      for (int sprmIndex = 0; sprmIndex < this.m_sprms.Count; ++sprmIndex)
      {
        if (this.m_sprms.GetSprmByIndex(sprmIndex).TypedOptions.ToString() == 61955.ToString())
        {
          byte[] byteArray = this.m_sprms.GetSprmByIndex(sprmIndex).ByteArray;
          ushortList1.Add(BitConverter.ToUInt16(byteArray, 1));
        }
        else if (this.m_sprms.GetSprmByIndex(sprmIndex).TypedOptions.ToString() == 61956.ToString())
        {
          byte[] byteArray = this.m_sprms.GetSprmByIndex(sprmIndex).ByteArray;
          ushortList2.Add(BitConverter.ToUInt16(byteArray, 1));
        }
      }
      if (this.Count < 1)
        return;
      for (int index = 0; index < this.Count; ++index)
      {
        if (ushortList1.Count > index)
        {
          this[index].Width = ushortList1[index];
          if (index + 1 < this.Count)
            this[index].Space = ushortList2[index];
        }
      }
    }
  }
}
