// Decompiled with JetBrains decompiler
// Type: NAPS2.Wia.WiaPropertyAttributes
// Assembly: NAPS2.Wia, Version=2.0.0.0, Culture=neutral, PublicKeyToken=e28619810b0dcd38
// MVID: 676F057C-91D3-4AC6-8454-A801FD0BB848
// Assembly location: D:\PDFGear\bin\NAPS2.Wia.dll

using NAPS2.Wia.Native;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace NAPS2.Wia;

public class WiaPropertyAttributes
{
  protected internal WiaPropertyAttributes(IntPtr storage, int id)
  {
    int flags;
    int min;
    int nom;
    int max;
    int step;
    int[] elems;
    WiaException.Check(NativeWiaMethods.GetPropertyAttributes(storage, id, out flags, out min, out nom, out max, out step, out int _, out elems));
    this.Flags = (WiaPropertyFlags) flags;
    this.Min = min;
    this.Nom = nom;
    this.Max = max;
    this.Step = step;
    this.Values = elems != null ? ((IEnumerable<int>) elems).Skip<int>(2).Cast<object>().ToArray<object>() : (object[]) null;
  }

  public WiaPropertyFlags Flags { get; }

  public int Min { get; }

  public int Nom { get; }

  public int Max { get; }

  public int Step { get; }

  public object[]? Values { get; }
}
