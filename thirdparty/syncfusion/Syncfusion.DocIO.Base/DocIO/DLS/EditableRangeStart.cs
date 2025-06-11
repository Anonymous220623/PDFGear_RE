// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.EditableRangeStart
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.Layouting;

#nullable disable
namespace Syncfusion.DocIO.DLS;

internal class EditableRangeStart : ParagraphItem
{
  private string m_id = string.Empty;
  private short m_colFirst = -1;
  private short m_colLast = -1;
  private string m_ed = string.Empty;
  private string m_edGrp = string.Empty;
  private byte m_bFlags;

  public override EntityType EntityType => EntityType.EditableRangeStart;

  internal string Id => this.m_id;

  internal short ColumnFirst
  {
    get => this.m_colFirst;
    set => this.m_colFirst = value;
  }

  internal short ColumnLast
  {
    get => this.m_colLast;
    set => this.m_colLast = value;
  }

  internal string Ed
  {
    get => this.m_ed;
    set => this.m_ed = value;
  }

  internal string EdGrp
  {
    get => this.m_edGrp;
    set => this.m_edGrp = value;
  }

  internal bool IsDetached
  {
    get => ((int) this.m_bFlags & 2) >> 1 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 253 | (value ? 1 : 0) << 1);
  }

  internal EditableRangeStart(WordDocument doc)
    : this((IWordDocument) doc, string.Empty)
  {
  }

  internal EditableRangeStart(IWordDocument doc, string id)
    : base((WordDocument) doc)
  {
    this.SetId(id);
  }

  internal void SetId(string id) => this.m_id = id;

  internal override void AttachToParagraph(WParagraph owner, int itemPos)
  {
    base.AttachToParagraph(owner, itemPos);
    if (!this.DeepDetached)
    {
      this.Document.EditableRanges.AttachEditableRangeStart(this);
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
    EditableRangeCollection editableRanges = this.Document.EditableRanges;
    EditableRange byId = editableRanges.FindById(this.Id);
    if (byId == null)
      return;
    byId.SetStart((EditableRangeStart) null);
    editableRanges.Remove(byId);
  }

  internal override void AttachToDocument()
  {
    if (!this.IsDetached)
      return;
    this.Document.EditableRanges.AttachEditableRangeStart(this);
    this.IsDetached = false;
  }

  protected override object CloneImpl()
  {
    EditableRangeStart editableRangeStart = (EditableRangeStart) base.CloneImpl();
    editableRangeStart.IsDetached = true;
    return (object) editableRangeStart;
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
