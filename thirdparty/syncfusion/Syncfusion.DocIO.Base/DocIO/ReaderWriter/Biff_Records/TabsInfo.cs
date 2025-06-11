// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Biff_Records.TabsInfo
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Biff_Records;

[CLSCompliant(false)]
internal class TabsInfo
{
  private short[] m_tabPositions = new short[0];
  private short[] m_tabDeletePositions = new short[0];
  private TabDescriptor[] m_tabDescriptors = new TabDescriptor[0];
  private byte m_tabsCount;
  private byte[] m_data;
  private byte m_opcode;
  private int m_deleteOffset;

  internal byte TabCount => this.m_tabsCount;

  internal short[] TabPositions => this.m_tabPositions;

  internal short[] TabDeletePositions
  {
    get => this.m_tabDeletePositions;
    set => this.m_tabDeletePositions = value;
  }

  internal TabDescriptor[] Descriptors => this.m_tabDescriptors;

  internal TabsInfo(byte length)
  {
    this.m_tabsCount = length;
    this.m_tabPositions = new short[(int) length];
    this.m_tabDescriptors = new TabDescriptor[(int) length];
  }

  internal TabsInfo(SinglePropertyModifierArray sprms, int sprm) => this.Parse(sprms[sprm]);

  internal TabsInfo(SinglePropertyModifierRecord record) => this.Parse(record);

  internal void Save(SinglePropertyModifierArray sprms, int sprmOption)
  {
    bool flag = this.SaveInit();
    if (this.m_data == null)
      return;
    this.SaveDeletePositions();
    if (flag)
    {
      this.SavePositions();
      this.SaveDescriptors();
    }
    sprms.SetByteArrayValue(sprmOption, this.m_data);
  }

  private void Parse(SinglePropertyModifierRecord record)
  {
    this.m_data = record.ByteArray;
    if (this.m_data == null)
      return;
    this.ParseInit();
    this.ParseDeletePositions();
    this.ParsePositions();
    this.ParseDescriptors();
  }

  private void ParseInit()
  {
    this.m_opcode = this.m_data[0];
    this.m_deleteOffset = 0;
  }

  private void ParseDeletePositions()
  {
    if (this.m_opcode <= (byte) 0)
      return;
    this.m_deleteOffset = (int) this.m_opcode * 2;
    this.m_tabDeletePositions = new short[(int) this.m_opcode];
    if (this.m_data.Length <= this.m_deleteOffset)
      return;
    Buffer.BlockCopy((Array) this.m_data, 1, (Array) this.m_tabDeletePositions, 0, (int) this.m_opcode * 2);
  }

  private void ParsePositions()
  {
    if (this.m_data.Length > this.m_deleteOffset + 1)
    {
      this.m_tabsCount = this.m_data[this.m_deleteOffset + 1];
      this.m_tabPositions = new short[(int) this.m_tabsCount];
      this.m_tabDescriptors = new TabDescriptor[(int) this.m_tabsCount];
    }
    if (this.m_data.Length <= this.m_deleteOffset + (int) this.m_tabsCount * 2 + 1)
      return;
    Buffer.BlockCopy((Array) this.m_data, this.m_deleteOffset + 2, (Array) this.m_tabPositions, 0, (int) this.m_tabsCount * 2);
  }

  private void ParseDescriptors()
  {
    if (this.m_data.Length <= this.m_deleteOffset + ((int) this.m_tabsCount + 1) * 2)
      return;
    int num = ((int) this.m_tabsCount + 1) * 2;
    for (int index = 0; index < (int) this.m_tabsCount; ++index)
    {
      this.m_tabDescriptors[index] = new TabDescriptor(this.m_data[num + this.m_deleteOffset]);
      ++num;
    }
  }

  private bool SaveInit()
  {
    this.m_opcode = this.m_tabDeletePositions != null ? (byte) this.m_tabDeletePositions.Length : (byte) 0;
    this.m_deleteOffset = (int) this.m_opcode * 2;
    int length = ((int) this.m_opcode + 1) * 2 + (int) this.m_tabsCount * 3;
    if (length == 0)
      return false;
    this.m_data = new byte[length];
    return true;
  }

  private void SaveDeletePositions()
  {
    if (this.m_data.Length <= this.m_deleteOffset || this.m_opcode <= (byte) 0)
      return;
    this.m_data[0] = this.m_opcode;
    Buffer.BlockCopy((Array) this.m_tabDeletePositions, 0, (Array) this.m_data, 1, this.m_deleteOffset);
  }

  private void SavePositions()
  {
    if (this.m_data.Length <= this.m_deleteOffset + (int) this.m_tabsCount * 2 + 1)
      return;
    this.m_data[this.m_deleteOffset + 1] = this.m_tabsCount;
    Buffer.BlockCopy((Array) this.m_tabPositions, 0, (Array) this.m_data, this.m_deleteOffset + 2, (int) this.m_tabsCount * 2);
  }

  private void SaveDescriptors()
  {
    if (this.m_data.Length <= this.m_deleteOffset + (int) this.m_tabsCount * 3 + 1)
      return;
    int num = ((int) this.m_tabsCount + 1) * 2;
    for (int index = 0; index < (int) this.m_tabsCount; ++index)
    {
      this.m_data[this.m_deleteOffset + num] = this.m_tabDescriptors[index].Save();
      ++num;
    }
  }
}
