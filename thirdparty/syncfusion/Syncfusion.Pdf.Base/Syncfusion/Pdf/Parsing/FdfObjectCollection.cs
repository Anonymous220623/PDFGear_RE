// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.FdfObjectCollection
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Primitives;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class FdfObjectCollection
{
  private Dictionary<string, IPdfPrimitive> m_objects;

  internal Dictionary<string, IPdfPrimitive> Objects
  {
    get => this.m_objects;
    set => this.m_objects = value;
  }

  internal FdfObjectCollection() => this.m_objects = new Dictionary<string, IPdfPrimitive>();

  internal void Add(string key, IPdfPrimitive value)
  {
    if (this.m_objects.ContainsKey(key))
      this.m_objects[key] = value;
    else
      this.m_objects.Add(key, value);
  }

  internal void Dispose()
  {
    this.m_objects.Clear();
    this.m_objects = (Dictionary<string, IPdfPrimitive>) null;
  }
}
