// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.LabelRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Text;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records;

[Biff(TBIFFRecord.Label)]
[CLSCompliant(false)]
public class LabelRecord : CellPositionBase, IStringValue, IValueHolder
{
  private const int DEF_FIXED_PART = 9;
  private string m_strLabel = string.Empty;

  public string Label
  {
    get => this.m_strLabel;
    set => this.m_strLabel = value;
  }

  public override int MinimumRecordSize => 8;

  protected override void ParseCellData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    this.m_strLabel = provider.ReadString16Bit(iOffset, out int _);
  }

  protected override void InfillCellData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    this.m_iLength = iOffset;
    provider.WriteString16BitUpdateOffset(ref this.m_iLength, this.m_strLabel);
    this.m_iLength -= iOffset;
  }

  public override int GetStoreSize(ExcelVersion version)
  {
    if (this.m_strLabel == null)
      this.m_strLabel = string.Empty;
    int storeSize = 9 + Encoding.Unicode.GetByteCount(this.m_strLabel);
    if (version != ExcelVersion.Excel97to2003)
      storeSize += 4;
    return storeSize;
  }

  string IStringValue.StringValue => this.Label;

  public object Value
  {
    get => (object) this.Label;
    set => this.Label = (string) value;
  }
}
