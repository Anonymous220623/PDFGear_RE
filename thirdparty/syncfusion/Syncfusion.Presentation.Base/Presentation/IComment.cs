// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.IComment
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Presentation;

public interface IComment
{
  double Left { get; set; }

  double Top { get; set; }

  string AuthorName { get; set; }

  string Initials { get; set; }

  string Text { get; set; }

  DateTime DateTime { get; set; }

  bool HasChild { get; }

  List<IComment> GetChild();

  IComment Parent { get; }
}
