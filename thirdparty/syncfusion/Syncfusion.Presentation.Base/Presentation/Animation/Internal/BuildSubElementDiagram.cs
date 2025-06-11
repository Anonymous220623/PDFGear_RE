// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.Animation.Internal.BuildSubElementDiagram
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

#nullable disable
namespace Syncfusion.Presentation.Animation.Internal;

internal class BuildSubElementDiagram
{
  private AnimationBuildType buildType;
  private bool isReverseAnimation;

  internal AnimationBuildType BuildType
  {
    get => this.buildType;
    set => this.buildType = value;
  }

  internal bool IsReverseAnimation
  {
    get => this.isReverseAnimation;
    set => this.isReverseAnimation = value;
  }

  internal BuildSubElementDiagram Clone() => (BuildSubElementDiagram) this.MemberwiseClone();
}
