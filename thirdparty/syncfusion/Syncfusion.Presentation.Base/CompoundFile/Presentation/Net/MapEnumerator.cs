// Decompiled with JetBrains decompiler
// Type: Syncfusion.CompoundFile.Presentation.Net.MapEnumerator
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using System;
using System.Collections;

#nullable disable
namespace Syncfusion.CompoundFile.Presentation.Net;

internal class MapEnumerator : IEnumerator
{
  private RBTreeNode m_current;
  private RBTreeNode m_parent;

  object IEnumerator.Current => (object) this.m_current;

  public RBTreeNode Current => this.m_current;

  public MapEnumerator(RBTreeNode parent)
  {
    this.m_parent = parent != null ? parent : throw new ArgumentNullException(nameof (parent));
  }

  public void Reset() => this.m_current = (RBTreeNode) null;

  public bool MoveNext()
  {
    this.m_current = this.m_current != null ? MapCollection.Inc(this.m_current) : this.m_parent;
    return this.m_current != null && !this.m_current.IsNil;
  }
}
