// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.IParagraphs
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Presentation;

public interface IParagraphs : IEnumerable<IParagraph>, IEnumerable
{
  int Count { get; }

  IParagraph this[int index] { get; }

  IParagraph Add();

  IParagraph Add(string text);

  void Insert(int index, IParagraph value);

  void Remove(IParagraph item);

  void RemoveAt(int index);

  int IndexOf(IParagraph item);

  void Clear();
}
