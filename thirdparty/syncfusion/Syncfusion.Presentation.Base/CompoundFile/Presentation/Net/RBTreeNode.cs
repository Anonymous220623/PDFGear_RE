// Decompiled with JetBrains decompiler
// Type: Syncfusion.CompoundFile.Presentation.Net.RBTreeNode
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

#nullable disable
namespace Syncfusion.CompoundFile.Presentation.Net;

internal class RBTreeNode
{
  private RBTreeNode m_left;
  private RBTreeNode m_right;
  private RBTreeNode m_parent;
  private NodeColor m_color;
  private bool m_bIsNil;
  private object m_key;
  private object m_value;

  public RBTreeNode Left
  {
    get => this.m_left;
    set => this.m_left = value;
  }

  public RBTreeNode Right
  {
    get => this.m_right;
    set => this.m_right = value;
  }

  public RBTreeNode Parent
  {
    get => this.m_parent;
    set => this.m_parent = value;
  }

  public NodeColor Color
  {
    get => this.m_color;
    set => this.m_color = value;
  }

  public bool IsNil
  {
    get => this.m_bIsNil;
    set => this.m_bIsNil = value;
  }

  public object Key
  {
    get => this.m_key;
    set => this.m_key = value;
  }

  public object Value
  {
    get => this.m_value;
    set => this.m_value = value;
  }

  public bool IsRed => this.Color == NodeColor.Red;

  public bool IsBlack => this.Color == NodeColor.Black;

  public RBTreeNode(
    RBTreeNode left,
    RBTreeNode parent,
    RBTreeNode right,
    object key,
    object value)
    : this(left, parent, right, key, value, NodeColor.Red)
  {
  }

  public RBTreeNode(
    RBTreeNode left,
    RBTreeNode parent,
    RBTreeNode right,
    object key,
    object value,
    NodeColor color)
  {
    this.m_left = left;
    this.m_parent = parent;
    this.m_right = right;
    this.m_key = key;
    this.m_value = value;
    this.m_color = color;
  }
}
