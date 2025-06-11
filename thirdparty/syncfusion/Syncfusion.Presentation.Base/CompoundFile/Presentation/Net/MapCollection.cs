// Decompiled with JetBrains decompiler
// Type: Syncfusion.CompoundFile.Presentation.Net.MapCollection
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.CompoundFile.Presentation.Net;

internal class MapCollection : IEnumerable
{
  private RBTreeNode m_MyHead;
  private int m_size;
  private IComparer m_comparer = (IComparer) Comparer<object>.Default;

  public RBTreeNode Empty => this.m_MyHead;

  public int Count => this.m_size;

  public object this[object key]
  {
    get
    {
      RBTreeNode rbTreeNode = this.LBound(key);
      return this.m_comparer.Compare(rbTreeNode.Key, key) == 0 ? rbTreeNode.Value : (object) null;
    }
    set
    {
      RBTreeNode rbTreeNode = this.LBound(key);
      if (rbTreeNode.IsNil || this.m_comparer.Compare(rbTreeNode.Key, key) != 0)
        this.Add(key, value);
      else
        rbTreeNode.Value = value;
    }
  }

  public MapCollection() => this.Initialize();

  public MapCollection(IComparer comparer)
  {
    if (comparer == null)
      throw new ArgumentNullException(nameof (comparer));
    this.Initialize();
    this.m_comparer = comparer;
  }

  protected void Initialize()
  {
    this.m_MyHead = new RBTreeNode((RBTreeNode) null, (RBTreeNode) null, (RBTreeNode) null, (object) null, (object) null, NodeColor.Black);
    this.m_MyHead.IsNil = true;
    this.m_MyHead.Parent = this.m_MyHead.Left = this.m_MyHead.Right = this.m_MyHead;
    this.m_size = 0;
  }

  public void Clear()
  {
    this.Erase(this.m_MyHead.Parent);
    this.m_MyHead.Parent = this.m_MyHead.Left = this.m_MyHead.Right = this.m_MyHead;
    this.m_size = 0;
  }

  public void Add(object key, object value)
  {
    RBTreeNode rbTreeNode = this.m_MyHead.Parent;
    RBTreeNode _where = this.m_MyHead;
    bool _addLeft = true;
    for (; !rbTreeNode.IsNil; rbTreeNode = _addLeft ? rbTreeNode.Left : rbTreeNode.Right)
    {
      _where = rbTreeNode;
      _addLeft = this.m_comparer.Compare(key, rbTreeNode.Key) < 0;
    }
    RBTreeNode node = _where;
    if (_addLeft)
    {
      if (_where == this.begin())
      {
        this.Insert(true, _where, key, value);
        return;
      }
      MapCollection.Dec(node);
    }
    this.Insert(_addLeft, _where, key, value);
  }

  public bool Contains(object key)
  {
    RBTreeNode rbTreeNode = this.LBound(key);
    return rbTreeNode != this.m_MyHead && this.m_comparer.Compare(rbTreeNode.Key, key) == 0;
  }

