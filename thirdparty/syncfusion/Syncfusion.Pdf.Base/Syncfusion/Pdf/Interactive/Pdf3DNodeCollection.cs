// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.Pdf3DNodeCollection
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public class Pdf3DNodeCollection : List<Pdf3DNode>
{
  public int Add(Pdf3DNode value)
  {
    base.Add(value);
    return base.IndexOf(value);
  }

  public new bool Contains(Pdf3DNode value) => base.Contains(value);

  public new int IndexOf(Pdf3DNode value) => base.IndexOf(value);

  public new void Insert(int index, Pdf3DNode value) => base.Insert(index, value);

  public void Remove(Pdf3DNode value) => base.Remove(value);

  public new Pdf3DNode this[int index]
  {
    get => base[index];
    set => base[index] = value;
  }
}
