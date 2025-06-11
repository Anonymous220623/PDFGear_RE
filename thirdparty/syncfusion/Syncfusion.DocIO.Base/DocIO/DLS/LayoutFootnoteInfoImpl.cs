// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.LayoutFootnoteInfoImpl
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS.Rendering;
using Syncfusion.DocIO.Rendering;
using Syncfusion.Layouting;

#nullable disable
namespace Syncfusion.DocIO.DLS;

internal class LayoutFootnoteInfoImpl : FootnoteLayoutInfo
{
  public LayoutFootnoteInfoImpl(WFootnote footnote)
    : base(ChildrenLayoutDirection.Horizontal)
  {
    WParagraph ownerParagraphValue = footnote.GetOwnerParagraphValue();
    DrawingContext drawingContext = DocumentLayouter.DrawingContext;
    int num;
    if (footnote.FootnoteType != FootnoteType.Endnote)
      num = ++DocumentLayouter.m_footnoteId;
    else
      DocumentLayouter.m_endnoteId = num = DocumentLayouter.m_endnoteId + 1;
    int id = num;
    if (this.GetBaseEntity((Entity) footnote) is WSection baseEntity)
    {
      if (footnote.FootnoteType == FootnoteType.Footnote)
      {
        if (baseEntity.PageSetup.RestartIndexForFootnotes == FootnoteRestartIndex.DoNotRestart)
          id += baseEntity.PageSetup.InitialFootnoteNumber - 1;
        else if (baseEntity.PageSetup.RestartIndexForFootnotes == FootnoteRestartIndex.RestartForEachPage)
          id = DocumentLayouter.m_footnoteIDRestartEachPage++;
        else if (baseEntity.PageSetup.RestartIndexForFootnotes == FootnoteRestartIndex.RestartForEachSection)
          id = DocumentLayouter.m_footnoteIDRestartEachSection++;
      }
      else if (baseEntity.PageSetup.RestartIndexForEndnote == EndnoteRestartIndex.DoNotRestart)
        id += baseEntity.PageSetup.InitialEndnoteNumber - 1;
      else if (baseEntity.PageSetup.RestartIndexForEndnote == EndnoteRestartIndex.RestartForEachSection)
        id = DocumentLayouter.m_footnoteIDRestartEachSection++;
    }
    if (footnote.CustomMarkerIsSymbol || footnote.CustomMarker != string.Empty)
      --DocumentLayouter.m_footnoteId;
    this.FootnoteID = this.GetFootnoteID(footnote, id);
    this.TextRange = footnote.GenerateText(this.FootnoteID);
    FormatBase baseFormat = footnote.GetCharFormat().BaseFormat;
    if (baseFormat != null)
      this.TextRange.CharacterFormat.ApplyBase(baseFormat);
    if (footnote.CustomMarkerIsSymbol && !footnote.MarkerCharacterFormat.HasValue(0) && footnote.SymbolFontName != string.Empty && footnote.SymbolFontName != footnote.MarkerCharacterFormat.FontName)
      this.TextRange.CharacterFormat.FontName = footnote.SymbolFontName;
    this.TextRange.SetOwner((OwnerHolder) ownerParagraphValue);
    this.Size = drawingContext.MeasureTextRange(this.TextRange, this.FootnoteID);
  }
}
