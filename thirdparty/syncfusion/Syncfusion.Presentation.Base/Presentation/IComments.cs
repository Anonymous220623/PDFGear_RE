// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.IComments
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Presentation;

public interface IComments : IEnumerable<IComment>, IEnumerable
{
  IComment this[int index] { get; }

  IComment Add(
    double left,
    double top,
    string authorName,
    string authorInitials,
    string text,
    DateTime dateTime);

  IComment Add(
    string authorName,
    string initials,
    string replyText,
    DateTime dateTime,
    IComment parent);

  int Count { get; }

  void RemoveAt(int index);

  void Remove(IComment comment);

  int IndexOf(IComment comment);

  void Insert(int index, IComment comment);

  void Clear();
}
