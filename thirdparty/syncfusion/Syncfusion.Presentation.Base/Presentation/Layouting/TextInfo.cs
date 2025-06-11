// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.Layouting.TextInfo
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Office;
using System.Drawing;

#nullable disable
namespace Syncfusion.Presentation.Layouting;

internal class TextInfo
{
  private ITextPart _textPart;
  private RectangleF _bounds;
  private int _position;
  private int _length;
  private float _ascent;
  private CharacterRangeType _characterRangeType;

  internal TextInfo(ITextPart textPart) => this._textPart = textPart;

  internal ITextPart TextPart => this._textPart;

  internal CharacterRangeType CharacterRange
  {
    get => this._characterRangeType;
    set => this._characterRangeType = value;
  }

  internal string Text
  {
    get
    {
      return this._textPart != null ? this._textPart.Text.Substring(this._position, this._length) : (string) null;
    }
  }

  internal RectangleF Bounds
  {
    get => this._bounds;
    set => this._bounds = value;
  }

  internal float X
  {
    get => this._bounds.X;
    set => this._bounds.X = value;
  }

  internal float Y
  {
    get => this._bounds.Y;
    set => this._bounds.Y = value;
  }

  internal float Width
  {
    get => this._bounds.Width;
    set => this._bounds.Width = value;
  }

  internal float Height
  {
    get => this._bounds.Height;
    set => this._bounds.Height = value;
  }

  internal int Position
  {
    get => this._position;
    set => this._position = value;
  }

  internal int Length
  {
    get => this._length;
    set => this._length = value;
  }

  internal float Ascent
  {
    get => this._ascent;
    set => this._ascent = value;
  }

  internal TextInfo Clone()
  {
    TextInfo textInfo = (TextInfo) this.MemberwiseClone();
    textInfo._textPart = (ITextPart) null;
    return textInfo;
  }
}
