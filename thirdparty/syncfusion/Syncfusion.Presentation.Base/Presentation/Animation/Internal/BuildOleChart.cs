// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.Animation.Internal.BuildOleChart
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

#nullable disable
namespace Syncfusion.Presentation.Animation.Internal;

internal class BuildOleChart : Syncfusion.Presentation.Animation.Internal.Build
{
  private bool animateBackground;
  private OleChartBuildType build;
  private uint groupId;
  private uint shapeId;
  private bool uiExpand;

  internal bool AnimateBackground
  {
    get => this.animateBackground;
    set => this.animateBackground = value;
  }

  internal OleChartBuildType Build
  {
    get => this.build;
    set => this.build = value;
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

  internal bool UiExpand
  {
    get => this.uiExpand;
    set => this.uiExpand = value;
  }

  internal BuildOleChart Clone() => (BuildOleChart) this.MemberwiseClone();
}
