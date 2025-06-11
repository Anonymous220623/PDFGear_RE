// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.ISlides
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Presentation;

public interface ISlides : IEnumerable<ISlide>, IEnumerable
{
  int Count { get; }

  ISlide this[int index] { get; }

  ISlide Add();

  ISlide Add(ILayoutSlide layoutSlide);

  int Add(ISlide value);

  void Add(ISlide clonedSlide, PasteOptions pasteOptions, IPresentation sourcePresentation);

  ISlide Add(SlideLayoutType slideLayout);

  void Insert(int index, ISlide value);

  void RemoveAt(int index);

  void Remove(ISlide value);

  int IndexOf(ISlide value);

  void Clear();

  void Insert(
    int index,
    ISlide clonedSlide,
    PasteOptions pasteOptions,
    IPresentation sourcePresentation);
}
