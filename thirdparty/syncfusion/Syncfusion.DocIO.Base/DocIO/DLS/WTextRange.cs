// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.WTextRange
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS.Rendering;
using Syncfusion.DocIO.DLS.XML;
using Syncfusion.DocIO.Rendering;
using Syncfusion.Layouting;
using Syncfusion.Office;
using System.Drawing;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class WTextRange : 
  ParagraphItem,
  IWTextRange,
  IParagraphItem,
  IEntity,
  IStringWidget,
  ISplitLeafWidget,
  ILeafWidget,
  IWidget,
  ITextMeasurable
{
  private int m_txtLength;
  private string m_detachedText = string.Empty;
  private string m_originalText = string.Empty;
  private float m_ascent = float.MinValue;
  private byte m_bFlags;
  private CharacterRangeType m_charRangeType;
  private FontScriptType m_fontScriptType;

  public override EntityType EntityType => EntityType.TextRange;

  public virtual string Text
  {
    get
    {
      if (!this.ItemDetached && this.Owner is WParagraph && this.IsDetachedTextChanged)
      {
        this.m_detachedText = this.OwnerParagraph.Text.Substring(this.StartPos, this.m_txtLength);
        this.IsDetachedTextChanged = false;
      }
      else if (!this.ItemDetached && this.Owner is InlineContentControl && !this.IsMappedItem && this.IsDetachedTextChanged)
      {
        WParagraph ownerParagraphValue = this.GetOwnerParagraphValue();
        if (ownerParagraphValue != null)
        {
          this.m_detachedText = ownerParagraphValue.Text.Substring(this.StartPos, this.m_txtLength);
          this.IsDetachedTextChanged = false;
        }
      }
      return this.m_detachedText;
    }
    set
    {
      WParagraph wparagraph = !(this.Owner is InlineContentControl) || this.IsMappedItem ? this.OwnerParagraph : this.GetOwnerParagraphValue();
      if (this.ItemDetached || wparagraph == null)
        this.m_detachedText = value;
      else if (value != this.Text)
      {
        wparagraph.UpdateText(this, value, true);
        this.m_txtLength = value.Length;
        this.IsDetachedTextChanged = true;
      }
      this.SafeText = this.Document.IsOpening;
    }
  }

  internal string OrignalText
  {
    get => this.m_originalText;
    set => this.m_originalText = value;
  }

  public WCharacterFormat CharacterFormat
  {
    get => this.m_charFormat;
    internal set => this.m_charFormat = value;
  }

  internal int TextLength
  {
    get => !this.ItemDetached ? this.m_txtLength : this.m_detachedText.Length;
    set
    {
      if (this.m_txtLength != value || this.Owner is WParagraph && this.OwnerParagraph.IsTextReplaced || this.Owner is WParagraph && this.StartPos >= 0 && this.StartPos + value <= this.OwnerParagraph.Text.Length && this.m_detachedText != this.OwnerParagraph.Text.Substring(this.StartPos, value))
        this.IsDetachedTextChanged = true;
      this.m_txtLength = value;
    }
  }

  internal CharacterRangeType CharacterRange
  {
    get => this.m_charRangeType;
    set => this.m_charRangeType = value;
  }

  internal FontScriptType ScriptType
  {
    get => this.m_fontScriptType;
    set => this.m_fontScriptType = value;
  }

  internal override int EndPos => base.EndPos + this.m_txtLength;

  internal bool SafeText
  {
    get => ((int) this.m_bFlags & 1) != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 254 | (value ? 1 : 0));
  }

  internal bool IsParagraphMark
  {
    get
    {
      return this.Owner == null && this.CharacterFormat != null && this.CharacterFormat.BaseFormat != null && this.CharacterFormat.BaseFormat.OwnerBase is WParagraph;
    }
  }

  public WTextRange(IWordDocument doc)
    : base((WordDocument) doc)
  {
    this.m_charFormat = new WCharacterFormat((IWordDocument) this.Document, (Entity) this);
  }

  internal override void AttachToParagraph(WParagraph paragraph, int itemPos)
  {
    this.m_txtLength = 0;
    this.IsDetachedTextChanged = true;
    base.AttachToParagraph(paragraph, itemPos);
    this.Text = this.m_detachedText;
  }

  internal void Attach(WParagraph paragraph, int itemPos, bool isField)
  {
    this.m_txtLength = 0;
    base.AttachToParagraph(paragraph, itemPos);
  }

  internal void InsertTextInParagraphText(WParagraph paragraph)
  {
    paragraph.UpdateText(this, this.m_detachedText, false);
    this.m_txtLength = this.m_detachedText.Length;
  }

  internal override void Detach()
  {
    base.Detach();
    WParagraph ownerParagraphValue = this.GetOwnerParagraphValue();
    if (ownerParagraphValue == null)
      return;
    this.m_detachedText = this.Text;
    ownerParagraphValue.UpdateText(this, string.Empty, true);
  }

  internal void Detach(bool isField) => base.Detach();

  protected override object CloneImpl()
  {
    WTextRange wtextRange = (WTextRange) base.CloneImpl();
    wtextRange.m_detachedText = this.Text;
    return (object) wtextRange;
  }

  internal object CloneImpl(bool isField) => (object) (WTextRange) base.CloneImpl();

  public void ApplyCharacterFormat(WCharacterFormat charFormat)
  {
    if (charFormat == null)
      return;
    this.SetParagraphItemCharacterFormat(charFormat);
  }

  internal void SplitWidgets()
  {
    this.SplitByTab();
    this.SplitByParagraphBreak();
  }

  private void SplitByTab()
  {
    string empty = string.Empty;
    if (!(this.Text != ControlChar.Tab) || !this.Text.Contains(ControlChar.Tab))
      return;
    int num1 = this.Text.IndexOf(ControlChar.Tab);
    string text = this.Text;
    ParagraphItemCollection paragraphItemCollection = this.Owner is InlineContentControl ? (this.Owner as InlineContentControl).ParagraphItems : this.OwnerParagraph.Items;
    int num2 = paragraphItemCollection.IndexOf((IEntity) this);
    string str = text.Substring(num1 + 1);
    WTextRange wtextRange = this.Clone() as WTextRange;
    if (num1 > 0)
    {
      wtextRange.Text = text.Substring(num1);
      this.Text = text.Substring(0, num1);
    }
    else if (str != string.Empty)
    {
      wtextRange.Text = str;
      this.Text = ControlChar.Tab;
    }
    paragraphItemCollection.Insert(num2 + 1, (IEntity) wtextRange);
  }

  internal override void Close()
  {
    if (this.m_detachedText != null)
      this.m_detachedText = (string) null;
    base.Close();
  }

  private void SplitByParagraphBreak()
  {
    string empty = string.Empty;
    string str1 = this.Text.Replace(ControlChar.CrLf, ControlChar.ParagraphBreak).Replace(ControlChar.LineFeedChar, '\r');
    if (!str1.Contains(ControlChar.ParagraphBreak))
      return;
    int length = str1.IndexOf(ControlChar.ParagraphBreak);
    string str2 = str1.Substring(length + 1);
    WTextRange wtextRange = this.Clone() as WTextRange;
    if (this.ParaItemCharFormat.TableStyleCharacterFormat != null)
      wtextRange.ParaItemCharFormat.TableStyleCharacterFormat = this.ParaItemCharFormat.TableStyleCharacterFormat;
    if (length > 0)
    {
      wtextRange.Text = str1.Substring(length + 1);
      this.Text = str1.Substring(0, length);
    }
    else if (str2 != string.Empty)
    {
      wtextRange.Text = str2;
      this.Text = string.Empty;
    }
    if (this.Owner is InlineContentControl)
    {
      ParagraphItemCollection paragraphItems = (this.Owner as InlineContentControl).ParagraphItems;
      int num1 = paragraphItems.IndexOf((IEntity) this);
      this.Document.IsSkipFieldDetach = true;
      Break @break = new Break((IWordDocument) this.Document, BreakType.LineBreak);
      @break.TextRange.Text = ControlChar.LineBreak;
      @break.TextRange.CharacterFormat.ImportContainer((FormatBase) wtextRange.CharacterFormat);
      @break.TextRange.CharacterFormat.CopyProperties((FormatBase) wtextRange.CharacterFormat);
      int num2;
      paragraphItems.Insert(num2 = num1 + 1, (IEntity) @break);
      int num3;
      paragraphItems.Insert(num3 = num2 + 1, (IEntity) wtextRange);
      if (str1 == ControlChar.ParagraphBreak)
      {
        this.Text = string.Empty;
        wtextRange.Text = string.Empty;
      }
    }
    else
    {
      WParagraph clonedParagraph = this.OwnerParagraph.Clone() as WParagraph;
      this.ApplyTableStyleFormatting(this.OwnerParagraph, clonedParagraph);
      clonedParagraph.ClearItems();
      clonedParagraph.m_layoutInfo = (ILayoutInfo) null;
      this.OwnerParagraph.OwnerTextBody.Items.Insert(this.OwnerParagraph.GetIndexInOwnerCollection() + 1, (IEntity) clonedParagraph);
      clonedParagraph.Items.Add((IEntity) wtextRange);
      if (str1 == ControlChar.ParagraphBreak)
      {
        this.Text = string.Empty;
        wtextRange.Text = string.Empty;
      }
      int num = this.OwnerParagraph.Items.IndexOf((IEntity) this);
      this.Document.IsSkipFieldDetach = true;
      while (num + 1 < this.OwnerParagraph.Items.Count)
        clonedParagraph.Items.Add((IEntity) this.OwnerParagraph.Items[num + 1]);
    }
    this.Document.IsSkipFieldDetach = false;
  }

  protected override void InitXDLSHolder()
  {
    this.XDLSHolder.AddElement("character-format", (object) this.m_charFormat);
  }

  protected override void WriteXmlContent(IXDLSContentWriter writer)
  {
    base.WriteXmlContent(writer);
    writer.WriteChildStringElement("text", this.Text);
  }

  protected override bool ReadXmlContent(IXDLSContentReader reader)
  {
    if (!(reader.TagName == "text"))
      return false;
    if (this.Owner is WParagraph)
      this.StartPos = this.OwnerParagraph.Text.Length;
    this.Text = reader.ReadChildStringContent();
    if (this.Text == "")
      reader.InnerReader.Read();
    this.SafeText = true;
    return true;
  }

  protected override void CreateLayoutInfo()
  {
    this.SplitWidgets();
    this.m_layoutInfo = this.Text == ControlChar.Tab ? (ILayoutInfo) new WTextRange.LayoutTabInfo(this) : (ILayoutInfo) new LayoutInfo(ChildrenLayoutDirection.Horizontal);
    WCharacterFormat charFormat = this.CharacterFormat;
    string str = this.Text.Trim(' ');
    if (this.IsDeleteRevision && !this.Document.RevisionOptions.ShowDeletedText)
      this.m_layoutInfo.IsSkip = true;
    if (str == string.Empty && this.Owner is WParagraph && this.OwnerParagraph.Text == this.Text)
      charFormat = this.OwnerParagraph.BreakCharacterFormat;
    this.m_layoutInfo.Font = new SyncFont(DocumentLayouter.DrawingContext.GetFont(this, charFormat, this.Text));
    if (!(this.Owner is WParagraph))
    {
      WParagraph wparagraph = this.Owner is InlineContentControl || this.Owner is XmlParagraphItem ? this.GetOwnerParagraphValue() : (this.CharacterFormat.BaseFormat == null ? (this.Owner as Break).OwnerParagraph : this.CharacterFormat.BaseFormat.OwnerBase as WParagraph);
      if (wparagraph != null)
        this.m_layoutInfo.IsVerticalText = wparagraph.IsVerticalText();
      if (wparagraph != null && wparagraph.SectionEndMark && wparagraph.PreviousSibling != null)
        this.m_layoutInfo.IsSkip = true;
    }
    else
      this.m_layoutInfo.IsVerticalText = this.OwnerParagraph.IsVerticalText();
    if (this.CharacterFormat.Hidden && this.OwnerParagraph != null)
      this.m_layoutInfo.IsSkip = true;
    if (this.Text == "" && !(this.m_layoutInfo is WTextRange.LayoutTabInfo) && !DocumentLayouter.DrawingContext.IsTOC(this) && !(this.PreviousSibling is Break))
      this.m_layoutInfo.IsSkip = true;
    this.m_layoutInfo.Size = this.GetTextRangeSize((WTextRange) null);
  }

  internal override void InitLayoutInfo(Entity entity, ref bool isLastTOCEntry)
  {
    this.m_layoutInfo = (ILayoutInfo) null;
    if (this != entity)
      return;
    isLastTOCEntry = true;
  }

  internal SizeF GetTextRangeSize(WTextRange clonedTextRange)
  {
    DrawingContext drawingContext = DocumentLayouter.DrawingContext;
    string text = this.Text;
    WParagraph ownerParagraphValue = this.GetOwnerParagraphValue();
    if (text == '\u0002'.ToString() && ownerParagraphValue.OwnerTextBody.Owner is WFootnote)
      text = ((ownerParagraphValue.OwnerTextBody.Owner as IWidget).LayoutInfo as FootnoteLayoutInfo).FootnoteID;
    SizeF sizeF = new SizeF();
    if (this.Text.Equals(string.Empty) || this.IsSpaceWidthSetToZero(ownerParagraphValue, text))
      return drawingContext.MeasureTextRange(clonedTextRange == null ? this : clonedTextRange, " ") with
      {
        Width = 0.0f
      };
    if (this.Owner is WParagraph && this.GetIndexInOwnerCollection() == this.OwnerParagraph.Items.Count - 1 && this.Text.Trim() != string.Empty && !(this.Owner as WParagraph).ParagraphFormat.Bidi)
      text = this.Text.TrimEnd();
    return drawingContext.MeasureTextRange(clonedTextRange == null ? this : clonedTextRange, clonedTextRange == null ? text : clonedTextRange.Text);
  }

  void IWidget.InitLayoutInfo()
  {
    if (this.OrignalText != string.Empty)
      this.Text = this.OrignalText;
    if (this.m_layoutInfo is WTextRange.LayoutTabInfo && this.Text == string.Empty)
      this.Text = ControlChar.Tab;
    this.m_layoutInfo = (ILayoutInfo) null;
  }

  void IWidget.InitLayoutInfo(IWidget widget)
  {
  }

  SizeF ITextMeasurable.Measure(string text)
  {
    DrawingContext drawingContext = new DrawingContext();
    if (!(text != this.Text))
      return ((IWidget) this).LayoutInfo.Size;
    if (text == null || !(text == string.Empty) && !this.IsLastTextRangeWithSpace(text))
      return drawingContext.MeasureTextRange(this, text);
    return drawingContext.MeasureTextRange(this, " ") with
    {
      Width = 0.0f
    };
  }

  SizeF ITextMeasurable.Measure(DrawingContext dc, string text)
  {
    if (!(text != this.Text))
      return ((IWidget) this).LayoutInfo.Size;
    if (text == null || !(text == string.Empty) && !this.IsLastTextRangeWithSpace(text))
      return dc.MeasureTextRange(this, text);
    return dc.MeasureTextRange(this, " ") with
    {
      Width = 0.0f
    };
  }

  double IStringWidget.GetTextAscent(DrawingContext dc, ref float exceededLineAscent)
  {
    if ((double) this.m_ascent == -3.4028234663852886E+38)
    {
      Font font = this.GetFont();
      this.m_ascent = dc.GetAscent(font);
    }
    string fontNameToRender = this.CharacterFormat.GetFontNameToRender(this.ScriptType);
    if (fontNameToRender == "Arial Unicode MS" || fontNameToRender == "Lucida Sans Unicode")
    {
      if (this.Text.Trim(' ') != "" && !this.CharacterFormat.ComplexScript)
      {
        exceededLineAscent = dc.GetExceededLineHeightForArialUnicodeMSFont(this.GetFont(), true) - this.m_ascent;
        if (fontNameToRender == "Lucida Sans Unicode")
          exceededLineAscent /= 2f;
      }
    }
    return (double) this.m_ascent;
  }

  private Font GetFont()
  {
    WCharacterFormat wcharacterFormat = this.CharacterFormat;
    if (this is WField && ((this as WField).FieldType == FieldType.FieldPage || (this as WField).FieldType == FieldType.FieldNumPages || (this as WField).FieldType == FieldType.FieldAutoNum))
      wcharacterFormat = (this as WField).GetCharacterFormatValue();
    Font font = wcharacterFormat.GetFontToRender(this.ScriptType);
    if (this is WCheckBox)
      font = ((IWidget) this).LayoutInfo.Font.GetFont(this.Document);
    return this.Document.FontSettings.GetFont(wcharacterFormat.GetFontNameFromHint(this.ScriptType), font.Size, font.Style);
  }

  int IStringWidget.OffsetToIndex(
    DrawingContext dc,
    double offset,
    string text,
    float clientWidth,
    float clientActiveAreaWidth,
    bool isSplitByCharacter)
  {
    float clientWidth1 = this.GetClientWidth(dc, clientWidth);
    bool flag = !(this.m_layoutInfo is ParagraphLayoutInfo layoutInfo) || layoutInfo.TextWrap;
    return dc.GetSplitIndexByOffset(text, (ITextMeasurable) this, offset, !flag, this.GetOwnerParagraphValue().IsInCell, clientWidth1, clientActiveAreaWidth, isSplitByCharacter);
  }

  internal float GetClientWidth(DrawingContext dc, float clientWidth)
  {
    float clientWidth1 = 0.0f;
    if (this.Owner != null)
    {
      Entity owner = this.Owner;
      bool flag = false;
      while (!(owner is WSection) && (!(owner is WTable) && !(owner is Shape) && !(owner is ChildShape) && !(owner is WTextBox) && (!(owner is WParagraph) || (double) (owner as WParagraph).ParagraphFormat.FrameWidth == 0.0 || (owner as WParagraph).OwnerTextBody.Owner == null || (owner as WParagraph).OwnerTextBody.Owner is WTextBox || (owner as WParagraph).OwnerTextBody.Owner is Shape || (owner as WParagraph).OwnerTextBody.Owner is ChildShape || (owner as WParagraph).IsInCell) || flag) && owner.Owner != null)
      {
        owner = owner.Owner;
        if (owner is WFootnote)
          flag = true;
      }
      switch (owner)
      {
        case WSection _:
          ParagraphLayoutInfo layoutInfo = ((IWidget) this.GetOwnerParagraphValue()).LayoutInfo as ParagraphLayoutInfo;
          clientWidth1 = clientWidth - (float) ((double) layoutInfo.Margins.Left + (double) layoutInfo.Margins.Right + (layoutInfo.IsFirstLine ? (double) layoutInfo.FirstLineIndent + (double) layoutInfo.ListTab : 0.0));
          break;
        case WTable _:
          clientWidth1 = dc.GetCellWidth((ParagraphItem) this);
          break;
        case Shape _:
          clientWidth1 = (owner as Shape).TextLayoutingBounds.Width;
          break;
        case WTextBox _:
          clientWidth1 = (owner as WTextBox).TextLayoutingBounds.Width;
          break;
        case ChildShape _:
          clientWidth1 = (owner as ChildShape).TextLayoutingBounds.Width;
          break;
        case WParagraph _ when (owner as WParagraph).ParagraphFormat.IsInFrame():
          clientWidth1 = (owner as WParagraph).ParagraphFormat.FrameWidth;
          break;
      }
    }
    return clientWidth1;
  }

  ISplitLeafWidget[] ISplitLeafWidget.SplitBySize(
    DrawingContext dc,
    SizeF offset,
    float clientWidth,
    float clientActiveAreaWidth,
    ref bool isLastWordFit,
    bool isTabStopInterSectingfloattingItem,
    bool isSplitByCharacter,
    bool isFirstItemInLine,
    ref int countForConsecutivelimit)
  {
    return SplitStringWidget.SplitBySize(dc, (double) offset.Width, (IStringWidget) this, (SplitStringWidget) null, clientWidth, clientActiveAreaWidth, ref isLastWordFit, isTabStopInterSectingfloattingItem, isSplitByCharacter, isFirstItemInLine, ref countForConsecutivelimit);
  }

  SizeF ILeafWidget.Measure(DrawingContext dc) => ((IWidget) this).LayoutInfo.Size;

  private bool IsLastTextRangeWithSpace(string text)
  {
    text = text.Trim(ControlChar.SpaceChar);
    if (!(text == string.Empty) || ((IWidget) this).LayoutInfo is TabsLayoutInfo)
      return false;
    if (!(this.NextSibling is Entity nextSibling) && this.Owner is InlineContentControl)
      nextSibling = this.Owner.NextSibling as Entity;
    do
    {
      if (!(nextSibling is WTextRange))
        goto label_6;
      goto label_5;
label_3:
      WParagraph ownerParagraphValue = this.GetOwnerParagraphValue();
      nextSibling = ownerParagraphValue != null ? ownerParagraphValue.GetNextSibling(nextSibling as IWidget) as Entity : (Entity) null;
      continue;
label_5:
      if ((nextSibling as WTextRange).Text.Trim(ControlChar.SpaceChar) == string.Empty && !((nextSibling as WTextRange).m_layoutInfo is TabsLayoutInfo) && !(nextSibling is WField))
        goto label_3;
label_6:
      if (nextSibling is BookmarkStart || nextSibling is BookmarkEnd || nextSibling is WFieldMark)
        goto label_3;
      break;
    }
    while (nextSibling != null);
    return nextSibling == null;
  }

  private bool IsSpaceWidthSetToZero(WParagraph ownerPara, string text)
  {
    bool zero = this.IsLastTextRangeWithSpace(text);
    if (ownerPara == null || !ownerPara.ParagraphFormat.Bidi || this.CharacterFormat.Bidi)
      return zero;
    return ownerPara.Text.Trim(ControlChar.SpaceChar).Equals(string.Empty) && zero;
  }

  internal class LayoutTabInfo : TabsLayoutInfo
  {
    public LayoutTabInfo(WTextRange textRange)
      : base(ChildrenLayoutDirection.Horizontal)
    {
      textRange.Text = string.Empty;
      WParagraph wparagraph = textRange.OwnerParagraph;
      if (textRange.Owner is InlineContentControl || textRange.Owner is XmlParagraphItem)
        wparagraph = textRange.GetOwnerParagraphValue();
      this.m_defaultTabWidth = wparagraph.GetDefaultTabWidth();
      WParagraphFormat paragraphFormat = wparagraph.ParagraphFormat;
      if (wparagraph.GetStyle() == null && !(textRange.Document.Styles.FindByName("Normal", StyleType.ParagraphStyle) is IWParagraphStyle))
      {
        IWParagraphStyle builtinStyle = (IWParagraphStyle) Style.CreateBuiltinStyle(BuiltinStyle.Normal, textRange.Document);
      }
      if (this.m_list.Count != 0)
        return;
      this.SortParagraphTabsCollection(paragraphFormat, (TabCollection) null, 0);
    }
  }
}
