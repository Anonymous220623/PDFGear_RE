// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Collections.MapEnumerator
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Collections;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Collections;

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
