// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.Layouting.ColumnInfo
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

#nullable disable
namespace Syncfusion.Presentation.Layouting;

internal class ColumnInfo
{
  private ITextBody _textBody;
  private int _paragraphStartIndex;
  private int _lineStartIndex;
  private float _height;

  internal ColumnInfo(ITextBody textBody) => this._textBody = textBody;

  internal ITextBody TextBody => this._textBody;

  internal int ParagraphStartIndex
  {
    get => this._paragraphStartIndex;
    set => this._paragraphStartIndex = value;
  }

  internal int LineStartIndex
  {
    get => this._lineStartIndex;
    set => this._lineStartIndex = value;
  }

  internal float Height
  {
    get => this._height;
    set => this._height = value;
  }
}