  public void Remove(object key)
  {
    RBTreeNode node1 = this.LBound(key);
    if (node1.IsNil)
      return;
    RBTreeNode rbTreeNode = node1;
    RBTreeNode node2;
    if (rbTreeNode.Left.IsNil)
      node2 = rbTreeNode.Right;
    else if (rbTreeNode.Right.IsNil)
    {
      node2 = rbTreeNode.Left;
    }
    else
    {
      rbTreeNode = MapCollection.Inc(node1);
      node2 = rbTreeNode.Right;
    }
    RBTreeNode _where;
    if (rbTreeNode == node1)
    {
      _where = node1.Parent;
      if (!node2.IsNil)
        node2.Parent = _where;
      if (this.m_MyHead.Parent == node1)
        this.m_MyHead.Parent = node2;
      else if (_where.Left == node1)
        _where.Left = node2;
      else
        _where.Right = node2;
      if (this.m_MyHead.Left == node1)
        this.m_MyHead.Left = node2.IsNil ? _where : MapCollection.Min(node2);
      if (this.m_MyHead.Right == node1)
        this.m_MyHead.Right = node2.IsNil ? _where : MapCollection.Max(node2);
    }
    else
    {
      node1.Left.Parent = rbTreeNode;
      rbTreeNode.Left = node1.Left;
      if (rbTreeNode == node1.Right)
      {
        _where = rbTreeNode;
      }
      else
      {
        _where = rbTreeNode.Parent;
        if (!node2.IsNil)
          node2.Parent = _where;
        _where.Left = node2;
        rbTreeNode.Right = node1.Right;
        node1.Right.Parent = rbTreeNode;
      }
      if (this.m_MyHead.Parent == node1)
        this.m_MyHead.Parent = rbTreeNode;
      else if (node1.Parent.Left == node1)
        node1.Parent.Left = rbTreeNode;
      else
        node1.Parent.Right = rbTreeNode;
      rbTreeNode.Parent = node1.Parent;
      NodeColor color = node1.Color;
      node1.Color = rbTreeNode.Color;
      rbTreeNode.Color = color;
    }
    if (node1.Color == NodeColor.Black)
    {
      while (node2 != this.m_MyHead.Parent && node2.Color == NodeColor.Black)
      {
        if (node2 == _where.Left)
        {
          RBTreeNode right = _where.Right;
          if (right.Color == NodeColor.Red)
          {
            right.Color = NodeColor.Black;
            _where.Color = NodeColor.Red;
            this.LRotate(_where);
            right = _where.Right;
          }
          if (right.IsNil)
            node2 = _where;
          else if (right.Left.Color == NodeColor.Black && right.Right.Color == NodeColor.Black)
          {
            right.Color = NodeColor.Red;
            node2 = _where;
          }
          else
          {
            if (right.Right.Color == NodeColor.Black)
            {
              right.Left.Color = NodeColor.Black;
              right.Color = NodeColor.Red;
              this.RRotate(right);
              right = _where.Right;
            }
            right.Color = _where.Color;
            _where.Color = NodeColor.Black;
            right.Right.Color = NodeColor.Black;
            this.LRotate(_where);
            break;
          }
        }
        else
        {
          RBTreeNode left = _where.Left;
          if (left.Color == NodeColor.Red)
          {
            left.Color = NodeColor.Black;
            _where.Color = NodeColor.Red;
            this.RRotate(_where);
            left = _where.Left;
          }
          if (left.IsNil)
            node2 = _where;
          else if (left.Right.Color == NodeColor.Black && left.Left.Color == NodeColor.Black)
          {
            left.Color = NodeColor.Red;
            node2 = _where;
          }
          else
          {
            if (left.Left.Color == NodeColor.Black)
            {
              left.Right.Color = NodeColor.Black;
              left.Color = NodeColor.Red;
              this.LRotate(left);
              left = _where.Left;
            }
            left.Color = _where.Color;
            _where.Color = NodeColor.Black;
            left.Left.Color = NodeColor.Black;
            this.RRotate(_where);
            break;
          }
        }
        _where = node2.Parent;
      }
      node2.Color = NodeColor.Black;
    }
    if (0 >= this.m_size)
      return;
    --this.m_size;
  }

  private RBTreeNode begin() => this.m_MyHead.Left;

  public static RBTreeNode Min(RBTreeNode node)
  {
    while (!node.Left.IsNil)
      node = node.Left;
    return node;
  }

  public static RBTreeNode Max(RBTreeNode node)
  {
    while (!node.Right.IsNil)
      node = node.Right;
    return node;
  }

  public static RBTreeNode Inc(RBTreeNode node)
  {
    if (node == null)
      throw new ArgumentNullException(nameof (node));
    if (node.IsNil)
      return node;
    if (!node.Right.IsNil)
    {
      node = MapCollection.Min(node.Right);
    }
    else
    {
      RBTreeNode parent;
      while (!(parent = node.Parent).IsNil && node == parent.Right)
        node = parent;
      node = parent;
    }
    return node;
  }

  public static RBTreeNode Dec(RBTreeNode node)
  {
    if (node == null)
      throw new ArgumentNullException(nameof (node));
    if (node.IsNil)
      node = node.Right;
    else if (!node.Left.IsNil)
    {
      node = MapCollection.Max(node.Left);
    }
    else
    {
      RBTreeNode parent;
      while (!(parent = node.Parent).IsNil && parent == parent.Left)
        node = parent;
      if (!parent.IsNil)
        node = parent;
    }
    return node;
  }

  protected RBTreeNode LBound(object key)
  {
    RBTreeNode rbTreeNode1 = this.m_MyHead.Parent;
    RBTreeNode rbTreeNode2 = this.m_MyHead;
    while (!rbTreeNode1.IsNil)
    {
      if (this.m_comparer.Compare(rbTreeNode1.Key, key) < 0)
      {
        rbTreeNode1 = rbTreeNode1.Right;
      }
      else
      {
        rbTreeNode2 = rbTreeNode1;
        rbTreeNode1 = rbTreeNode1.Left;
      }
    }
    return rbTreeNode2;
  }

