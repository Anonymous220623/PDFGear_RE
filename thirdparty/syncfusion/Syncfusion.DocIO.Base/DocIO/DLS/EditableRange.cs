// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.EditableRange
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

#nullable disable
namespace Syncfusion.DocIO.DLS;

internal class EditableRange
{
  private EditableRangeStart m_editableRangeStart;
  private EditableRangeEnd m_editableRangeEnd;

  internal string Id => this.m_editableRangeStart.Id;

  internal EditableRangeStart EditableRangeStart => this.m_editableRangeStart;

  internal EditableRangeEnd EditableRangeEnd => this.m_editableRangeEnd;

  internal EditableRange(EditableRangeStart start)
    : this(start, (EditableRangeEnd) null)
  {
  }

  internal EditableRange(EditableRangeStart start, EditableRangeEnd end)
  {
    this.m_editableRangeStart = start;
    this.m_editableRangeEnd = end;
  }

  internal void SetStart(EditableRangeStart start) => this.m_editableRangeStart = start;

  internal void SetEnd(EditableRangeEnd end) => this.m_editableRangeEnd = end;
}
