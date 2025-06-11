// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.WAbsoluteTab
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

internal class WAbsoluteTab : ParagraphItem, ILeafWidget, IWidget
{
  private AbsoluteTabAlignment m_alignment;
  private AbsoluteTabRelation m_relation;
  private TabLeader m_tabLeader;

  internal string Text
  {
    get => this.Alignment == AbsoluteTabAlignment.Left ? ControlChar.LineBreak : ControlChar.Tab;
  }

  internal float Position => this.GetTabPostion();

  public override EntityType EntityType => EntityType.AbsoluteTab;

  internal AbsoluteTabAlignment Alignment
  {
    get => this.m_alignment;
    set => this.m_alignment = value;
  }

  internal AbsoluteTabRelation Relation
  {
    get => this.m_relation;
    set => this.m_relation = value;
  }

  internal TabLeader TabLeader
  {
    get => this.m_tabLeader;
    set => this.m_tabLeader = value;
  }

  internal WCharacterFormat CharacterFormat
  {
    get => this.m_charFormat;
    set => this.m_charFormat = value;
  }

  internal WAbsoluteTab(IWordDocument doc)
    : base((WordDocument) doc)
  {
    this.m_charFormat = new WCharacterFormat((IWordDocument) this.Document);
    this.m_charFormat.SetOwner((OwnerHolder) this);
  }

  private float GetTabPostion()
  {
    return this.m_relation == AbsoluteTabRelation.Margin ? this.GetTabPostionRelativeToMargin() : this.GetTabPostionRelativeToIndent();
  }

  private float GetTabPostionRelativeToMargin()
  {
    float relativeToMargin = 0.0f;
    WParagraph wparagraph = this.OwnerParagraph ?? this.GetOwnerParagraphValue();
    Entity ownerEntity = wparagraph.GetOwnerEntity();
    switch (this.m_alignment)
    {
      case AbsoluteTabAlignment.Center:
        if (wparagraph.Owner.Owner is WSection)
        {
          relativeToMargin = (wparagraph.Owner.Owner as WSection).PageSetup.ClientWidth / 2f;
          break;
        }
        if (wparagraph.IsInCell)
        {
          relativeToMargin = this.GetCellWidth(ownerEntity as WTableCell) / 2f;
          break;
        }
        switch (ownerEntity)
        {
          case WTextBox _:
          case Shape _:
            relativeToMargin = (this.GetAbsoluteTabBaseEntity((Entity) this) as WSection).PageSetup.ClientWidth / 2f;
            break;
        }
        break;
      case AbsoluteTabAlignment.Right:
        if (wparagraph.Owner.Owner is WSection)
        {
          relativeToMargin = (wparagraph.Owner.Owner as WSection).PageSetup.ClientWidth;
          break;
        }
        if (wparagraph.IsInCell)
        {
          relativeToMargin = this.GetCellWidth(ownerEntity as WTableCell);
          break;
        }
        switch (ownerEntity)
        {
          case WTextBox _:
          case Shape _:
            relativeToMargin = (this.GetAbsoluteTabBaseEntity((Entity) this) as WSection).PageSetup.ClientWidth;
            break;
        }
        break;
    }
    return relativeToMargin;
  }

  private float GetTabPostionRelativeToIndent()
  {
    float relativeToIndent = 0.0f;
    WParagraph wparagraph = this.OwnerParagraph ?? this.GetOwnerParagraphValue();
    Entity ownerEntity = wparagraph.GetOwnerEntity();
    switch (this.m_alignment)
    {
      case AbsoluteTabAlignment.Center:
        if (wparagraph.Owner.Owner is WSection)
        {
          relativeToIndent = (float) (((double) (wparagraph.Owner.Owner as WSection).PageSetup.ClientWidth + (double) wparagraph.ParagraphFormat.LeftIndent) / 2.0);
          break;
        }
        if (wparagraph.IsInCell)
        {
          relativeToIndent = (float) (((double) this.GetCellWidth(ownerEntity as WTableCell) + (double) wparagraph.ParagraphFormat.LeftIndent) / 2.0);
          break;
        }
        switch (ownerEntity)
        {
          case WTextBox _:
          case Shape _:
            relativeToIndent = (float) (((double) (this.GetAbsoluteTabBaseEntity((Entity) this) as WSection).PageSetup.ClientWidth + (double) wparagraph.ParagraphFormat.LeftIndent) / 2.0);
            break;
        }
        break;
      case AbsoluteTabAlignment.Right:
        if (wparagraph.Owner.Owner is WSection)
        {
          relativeToIndent = (wparagraph.Owner.Owner as WSection).PageSetup.ClientWidth - wparagraph.ParagraphFormat.RightIndent;
          break;
        }
        if (wparagraph.IsInCell)
        {
          relativeToIndent = this.GetCellWidth(ownerEntity as WTableCell) - wparagraph.ParagraphFormat.RightIndent;
          break;
        }
        switch (ownerEntity)
        {
          case WTextBox _:
          case Shape _:
            relativeToIndent = (this.GetAbsoluteTabBaseEntity((Entity) this) as WSection).PageSetup.ClientWidth - wparagraph.ParagraphFormat.RightIndent;
            break;
        }
        break;
    }
    return relativeToIndent;
  }

