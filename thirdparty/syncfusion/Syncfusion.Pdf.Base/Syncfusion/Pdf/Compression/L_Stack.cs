// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Compression.L_Stack
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf.Compression;

internal class L_Stack
{
  private int m_nalloc;
  private List<object> m_array;
  private L_Stack m_auxStack;

  internal int Nalloc
  {
    get => this.m_nalloc;
    set => this.m_nalloc = value;
  }

  internal List<object> Array
  {
    get => this.m_array;
    set => this.m_array = value;
  }

  internal L_Stack AuxStack
  {
    get => this.m_auxStack;
    set => this.m_auxStack = value;
  }

  internal L_Stack(int arg)
  {
    this.Nalloc = arg;
    this.Array = new List<object>();
  }
}
