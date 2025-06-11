// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Compression.JBIG2.TiffFieldInfo
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Compression.JBIG2;

internal class TiffFieldInfo
{
  public const short Variable = -1;
  public const short Spp = -2;
  public const short Variable2 = -3;
  private TiffTag m_tag;
  private short m_readCount;
  private short m_writeCount;
  private TiffType m_type;
  private short m_bit;
  private bool m_okToChange;
  private bool m_passCount;
  private string m_name;

  public TiffFieldInfo(
    TiffTag tag,
    short readCount,
    short writeCount,
    TiffType type,
    short bit,
    bool okToChange,
    bool passCount,
    string name)
  {
    this.m_tag = tag;
    this.m_readCount = readCount;
    this.m_writeCount = writeCount;
    this.m_type = type;
    this.m_bit = bit;
    this.m_okToChange = okToChange;
    this.m_passCount = passCount;
    this.m_name = name;
  }

  public override string ToString()
  {
    return this.m_bit != (short) 65 || this.m_name.Length == 0 ? this.m_tag.ToString() : this.m_name;
  }

  public TiffTag Tag => this.m_tag;

  public short ReadCount => this.m_readCount;

  public short WriteCount => this.m_writeCount;

  public TiffType Type => this.m_type;

  public short Bit => this.m_bit;

  public bool OkToChange => this.m_okToChange;

  public bool PassCount => this.m_passCount;

  public string Name
  {
    get => this.m_name;
    internal set => this.m_name = value;
  }
}