  protected RBTreeNode UBound(object key)
  {
    RBTreeNode rbTreeNode1 = this.m_MyHead.Parent;
    RBTreeNode rbTreeNode2 = this.m_MyHead;
    while (!rbTreeNode1.IsNil)
    {
      if (this.m_comparer.Compare(key, rbTreeNode1.Key) < 0)
      {
        rbTreeNode2 = rbTreeNode1;
        rbTreeNode1 = rbTreeNode1.Left;
      }
      else
        rbTreeNode1 = rbTreeNode1.Right;
    }
    return rbTreeNode2;
  }

  protected void LRotate(RBTreeNode _where)
  {
    RBTreeNode right = _where.Right;
    _where.Right = right.Left;
    if (!right.Left.IsNil)
      right.Left.Parent = _where;
    right.Parent = _where.Parent;
    if (_where == this.m_MyHead.Parent)
      this.m_MyHead.Parent = right;
    else if (_where == _where.Parent.Left)
      _where.Parent.Left = right;
    else
      _where.Parent.Right = right;
    right.Left = _where;
    _where.Parent = right;
  }

  protected void RRotate(RBTreeNode _where)
  {
    RBTreeNode left = _where.Left;
    _where.Left = left.Right;
    if (!left.Right.IsNil)
      left.Right.Parent = _where;
    left.Parent = _where.Parent;
    if (_where == this.m_MyHead.Parent)
      this.m_MyHead.Parent = left;
    else if (_where == _where.Parent.Right)
      _where.Parent.Right = left;
    else
      _where.Parent.Left = left;
    left.Right = _where;
    _where.Parent = left;
  }

  protected void Erase(RBTreeNode _root)
  {
    RBTreeNode rbTreeNode = _root;
    while (!rbTreeNode.IsNil)
    {
      this.Erase(rbTreeNode.Right);
      rbTreeNode = rbTreeNode.Left;
      _root = rbTreeNode;
    }
  }

  protected void Insert(bool _addLeft, RBTreeNode _where, object key, object value)
  {
    RBTreeNode rbTreeNode = new RBTreeNode(this.m_MyHead, _where, this.m_MyHead, key, value);
    ++this.m_size;
    if (_where == this.m_MyHead)
      this.m_MyHead.Parent = this.m_MyHead.Left = this.m_MyHead.Right = rbTreeNode;
    else if (_addLeft)
    {
      _where.Left = rbTreeNode;
      if (_where == this.m_MyHead.Left)
        this.m_MyHead.Left = rbTreeNode;
    }
    else
    {
      _where.Right = rbTreeNode;
      if (_where == this.m_MyHead.Right)
        this.m_MyHead.Right = rbTreeNode;
    }
    RBTreeNode _where1 = rbTreeNode;
    while (_where1.Parent.Color == NodeColor.Red)
    {
      if (_where1.Parent == _where1.Parent.Parent.Left)
      {
        _where = _where1.Parent.Parent.Right;
        if (_where.Color == NodeColor.Red)
        {
          _where1.Parent.Color = NodeColor.Black;
          _where.Color = NodeColor.Black;
          _where1.Parent.Parent.Color = NodeColor.Red;
          _where1 = _where1.Parent.Parent;
        }
        else
        {
          if (_where1 == _where1.Parent.Right)
          {
            _where1 = _where1.Parent;
            this.LRotate(_where1);
          }
          _where1.Parent.Color = NodeColor.Black;
          _where1.Parent.Parent.Color = NodeColor.Red;
          this.RRotate(_where1.Parent.Parent);
        }
      }
      else
      {
        _where = _where1.Parent.Parent.Left;
        if (_where.Color == NodeColor.Red)
        {
          _where1.Parent.Color = NodeColor.Black;
          _where.Color = NodeColor.Black;
          _where1.Parent.Parent.Color = NodeColor.Red;
          _where1 = _where1.Parent.Parent;
        }
        else
        {
          if (_where1 == _where1.Parent.Left)
          {
            _where1 = _where1.Parent;
            this.RRotate(_where1);
          }
          _where1.Parent.Color = NodeColor.Black;
          _where1.Parent.Parent.Color = NodeColor.Red;
          this.LRotate(_where1.Parent.Parent);
        }
      }
    }
    this.m_MyHead.Parent.Color = NodeColor.Black;
  }

  public IEnumerator GetEnumerator() => (IEnumerator) new MapEnumerator(this.m_MyHead.Left);

  internal void ForAll(MapCollection.NodeFunction function) => this.ForAll(this.m_MyHead, function);

  private void ForAll(RBTreeNode startNode, MapCollection.NodeFunction function)
  {
    if (startNode == null || startNode.IsNil && startNode.Left.IsNil && startNode.Right.IsNil)
      return;
    if (!startNode.IsNil)
      function(startNode);
    this.ForAll(startNode.Left, function);
    this.ForAll(startNode.Right, function);
  }

  internal delegate void NodeFunction(RBTreeNode node);
}
