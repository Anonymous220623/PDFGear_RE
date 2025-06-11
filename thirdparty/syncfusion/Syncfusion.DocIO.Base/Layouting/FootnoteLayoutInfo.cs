// Decompiled with JetBrains decompiler
// Type: Syncfusion.Layouting.FootnoteLayoutInfo
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;

#nullable disable
namespace Syncfusion.Layouting;

internal class FootnoteLayoutInfo : LayoutInfo
{
  private string m_footnoteID;
  private WTextRange m_textRange;
  private float m_height;
  private float m_endnoteheight;

  internal float FootnoteHeight
  {
    get => this.m_height;
    set => this.m_height = value;
  }

  internal float Endnoteheight
  {
    get => this.m_endnoteheight;
    set => this.m_endnoteheight = value;
  }

  internal string FootnoteID
  {
    get => this.m_footnoteID;
    set => this.m_footnoteID = value;
  }

  internal WTextRange TextRange
  {
    get => this.m_textRange;
    set => this.m_textRange = value;
  }

  internal FootnoteLayoutInfo(ChildrenLayoutDirection childLayoutDirection)
    : base(childLayoutDirection)
  {
  }

  internal string GetFootnoteID(WFootnote footnote, int id)
  {
    WSection baseEntity = this.GetBaseEntity((Entity) footnote) as WSection;
    string footnoteId = id.ToString();
    if (baseEntity != null)
    {
      FootEndNoteNumberFormat numberFormat = baseEntity.PageSetup.FootnoteNumberFormat;
      if (footnote.FootnoteType == FootnoteType.Endnote)
        numberFormat = baseEntity.PageSetup.EndnoteNumberFormat;
      footnoteId = baseEntity.PageSetup.GetNumberFormatValue((byte) numberFormat, id);
      if (footnote.CustomMarkerIsSymbol)
        footnoteId = char.ConvertFromUtf32((int) footnote.SymbolCode);
      else if (footnote.m_strCustomMarker != string.Empty)
        footnoteId = footnote.m_strCustomMarker;
    }
    return footnoteId;
  }

  internal Entity GetBaseEntity(Entity entity)
  {
    Entity baseEntity = entity;
    while (!(baseEntity is WSection) && baseEntity.Owner != null)
      baseEntity = baseEntity.Owner;
    return baseEntity;
  }
}
