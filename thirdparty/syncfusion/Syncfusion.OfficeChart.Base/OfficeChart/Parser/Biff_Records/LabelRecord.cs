// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.LabelRecord
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.Text;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records;

[Biff(TBIFFRecord.Label)]
[CLSCompliant(false)]
internal class LabelRecord : CellPositionBase, IStringValue, IValueHolder
{
  private const int DEF_FIXED_PART = 9;
  private string m_strLabel = string.Empty;

  public string Label
  {
    get => this.m_strLabel;
    set => this.m_strLabel = value;
  }

  public override int MinimumRecordSize => 8;

  protected override void ParseCellData(DataProvider provider, int iOffset, OfficeVersion version)
  {
    this.m_strLabel = provider.ReadString16Bit(iOffset, out int _);
  }

  protected override void InfillCellData(DataProvider provider, int iOffset, OfficeVersion version)
  {
    this.m_iLength = iOffset;
    provider.WriteString16BitUpdateOffset(ref this.m_iLength, this.m_strLabel);
    this.m_iLength -= iOffset;
  }

  public override int GetStoreSize(OfficeVersion version)
  {
    if (this.m_strLabel == null)
      this.m_strLabel = string.Empty;
    int storeSize = 9 + Encoding.Unicode.GetByteCount(this.m_strLabel);
    if (version != OfficeVersion.Excel97to2003)
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
