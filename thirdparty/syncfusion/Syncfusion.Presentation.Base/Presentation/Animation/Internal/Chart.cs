// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.Animation.Internal.Chart
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

#nullable disable
namespace Syncfusion.Presentation.Animation.Internal;

internal class Chart
{
  private AnimationBuildStep animationBuildStep;
  private int categoryIndex;
  private int seriesIndex;

  internal AnimationBuildStep AnimationBuildStep
  {
    get => this.animationBuildStep;
    set => this.animationBuildStep = value;
  }

  internal int CategoryIndex
  {
    get => this.categoryIndex;
    set => this.categoryIndex = value;
  }

  internal int SeriesIndex
  {
    get => this.seriesIndex;
    set => this.seriesIndex = value;
  }

  internal Chart Clone() => (Chart) this.MemberwiseClone();
}