  private Entity GetAbsoluteTabBaseEntity(Entity ent)
  {
    while (!(ent is WSection) && ent.Owner != null)
      ent = ent.Owner;
    return ent;
  }

  private float GetCellWidth(WTableCell tableCell)
  {
    float num = 0.0f;
    if ((double) tableCell.OwnerRow.OwnerTable.TableFormat.CellSpacing > 0.0)
      num = (float) Math.Round((double) tableCell.OwnerRow.OwnerTable.TableFormat.CellSpacing, 2) * 2f;
    return tableCell.Width - this.GetLeftPadding(tableCell) - this.GetRightPadding(tableCell) - num;
  }

  private float GetLeftPadding(WTableCell tableCell)
  {
    float leftPadding = tableCell.CellFormat.Paddings.Left;
    if (!tableCell.CellFormat.Paddings.HasKey(1) || (double) leftPadding == -0.05000000074505806)
      leftPadding = !tableCell.OwnerRow.RowFormat.Paddings.HasKey(1) ? (!tableCell.OwnerRow.OwnerTable.TableFormat.Paddings.HasKey(1) ? (tableCell.Document.ActualFormatType != FormatType.Doc ? (!(tableCell.OwnerRow.OwnerTable.GetStyle() is WTableStyle style) || !style.TableProperties.Paddings.HasKey(1) ? 5.4f : style.TableProperties.Paddings.Left) : 0.0f) : tableCell.OwnerRow.OwnerTable.TableFormat.Paddings.Left) : tableCell.OwnerRow.RowFormat.Paddings.Left;
    return leftPadding;
  }

  private float GetRightPadding(WTableCell tableCell)
  {
    float rightPadding = tableCell.CellFormat.Paddings.Right;
    if (!tableCell.CellFormat.Paddings.HasKey(4) || (double) rightPadding == -0.05000000074505806)
      rightPadding = !tableCell.OwnerRow.RowFormat.Paddings.HasKey(4) ? (!tableCell.OwnerRow.OwnerTable.TableFormat.Paddings.HasKey(4) ? (tableCell.Document.ActualFormatType != FormatType.Doc ? (!(tableCell.OwnerRow.OwnerTable.GetStyle() is WTableStyle style) || !style.TableProperties.Paddings.HasKey(4) ? 5.4f : style.TableProperties.Paddings.Right) : 0.0f) : tableCell.OwnerRow.OwnerTable.TableFormat.Paddings.Right) : tableCell.OwnerRow.RowFormat.Paddings.Right;
    return rightPadding;
  }

  internal override void AttachToParagraph(WParagraph paragraph, int itemPos)
  {
    base.AttachToParagraph(paragraph, itemPos);
    if (paragraph.ParagraphFormat.AbsoluteTab != null)
      return;
    paragraph.ParagraphFormat.AbsoluteTab = this;
  }

  internal override void Detach()
  {
    WParagraph wparagraph = this.OwnerParagraph ?? this.GetOwnerParagraphValue();
    wparagraph.ParagraphFormat.AbsoluteTab = (WAbsoluteTab) null;
    foreach (Entity childEntity in (CollectionImpl) wparagraph.ChildEntities)
    {
      if (childEntity is WAbsoluteTab)
      {
        wparagraph.ParagraphFormat.AbsoluteTab = childEntity as WAbsoluteTab;
        break;
      }
    }
    base.Detach();
  }

