// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.ObjRecords.LbsDropData
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.ObjRecords;

public class LbsDropData
{
  private short m_sOptions;
  private short m_sLinesNumber;
  private short m_sMinimum;
  private string m_strValue;

  public short Options
  {
    get => this.m_sOptions;
    set => this.m_sOptions = value;
  }

  public short LinesNumber
  {
    get => this.m_sLinesNumber;
    set => this.m_sLinesNumber = value;
  }

  public short Minimum
  {
    get => this.m_sMinimum;
    set => this.m_sMinimum = value;
  }

  public string Value
  {
    get => this.m_strValue;
    set => this.m_strValue = value;
  }

  public void Serialize(DataProvider provider, int offset)
  {
    int num = offset;
    provider.WriteInt16(offset, this.m_sOptions);
    offset += 2;
    provider.WriteInt16(offset, this.m_sLinesNumber);
    offset += 2;
    provider.WriteInt16(offset, this.m_sMinimum);
    offset += 2;
    provider.WriteString16BitUpdateOffset(ref offset, this.m_strValue);
    if ((offset - num) % 2 == 0)
      return;
    provider.WriteByte(offset, (byte) 10);
  }

  public int Parse(DataProvider provider, int offset)
  {
    int num = offset;
    this.m_sOptions = provider.ReadInt16(offset);
    offset += 2;
    this.m_sLinesNumber = provider.ReadInt16(offset);
    offset += 2;
    if (provider.Capacity > offset + 2)
    {
      this.m_sMinimum = provider.ReadInt16(offset);
      offset += 2;
      if (provider.Capacity > offset + 2)
        this.m_strValue = provider.ReadString16BitUpdateOffset(ref offset);
    }
    if ((offset - num) % 2 != 0)
      ++offset;
    return offset;
  }

  public int GetStoreSize()
  {
    int num = 0;
    if (this.m_strValue != null)
      num = num + this.m_strValue.Length * 2 + 3;
    int storeSize = num + 6;
    if (storeSize % 2 != 0)
      ++storeSize;
    return storeSize;
  }

  public LbsDropData Clone() => (LbsDropData) this.MemberwiseClone();
}
