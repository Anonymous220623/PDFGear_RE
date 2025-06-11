// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.Break
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS.XML;
using Syncfusion.DocIO.Rendering;
using Syncfusion.Layouting;
using Syncfusion.Office;
using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class Break : ParagraphItem, ILeafWidget, IWidget
{
  private BreakType m_breakType;
  private WTextRange m_lineBreakText;
  internal HtmlToDocLayoutInfo m_htmlToDocLayoutInfo;

  internal HtmlToDocLayoutInfo HtmlToDocLayoutInfo
  {
    get
    {
      if (this.m_htmlToDocLayoutInfo == null)
        this.m_htmlToDocLayoutInfo = new HtmlToDocLayoutInfo();
      return this.m_htmlToDocLayoutInfo;
    }
  }

  public override EntityType EntityType => EntityType.Break;

  public BreakType BreakType => this.m_breakType;

  internal WTextRange TextRange
  {
    get
    {
      if (this.m_lineBreakText == null)
      {
        this.m_lineBreakText = new WTextRange((IWordDocument) this.Document);
        this.m_lineBreakText.SetOwner((OwnerHolder) this);
      }
      return this.m_lineBreakText;
    }
    set => this.m_lineBreakText = value;
  }

  internal WCharacterFormat CharacterFormat => this.TextRange.CharacterFormat;

  internal override int EndPos
  {
    get => base.EndPos + (this.m_lineBreakText != null ? this.m_lineBreakText.Text.Length : 0);
  }

  public Break(IWordDocument doc)
    : this(doc, BreakType.LineBreak)
  {
  }

  public Break(IWordDocument doc, BreakType breakType)
    : base((WordDocument) doc)
  {
    this.m_breakType = breakType;
  }

  internal override void Close()
  {
    if (this.m_lineBreakText != null)
    {
      this.m_lineBreakText.Close();
      this.m_lineBreakText = (WTextRange) null;
    }
    base.Close();
  }

  protected override object CloneImpl()
  {
    Break owner = (Break) base.CloneImpl();
    if (this.m_lineBreakText != null)
    {
      owner.m_lineBreakText = this.m_lineBreakText.Clone() as WTextRange;
      owner.m_lineBreakText.SetOwner((OwnerHolder) owner);
    }
    return (object) owner;
  }

  internal override void CloneRelationsTo(WordDocument doc, OwnerHolder nextOwner)
  {
    base.CloneRelationsTo(doc, nextOwner);
    if (this.m_lineBreakText == null)
      return;
    this.m_lineBreakText.CloneRelationsTo(doc, nextOwner);
  }

  internal override void AttachToParagraph(WParagraph paragraph, int itemPos)
  {
    base.AttachToParagraph(paragraph, itemPos);
    WParagraph ownerParagraphValue = this.GetOwnerParagraphValue();
    if (ownerParagraphValue == null || this.m_breakType != BreakType.LineBreak || this.m_lineBreakText == null)
      return;
    ownerParagraphValue.UpdateText((ParagraphItem) this, 0, this.m_lineBreakText.Text, true);
  }

  internal override void Detach()
  {
    base.Detach();
    WParagraph ownerParagraphValue = this.GetOwnerParagraphValue();
    if (ownerParagraphValue == null || this.m_breakType != BreakType.LineBreak || this.m_lineBreakText == null)
      return;
    ownerParagraphValue.UpdateText((ParagraphItem) this, this.m_lineBreakText.Text.Length, string.Empty, true);
  }

  protected override void WriteXmlAttributes(IXDLSAttributeWriter writer)
  {
    base.WriteXmlAttributes(writer);
    writer.WriteValue("type", (Enum) ParagraphItemType.Break);
    writer.WriteValue("BreakType", (Enum) this.BreakType);
  }

  protected override void ReadXmlAttributes(IXDLSAttributeReader reader)
  {
    base.ReadXmlAttributes(reader);
    this.m_breakType = (BreakType) reader.ReadEnum("BreakType", typeof (BreakType));
  }

  protected override void InitXDLSHolder()
  {
    this.XDLSHolder.AddElement("text-range", (object) this.TextRange);
  }

  protected override void CreateLayoutInfo()
  {
    switch (this.m_breakType)
    {
      case BreakType.PageBreak:
        this.m_layoutInfo = (ILayoutInfo) new ParagraphLayoutInfo(ChildrenLayoutDirection.Vertical, false);
        this.m_layoutInfo.IsPageBreakItem = true;
        break;
      case BreakType.ColumnBreak:
        this.m_layoutInfo = (ILayoutInfo) new ParagraphLayoutInfo(ChildrenLayoutDirection.Vertical, false);
        this.m_layoutInfo.IsPageBreakItem = true;
        break;
      case BreakType.LineBreak:
        this.m_layoutInfo = (ILayoutInfo) new ParagraphLayoutInfo(ChildrenLayoutDirection.Vertical, false);
        this.m_layoutInfo.IsLineBreak = true;
        break;
    }
    if (this.CharacterFormat.Hidden)
      this.m_layoutInfo.IsSkip = true;
    if (this.CharacterFormat.IsDeleteRevision && !this.Document.RevisionOptions.ShowDeletedText)
      this.m_layoutInfo.IsSkip = true;
    if (this.m_layoutInfo != null && !this.m_layoutInfo.IsLineBreak && this.IsPageBreakNeedToBeSkipped())
    {
      if ((this.Owner is WParagraph ? this.OwnerParagraph.GetOwnerEntity() : (Entity) null) is WTableCell)
        this.m_layoutInfo.IsSkip = true;
      else
        this.m_layoutInfo.IsLineBreak = true;
      this.m_layoutInfo.IsPageBreakItem = false;
    }
    else if (this.m_layoutInfo != null && this.OwnerParagraph != null && !this.OwnerParagraph.IsInCell && this.m_layoutInfo.IsLineBreak && this.TextRange.Text == ControlChar.CarriegeReturn)
    {
      WParagraph clonedParagraph = this.OwnerParagraph.Clone() as WParagraph;
      this.ApplyTableStyleFormatting(this.OwnerParagraph, clonedParagraph);
      clonedParagraph.ClearItems();
      clonedParagraph.m_layoutInfo = (ILayoutInfo) null;
      this.OwnerParagraph.OwnerTextBody.Items.Insert(this.OwnerParagraph.Index + 1, (IEntity) clonedParagraph);
      this.Document.IsSkipFieldDetach = true;
      while (this.Index + 1 < this.OwnerParagraph.Items.Count)
        clonedParagraph.Items.Add((IEntity) this.OwnerParagraph.Items[this.Index + 1]);
      this.Document.IsSkipFieldDetach = false;
      this.m_layoutInfo.IsSkip = true;
      ParagraphLayoutInfo layoutInfo = ((IWidget) this.OwnerParagraph).LayoutInfo as ParagraphLayoutInfo;
      layoutInfo.Margins.Bottom = 0.0f;
      layoutInfo.BottomMargin = 0.0f;
      (((IWidget) clonedParagraph).LayoutInfo as ParagraphLayoutInfo).Margins.Top = 0.0f;
      this.OwnerParagraph.Items.Remove((IEntity) this);
    }
    this.m_layoutInfo.Font = new SyncFont(this.CharacterFormat.GetFontToRender(FontScriptType.English));
  }

  internal override void InitLayoutInfo(Entity entity, ref bool isLastTOCEntry)
  {
    this.m_layoutInfo = (ILayoutInfo) null;
    if (this != entity)
      return;
    isLastTOCEntry = true;
  }

  private bool IsPageBreakNeedToBeSkipped()
  {
    Entity entity = (Entity) this;
    while (entity.Owner != null)
    {
      entity = entity.Owner;
      switch (entity)
      {
        case WTextBox _:
        case WFootnote _:
        case HeaderFooter _:
        case Shape _:
        case WTableCell _:
          return true;
        default:
          continue;
      }
    }
    return false;
  }

  SizeF ILeafWidget.Measure(DrawingContext dc)
  {
    SizeF sizeF = new SizeF();
    WParagraph wparagraph = this.OwnerParagraph;
    if (this.Owner is InlineContentControl || this.Owner is XmlParagraphItem)
      wparagraph = this.GetOwnerParagraphValue();
    ParagraphItemCollection paragraphItemCollection = wparagraph.Items;
    if (wparagraph.HasSDTInlineItem)
      paragraphItemCollection = wparagraph.GetParagraphItems();
    if (this.BreakType == BreakType.LineBreak)
      sizeF.Height = dc.MeasureString(" ", this != null ? ((IWidget) this).LayoutInfo.Font.GetFont(this.Document) : this.CharacterFormat.GetFontToRender(FontScriptType.English), (StringFormat) null).Height;
    bool flag = false;
    if (this.BreakType == BreakType.PageBreak)
      flag = this.BreakType == BreakType.PageBreak && paragraphItemCollection.Count == 1 && wparagraph.PreviousSibling != null && wparagraph.NextSibling != null && !this.m_doc.Settings.CompatibilityOptions[CompatibilityOption.SplitPgBreakAndParaMark];
    if (flag && (wparagraph.NextSibling is WParagraph && !(wparagraph.NextSibling as WParagraph).SectionEndMark || wparagraph.NextSibling is BlockContentControl || wparagraph.NextSibling is WTable))
      sizeF.Height = dc.MeasureString(" ", this != null ? ((IWidget) this).LayoutInfo.Font.GetFont(this.Document) : this.CharacterFormat.GetFontToRender(FontScriptType.English), (StringFormat) null).Height;
    return this.BreakType != BreakType.LineBreak && !flag || this.BreakType == BreakType.ColumnBreak ? SizeF.Empty : sizeF;
  }
}
