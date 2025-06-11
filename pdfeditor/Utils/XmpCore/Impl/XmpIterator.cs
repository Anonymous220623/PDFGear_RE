// Decompiled with JetBrains decompiler
// Type: XmpCore.Impl.XmpIterator
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Sharpen;
using System;
using System.Linq;
using XmpCore.Impl.XPath;
using XmpCore.Options;

#nullable disable
namespace XmpCore.Impl;

public sealed class XmpIterator : IXmpIterator, IIterator
{
  private bool _skipSiblings;
  private readonly IIterator _nodeIterator;

  public XmpIterator(XmpMeta xmp, string schemaNs, string propPath, IteratorOptions options)
  {
    this.Options = options ?? new IteratorOptions();
    string parentPath = (string) null;
    bool flag1 = !string.IsNullOrEmpty(schemaNs);
    bool flag2 = !string.IsNullOrEmpty(propPath);
    XmpNode xmpNode;
    if (!flag1 && !flag2)
      xmpNode = xmp.GetRoot();
    else if (flag1 & flag2)
    {
      XmpPath xpath = XmpPathParser.ExpandXPath(schemaNs, propPath);
      XmpPath xmpPath = new XmpPath();
      for (int index = 0; index < xpath.Size() - 1; ++index)
        xmpPath.Add(xpath.GetSegment(index));
      xmpNode = XmpNodeUtils.FindNode(xmp.GetRoot(), xpath, false, (PropertyOptions) null);
      this.BaseNamespace = schemaNs;
      parentPath = xmpPath.ToString();
    }
    else
    {
      if (!flag1 || flag2)
        throw new XmpException("Schema namespace URI is required", XmpErrorCode.BadSchema);
      xmpNode = XmpNodeUtils.FindSchemaNode(xmp.GetRoot(), schemaNs, false);
    }
    this._nodeIterator = xmpNode != null ? (!this.Options.IsJustChildren ? (IIterator) new XmpIterator.NodeIterator(this, xmpNode, parentPath, 1) : (IIterator) new XmpIterator.NodeIteratorChildren(this, xmpNode, parentPath)) : (IIterator) Enumerable.Empty<object>().Iterator<object>();
  }

  public void SkipSubtree()
  {
  }

  public void SkipSiblings()
  {
    this.SkipSubtree();
    this._skipSiblings = true;
  }

  public bool HasNext() => this._nodeIterator.HasNext();

  public object Next() => this._nodeIterator.Next();

  public void Remove()
  {
    throw new NotSupportedException("The XMPIterator does not support remove().");
  }

  private IteratorOptions Options { get; }

  private string BaseNamespace { get; set; }

  private class NodeIterator : IIterator
  {
    private const int IterateNode = 0;
    private const int IterateChildren = 1;
    private const int IterateQualifier = 2;
    private int _state;
    private readonly XmpNode _visitedNode;
    private readonly string _path;
    private IIterator _childrenIterator;
    private int _index;
    private IIterator _subIterator = (IIterator) Enumerable.Empty<object>().Iterator<object>();
    private IXmpPropertyInfo _returnProperty;
    private readonly XmpIterator _enclosing;

    protected NodeIterator(XmpIterator enclosing) => this._enclosing = enclosing;

    public NodeIterator(XmpIterator enclosing, XmpNode visitedNode, string parentPath, int index)
    {
      this._enclosing = enclosing;
      this._visitedNode = visitedNode;
      this._state = 0;
      if (visitedNode.Options.IsSchemaNode)
        this._enclosing.BaseNamespace = visitedNode.Name;
      this._path = this.AccumulatePath(visitedNode, parentPath, index);
    }

    public virtual bool HasNext()
    {
      if (this._returnProperty != null)
        return true;
      switch (this._state)
      {
        case 0:
          return this.ReportNode();
        case 1:
          if (this._childrenIterator == null)
            this._childrenIterator = this._visitedNode.IterateChildren();
          bool flag = this.IterateChildrenMethod(this._childrenIterator);
          if (!flag && this._visitedNode.HasQualifier && !this._enclosing.Options.IsOmitQualifiers)
          {
            this._state = 2;
            this._childrenIterator = (IIterator) null;
            flag = this.HasNext();
          }
          return flag;
        default:
          if (this._childrenIterator == null)
            this._childrenIterator = this._visitedNode.IterateQualifier();
          return this.IterateChildrenMethod(this._childrenIterator);
      }
    }

    private bool ReportNode()
    {
      this._state = 1;
      if (this._visitedNode.Parent == null || this._enclosing.Options.IsJustLeafNodes && this._visitedNode.HasChildren)
        return this.HasNext();
      this._returnProperty = XmpIterator.NodeIterator.CreatePropertyInfo(this._visitedNode, this._enclosing.BaseNamespace, this._path);
      return true;
    }

