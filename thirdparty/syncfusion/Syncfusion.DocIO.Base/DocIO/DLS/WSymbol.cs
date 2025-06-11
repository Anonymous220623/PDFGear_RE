// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.WSymbol
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS.Rendering;
using Syncfusion.DocIO.DLS.XML;
using Syncfusion.DocIO.Rendering;
using Syncfusion.Layouting;
using Syncfusion.Office;
using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class WSymbol : ParagraphItem, ILeafWidget, IWidget
{
  private string m_fontName = "Symbol";
  private byte m_charCode;
  private byte m_charCodeExt;

  public override EntityType EntityType => EntityType.Symbol;

  public WCharacterFormat CharacterFormat => this.m_charFormat;

  public string FontName
  {
    get => this.m_fontName;
    set => this.m_fontName = value;
  }

  public byte CharacterCode
  {
    get => this.m_charCode;
    set => this.m_charCode = value;
  }

  internal byte CharCodeExt
  {
    get => this.m_charCodeExt;
    set => this.m_charCodeExt = value;
  }

  public WSymbol(IWordDocument doc)
    : base((WordDocument) doc)
  {
    this.m_charFormat = new WCharacterFormat(doc, (Entity) this);
  }

  protected override void CreateLayoutInfo()
  {
    this.m_layoutInfo = (ILayoutInfo) new LayoutInfo();
    this.m_layoutInfo.IsSkip = false;
    string text = char.ConvertFromUtf32((int) this.CharacterCode);
    WParagraph ownerParagraphValue = this.GetOwnerParagraphValue();
    if (ownerParagraphValue.IsInCell || ownerParagraphValue != null && ownerParagraphValue.IsNeedToFitSymbol(ownerParagraphValue) && ((IWidget) ownerParagraphValue).LayoutInfo.IsClipped)
      this.m_layoutInfo.IsClipped = true;
    this.m_layoutInfo.IsVerticalText = ((IWidget) ownerParagraphValue).LayoutInfo.IsVerticalText;
    if (!this.CharacterFormat.HasValue(0) && this.FontName != string.Empty && this.FontName != this.CharacterFormat.FontName)
    {
      WCharacterFormat charFormat = new WCharacterFormat((IWordDocument) this.Document);
      charFormat.ImportContainer((FormatBase) this.CharacterFormat);
      charFormat.CopyProperties((FormatBase) this.CharacterFormat);
      charFormat.ApplyBase(ownerParagraphValue.BreakCharacterFormat.BaseFormat);
      charFormat.FontName = this.FontName;
      this.m_layoutInfo.Font = new SyncFont(DocumentLayouter.DrawingContext.GetFont(FontScriptType.English, charFormat, text));
    }
    else
      this.m_layoutInfo.Font = new SyncFont(DocumentLayouter.DrawingContext.GetFont(FontScriptType.English, this.CharacterFormat, text));
  }

  internal override void InitLayoutInfo(Entity entity, ref bool isLastTOCEntry)
  {
    this.m_layoutInfo = (ILayoutInfo) null;
    if (this != entity)
      return;
    isLastTOCEntry = true;
  }

  protected override object CloneImpl() => (object) (WSymbol) base.CloneImpl();

  protected override void InitXDLSHolder()
  {
    base.InitXDLSHolder();
    this.XDLSHolder.AddElement("character-format", (object) this.m_charFormat);
  }

  protected override void WriteXmlAttributes(IXDLSAttributeWriter writer)
  {
    base.WriteXmlAttributes(writer);
    writer.WriteValue("type", (Enum) ParagraphItemType.Symbol);
    writer.WriteValue("FontName", this.FontName);
    writer.WriteValue("CharCode", (int) this.CharacterCode);
    writer.WriteValue("CharCodeExt", (int) this.CharCodeExt);
  }

  protected override void ReadXmlAttributes(IXDLSAttributeReader reader)
  {
    base.ReadXmlAttributes(reader);
    if (reader.HasAttribute("FontName"))
      this.FontName = reader.ReadString("FontName");
    if (reader.HasAttribute("CharCode"))
      this.CharacterCode = reader.ReadByte("CharCode");
    if (!reader.HasAttribute("CharCodeExt"))
      return;
    this.CharCodeExt = reader.ReadByte("CharCodeExt");
  }

  SizeF ILeafWidget.Measure(DrawingContext dc)
  {
    string text = char.ConvertFromUtf32((int) this.CharacterCode);
    WCharacterFormat charFormat = new WCharacterFormat((IWordDocument) this.Document);
    if (this.CharacterFormat.HasValue(0) || !(this.FontName != string.Empty) || !(this.FontName != this.CharacterFormat.FontName))
      return dc.MeasureString(text, dc.GetFont(FontScriptType.English, this.CharacterFormat, text), (StringFormat) null, this.CharacterFormat, false);
    charFormat.ImportContainer((FormatBase) this.CharacterFormat);
    charFormat.CopyProperties((FormatBase) this.CharacterFormat);
    WParagraph ownerParagraphValue = this.GetOwnerParagraphValue();
    charFormat.ApplyBase(ownerParagraphValue.BreakCharacterFormat.BaseFormat);
    charFormat.FontName = this.FontName;
    return dc.MeasureString(text, dc.GetFont(FontScriptType.English, charFormat, text), (StringFormat) null, charFormat, false);
  }

  internal Font GetFont(DrawingContext dc)
  {
    return !this.CharacterFormat.HasValue(0) && this.FontName != string.Empty && this.FontName != this.CharacterFormat.FontName ? this.Document.FontSettings.GetFont(this.FontName, this.CharacterFormat.FontSize, this.CharacterFormat.GetFontToRender(FontScriptType.English).Style) : this.CharacterFormat.GetFontToRender(FontScriptType.English);
  }

  void IWidget.InitLayoutInfo() => this.m_layoutInfo = (ILayoutInfo) null;

  void IWidget.InitLayoutInfo(IWidget widget)
  {
  }
}
