// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.WParagraphStyle
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class WParagraphStyle : Style, IWParagraphStyle, IStyle
{
  protected WParagraphFormat m_prFormat;
  protected WListFormat m_listFormat;
  private int m_listIndex = -1;
  private int m_listLevel = -1;

  public WParagraphFormat ParagraphFormat => this.m_prFormat;

  public WParagraphStyle BaseStyle => base.BaseStyle as WParagraphStyle;

  public override StyleType StyleType => StyleType.ParagraphStyle;

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

  public WParagraphStyle(IWordDocument doc)
    : base((WordDocument) doc)
  {
    this.m_prFormat = new WParagraphFormat((IWordDocument) this.Document);
    this.m_prFormat.SetOwner((OwnerHolder) this);
    if (!(doc as WordDocument).CreateBaseStyle)
      return;
    (doc as WordDocument).CreateBaseStyle = false;
    this.ApplyBaseStyle(BuiltinStyle.Normal);
    (doc as WordDocument).CreateBaseStyle = true;
  }

  public override void ApplyBaseStyle(string styleName)
  {
    base.ApplyBaseStyle(styleName);
    if (this.BaseStyle == null)
      return;
    this.m_prFormat.ApplyBase((FormatBase) this.BaseStyle.ParagraphFormat);
  }

  public override IStyle Clone() => (IStyle) this.CloneImpl();

  internal override bool Compare(Style style)
  {
    return base.Compare(style) && ((style as WParagraphStyle).ParagraphFormat == null || this.ParagraphFormat == null || this.ParagraphFormat.Compare((style as WParagraphStyle).ParagraphFormat) && this.ListFormat.Compare((style as WParagraphStyle).ListFormat));
  }

  internal override void ApplyBaseStyle(Style baseStyle)
  {
    base.ApplyBaseStyle(baseStyle);
    if (this.BaseStyle == null)
      return;
    this.m_prFormat.ApplyBase((FormatBase) this.BaseStyle.ParagraphFormat);
  }

  protected override object CloneImpl()
  {
    WParagraphStyle owner = (WParagraphStyle) base.CloneImpl();
    owner.m_prFormat = new WParagraphFormat((IWordDocument) this.Document);
    owner.m_prFormat.ImportContainer((FormatBase) this.ParagraphFormat);
    owner.m_prFormat.CopyFormat((FormatBase) this.ParagraphFormat);
    owner.m_prFormat.SetOwner((OwnerHolder) owner);
    owner.m_listFormat = new WListFormat(this.Document, this);
    owner.m_listFormat.ImportContainer((FormatBase) this.ListFormat);
    owner.m_listFormat.SetOwner((OwnerHolder) owner);
    if (this.BaseStyle != null)
      owner.ApplyBaseStyle((Style) this.BaseStyle);
    return (object) owner;
  }

  void IWParagraphStyle.Close() => this.Close();

  internal override void Close()
  {
    base.Close();
    if (this.m_prFormat != null)
    {
      this.m_prFormat.Close();
      this.m_prFormat = (WParagraphFormat) null;
    }
    if (this.m_listFormat == null)
      return;
    this.m_listFormat.Close();
    this.m_listFormat = (WListFormat) null;
  }

  protected override void InitXDLSHolder()
  {
    base.InitXDLSHolder();
    this.XDLSHolder.AddElement("paragraph-format", (object) this.m_prFormat);
  }
}
