// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.NotesSize
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using System;

#nullable disable
namespace Syncfusion.Presentation;

internal class NotesSize : INotesSize
{
  private long _cx;
  private long _cy;
  private uint? _viewHeightPercentage;
  private uint? _viewWidthPercentage;
  private SplitterBarState _horizontalBarState;

  internal NotesSize()
  {
    this._cx = 6858000L;
    this._cy = 9144000L;
  }

  internal NotesSize(NotesSize notesSize)
  {
    this._cx = notesSize._cx;
    this._cy = notesSize._cy;
  }

  internal long CX => this._cx;

  internal long CY => this._cy;

  internal SplitterBarState HorizontalBarState
  {
    get => this._horizontalBarState;
    set => this._horizontalBarState = value;
  }

  public uint Height
  {
    get
    {
      return this._viewHeightPercentage.HasValue ? 100U - this._viewHeightPercentage.Value / 1000U : 0U;
    }
    set
    {
      this._viewHeightPercentage = value >= 0U && value <= 100U ? new uint?((uint) ((100 - (int) value) * 1000)) : throw new ArgumentException("Incorrect width percentage value");
    }
  }

  internal void SetSize(long cx, long cy)
  {
    this._cx = cx;
    this._cy = cy;
  }

  internal void AssignHeight(uint heightValue)
  {
    this._viewHeightPercentage = new uint?(heightValue);
  }

  internal void SetWidth(uint widthValue) => this._viewWidthPercentage = new uint?(widthValue);

  internal uint? GetLeft() => this._viewWidthPercentage;

  internal uint? GetTop() => this._viewHeightPercentage;

  public NotesSize Clone() => (NotesSize) this.MemberwiseClone();
}
