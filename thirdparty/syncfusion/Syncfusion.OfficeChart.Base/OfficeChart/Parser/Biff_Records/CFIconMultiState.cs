// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.CFIconMultiState
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records;

internal class CFIconMultiState
{
  private const ushort DEF_MINIMUM_SIZE = 5;
  private CFVO m_cfvo;
  private byte m_isEqual;
  private uint m_undefined;

  public CFVO CFVO
  {
    get => this.m_cfvo;
    set => this.m_cfvo = value;
  }

  public byte IsEqulal
  {
    get => this.m_isEqual;
    set => this.m_isEqual = value;
  }

  public CFIconMultiState() => this.m_cfvo = new CFVO();

  public int ParseCFIconMultistate(DataProvider provider, int iOffset, OfficeVersion version)
  {
    iOffset = this.m_cfvo.ParseCFVO(provider, iOffset, version);
    this.m_isEqual = provider.ReadByte(iOffset);
    ++iOffset;
    this.m_undefined = provider.ReadUInt32(iOffset);
    iOffset += 4;
    return iOffset;
  }

  public int SerializeCFIconMultistate(DataProvider provider, int iOffset, OfficeVersion version)
  {
    iOffset = this.m_cfvo.SerializeCFVO(provider, iOffset, version);
    provider.WriteByte(iOffset, this.m_isEqual);
    ++iOffset;
    provider.WriteUInt32(iOffset, this.m_undefined);
    iOffset += 4;
    return iOffset;
  }

  public int GetStoreSize(OfficeVersion version) => 5 + this.m_cfvo.GetStoreSize(version);

  internal void ClearAll()
  {
    this.m_cfvo.ClearAll();
    this.m_cfvo = (CFVO) null;
  }
}
