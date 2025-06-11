// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.TextInfoImpl
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System.Drawing;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

internal class TextInfoImpl
{
  private readonly string _text;
  private float _ascent;
  private RectangleF _bounds;
  private IFont _font;
  private string _unicodeFont;
  private int _length;
  private int _position;

  internal TextInfoImpl(string text) => this._text = text;

  internal string Text => this._text.Substring(this._position, this._length);

  internal IFont Font
  {
    get => this._font;
    set => this._font = value;
  }

  internal string UnicodeFont
  {
    get => this._unicodeFont;
    set => this._unicodeFont = value;
  }

  internal RectangleF Bounds
  {
    get => this._bounds;
    set => this._bounds = value;
  }

  internal float Ascent
  {
    get => this._ascent;
    set => this._ascent = value;
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

  internal string GetOriginalText() => this._text;

  internal void CopyTo(TextInfoImpl destination)
  {
    destination.Font = this.Font;
    destination.Ascent = this.Ascent;
    destination.UnicodeFont = this.UnicodeFont;
  }

  internal void Dispose() => this._font = (IFont) null;
}
