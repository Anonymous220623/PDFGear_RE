// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.BookmarkEnd
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS.XML;
using Syncfusion.DocIO.Rendering;
using Syncfusion.Layouting;
using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class BookmarkEnd : ParagraphItem, ILeafWidget, IWidget
{
  private string m_strName = string.Empty;
  private byte m_bFlags;
  private string m_displacedByCustomXml;

  public override EntityType EntityType => EntityType.BookmarkEnd;

  public string Name => this.m_strName;

  internal bool IsCellGroupBkmk
  {
    get => ((int) this.m_bFlags & 1) != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 254 | (value ? 1 : 0));
  }

  internal bool IsDetached
  {
    get => ((int) this.m_bFlags & 2) >> 1 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 253 | (value ? 1 : 0) << 1);
  }

  internal bool IsAfterParagraphMark
  {
    get => ((int) this.m_bFlags & 4) >> 2 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 251 | (value ? 1 : 0) << 2);
  }

  internal bool IsAfterCellMark
  {
    get => ((int) this.m_bFlags & 8) >> 3 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 247 | (value ? 1 : 0) << 3);
  }

  internal bool IsAfterRowMark
  {
    get => ((int) this.m_bFlags & 16 /*0x10*/) >> 4 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 239 | (value ? 1 : 0) << 4);
  }

  internal bool IsAfterTableMark
  {
    get => ((int) this.m_bFlags & 32 /*0x20*/) >> 5 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 223 | (value ? 1 : 0) << 5);
  }

  internal string DisplacedByCustomXml
  {
    get => this.m_displacedByCustomXml;
    set => this.m_displacedByCustomXml = value;
  }

  internal BookmarkEnd(WordDocument doc)
    : this((IWordDocument) doc, string.Empty)
  {
  }

  public BookmarkEnd(IWordDocument document, string name)
    : base((WordDocument) document)
  {
    this.SetName(name);
  }

  internal override void Close()
  {
    if (!string.IsNullOrEmpty(this.m_strName))
      this.m_strName = string.Empty;
    base.Close();
  }

  internal void SetName(string name) => this.m_strName = name.Replace('-', '_');

  internal override void AttachToParagraph(WParagraph owner, int itemPos)
  {
    base.AttachToParagraph(owner, itemPos);
    if (!this.DeepDetached)
    {
      this.Document.Bookmarks.AttachBookmarkEnd(this);
      this.IsDetached = false;
    }
    else
      this.IsDetached = true;
  }

  internal override void Detach()
  {
    base.Detach();
    if (this.DeepDetached)
      return;
    this.Document.Bookmarks.FindByName(this.Name)?.SetEnd((BookmarkEnd) null);
  }

  internal override void AttachToDocument()
  {
    if (!this.IsDetached)
      return;
    this.Document.Bookmarks.AttachBookmarkEnd(this);
    this.IsDetached = false;
  }

  protected override object CloneImpl()
  {
    BookmarkEnd bookmarkEnd = (BookmarkEnd) base.CloneImpl();
    bookmarkEnd.IsDetached = true;
    return (object) bookmarkEnd;
  }

  internal bool HasRenderableItemBefore()
  {
    WParagraph ownerParagraph = this.OwnerParagraph;
    for (int index = 0; ownerParagraph != null && this.Index != -1 && index < this.Index; ++index)
    {
      switch (ownerParagraph.ChildEntities[index])
      {
        case BookmarkStart _:
        case BookmarkEnd _:
        case EditableRangeStart _:
        case EditableRangeEnd _:
          continue;
        default:
          return true;
      }
    }
    return false;
  }

  protected override void WriteXmlAttributes(IXDLSAttributeWriter writer)
  {
    base.WriteXmlAttributes(writer);
    writer.WriteValue("type", (Enum) ParagraphItemType.BookmarkEnd);
    writer.WriteValue("BookmarkName", this.Name);
    if (!this.IsCellGroupBkmk)
      return;
    writer.WriteValue("IsCellGroupBkmk", this.IsCellGroupBkmk);
  }

  protected override void ReadXmlAttributes(IXDLSAttributeReader reader)
  {
    base.ReadXmlAttributes(reader);
    this.m_strName = reader.ReadString("BookmarkName");
    this.Document.Bookmarks.AttachBookmarkEnd(this);
    if (!reader.HasAttribute("IsCellGroupBkmk"))
      return;
    this.IsCellGroupBkmk = reader.ReadBoolean("IsCellGroupBkmk");
  }

  protected override void CreateLayoutInfo()
  {
    this.m_layoutInfo = (ILayoutInfo) new LayoutInfo(ChildrenLayoutDirection.Horizontal);
    this.m_layoutInfo.IsClipped = ((IWidget) this.GetOwnerParagraphValue()).LayoutInfo.IsClipped;
  }

  SizeF ILeafWidget.Measure(DrawingContext dc)
  {
    SizeF sizeF = new SizeF();
    WParagraph ownerParagraphValue = this.GetOwnerParagraphValue();
    if (ownerParagraphValue.IsNeedToMeasureBookMarkSize)
      sizeF.Height = ((IWidget) ownerParagraphValue).LayoutInfo.Size.Height;
    return sizeF;
  }

  internal override void InitLayoutInfo(Entity entity, ref bool isLastTOCEntry)
  {
    this.m_layoutInfo = (ILayoutInfo) null;
    if (this != entity)
      return;
    isLastTOCEntry = true;
  }
}
