// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.IPictures
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using System.Collections;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.Presentation;

public interface IPictures : IEnumerable<IPicture>, IEnumerable
{
  IPicture this[int index] { get; }

  int Count { get; }

  IPicture AddPicture(Stream stream, double left, double top, double width, double height);

  IPicture AddPicture(
    Stream svgStream,
    Stream imageStream,
    double left,
    double top,
    double width,
    double height);

  int IndexOf(IPicture picture);

  void Remove(IPicture picture);

  void RemoveAt(int index);
}
