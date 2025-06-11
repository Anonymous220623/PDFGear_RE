// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.WordFKPData
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.ReaderWriter.Biff_Records;
using Syncfusion.DocIO.ReaderWriter.Biff_Records.Structures;
using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter;

[CLSCompliant(false)]
internal class WordFKPData
{
  private const int DEF_COUNTRUN_SIZE = 1;
  private Fib m_fib;
  private WPTablesData m_tables;
  private List<uint> m_papxPositions = new List<uint>();
  private List<ParagraphExceptionInDiskPage> m_papxProps = new List<ParagraphExceptionInDiskPage>();
  private List<uint> m_chpxPositions = new List<uint>();
  private List<CharacterPropertyException> m_chpxProps = new List<CharacterPropertyException>();
  private List<int> m_sepxPositions = new List<int>();
  private List<SectionPropertyException> m_sepxProps = new List<SectionPropertyException>();
  private FKPStructure[] m_chpxFKPs = new FKPStructure[0];
  private FKPStructure[] m_papxFKPs = new FKPStructure[0];
  private ParagraphPropertiesPage[] m_papxPages;
  private CharacterPropertiesPage[] m_chpxPages;
  private SectionPropertyException[] m_secProperties;
  private long m_lastSepxPosition;

  internal WordFKPData(Fib fib, WPTablesData tables)
  {
    this.m_fib = fib;
    this.m_tables = tables;
  }

  internal Fib Fib => this.m_fib;

  internal long EndOfSepx => this.m_lastSepxPosition;

  internal int SepxAddedCount => this.m_sepxProps.Count;

  internal WPTablesData Tables => this.m_tables;

  internal SectionPropertyException GetSepx(int index) => this.m_secProperties[index];

  internal void AddChpxProperties(uint pos, CharacterPropertyException chpx)
  {
    if (pos < 0U)
      throw new ArgumentOutOfRangeException(nameof (pos));
    if (chpx == null)
      chpx = new CharacterPropertyException();
    this.m_chpxPositions.Add(pos);
    this.m_chpxProps.Add(chpx);
  }

  internal void AddPapxProperties(
    uint pos,
    ParagraphExceptionInDiskPage papx,
    MemoryStream dataStream)
  {
    if (pos < 0U)
      throw new ArgumentOutOfRangeException(nameof (pos));
    if (papx == null)
      papx = new ParagraphExceptionInDiskPage();
    this.m_papxPositions.Add(pos);
    if (papx.Length < 485)
    {
      this.m_papxProps.Add(papx);
    }
    else
    {
      int position = (int) dataStream.Position;
      byte[] bytes = BitConverter.GetBytes((short) papx.PropertyModifiers.Length);
      dataStream.Write(bytes, 0, bytes.Length);
      papx.SaveHugePapx((Stream) dataStream);
      papx = new ParagraphExceptionInDiskPage();
      papx.PropertyModifiers.SetIntValue(26182, position);
      this.m_papxProps.Add(papx);
    }
  }

  internal void AddSepxProperties(int pos, SectionPropertyException sepx)
  {
    this.m_sepxPositions.Add(pos);
    this.m_sepxProps.Add(sepx);
  }

  internal void Read(MemoryStream stream)
  {
    this.m_chpxFKPs = this.ReadFKPs(stream, this.m_tables.CHPXBinaryTable);
    this.m_chpxPages = new CharacterPropertiesPage[this.m_chpxFKPs.Length];
    this.m_papxFKPs = this.ReadFKPs(stream, this.m_tables.PAPXBinaryTable);
    this.m_papxPages = new ParagraphPropertiesPage[this.m_papxFKPs.Length];
    SectionDescriptor[] descriptors = this.m_tables.SectionsTable.Descriptors;
    this.m_secProperties = new SectionPropertyException[descriptors.Length];
    int index = 0;
    for (int length = descriptors.Length; index < length; ++index)
    {
      uint sepxPosition = descriptors[index].SepxPosition;
      if (sepxPosition != uint.MaxValue)
      {
        stream.Position = (long) sepxPosition;
        this.m_secProperties[index] = new SectionPropertyException((Stream) stream);
      }
      else
        this.m_secProperties[index] = new SectionPropertyException(true);
    }
    this.m_lastSepxPosition = stream.Position;
  }

  internal void Write(Stream stream)
  {
    this.WriteChpx(stream);
    this.WritePapx(stream);
    this.WriteSepx(stream);
  }

