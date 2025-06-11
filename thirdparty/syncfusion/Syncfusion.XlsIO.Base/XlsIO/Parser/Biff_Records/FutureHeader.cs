// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.FutureHeader
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records;

[CLSCompliant(false)]
internal class FutureHeader
{
  [BiffRecordPos(0, 2)]
  private ushort m_usType;
  [BiffRecordPos(2, 2)]
  private ushort m_usAttributes;

  public ushort Type
  {
    get => this.m_usType;
    set => this.m_usType = value;
  }

  public ushort Attributes
  {
    get => this.m_usAttributes;
    set => this.m_usAttributes = value;
  }

  public void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    provider.WriteUInt16(iOffset, this.Type);
    iOffset += 2;
    provider.WriteUInt16(iOffset, this.Attributes);
    iOffset += 2;
    provider.WriteInt64(iOffset, 0L);
  }

  public int GetStoreSize() => 12;
}
