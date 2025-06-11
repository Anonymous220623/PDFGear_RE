// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.Animation.Internal.Build
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Presentation.Animation.Internal;

internal class Build
{
  private List<object> buildElements;

  internal List<object> BuildElements
  {
    get => this.buildElements;
    set => this.buildElements = value;
  }

  internal Build CloneBuild()
  {
    Build build = (Build) this.MemberwiseClone();
    build.buildElements = this.CloneBuildElements();
    return build;
  }

  private List<object> CloneBuildElements()
  {
    List<object> objectList = new List<object>();
    foreach (object buildElement in this.buildElements)
    {
      object obj = (object) null;
      switch (buildElement)
      {
        case BuildDiagram _:
          obj = (object) (buildElement as BuildDiagram).Clone();
          break;
        case BuildParagraph _:
          obj = (object) (buildElement as BuildParagraph).Clone();
          break;
        case BuildGraphics _:
          obj = (object) (buildElement as BuildGraphics).Clone();
          break;
        case BuildOleChart _:
          obj = (object) (buildElement as BuildOleChart).Clone();
          break;
      }
      objectList.Add(obj);
    }
    return objectList;
  }
}