  internal ParagraphPropertiesPage GetPapxPage(int i)
  {
    if (this.m_papxPages[i] == null)
      this.m_papxPages[i] = new ParagraphPropertiesPage(this.m_papxFKPs[i]);
    return this.m_papxPages[i];
  }

  internal CharacterPropertiesPage GetChpxPage(int i)
  {
    if (this.m_chpxPages[i] == null)
      this.m_chpxPages[i] = new CharacterPropertiesPage(this.m_chpxFKPs[i]);
    return this.m_chpxPages[i];
  }

  internal void CloneAndAddLastPapx(uint pos)
  {
    this.m_papxProps[this.m_papxProps.Count - 1] = this.m_papxProps[this.m_papxPositions.IndexOf(pos)];
  }

  internal void CloneAndAddLastChpx(uint pos)
  {
    this.m_chpxProps[this.m_chpxProps.Count - 1] = this.m_chpxProps[this.m_chpxPositions.IndexOf(pos)];
  }

  internal void Close()
  {
    this.m_fib = (Fib) null;
    if (this.m_tables != null)
    {
      this.m_tables.Close();
      this.m_tables = (WPTablesData) null;
    }
    if (this.m_papxPositions != null)
    {
      this.m_papxPositions.Clear();
      this.m_papxPositions = (List<uint>) null;
    }
    if (this.m_papxProps != null)
    {
      this.m_papxProps.Clear();
      this.m_papxProps = (List<ParagraphExceptionInDiskPage>) null;
    }
    if (this.m_chpxPositions != null)
    {
      this.m_chpxPositions.Clear();
      this.m_chpxPositions = (List<uint>) null;
    }
    if (this.m_chpxProps != null)
    {
      this.m_chpxProps.Clear();
      this.m_chpxProps = (List<CharacterPropertyException>) null;
    }
    if (this.m_sepxPositions != null)
    {
      this.m_sepxPositions.Clear();
      this.m_sepxPositions = (List<int>) null;
    }
    if (this.m_sepxProps != null)
    {
      this.m_sepxProps.Clear();
      this.m_sepxProps = (List<SectionPropertyException>) null;
    }
    if (this.m_chpxFKPs != null)
      this.m_chpxFKPs = (FKPStructure[]) null;
    if (this.m_papxFKPs != null)
      this.m_papxFKPs = (FKPStructure[]) null;
    if (this.m_papxPages != null)
      this.m_papxPages = (ParagraphPropertiesPage[]) null;
    if (this.m_chpxPages != null)
      this.m_chpxPages = (CharacterPropertiesPage[]) null;
    if (this.m_secProperties == null)
      return;
    this.m_secProperties = (SectionPropertyException[]) null;
  }

  internal FKPStructure[] ReadFKPs(MemoryStream stream, BinaryTable table)
  {
    int length = table.Entries.Length;
    FKPStructure[] fkpStructureArray = new FKPStructure[length];
    for (int index = 0; index < length; ++index)
    {
      int num = table.Entries[index].Value;
      stream.Position = (long) (num * 512 /*0x0200*/);
      fkpStructureArray[index] = new FKPStructure((Stream) stream);
    }
    return fkpStructureArray;
  }

  private void WritePapx(Stream stream)
  {
    int count = this.m_papxPositions.Count;
    int papxIndex = 0;
    uint num = this.m_fib.BaseReserved5;
    BinaryWriter writer = new BinaryWriter(stream);
    while (papxIndex < count)
    {
      ParagraphPropertiesPage page = new ParagraphPropertiesPage();
      papxIndex = this.FillPapxPage(page, num, papxIndex);
      int papxPos = this.AlignByDiskPage(stream);
      this.m_tables.AddPapxRecord(num, papxPos);
      page.SaveToStream(writer, stream);
      num = this.m_papxPositions[papxIndex - 1];
    }
  }

  private int FillPapxPage(ParagraphPropertiesPage page, uint pagePos, int papxIndex)
  {
    page.RunsCount = this.GetPapxCountPerPage(papxIndex);
    if (page.RunsCount == 0)
      throw new Exception(string.Empty);
    page.FileCharPos[0] = pagePos;
    int index = 0;
    for (int runsCount = page.RunsCount; index < runsCount; ++index)
    {
      page.ParagraphProperties[index] = this.m_papxProps[papxIndex];
      page.FileCharPos[index + 1] = this.m_papxPositions[papxIndex];
      ++papxIndex;
    }
    return papxIndex;
  }

