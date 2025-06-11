// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.SlideImplementation.NotesMasterSlide
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Presentation.RichText;
using Syncfusion.Presentation.Themes;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Presentation.SlideImplementation;

internal class NotesMasterSlide : BaseSlide
{
  private string _notesMasterSlideId;
  private Theme _theme;
  internal new Dictionary<string, string> ColorMap;
  private TextBody _notesTextStyle;

  internal NotesMasterSlide(Syncfusion.Presentation.Presentation presentation, string notesMasterId)
    : base(presentation)
  {
    this._notesMasterSlideId = notesMasterId;
    this._theme = new Theme(this);
  }

  internal Theme Theme => this._theme;

  internal TextBody NotesTextStyle
  {
    get => this._notesTextStyle;
    set => this._notesTextStyle = value;
  }

  public NotesMasterSlide Clone()
  {
    NotesMasterSlide notesMasterSlide = (NotesMasterSlide) this.MemberwiseClone();
    if (this._notesTextStyle != null)
    {
      notesMasterSlide._notesTextStyle = this._notesTextStyle.Clone();
      notesMasterSlide._notesTextStyle.SetParent((BaseSlide) notesMasterSlide);
    }
    notesMasterSlide._theme = this._theme.Clone();
    notesMasterSlide._theme.SetParent((BaseSlide) notesMasterSlide);
    return notesMasterSlide;
  }
}
