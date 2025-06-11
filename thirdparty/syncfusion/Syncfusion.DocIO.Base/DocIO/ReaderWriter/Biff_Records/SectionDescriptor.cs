// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Biff_Records.SectionDescriptor
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.ReaderWriter.Biff_Records.Structures;
using System;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Biff_Records;

[CLSCompliant(false)]
internal class SectionDescriptor : BaseWordRecord
{
  internal const int DEF_RECORD_SIZE = 12;
  private SectionDescriptorStructure m_structure = new SectionDescriptorStructure();

  internal SectionDescriptor()
  {
  }

  internal SectionDescriptor(byte[] arrData)
    : base(arrData)
  {
  }

  internal SectionDescriptor(byte[] arrData, int iOffset)
    : base(arrData, iOffset)
  {
  }

  internal short Internal1
  {
    get => this.m_structure.Internal1;
    set => this.m_structure.Internal1 = value;
  }

  internal short Internal2
  {
    get => this.m_structure.Internal2;
    set => this.m_structure.Internal2 = value;
  }

  internal uint SepxPosition
  {
    get => this.m_structure.SepxPosition;
    set => this.m_structure.SepxPosition = value;
  }

  internal int MacPrintOffset
  {
    get => this.m_structure.MacPrintOffset;
    set => this.m_structure.MacPrintOffset = value;
  }

  protected override IDataStructure UnderlyingStructure => (IDataStructure) this.m_structure;

  internal override int Length => 12;

  internal override void Parse(byte[] arrData, int iOffset, int iCount)
  {
    this.m_structure.Parse(arrData, iOffset);
  }
}
