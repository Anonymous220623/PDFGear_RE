// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.LineInfoImpl
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

internal class LineInfoImpl
{
  private List<TextInfoImpl> _textInfoCollection;
  private float m_height;

  internal LineInfoImpl() => this._textInfoCollection = new List<TextInfoImpl>();

  internal List<TextInfoImpl> TextInfoCollection
  {
    get => this._textInfoCollection;
    set => this._textInfoCollection = value;
  }

  internal string Text
  {
    get
    {
      string empty = string.Empty;
      foreach (TextInfoImpl textInfo in this._textInfoCollection)
        empty += textInfo.Text;
      return empty;
    }
  }

  internal float Width
  {
    get
    {
      float width = 0.0f;
      foreach (TextInfoImpl textInfo in this._textInfoCollection)
        width += textInfo.Width;
      return width;
    }
  }

  internal float Height
  {
    get
    {
      if (this._textInfoCollection.Count > 0)
      {
        this.m_height = 0.0f;
        foreach (TextInfoImpl textInfo in this._textInfoCollection)
        {
          if ((double) this.m_height < (double) textInfo.Height)
            this.m_height = textInfo.Height;
        }
      }
      return this.m_height;
    }
    set => this.m_height = value;
  }

  internal void Dispose()
  {
    foreach (TextInfoImpl textInfo in this._textInfoCollection)
      textInfo.Dispose();
    this._textInfoCollection.Clear();
    this._textInfoCollection = (List<TextInfoImpl>) null;
  }
}
