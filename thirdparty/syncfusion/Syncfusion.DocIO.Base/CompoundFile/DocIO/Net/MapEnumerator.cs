// Decompiled with JetBrains decompiler
// Type: Syncfusion.CompoundFile.DocIO.Net.MapEnumerator
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.Collections;

#nullable disable
namespace Syncfusion.CompoundFile.DocIO.Net;

public class MapEnumerator : IEnumerator
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
