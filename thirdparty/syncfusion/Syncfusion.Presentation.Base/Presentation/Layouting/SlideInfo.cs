// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.Layouting.SlideInfo
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Presentation.SlideImplementation;
using System.Drawing;

#nullable disable
namespace Syncfusion.Presentation.Layouting;

internal class SlideInfo
{
  private ISlide _slide;
  private RectangleF _bounds;
  private NotesSlide _notesSlide;

  internal SlideInfo(ISlide slide) => this._slide = slide;

  public SlideInfo(NotesSlide notesSlide) => this._notesSlide = notesSlide;

  internal Slide Slide => (Slide) this._slide;

  internal RectangleF Bounds
  {
    get => this._bounds;
    set => this._bounds = value;
  }

  internal SlideInfo Clone()
  {
    SlideInfo slideInfo = (SlideInfo) this.MemberwiseClone();
    slideInfo._bounds = this._bounds;
    return slideInfo;
  }

  internal void SetParent(Slide slide) => this._slide = (ISlide) slide;
}
