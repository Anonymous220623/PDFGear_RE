// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.CellPositionBase
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.Diagnostics;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records;

internal abstract class CellPositionBase : BiffRecordRaw, ICellPositionFormat
{
  protected int m_iRow;
  protected int m_iColumn;
  [CLSCompliant(false)]
  protected ushort m_usExtendedFormat;

  public int Row
  {
    [DebuggerStepThrough] get => this.m_iRow;
    [DebuggerStepThrough] set => this.m_iRow = value;
  }

  public int Column
  {
    [DebuggerStepThrough] get => this.m_iColumn;
    [DebuggerStepThrough] set => this.m_iColumn = value;
  }

  [CLSCompliant(false)]
  public ushort ExtendedFormatIndex
  {
    get => this.m_usExtendedFormat;
    set => this.m_usExtendedFormat = value;
  }

  public override void InfillInternalData(
    DataProvider provider,
    int iOffset,
    OfficeVersion version)
  {
    switch (version)
    {
      case OfficeVersion.Excel97to2003:
        provider.WriteUInt16(iOffset, (ushort) this.m_iRow);
        iOffset += 2;
        provider.WriteInt16(iOffset, (short) this.m_iColumn);
        iOffset += 2;
        break;
      case OfficeVersion.Excel2007:
      case OfficeVersion.Excel2010:
      case OfficeVersion.Excel2013:
        provider.WriteInt32(iOffset, this.m_iRow);
        iOffset += 4;
        provider.WriteInt32(iOffset, this.m_iColumn);
        iOffset += 4;
        break;
    }
    provider.WriteUInt16(iOffset, this.m_usExtendedFormat);
    iOffset += 2;
    this.InfillCellData(provider, iOffset, version);
    this.m_iLength = this.GetStoreSize(version);
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    OfficeVersion version)
  {
    switch (version)
    {
      case OfficeVersion.Excel97to2003:
        this.m_iRow = (int) provider.ReadUInt16(iOffset);
        iOffset += 2;
        this.m_iColumn = (int) provider.ReadInt16(iOffset);
        iOffset += 2;
        break;
      case OfficeVersion.Excel2007:
      case OfficeVersion.Excel2010:
      case OfficeVersion.Excel2013:
        this.m_iRow = provider.ReadInt32(iOffset);
        iOffset += 4;
        this.m_iColumn = provider.ReadInt32(iOffset);
        iOffset += 4;
        break;
    }
    this.m_usExtendedFormat = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.ParseCellData(provider, iOffset, version);
  }

  protected abstract void ParseCellData(DataProvider provider, int iOffset, OfficeVersion version);

  protected abstract void InfillCellData(DataProvider provider, int iOffset, OfficeVersion version);

  public override int GetStoreSize(OfficeVersion version)
  {
    int storeSize = base.GetStoreSize(version);
    if (version != OfficeVersion.Excel97to2003)
      storeSize += 4;
    return storeSize;
  }
}
