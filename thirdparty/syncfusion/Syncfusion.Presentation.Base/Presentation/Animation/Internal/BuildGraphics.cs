// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.Animation.Internal.BuildGraphics
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

#nullable disable
namespace Syncfusion.Presentation.Animation.Internal;

internal class BuildGraphics : Build
{
  private BuildAsOne buildAsOne;
  private BuildSubElements buildSubElements;
  private uint groupId;
  private uint shapeId;
  private bool isUiExpand;

  internal BuildAsOne BuildAsOne
  {
    get => this.buildAsOne;
    set => this.buildAsOne = value;
  }

  internal BuildSubElements BuildSubElements
  {
    get => this.buildSubElements;
    set => this.buildSubElements = value;
  }

  internal uint GroupId
  {
    get => this.groupId;
    set => this.groupId = value;
  }

  internal uint ShapeId
  {
    get => this.shapeId;
    set => this.shapeId = value;
  }

  internal bool IsUiExpand
  {
    get => this.isUiExpand;
    set => this.isUiExpand = value;
  }

  internal BuildGraphics Clone()
  {
    BuildGraphics buildGraphics = (BuildGraphics) this.MemberwiseClone();
    if (this.buildSubElements != null)
      buildGraphics.buildSubElements = this.buildSubElements.Clone();
    return buildGraphics;
  }
}
