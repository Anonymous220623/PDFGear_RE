// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.EditableRangeEnd
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.Layouting;

#nullable disable
namespace Syncfusion.DocIO.DLS;

internal class EditableRangeEnd : ParagraphItem
{
  private string m_id = string.Empty;
  private byte m_bFlags;

  public override EntityType EntityType => EntityType.EditableRangeEnd;

  internal string Id => this.m_id;

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

  internal bool IsDetached
  {
    get => ((int) this.m_bFlags & 2) >> 1 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 253 | (value ? 1 : 0) << 1);
  }

  internal EditableRangeEnd(WordDocument doc)
    : this((IWordDocument) doc, string.Empty)
  {
  }

  internal EditableRangeEnd(IWordDocument document, string id)
    : base((WordDocument) document)
  {
    this.SetId(id);
  }

  internal void SetId(string id) => this.m_id = id;

  internal override void AttachToParagraph(WParagraph owner, int itemPos)
  {
    base.AttachToParagraph(owner, itemPos);
    if (!this.DeepDetached)
    {
      this.Document.EditableRanges.AttacheEditableRangeEnd(this);
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
    this.Document.EditableRanges.FindById(this.Id)?.SetEnd((EditableRangeEnd) null);
  }

  internal override void AttachToDocument()
  {
    if (!this.IsDetached)
      return;
    this.Document.EditableRanges.AttacheEditableRangeEnd(this);
    this.IsDetached = false;
  }

  protected override object CloneImpl()
  {
    EditableRangeEnd editableRangeEnd = (EditableRangeEnd) base.CloneImpl();
    editableRangeEnd.IsDetached = true;
    return (object) editableRangeEnd;
  }

  internal override void Close()
  {
    if (!string.IsNullOrEmpty(this.m_id))
      this.m_id = string.Empty;
    base.Close();
  }

  protected override void CreateLayoutInfo()
  {
    this.m_layoutInfo = (ILayoutInfo) new ParagraphLayoutInfo(ChildrenLayoutDirection.Horizontal);
    this.m_layoutInfo.IsSkip = true;
  }
}
