// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.PivotTables.InternalReference
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.PivotTables;

internal class InternalReference
{
  private PivotAreaReference m_pivotAreaReference;
  private List<InternalReference> m_innerItems;
  private int m_index;
  private int m_childCurrentIndex;

  internal PivotAreaReference PivotAreaReference
  {
    get => this.m_pivotAreaReference;
    set => this.m_pivotAreaReference = value;
  }

  internal List<InternalReference> Items
  {
    get => this.m_innerItems;
    set => this.m_innerItems = value;
  }

  internal int Index
  {
    get => this.m_index;
    set => this.m_index = value;
  }

  internal int ChildCurrentIndex
  {
    get => this.m_childCurrentIndex;
    set => this.m_childCurrentIndex = value;
  }

  internal InternalReference()
  {
  }

  internal InternalReference(int index, PivotAreaReference pivotAreaReference)
  {
    this.m_index = index;
    this.m_pivotAreaReference = pivotAreaReference;
  }
}
