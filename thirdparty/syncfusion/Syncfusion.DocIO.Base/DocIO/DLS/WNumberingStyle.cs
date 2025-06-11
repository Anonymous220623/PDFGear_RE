// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.WNumberingStyle
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

#nullable disable
namespace Syncfusion.DocIO.DLS;

internal class WNumberingStyle : Style
{
  private WParagraphFormat m_paragraphFormat;
  protected WListFormat m_listFormat;
  private int m_listIndex = -1;
  private int m_listLevel = -1;

  public WParagraphFormat ParagraphFormat => this.m_paragraphFormat;

  public WNumberingStyle BaseStyle => base.BaseStyle as WNumberingStyle;

  public override StyleType StyleType => StyleType.NumberingStyle;

  public WListFormat ListFormat
  {
    get
    {
      if (this.m_listFormat == null)
        this.m_listFormat = new WListFormat(this.Document, this);
      return this.m_listFormat;
    }
  }

  internal int ListIndex
  {
    get => this.m_listIndex;
    set => this.m_listIndex = value;
  }

  internal int ListLevel
  {
    get => this.m_listLevel;
    set => this.m_listLevel = value;
  }

  internal WNumberingStyle(IWordDocument doc)
    : base((WordDocument) doc)
  {
    this.m_paragraphFormat = new WParagraphFormat((IWordDocument) this.Document);
    this.m_paragraphFormat.SetOwner((OwnerHolder) this);
  }

  public override IStyle Clone() => (IStyle) this.CloneImpl();

  protected override object CloneImpl()
  {
    WNumberingStyle owner = (WNumberingStyle) base.CloneImpl();
    owner.m_paragraphFormat = new WParagraphFormat((IWordDocument) this.Document);
    owner.m_paragraphFormat.ImportContainer((FormatBase) this.ParagraphFormat);
    owner.m_paragraphFormat.SetOwner((OwnerHolder) owner);
    owner.m_listFormat = new WListFormat(this.Document, this);
    owner.m_listFormat.ImportContainer((FormatBase) this.ListFormat);
    owner.m_listFormat.SetOwner((OwnerHolder) owner);
    return (object) owner;
  }

  internal override void Close()
  {
    base.Close();
    if (this.m_paragraphFormat == null)
      return;
    this.m_paragraphFormat.Close();
    this.m_paragraphFormat = (WParagraphFormat) null;
  }

  protected override void InitXDLSHolder()
  {
    base.InitXDLSHolder();
    this.XDLSHolder.AddElement("paragraph-format", (object) this.m_paragraphFormat);
  }
}
