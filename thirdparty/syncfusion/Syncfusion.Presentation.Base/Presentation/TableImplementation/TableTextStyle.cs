// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.TableImplementation.TableTextStyle
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

#nullable disable
namespace Syncfusion.Presentation.TableImplementation;

internal class TableTextStyle
{
  private ColorObject _color;
  private TablePartStyle _parent;
  private OnOffStyleType _bold;
  private OnOffStyleType _italic;
  private string _latin;
  private string _fontRefIdx;
  private ColorObject _textRefColor;
  private bool _hasFontColor;

  internal TableTextStyle(TablePartStyle tablePartStyle)
  {
    this._parent = tablePartStyle;
    this._color = new ColorObject(true);
    this._textRefColor = new ColorObject(true);
  }

  internal TablePartStyle Parent => this._parent;

  internal IColor TextColor
  {
    get
    {
      this._color.UpdateColorObject((object) this._parent.Parent.Presentation);
      return (IColor) this._color;
    }
  }

  internal OnOffStyleType Bold
  {
    get => this._bold;
    set => this._bold = value;
  }

  internal OnOffStyleType Italic
  {
    get => this._italic;
    set => this._italic = value;
  }

  internal string Latin
  {
    get
    {
      if (this._latin == null)
      {
        switch (this._fontRefIdx)
        {
          case "minor":
            return this._parent.Parent.Presentation.Theme.MinorFont.Latin;
          case "major":
            return this._parent.Parent.Presentation.Theme.MajorFont.Latin;
        }
      }
      return this._latin;
    }
    set => this._latin = value;
  }

  internal string FontRefIndex
  {
    get => this._fontRefIdx;
    set => this._fontRefIdx = value;
  }

  internal IColor TextRefColor
  {
    get
    {
      this._textRefColor.UpdateColorObject((object) this._parent.Parent.Presentation);
      return (IColor) this._textRefColor;
    }
    set => this._textRefColor.SetColor(ColorType.RGB, ((ColorObject) value).ToArgb());
  }

  internal bool HasFontColor
  {
    get => this._hasFontColor;
    set => this._hasFontColor = value;
  }

  internal void SetColorObject(ColorObject color) => this._color = color;

  internal void Close() => this.ClearAll();

  private void ClearAll()
  {
    if (this._color != null)
    {
      this._color.Close();
      this._color = (ColorObject) null;
    }
    if (this._textRefColor == null)
      return;
    this._textRefColor.Close();
    this._textRefColor = (ColorObject) null;
  }

  public TableTextStyle Clone()
  {
    TableTextStyle tableTextStyle = (TableTextStyle) this.MemberwiseClone();
    if (this._color != null)
      tableTextStyle._color = this._color.CloneColorObject();
    if (this._textRefColor != null)
      tableTextStyle._textRefColor = this._textRefColor.CloneColorObject();
    return tableTextStyle;
  }

  internal void SetParent(TablePartStyle tablePartStyle) => this._parent = tablePartStyle;
}
