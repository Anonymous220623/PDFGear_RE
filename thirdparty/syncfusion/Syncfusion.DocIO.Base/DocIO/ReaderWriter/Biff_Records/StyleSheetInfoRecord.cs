// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Biff_Records.StyleSheetInfoRecord
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.ReaderWriter.Biff_Records.Structures;
using System;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Biff_Records;

[CLSCompliant(false)]
internal class StyleSheetInfoRecord : BaseWordRecord
{
  private const int DEF_BIT_STYLE_NAMES_WRITTEN = 0;
  private StyleSheetInfoStructure m_structure = new StyleSheetInfoStructure();

  internal ushort StylesCount
  {
    get => this.m_structure.StylesCount;
    set => this.m_structure.StylesCount = value;
  }

  internal ushort STDBaseLength
  {
    get => this.m_structure.STDBaseLength;
    set => this.m_structure.STDBaseLength = value;
  }

  internal bool IsStdStyleNamesWritten
  {
    get => this.m_structure.IsStdStyleNamesWritten;
    set => this.m_structure.IsStdStyleNamesWritten = value;
  }

  internal ushort StiMaxWhenSaved
  {
    get => this.m_structure.StiMaxWhenSaved;
    set => this.m_structure.StiMaxWhenSaved = value;
  }

  internal ushort ISTDMaxFixedWhenSaved
  {
    get => this.m_structure.ISTDMaxFixedWhenSaved;
    set => this.m_structure.ISTDMaxFixedWhenSaved = value;
  }

  internal ushort BuiltInNamesVersion
  {
    get => this.m_structure.BuiltInNamesVersion;
    set => this.m_structure.BuiltInNamesVersion = value;
  }

  internal ushort[] StandardChpStsh
  {
    get => this.m_structure.StandardChpStsh;
    set => this.m_structure.StandardChpStsh = value;
  }

  protected override IDataStructure UnderlyingStructure => (IDataStructure) this.m_structure;

  internal override int Length => this.m_structure.Length;

  internal ushort FtcBi
  {
    get => this.m_structure.FtcBi;
    set => this.m_structure.FtcBi = value;
  }

  internal StyleSheetInfoRecord(ushort iSTDBaseLength)
  {
    this.STDBaseLength = iSTDBaseLength;
    this.StiMaxWhenSaved = (ushort) 91;
    this.ISTDMaxFixedWhenSaved = (ushort) 15;
    this.BuiltInNamesVersion = (ushort) 0;
    this.IsStdStyleNamesWritten = true;
  }

  internal StyleSheetInfoRecord(byte[] arrData)
    : base(arrData)
  {
  }

  internal StyleSheetInfoRecord(byte[] arrData, int iOffset)
    : base(arrData, iOffset)
  {
  }

  internal StyleSheetInfoRecord(byte[] arrData, int iOffset, int iCount)
    : base(arrData, iOffset, iCount)
  {
  }

  internal StyleSheetInfoRecord(Stream stream, int iCount)
    : base(stream, iCount)
  {
  }

  internal override void Parse(byte[] arrData, int iOffset, int iCount)
  {
    base.Parse(arrData, iOffset, iCount);
  }

  internal override void Close()
  {
    base.Close();
    this.m_structure = (StyleSheetInfoStructure) null;
  }
}
