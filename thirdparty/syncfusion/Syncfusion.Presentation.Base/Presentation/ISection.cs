// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.ISection
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

#nullable disable
namespace Syncfusion.Presentation;

public interface ISection
{
  string Name { get; set; }

  int SlidesCount { get; }

  void Move(int toPosition);

  ISlides Slides { get; }

  ISlide AddSlide(SlideLayoutType slideLayoutType);

  void InsertSlide(int slideIndex, ISlide slide);

  ISlides Clone();
}
