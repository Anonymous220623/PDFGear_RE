// Decompiled with JetBrains decompiler
// Type: Syncfusion.CompoundFile.DocIO.Native.DataObjectEntry
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

#nullable disable
namespace Syncfusion.CompoundFile.DocIO.Native;

internal class DataObjectEntry
{
  private STGMEDIUM m_medium;
  private FORMATETC m_format;
  private DATADIR m_dir;

  public STGMEDIUM Medium => this.m_medium;

  public FORMATETC Format => this.m_format;

  public DATADIR Direction => this.m_dir;

  public DataObjectEntry(DATADIR dir, STGMEDIUM medium, FORMATETC format)
  {
    this.m_medium = medium;
    this.m_format = format;
    this.m_dir = dir;
  }
}
