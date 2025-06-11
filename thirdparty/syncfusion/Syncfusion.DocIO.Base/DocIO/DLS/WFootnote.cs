// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.WFootnote
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS.Rendering;
using Syncfusion.DocIO.DLS.XML;
using Syncfusion.DocIO.Rendering;
using Syncfusion.Layouting;
using System;
using System.Collections;
using System.Drawing;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class WFootnote : ParagraphItem, ILeafWidget, IWidget
{
  internal const int DEF_FTNSTYLE_REF_ID = 38;
  internal const int DEF_EDNSTYLE_REF_ID = 39;
  private FootnoteType m_footnoteType;
  private WTextBody m_textBody;
  private byte m_symbolCode;
  private string m_strSymbolFontName = "Symbol";
  internal string m_strCustomMarker = string.Empty;
  private short m_changesCount;
  private byte m_bFlags = 1;

  public override EntityType EntityType => EntityType.Footnote;

  public FootnoteType FootnoteType
  {
    get => this.m_footnoteType;
    set => this.m_footnoteType = value;
  }

  public bool IsAutoNumbered
  {
    get => ((int) this.m_bFlags & 1) != 0;
    set
    {
      if (this.IsAutoNumbered == value)
        return;
      if (!this.Document.IsOpening)
      {
        this.UpdateChangeFlag(true);
        this.UpdateAutoMarker(value);
      }
      this.m_bFlags = (byte) ((int) this.m_bFlags & 254 | (value ? 1 : 0));
    }
  }

  public WTextBody TextBody => this.m_textBody;

  public WCharacterFormat MarkerCharacterFormat => this.m_charFormat;

  public byte SymbolCode
  {
    get => this.m_symbolCode;
    set
    {
      if ((int) value != (int) this.m_symbolCode && !this.Document.IsOpening)
      {
        this.UpdateChangeFlag(true);
        this.UpdateSymbolMarker(value);
      }
      this.m_symbolCode = value;
    }
  }

  internal string SymbolFontName
  {
    get => this.m_strSymbolFontName;
    set
    {
      if (value != this.m_strSymbolFontName && !this.Document.IsOpening)
        this.UpdateChangeFlag(true);
      this.m_strSymbolFontName = value;
    }
  }

  public string CustomMarker
  {
    get => this.GetCustomMarkerValue();
    set
    {
      string customMarkerValue = this.GetCustomMarkerValue();
      string destMarker = value;
      if (destMarker.Length > 10)
        destMarker = destMarker.Substring(0, 10);
      if (destMarker != customMarkerValue && !this.IsAutoNumbered && !this.Document.IsOpening && this.m_symbolCode == (byte) 0)
      {
        this.UpdateChangeFlag(true);
        this.UpdateCustomMarker(customMarkerValue, destMarker);
      }
      this.ClearPreviousCustomMarker();
      this.m_strCustomMarker = destMarker;
    }
  }

  internal bool CustomMarkerIsSymbol => this.m_symbolCode > (byte) 0;

  internal bool IsLayouted
  {
    get => ((int) this.m_bFlags & 2) >> 1 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 253 | (value ? 1 : 0) << 1);
  }

  public WFootnote(IWordDocument doc)
    : base((WordDocument) doc)
  {
    this.m_textBody = new WTextBody(this.Document, (Entity) this);
    this.m_charFormat = new WCharacterFormat((IWordDocument) this.Document, (Entity) this);
  }

  internal WFootnote(IWordDocument doc, string marker)
    : this(doc)
  {
    this.m_strCustomMarker = marker;
    this.IsAutoNumbered = false;
  }

  internal override void AddSelf()
  {
    if (this.m_textBody == null)
      return;
    this.m_textBody.AddSelf();
  }

  internal override void AttachToParagraph(WParagraph owner, int itemPos)
  {
    base.AttachToParagraph(owner, itemPos);
    if (this.m_textBody == null)
      return;
    this.m_textBody.AttachToDocument();
  }

  protected override void CreateLayoutInfo()
  {
    this.m_layoutInfo = (ILayoutInfo) new LayoutFootnoteInfoImpl(this);
    WParagraph ownerParagraphValue = this.GetOwnerParagraphValue();
    if (ownerParagraphValue == null)
      return;
    this.m_layoutInfo.IsVerticalText = ownerParagraphValue.IsVerticalText();
  }

  internal override void InitLayoutInfo(Entity entity, ref bool isLastTOCEntry)
  {
    this.m_layoutInfo = (ILayoutInfo) null;
    if (this.m_textBody != null)
    {
      this.m_textBody.InitLayoutInfo(entity, ref isLastTOCEntry);
      if (isLastTOCEntry)
        return;
    }
    if (this != entity)
      return;
    isLastTOCEntry = true;
  }

  protected override object CloneImpl()
  {
    WFootnote owner = (WFootnote) base.CloneImpl();
    owner.m_textBody = (WTextBody) this.m_textBody.Clone();
    owner.m_textBody.SetOwner((OwnerHolder) owner);
    return (object) owner;
  }

  internal override void CloneRelationsTo(WordDocument doc, OwnerHolder nextOwner)
  {
    if (this.m_textBody != null)
      this.m_textBody.CloneRelationsTo(doc, nextOwner);
    base.CloneRelationsTo(doc, nextOwner);
    this.m_doc = doc;
  }

  internal override void OnStateChange(object sender)
  {
    if (!(sender is WCharacterFormat))
      return;
    this.UpdateChangeFlag(true);
  }

  internal override void Close()
  {
    if (this.m_textBody != null)
    {
      this.m_textBody.Close();
      this.m_textBody = (WTextBody) null;
    }
    if (this.m_strCustomMarker != null)
      this.m_strCustomMarker = string.Empty;
    if (this.m_strSymbolFontName != null)
      this.m_strSymbolFontName = string.Empty;
    base.Close();
  }

  protected override void InitXDLSHolder()
  {
    this.XDLSHolder.AddElement("body", (object) this.m_textBody);
    this.XDLSHolder.AddElement("marker-character-format", (object) this.m_charFormat);
  }

  protected override void WriteXmlAttributes(IXDLSAttributeWriter writer)
  {
    base.WriteXmlAttributes(writer);
    writer.WriteValue("type", (Enum) FootnoteType.Footnote);
    writer.WriteValue("AutoNumbered", this.IsAutoNumbered);
    if (this.m_footnoteType == FootnoteType.Endnote)
      writer.WriteValue("IsEndnoteAttr", true);
    if (this.m_strCustomMarker != string.Empty)
      writer.WriteValue("CustomMarker", this.m_strCustomMarker);
    if (!this.CustomMarkerIsSymbol)
      return;
    writer.WriteValue("SymbolCode", (int) this.m_symbolCode);
    writer.WriteValue("SymbolFontName", this.m_strSymbolFontName);
  }

  protected override void ReadXmlAttributes(IXDLSAttributeReader reader)
  {
    base.ReadXmlAttributes(reader);
    if (reader.HasAttribute("AutoNumbered"))
      this.IsAutoNumbered = reader.ReadBoolean("AutoNumbered");
    if (reader.HasAttribute("CustomMarker"))
      this.m_strCustomMarker = reader.ReadString("CustomMarker");
    if (reader.HasAttribute("IsEndnoteAttr"))
      this.m_footnoteType = reader.ReadBoolean("IsEndnoteAttr") ? FootnoteType.Endnote : FootnoteType.Footnote;
    if (reader.HasAttribute("SymbolCode"))
      this.m_symbolCode = reader.ReadByte("SymbolCode");
    if (!reader.HasAttribute("SymbolFontName"))
      return;
    this.m_strSymbolFontName = reader.ReadString("SymbolFontName");
  }

  private void UpdateAutoMarker(bool isAuto)
  {
    if (this.Document.IsOpening || !this.IsAutoNumbered && string.IsNullOrEmpty(this.m_strCustomMarker))
      return;
    string empty1 = string.Empty;
    string empty2 = string.Empty;
    string strCustomMarker1;
    string strCustomMarker2;
    if (!isAuto)
    {
      strCustomMarker1 = '\u0002'.ToString();
      strCustomMarker2 = this.m_strCustomMarker;
    }
    else
    {
      strCustomMarker1 = this.m_strCustomMarker;
      strCustomMarker2 = '\u0002'.ToString();
    }
    this.UpdateFtnMarker(strCustomMarker1, strCustomMarker2);
  }

  private void UpdateCustomMarker(string curMarker, string destMarker)
  {
    if (this.Document.IsOpening || string.IsNullOrEmpty(curMarker))
      return;
    this.UpdateFtnMarker(curMarker, destMarker);
  }

  private void UpdateSymbolMarker(byte symbolCode)
  {
    if (this.Document.IsOpening || this.m_textBody.Items.Count == 0 || this.IsAutoNumbered || symbolCode <= (byte) 0 || string.IsNullOrEmpty(this.m_strCustomMarker))
      return;
    WParagraph para = this.m_textBody.Items[0] as WParagraph;
    TextSelection selection = para.Find(this.m_strCustomMarker, true, true);
    this.ReplaceSelection(this.GenerateSymbol(symbolCode), para, selection);
    this.m_charFormat.FontName = this.m_strSymbolFontName;
    this.UpdateChangeFlag(false);
  }

  private void ClearPreviousCustomMarker()
  {
    if (!(this.Owner is WParagraph))
      return;
    WParagraph ownerParagraph = this.OwnerParagraph;
    int num = this.OwnerParagraph.ChildEntities.IndexOf((IEntity) this);
    int length = this.m_strCustomMarker.Length;
    if (length >= 10)
      return;
    for (int index = num + 1; index < ownerParagraph.Items.Count && ownerParagraph.Items[index].EntityType == EntityType.TextRange; ++index)
    {
      WTextRange wtextRange = ownerParagraph.Items[index] as WTextRange;
      bool flag = this.CompareCharacterFormat(wtextRange.CharacterFormat);
      if (wtextRange.Text.Contains(" "))
      {
        if (!flag)
          break;
        int count = wtextRange.Text.IndexOf(' ');
        if (length + count > 10)
        {
          wtextRange.Text = wtextRange.Text.Remove(0, 10 - length);
          break;
        }
        wtextRange.Text = wtextRange.Text.Remove(0, count);
        break;
      }
      if (flag)
      {
        if (length + wtextRange.Text.Length > 10)
        {
          wtextRange.Text = wtextRange.Text.Remove(0, 10 - length);
          break;
        }
        ownerParagraph.Items.Remove((IEntity) wtextRange);
        length += wtextRange.Text.Length;
        if (length == 10)
          break;
        --index;
      }
    }
  }

  private string GetCustomMarkerValue()
  {
    if (!(this.Owner is WParagraph))
      return this.m_strCustomMarker;
    string strCustomMarker = this.m_strCustomMarker;
    if (strCustomMarker.Length == 10)
      return strCustomMarker.Contains(" ") ? strCustomMarker.Substring(0, strCustomMarker.IndexOf(" ")) : strCustomMarker;
    if (strCustomMarker.Length <= 10)
      return strCustomMarker;
    if (!strCustomMarker.Contains(" "))
      return strCustomMarker.Substring(0, 10);
    int length = strCustomMarker.IndexOf(" ");
    if (length > 10)
      length = 10;
    return strCustomMarker.Substring(0, length);
  }

  internal bool CompareCharacterFormat(WCharacterFormat textRangeFormat)
  {
    return !(this.MarkerCharacterFormat.FontName != textRangeFormat.FontName) && (double) this.MarkerCharacterFormat.FontSize == (double) textRangeFormat.FontSize && this.MarkerCharacterFormat.ComplexScript == textRangeFormat.ComplexScript && this.MarkerCharacterFormat.Bold == textRangeFormat.Bold && this.MarkerCharacterFormat.Italic == textRangeFormat.Italic && this.MarkerCharacterFormat.Strikeout == textRangeFormat.Strikeout && this.MarkerCharacterFormat.AllCaps == textRangeFormat.AllCaps && !(this.MarkerCharacterFormat.TextColor != textRangeFormat.TextColor) && this.MarkerCharacterFormat.DoubleStrike == textRangeFormat.DoubleStrike && !(this.MarkerCharacterFormat.FontNameFarEast != textRangeFormat.FontNameFarEast) && !(this.MarkerCharacterFormat.TextBackgroundColor != textRangeFormat.TextBackgroundColor) && this.MarkerCharacterFormat.Emboss == textRangeFormat.Emboss && this.MarkerCharacterFormat.Engrave == textRangeFormat.Engrave && !(this.MarkerCharacterFormat.HighlightColor != textRangeFormat.HighlightColor) && !(this.MarkerCharacterFormat.FontNameAscii != textRangeFormat.FontNameAscii) && !(this.MarkerCharacterFormat.FontNameBidi != textRangeFormat.FontNameBidi) && this.MarkerCharacterFormat.OutLine == textRangeFormat.OutLine && (double) this.MarkerCharacterFormat.Position == (double) textRangeFormat.Position && (double) this.MarkerCharacterFormat.FontSizeBidi == (double) textRangeFormat.FontSizeBidi && !(this.MarkerCharacterFormat.CharStyleName != textRangeFormat.CharStyleName) && this.MarkerCharacterFormat.Shadow == textRangeFormat.Shadow && this.MarkerCharacterFormat.SubSuperScript == textRangeFormat.SubSuperScript && this.MarkerCharacterFormat.SmallCaps == textRangeFormat.SmallCaps && this.MarkerCharacterFormat.Special == textRangeFormat.Special && (double) this.MarkerCharacterFormat.CharacterSpacing == (double) textRangeFormat.CharacterSpacing && this.MarkerCharacterFormat.FieldVanish == textRangeFormat.FieldVanish && !(this.MarkerCharacterFormat.FontNameNonFarEast != textRangeFormat.FontNameNonFarEast) && this.MarkerCharacterFormat.Hidden == textRangeFormat.Hidden && this.MarkerCharacterFormat.ItalicBidi == textRangeFormat.ItalicBidi && (int) this.MarkerCharacterFormat.LocaleIdBidi == (int) textRangeFormat.LocaleIdBidi && this.MarkerCharacterFormat.NumberSpacing == textRangeFormat.NumberSpacing && this.MarkerCharacterFormat.UnderlineStyle == textRangeFormat.UnderlineStyle && this.MarkerCharacterFormat.BoldBidi == textRangeFormat.BoldBidi && this.MarkerCharacterFormat.TextureStyle == textRangeFormat.TextureStyle && (this.MarkerCharacterFormat.Border == null || textRangeFormat.Border == null || this.CompareBorderProperty(textRangeFormat.Border));
  }

  private bool CompareBorderProperty(Border textRangeBorder)
  {
    return this.MarkerCharacterFormat.Border.BorderPosition == textRangeBorder.BorderPosition && this.MarkerCharacterFormat.Border.BorderType == textRangeBorder.BorderType && !(this.MarkerCharacterFormat.Border.Color != textRangeBorder.Color) && (double) this.MarkerCharacterFormat.Border.LineWidth == (double) textRangeBorder.LineWidth && this.MarkerCharacterFormat.Border.Shadow == textRangeBorder.Shadow && (double) this.MarkerCharacterFormat.Border.Space == (double) textRangeBorder.Space;
  }

  internal void UpdateFtnMarker(string curMarker, string destMarker)
  {
    if (this.m_textBody.Items.Count == 0)
      return;
    WParagraph para = this.m_textBody.Items[0] as WParagraph;
    if (!string.IsNullOrEmpty(curMarker))
      this.ReplaceMarker(para.Find(curMarker, true, false), destMarker);
    else
      this.AppendMarker(destMarker, para);
    this.UpdateChangeFlag(false);
  }

  internal void EnsureFtnMarker()
  {
    if (this.m_changesCount <= (short) 0)
      return;
    if (!this.IsAutoNumbered && this.m_symbolCode > (byte) 0)
    {
      this.AppendFtnSymbol();
      this.UpdateChangeFlag(false);
    }
    else
    {
      string str = this.IsAutoNumbered ? '\u0002'.ToString() : this.CustomMarker;
      if (str.TrimStart(' ') != string.Empty)
        str = str.TrimStart(' ');
      WParagraph para = new WParagraph((IWordDocument) this.m_doc);
      if (this.m_textBody.Items.Count == 0)
      {
        this.AppendMarker(str, para);
        this.m_textBody.Items.Insert(0, (IEntity) para);
      }
      else
      {
        TextSelection selection = (TextSelection) null;
        foreach (WParagraph paragraph in (IEnumerable) this.m_textBody.Paragraphs)
        {
          selection = paragraph.Find(str, true, false);
          if (selection != null)
          {
            para = paragraph;
            break;
          }
        }
        if (selection == null)
        {
          if (this.m_textBody.Paragraphs.Count > 0)
            para = this.m_textBody.Paragraphs[0];
          else
            this.m_textBody.Items.Insert(0, (IEntity) para);
          this.AppendMarker(str, para);
        }
        else
          this.ReplaceMarker(selection, str);
      }
      this.UpdateChangeFlag(false);
    }
  }

  private void AppendFtnSymbol()
  {
    WTextRange symbol = this.GenerateSymbol(this.m_symbolCode);
    this.m_charFormat.FontName = this.m_strSymbolFontName;
    if (this.m_textBody.Items.Count == 0)
    {
      WParagraph wparagraph = new WParagraph((IWordDocument) this.m_doc);
      wparagraph.Items.Add((IEntity) symbol);
      wparagraph.AppendText(" ");
      this.m_textBody.Items.Insert(0, (IEntity) wparagraph);
    }
    else
    {
      string given = this.IsAutoNumbered ? '\u0002'.ToString() : this.CustomMarker;
      WParagraph para = this.m_textBody.Items[0] as WParagraph;
      TextSelection selection = (TextSelection) null;
      if (given != string.Empty)
        selection = para.Find(given, true, true);
      this.ReplaceSelection(symbol, para, selection);
    }
  }

  private WTextRange GenerateSymbol(byte symbolCode)
  {
    WTextRange symbol = new WTextRange((IWordDocument) this.Document);
    symbol.Text = ((char) symbolCode).ToString();
    symbol.CharacterFormat.ImportContainer((FormatBase) this.m_charFormat);
    symbol.CharacterFormat.FontName = this.m_strSymbolFontName;
    return symbol;
  }

  private void ReplaceSelection(WTextRange symbol, WParagraph para, TextSelection selection)
  {
    if (selection == null)
    {
      para.Items.Insert(0, (IEntity) symbol);
      para.Items.Insert(1, (IEntity) new WTextRange((IWordDocument) para.Document)
      {
        Text = " "
      });
    }
    else
    {
      WTextRange asOneRange = selection.GetAsOneRange();
      int inOwnerCollection = asOneRange.GetIndexInOwnerCollection();
      para.Items.Remove((IEntity) asOneRange);
      para.Items.Insert(inOwnerCollection, (IEntity) symbol);
    }
  }

  internal void EnsureFtnStyle()
  {
    if (this.Document.IsOpening)
      return;
    string charStyleName = this.m_charFormat.CharStyleName;
    Style style = (Style) null;
    if (!string.IsNullOrEmpty(charStyleName))
      style = this.Document.Styles.FindByName(charStyleName) as Style;
    if (style != null && (style.StyleId == 39 || style.StyleId == 38))
      return;
    WCharacterStyle wcharacterStyle = (WCharacterStyle) null;
    if (this.m_footnoteType == FootnoteType.Footnote)
      wcharacterStyle = (WCharacterStyle) Style.CreateBuiltinStyle(BuiltinStyle.FootnoteReference, StyleType.CharacterStyle, this.Document);
    else if (this.m_footnoteType == FootnoteType.Endnote)
      wcharacterStyle = (WCharacterStyle) Style.CreateBuiltinStyle(BuiltinStyle.EndnoteReference, StyleType.CharacterStyle, this.Document);
    if (wcharacterStyle == null)
      return;
    this.m_charFormat.CharStyleName = wcharacterStyle.Name;
    this.Document.Styles.Add((IStyle) wcharacterStyle);
  }

  private void UpdateChangeFlag(bool value)
  {
    if (this.Document.IsOpening)
      return;
    if (value)
      ++this.m_changesCount;
    else
      --this.m_changesCount;
  }

  private void ReplaceMarker(TextSelection selection, string replaceText)
  {
    if (selection == null)
      return;
    selection.GetAsOneRange().Text = replaceText;
  }

  private void AppendMarker(string marker, WParagraph para)
  {
    WTextRange text = this.GenerateText(marker);
    para.Items.Insert(0, (IEntity) text);
    para.Items.Insert(1, (IEntity) new WTextRange((IWordDocument) this.m_textBody.Document)
    {
      Text = " "
    });
  }

  internal WTextRange GenerateText(string marker)
  {
    WTextRange text = new WTextRange((IWordDocument) this.m_doc);
    text.Text = marker;
    text.CharacterFormat.ImportContainer((FormatBase) this.m_charFormat);
    return text;
  }

  SizeF ILeafWidget.Measure(DrawingContext dc) => ((IWidget) this).LayoutInfo.Size;

  void IWidget.InitLayoutInfo()
  {
    if (this.m_layoutInfo == null || this.FootnoteType == FootnoteType.Endnote)
      return;
    if (DocumentLayouter.m_footnoteId > 0)
    {
      DrawingContext drawingContext = DocumentLayouter.DrawingContext;
      --DocumentLayouter.m_footnoteId;
    }
    if (DocumentLayouter.m_footnoteIDRestartEachPage > 1)
      --DocumentLayouter.m_footnoteIDRestartEachPage;
    if (DocumentLayouter.m_footnoteIDRestartEachSection > 1)
      --DocumentLayouter.m_footnoteIDRestartEachSection;
    this.m_layoutInfo = (ILayoutInfo) null;
  }

  void IWidget.InitLayoutInfo(IWidget widget)
  {
  }
}