    private bool IterateChildrenMethod(IIterator iterator)
    {
      if (this._enclosing._skipSiblings)
      {
        this._enclosing._skipSiblings = false;
        this._subIterator = (IIterator) Enumerable.Empty<object>().Iterator<object>();
      }
      if (!this._subIterator.HasNext() && iterator.HasNext())
      {
        XmpNode visitedNode = (XmpNode) iterator.Next();
        ++this._index;
        this._subIterator = (IIterator) new XmpIterator.NodeIterator(this._enclosing, visitedNode, this._path, this._index);
      }
      if (!this._subIterator.HasNext())
        return false;
      this._returnProperty = (IXmpPropertyInfo) this._subIterator.Next();
      return true;
    }

    public virtual object Next()
    {
      if (!this.HasNext())
        throw new InvalidOperationException("There are no more nodes to return");
      IXmpPropertyInfo returnProperty = this._returnProperty;
      this._returnProperty = (IXmpPropertyInfo) null;
      return (object) returnProperty;
    }

    public virtual void Remove() => throw new NotSupportedException();

    protected string AccumulatePath(XmpNode currNode, string parentPath, int currentIndex)
    {
      if (currNode.Parent == null || currNode.Options.IsSchemaNode)
        return (string) null;
      string str1;
      string str2;
      if (currNode.Parent.Options.IsArray)
      {
        str1 = string.Empty;
        str2 = $"[{currentIndex.ToString()}]";
      }
      else
      {
        str1 = "/";
        str2 = currNode.Name;
      }
      if (string.IsNullOrEmpty(parentPath))
        return str2;
      if (!this._enclosing.Options.IsJustLeafName)
        return parentPath + str1 + str2;
      return str2.StartsWith("?") ? str2.Substring(1) : str2;
    }

    protected static IXmpPropertyInfo CreatePropertyInfo(XmpNode node, string baseNs, string path)
    {
      string str = node.Options.IsSchemaNode ? (string) null : node.Value;
      return (IXmpPropertyInfo) new XmpIterator.NodeIterator.XmpPropertyInfo(node, baseNs, path, str);
    }

    protected IXmpPropertyInfo GetReturnProperty() => this._returnProperty;

    protected void SetReturnProperty(IXmpPropertyInfo returnProperty)
    {
      this._returnProperty = returnProperty;
    }

    private sealed class XmpPropertyInfo : IXmpPropertyInfo, IXmpProperty
    {
      private readonly XmpNode _node;
      private readonly string _baseNs;

      public string Path { get; }

      public string Value { get; }

      public XmpPropertyInfo(XmpNode node, string baseNs, string path, string value)
      {
        this._node = node;
        this._baseNs = baseNs;
        this.Path = path;
        this.Value = value;
      }

      public string Namespace
      {
        get
        {
          return !this._node.Options.IsSchemaNode ? XmpMetaFactory.SchemaRegistry.GetNamespaceUri(new QName(this._node.Name).Prefix) : this._baseNs;
        }
      }

      public PropertyOptions Options => this._node.Options;

      public string Language => (string) null;
    }
  }

  private sealed class NodeIteratorChildren : XmpIterator.NodeIterator
  {
    private readonly string _parentPath;
    private readonly IIterator _childrenIterator;
    private int _index;
    private readonly XmpIterator _enclosing;

    public NodeIteratorChildren(XmpIterator enclosing, XmpNode parentNode, string parentPath)
      : base(enclosing)
    {
      this._enclosing = enclosing;
      if (parentNode.Options.IsSchemaNode)
        this._enclosing.BaseNamespace = parentNode.Name;
      this._parentPath = this.AccumulatePath(parentNode, parentPath, 1);
      this._childrenIterator = parentNode.IterateChildren();
    }

    public override bool HasNext()
    {
      if (this.GetReturnProperty() != null)
        return true;
      if (this._enclosing._skipSiblings || !this._childrenIterator.HasNext())
        return false;
      XmpNode xmpNode = (XmpNode) this._childrenIterator.Next();
      ++this._index;
      string path = (string) null;
      if (xmpNode.Options.IsSchemaNode)
        this._enclosing.BaseNamespace = xmpNode.Name;
      else if (xmpNode.Parent != null)
        path = this.AccumulatePath(xmpNode, this._parentPath, this._index);
      if (this._enclosing.Options.IsJustLeafNodes && xmpNode.HasChildren)
        return this.HasNext();
      this.SetReturnProperty(XmpIterator.NodeIterator.CreatePropertyInfo(xmpNode, this._enclosing.BaseNamespace, path));
      return true;
    }
  }
}
