// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.Animation.Internal.ParallelTimeNode
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Presentation.SlideImplementation;

#nullable disable
namespace Syncfusion.Presentation.Animation.Internal;

internal class ParallelTimeNode
{
  private CommonTimeNode commonTimeNode;

  internal CommonTimeNode CommonTimeNode
  {
    get => this.commonTimeNode;
    set => this.commonTimeNode = value;
  }

  internal ParallelTimeNode Clone(BaseSlide newBaseSlide)
  {
    ParallelTimeNode parallelTimeNode = (ParallelTimeNode) this.MemberwiseClone();
    if (this.commonTimeNode != null)
      parallelTimeNode.commonTimeNode = this.commonTimeNode.Clone(newBaseSlide);
    return parallelTimeNode;
  }
}
