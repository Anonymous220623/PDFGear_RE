// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.CellPositionBase
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Diagnostics;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records;

public abstract class CellPositionBase : BiffRecordRaw, ICellPositionFormat
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

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    switch (version)
    {
      case ExcelVersion.Excel97to2003:
        provider.WriteUInt16(iOffset, (ushort) this.m_iRow);
        iOffset += 2;
        provider.WriteInt16(iOffset, (short) this.m_iColumn);
        iOffset += 2;
        break;
      case ExcelVersion.Excel2007:
      case ExcelVersion.Excel2010:
      case ExcelVersion.Excel2013:
      case ExcelVersion.Excel2016:
      case ExcelVersion.Xlsx:
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
    ExcelVersion version)
  {
    switch (version)
    {
      case ExcelVersion.Excel97to2003:
        this.m_iRow = (int) provider.ReadUInt16(iOffset);
        iOffset += 2;
        this.m_iColumn = (int) provider.ReadInt16(iOffset);
        iOffset += 2;
        break;
      case ExcelVersion.Excel2007:
      case ExcelVersion.Excel2010:
      case ExcelVersion.Excel2013:
      case ExcelVersion.Excel2016:
      case ExcelVersion.Xlsx:
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

  protected abstract void ParseCellData(DataProvider provider, int iOffset, ExcelVersion version);

  protected abstract void InfillCellData(DataProvider provider, int iOffset, ExcelVersion version);

  public override int GetStoreSize(ExcelVersion version)
  {
    int storeSize = base.GetStoreSize(version);
    if (version != ExcelVersion.Excel97to2003)
      storeSize += 4;
    return storeSize;
  }
}
