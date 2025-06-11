// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.IBaseSlide
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Presentation.Interfaces;

#nullable disable
namespace Syncfusion.Presentation;

public interface IBaseSlide
{
  string Name { get; set; }

  IShapes Shapes { get; }

  IBackground Background { get; }

  IGroupShapes GroupShapes { get; }

  IPictures Pictures { get; }

  ITables Tables { get; }

  IPresentationCharts Charts { get; }

  ISlideSize SlideSize { get; }

  IAnimationTimeline Timeline { get; }

  ISlideShowTransition SlideTransition { get; }

  IHeadersFooters HeadersFooters { get; }
}
