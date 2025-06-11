// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.Layouting.LineInfo
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Presentation.Layouting;

internal class LineInfo
{
  private List<TextInfo> _textInfoCollection;
  private float _height;
  private float _maxAscent;

  internal List<TextInfo> TextInfoCollection
  {
    get => this._textInfoCollection;
    set => this._textInfoCollection = value;
  }

  internal string Text
  {
    get
    {
      string empty = string.Empty;
      foreach (TextInfo textInfo in this._textInfoCollection)
      {
        if (textInfo.TextPart != null)
          empty += textInfo.Text;
      }
      return empty;
    }
  }

  internal float Height
  {
    get => this._height;
    set => this._height = value;
  }

  internal float MaximumAscent
  {
    get => this._maxAscent;
    set => this._maxAscent = value;
  }

  internal bool HasDifferentHeight()
  {
    foreach (TextInfo textInfo in this._textInfoCollection)
    {
      if ((double) textInfo.Height != (double) this.Height)
        return true;
    }
    return false;
  }

  internal void Close()
  {
    if (this._textInfoCollection == null)
      return;
    this._textInfoCollection.Clear();
    this._textInfoCollection = (List<TextInfo>) null;
  }

  internal LineInfo Clone()
  {
    LineInfo lineInfo = (LineInfo) this.MemberwiseClone();
    lineInfo._textInfoCollection = this.CloneTextInfoList();
    return lineInfo;
  }

  private List<TextInfo> CloneTextInfoList()
  {
    List<TextInfo> textInfoList = new List<TextInfo>();
    foreach (TextInfo textInfo1 in this._textInfoCollection)
    {
      TextInfo textInfo2 = textInfo1.Clone();
      textInfoList.Add(textInfo2);
    }
    return textInfoList;
  }
}