  private int GetPapxCountPerPage(int papxIndex)
  {
    int num = 0;
    int count = this.m_papxPositions.Count;
    int index;
    for (index = papxIndex; index < count; ++index)
    {
      ParagraphExceptionInDiskPage papxProp = this.m_papxProps[index];
      if (papxProp.Length + 13 + 8 + num <= 511 /*0x01FF*/)
        num += papxProp.Length + 13 + 4;
      else
        break;
    }
    return index - papxIndex;
  }

  private void WriteChpx(Stream stream)
  {
    int count = this.m_chpxPositions.Count;
    int chpxIndex = 0;
    uint num = this.m_fib.BaseReserved5;
    BinaryWriter writer = new BinaryWriter(stream);
    while (chpxIndex < count)
    {
      CharacterPropertiesPage page = new CharacterPropertiesPage();
      chpxIndex = this.FillChpxPage(page, num, chpxIndex);
      int chpxPos = this.AlignByDiskPage(stream);
      this.m_tables.AddChpxRecord(num, chpxPos);
      page.SaveToStream(writer, stream);
      num = this.m_chpxPositions[chpxIndex - 1];
    }
  }

  private int FillChpxPage(CharacterPropertiesPage page, uint pagePos, int chpxIndex)
  {
    page.RunsCount = this.GetChpxCountPerPage(chpxIndex);
    page.FileCharPos[0] = pagePos;
    int index = 0;
    for (int runsCount = page.RunsCount; index < runsCount; ++index)
    {
      page.CharacterProperties[index] = this.m_chpxProps[chpxIndex];
      page.FileCharPos[index + 1] = this.m_chpxPositions[chpxIndex];
      ++chpxIndex;
    }
    return chpxIndex;
  }

  private int GetChpxCountPerPage(int chpxIndex)
  {
    int num1 = 0;
    int count = this.m_chpxPositions.Count;
    int num2;
    for (num2 = chpxIndex; num2 < count; ++num2)
    {
      CharacterPropertyException chpxProp = this.m_chpxProps[num2];
      int num3 = chpxProp.Length % 2 != 0 ? chpxProp.Length + 1 : chpxProp.Length;
      if (this.IsChpxRepeats(chpxIndex, num2))
      {
        if (num1 + 8 + 1 < 511 /*0x01FF*/)
          num1 += 5;
        else
          break;
      }
      else if (num1 + num3 + 8 + 1 < 511 /*0x01FF*/)
        num1 += num3 + 4 + 1;
      else
        break;
    }
    return num2 - chpxIndex;
  }

  internal bool IsChpxRepeats(int chpxIndex, int CurrentIndex)
  {
    CharacterPropertyException chpxProp1 = this.m_chpxProps[CurrentIndex];
    bool flag = false;
    for (int index = chpxIndex; index < CurrentIndex; ++index)
    {
      CharacterPropertyException chpxProp2 = this.m_chpxProps[index];
      if (chpxProp1.Length == chpxProp2.Length && chpxProp1.ModifiersCount == chpxProp2.ModifiersCount && (flag = chpxProp1.Equals(chpxProp2)))
        break;
    }
    return flag;
  }

  private void WriteSepx(Stream stream)
  {
    this.m_secProperties = new SectionPropertyException[this.m_sepxPositions.Count];
    int index = 0;
    for (int count = this.m_sepxPositions.Count; index < count; ++index)
    {
      int sepxPosition = this.m_sepxPositions[index];
      SectionPropertyException sepxProp = this.m_sepxProps[index];
      int sepxPos = this.AlignByDiskPage(stream);
      this.m_tables.AddSectionRecord(sepxPosition, sepxPos);
      this.m_secProperties[index] = sepxProp;
      sepxProp.Save(stream);
    }
  }

  private int AlignByDiskPage(Stream stream)
  {
    int position = (int) stream.Position;
    int num1 = position / 512 /*0x0200*/;
    if (position % 512 /*0x0200*/ != 0)
      ++num1;
    int num2 = 512 /*0x0200*/ * num1;
    while (stream.Position < (long) num2)
    {
      byte[] buffer = new byte[(long) num2 - stream.Position];
      stream.Write(buffer, 0, (int) ((long) num2 - stream.Position));
    }
    return num1;
  }
}
