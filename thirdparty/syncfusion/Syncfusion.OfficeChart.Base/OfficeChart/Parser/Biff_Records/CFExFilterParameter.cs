// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.CFExFilterParameter
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records;

internal class CFExFilterParameter
{
  private bool m_isTopOrBottom;
  private bool m_isPercent;
  private ushort m_filterValue;

  public bool IsTopOrBottom
  {
    get => this.m_isTopOrBottom;
    set => this.m_isTopOrBottom = value;
  }

  public bool IsPercent
  {
    get => this.m_isPercent;
    set => this.m_isPercent = value;
  }

  public ushort FilterValue
  {
    get => this.m_filterValue;
    set => this.m_filterValue = value;
  }

  public void ParseFilterTemplateParameter(
    DataProvider provider,
    int iOffset,
    OfficeVersion version)
  {
    this.m_isTopOrBottom = provider.ReadBit(iOffset, 0);
    this.m_isPercent = provider.ReadBit(iOffset, 1);
    ++iOffset;
    this.m_filterValue = provider.ReadUInt16(iOffset);
    iOffset += 2;
    provider.ReadInt64(iOffset);
    iOffset += 13;
  }

  public void SerializeFilterParameter(DataProvider provider, int iOffset, OfficeVersion version)
  {
    provider.WriteBit(iOffset, this.m_isTopOrBottom, 0);
    provider.WriteBit(iOffset, this.m_isPercent, 1);
    ++iOffset;
    provider.WriteUInt16(iOffset, this.m_filterValue);
    iOffset += 2;
    provider.WriteInt64(iOffset, 0L);
    iOffset += 13;
  }
}
