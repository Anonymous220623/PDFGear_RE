// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.GradStops
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records;

public class GradStops
{
  private ushort m_colorType;
  private int m_gradColorValue;
  private long m_gradPostition;
  private long m_gradTint;

  public int ParseGradStops(DataProvider provider, int iOffset, ExcelVersion version)
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

  public int InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
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
