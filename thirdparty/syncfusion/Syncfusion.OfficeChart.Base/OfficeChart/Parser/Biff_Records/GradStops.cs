// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.GradStops
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records;

internal class GradStops
{
  private ushort m_colorType;
  private int m_gradColorValue;
  private long m_gradPostition;
  private long m_gradTint;

  public int ParseGradStops(DataProvider provider, int iOffset, OfficeVersion version)
  {
    this.m_colorType = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_gradColorValue = provider.ReadInt32(iOffset);
    iOffset += 4;
    this.m_gradPostition = provider.ReadInt64(iOffset);
    iOffset += 8;
    this.m_gradTint = provider.ReadInt64(iOffset);
    iOffset += 8;
    return iOffset;
  }

  public int InfillInternalData(DataProvider provider, int iOffset, OfficeVersion version)
  {
    provider.WriteUInt16(iOffset, this.m_colorType);
    iOffset += 2;
    provider.WriteInt32(iOffset, this.m_gradColorValue);
    iOffset += 4;
    provider.WriteInt64(iOffset, this.m_gradPostition);
    iOffset += 8;
    provider.WriteInt64(iOffset, this.m_gradTint);
    iOffset += 8;
    return iOffset;
  }
}
