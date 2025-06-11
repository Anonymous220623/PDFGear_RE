// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.Drawing.EffectList
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.Presentation.Drawing;

internal class EffectList
{
  private ColorObject _outerShadow;
  private int _blurRadius;
  private int _direction;
  private int _distance;
  private bool _rotateWithShape;
  private int _horizontalScaling;
  private int _verticalScaling;
  private int _horizontalSkew;
  private int _verticalskew;
  private RectangleAlignmentType _rectAlignmentType;
  private Dictionary<string, Stream> _preservedElements;
  private Syncfusion.Presentation.Presentation _presentation;

  public EffectList(Syncfusion.Presentation.Presentation presentation)
  {
    this._presentation = presentation;
    this._blurRadius = -1;
    this._direction = -1;
    this._distance = -1;
  }

  internal ColorObject OuterShadow
  {
    get => this._outerShadow;
    set => this._outerShadow = value;
  }

  internal bool RotateWithShape
  {
    get => this._rotateWithShape;
    set => this._rotateWithShape = value;
  }

  internal Dictionary<string, Stream> PreservedElements
  {
    get => this._preservedElements ?? (this._preservedElements = new Dictionary<string, Stream>());
  }

  internal int BlurRadius
  {
    get => this._blurRadius;
    set
    {
      this._blurRadius = value >= 0 && value <= int.MaxValue ? value : throw new ArgumentException("The value range of BlurRadius must be within 0 to 2147483647");
    }
  }

  internal int ShadowDirection
  {
    get => this._direction;
    set => this._direction = value;
  }

  internal int ShadowDistance
  {
    get => this._distance;
    set => this._distance = value;
  }

  internal RectangleAlignmentType RectangleAlignType
  {
    get => this._rectAlignmentType;
    set => this._rectAlignmentType = value;
  }

  internal Syncfusion.Presentation.Presentation Presentation => this._presentation;

  internal int HorizontalSkew
  {
    get => this._horizontalSkew;
    set => this._horizontalSkew = value;
  }

  internal int VerticalSkew
  {
    get => this._verticalskew;
    set => this._verticalskew = value;
  }

  internal void SetHorizontalScaling(int value) => this._horizontalScaling = value;

  internal int GetHorizontalScaling() => this._horizontalScaling;

  internal int GetVerticalScaling() => this._verticalScaling;

  internal void SetVerticalScaling(int value) => this._verticalScaling = value;

  internal void Close()
  {
    this._presentation = (Syncfusion.Presentation.Presentation) null;
    if (this._outerShadow != null)
      this._outerShadow.Close();
    if (this._preservedElements == null)
      return;
    foreach (KeyValuePair<string, Stream> preservedElement in this._preservedElements)
      preservedElement.Value.Dispose();
    this._preservedElements.Clear();
    this._preservedElements = (Dictionary<string, Stream>) null;
  }

  public EffectList Clone()
  {
    EffectList effectList = (EffectList) this.MemberwiseClone();
    if (this._outerShadow != null)
      effectList._outerShadow = this._outerShadow.CloneColorObject();
    if (this._preservedElements != null)
      effectList._preservedElements = Helper.CloneDictionary(this._preservedElements);
    return effectList;
  }

  internal void SetParent(Syncfusion.Presentation.Presentation presentation)
  {
    this._presentation = presentation;
  }
}
