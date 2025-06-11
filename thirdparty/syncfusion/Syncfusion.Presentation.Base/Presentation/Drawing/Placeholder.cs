// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.Drawing.Placeholder
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Presentation.SlideImplementation;
using System;

#nullable disable
namespace Syncfusion.Presentation.Drawing;

internal class Placeholder : IPlaceholderFormat
{
  private Shape _baseShape;
  private bool _customPrompt;
  private string _id;
  private Orientation _placeholderDirection;
  private PlaceholderSize _placeHolderSize;
  private PlaceholderType _placeHolderType;
  private string _name;

  internal Placeholder(Shape shape) => this._baseShape = shape;

  public uint Index => Convert.ToUInt32(this._id);

  public Orientation Orientation => this._placeholderDirection;

  public PlaceholderSize Size => this._placeHolderSize;

  public PlaceholderType Type
  {
    get
    {
      return this._baseShape.SlideItemType == SlideItemType.Placeholder && this._placeHolderType == (PlaceholderType) 0 && this._placeholderDirection != Orientation.Vertical ? PlaceholderType.Object : this._placeHolderType;
    }
  }

  public string Name
  {
    get => this._name;
    set => this._name = value;
  }

  internal bool HasCustomPrompt
  {
    get => this._customPrompt;
    set => this._customPrompt = value;
  }

  internal BaseSlide BaseSlide => this._baseShape.BaseSlide;

  internal Shape GetBaseShape() => this._baseShape;

  internal string ObtainIndex() => this._id;

  internal PlaceholderType GetPlaceholderType() => this._placeHolderType;

  internal PlaceholderSize ObtainSize() => this._placeHolderSize;

  internal void SetDirection(Orientation orientation)
  {
    this._placeholderDirection = orientation;
    this.SetTypeBasedOnDirection();
  }

  internal void SetIndex(string index) => this._id = index;

  private void SetTypeBasedOnDirection()
  {
    if (this._placeholderDirection != Orientation.Vertical)
      return;
    switch (this._placeHolderType)
    {
      case PlaceholderType.Title:
        this._placeHolderType = PlaceholderType.VerticalTitle;
        break;
      case PlaceholderType.Body:
        this._placeHolderType = PlaceholderType.VerticalBody;
        break;
    }
    if (this._placeHolderType != (PlaceholderType) 0)
      return;
    this._placeHolderType = PlaceholderType.VerticalObject;
  }

  internal void SetPlaceholderValues(
    PlaceholderType placeHolderType,
    PlaceholderSize placeHolderSize,
    Orientation placeHolderDirection,
    string index)
  {
    this._placeHolderType = placeHolderType;
    this._placeHolderSize = placeHolderSize;
    this._placeholderDirection = placeHolderDirection;
    this.SetTypeBasedOnDirection();
    this._id = index;
  }

  internal void AssignSize(PlaceholderSize placeholderSize)
  {
    this._placeHolderSize = placeholderSize;
  }

  internal void SetType(PlaceholderType placeholderType) => this._placeHolderType = placeholderType;

  public Placeholder Clone() => (Placeholder) this.MemberwiseClone();

  internal void SetParent(Shape shape) => this._baseShape = shape;

  internal void Close() => this._baseShape = (Shape) null;
}