  protected override void WriteXmlAttributes(IXDLSAttributeWriter writer)
  {
  }

  protected override void ReadXmlAttributes(IXDLSAttributeReader reader)
  {
  }

  protected override void InitXDLSHolder()
  {
  }

  internal override void Close() => base.Close();

  protected override void CreateLayoutInfo()
  {
    this.m_layoutInfo = (ILayoutInfo) new TabsLayoutInfo(ChildrenLayoutDirection.Horizontal);
    (this.m_layoutInfo as TabsLayoutInfo).AddTab(this.GetLayoutTabPostion(), (Syncfusion.Layouting.TabJustification) this.Alignment, (Syncfusion.Layouting.TabLeader) this.TabLeader);
    this.m_layoutInfo.Font = new SyncFont(this.CharacterFormat.GetFontToRender(FontScriptType.English));
  }

  internal override void InitLayoutInfo(Entity entity, ref bool isLastTOCEntry)
  {
    this.m_layoutInfo = (ILayoutInfo) null;
    if (this != entity)
      return;
    isLastTOCEntry = true;
  }

  SizeF ILeafWidget.Measure(DrawingContext dc)
  {
    return dc.MeasureString(" ", this.CharacterFormat.GetFontToRender(FontScriptType.English), (StringFormat) null) with
    {
      Width = 0.0f
    };
  }

  private float GetLayoutTabPostion()
  {
    return this.GetAbsolutePosition((IEntity) this.GetAbsoluteTabBaseEntity((Entity) this.GetOwnerParagraphValue()), 0.0f);
  }

  internal float GetAbsolutePosition(IEntity ent, float position)
  {
    WParagraph ownerParagraphValue = this.GetOwnerParagraphValue();
    WTableCell ownerEntity = ownerParagraphValue.GetOwnerEntity() as WTableCell;
    switch (this.m_alignment)
    {
      case AbsoluteTabAlignment.Left:
      case AbsoluteTabAlignment.Right:
        switch (this.Relation)
        {
          case AbsoluteTabRelation.Margin:
            if (ownerParagraphValue.IsInCell)
            {
              position = (((IWidget) ownerEntity).LayoutInfo as CellLayoutInfo).CellContentLayoutingBounds.Width;
              break;
            }
            if (ent is WSection)
            {
              position = (ent as WSection).PageSetup.ClientWidth;
              break;
            }
            break;
          case AbsoluteTabRelation.Indent:
            if (ownerParagraphValue.IsInCell)
            {
              position = (((IWidget) ownerEntity).LayoutInfo as CellLayoutInfo).CellContentLayoutingBounds.Width - ownerParagraphValue.ParagraphFormat.RightIndent;
              break;
            }
            if (ent is WSection)
            {
              position = (ent as WSection).PageSetup.ClientWidth - ownerParagraphValue.ParagraphFormat.RightIndent;
              break;
            }
            break;
        }
        return position;
      case AbsoluteTabAlignment.Center:
        switch (this.Relation)
        {
          case AbsoluteTabRelation.Margin:
            if (ownerParagraphValue.IsInCell)
            {
              position = (((IWidget) ownerEntity).LayoutInfo as CellLayoutInfo).CellContentLayoutingBounds.Width / 2f;
              break;
            }
            if (ent is WSection)
            {
              position = (ent as WSection).PageSetup.ClientWidth / 2f;
              break;
            }
            break;
          case AbsoluteTabRelation.Indent:
            if (ownerParagraphValue.IsInCell)
            {
              position = (float) (((double) (((IWidget) ownerEntity).LayoutInfo as CellLayoutInfo).CellContentLayoutingBounds.Width + (double) ownerParagraphValue.ParagraphFormat.LeftIndent) / 2.0);
              break;
            }
            if (ent is WSection)
            {
              position = (float) (((double) (ent as WSection).PageSetup.ClientWidth + (double) ownerParagraphValue.ParagraphFormat.LeftIndent) / 2.0);
              break;
            }
            break;
        }
        return position;
      default:
        return position;
    }
  }

  void IWidget.InitLayoutInfo() => this.m_layoutInfo = (ILayoutInfo) null;

  void IWidget.InitLayoutInfo(IWidget widget)
  {
  }
}
