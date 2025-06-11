// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.CFExFilterParameter
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records;

public class CFExFilterParameter
{
  private bool m_isTopOrBottom;
  private bool m_isPercent;
  private ushort m_filterValue;
  private TopBottomImpl m_topBottom;

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

  internal ITopBottom TopBottom
  {
    get => (ITopBottom) this.m_topBottom;
    set => this.m_topBottom = value as TopBottomImpl;
  }

  private void CopyFilterParameter()
  {
    this.m_topBottom = new TopBottomImpl();
    this.m_topBottom.Type = this.m_isTopOrBottom ? ExcelCFTopBottomType.Top : ExcelCFTopBottomType.Bottom;
    this.m_topBottom.Percent = this.m_isPercent;
    this.m_topBottom.Rank = (int) this.m_filterValue;
  }

  internal object Clone()
  {
    CFExFilterParameter exFilterParameter = (CFExFilterParameter) this.MemberwiseClone();
    if (this.m_topBottom != (TopBottomImpl) null)
      exFilterParameter.m_topBottom = this.m_topBottom.Clone();
    return (object) exFilterParameter;
  }

  internal new int GetHashCode()
  {
    return this.m_isTopOrBottom.GetHashCode() ^ this.m_isPercent.GetHashCode() ^ this.m_filterValue.GetHashCode();
  }

  internal new bool Equals(object obj)
  {
    return obj is CFExFilterParameter exFilterParameter && this.m_isTopOrBottom == exFilterParameter.m_isTopOrBottom && this.m_isPercent == exFilterParameter.m_isPercent && (int) this.m_filterValue == (int) exFilterParameter.m_filterValue && this.m_topBottom == exFilterParameter.m_topBottom;
  }

  public void ParseFilterTemplateParameter(
    DataProvider provider,
    int iOffset,
    ExcelVersion version)
  {
    this.m_isTopOrBottom = provider.ReadBit(iOffset, 0);
    this.m_isPercent = provider.ReadBit(iOffset, 1);
    ++iOffset;
    this.m_filterValue = provider.ReadUInt16(iOffset);
    iOffset += 2;
    provider.ReadInt64(iOffset);
    iOffset += 13;
    this.CopyFilterParameter();
  }

  public void SerializeFilterParameter(DataProvider provider, int iOffset, ExcelVersion version)
  {
    if (this.m_topBottom != (TopBottomImpl) null)
    {
      this.m_isTopOrBottom = this.m_topBottom.Type == ExcelCFTopBottomType.Top;
      this.m_isPercent = this.m_topBottom.Percent;
      this.m_filterValue = (ushort) this.m_topBottom.Rank;
    }
    provider.WriteBit(iOffset, this.m_isTopOrBottom, 0);
    provider.WriteBit(iOffset, this.m_isPercent, 1);
    ++iOffset;
    provider.WriteUInt16(iOffset, this.m_filterValue);
    iOffset += 2;
    provider.WriteInt64(iOffset, 0L);
    iOffset += 13;
  }
}
