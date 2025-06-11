// Decompiled with JetBrains decompiler
// Type: XmpCore.Impl.XmpNode
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Sharpen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XmpCore.Options;

#nullable disable
namespace XmpCore.Impl;

public sealed class XmpNode : IComparable
{
  private List<XmpNode> _children;
  private Dictionary<string, XmpNode> _childrenLookup;
  private List<XmpNode> _qualifier;
  private PropertyOptions _options;

  public XmpNode(string name, string value, PropertyOptions options)
  {
    this.Name = name;
    this.Value = value;
    this._options = options;
  }

  public XmpNode(string name, PropertyOptions options)
    : this(name, (string) null, options)
  {
  }

  public void Clear()
  {
    this._options = (PropertyOptions) null;
    this.Name = (string) null;
    this.Value = (string) null;
    this._children = (List<XmpNode>) null;
    this._childrenLookup = (Dictionary<string, XmpNode>) null;
    this._qualifier = (List<XmpNode>) null;
  }

  public XmpNode Parent { get; internal set; }

  public XmpNode GetChild(int index) => this.GetChildren()[index - 1];

  public void AddChild(XmpNode node)
  {
    this.AssertChildNotExisting(node.Name);
    node.Parent = this;
    this.GetChildren().Add(node);
    if (this._childrenLookup == null || !(node.Name != "rdf:li") || !(node.Name != "[]"))
      return;
    this._childrenLookup[node.Name] = node;
  }

  public void AddChild(int index, XmpNode node)
  {
    this.AssertChildNotExisting(node.Name);
    node.Parent = this;
    this.GetChildren().Insert(index - 1, node);
    if (this._childrenLookup == null || !(node.Name != "rdf:li") || !(node.Name != "[]"))
      return;
    this._childrenLookup[node.Name] = node;
  }

  public void ReplaceChild(int index, XmpNode node)
  {
    node.Parent = this;
    this.GetChildren()[index - 1] = node;
    if (this._childrenLookup == null)
      return;
    this._childrenLookup[node.Name] = node;
  }

  public void RemoveChild(int itemIndex)
  {
    this._childrenLookup?.Remove(this.GetChildren()[itemIndex - 1].Name);
    this.GetChildren().RemoveAt(itemIndex - 1);
    this.CleanupChildren();
  }

  public void RemoveChild(XmpNode node)
  {
    this._childrenLookup?.Remove(node.Name);
    this.GetChildren().Remove(node);
    this.CleanupChildren();
  }

  private void CleanupChildren()
  {
    if (this._children.Count != 0)
      return;
    this._children = (List<XmpNode>) null;
    this._childrenLookup = (Dictionary<string, XmpNode>) null;
  }

  public void RemoveChildren()
  {
    this._children = (List<XmpNode>) null;
    this._childrenLookup = (Dictionary<string, XmpNode>) null;
  }

  public int GetChildrenLength()
  {
    List<XmpNode> children = this._children;
    // ISSUE: explicit non-virtual call
    return children == null ? 0 : __nonvirtual (children.Count);
  }

  public XmpNode FindChildByName(string expr)
  {
    return XmpNode.FindChild(this.GetChildren(), ref this._childrenLookup, expr);
  }

  public XmpNode GetQualifier(int index) => this.GetQualifier()[index - 1];

  public int GetQualifierLength()
  {
    List<XmpNode> qualifier = this._qualifier;
    // ISSUE: explicit non-virtual call
    return qualifier == null ? 0 : __nonvirtual (qualifier.Count);
  }

  public void AddQualifier(XmpNode qualNode)
  {
    this.AssertQualifierNotExisting(qualNode.Name);
    qualNode.Parent = this;
    qualNode.Options.IsQualifier = true;
    this.Options.HasQualifiers = true;
    if (qualNode.IsLanguageNode)
    {
      this._options.HasLanguage = true;
      this.GetQualifier().Insert(0, qualNode);
    }
    else if (qualNode.IsTypeNode)
    {
      this._options.HasType = true;
      this.GetQualifier().Insert(this._options.HasLanguage ? 1 : 0, qualNode);
    }
    else
      this.GetQualifier().Add(qualNode);
  }

  public void RemoveQualifier(XmpNode qualNode)
  {
    PropertyOptions options = this.Options;
    if (qualNode.IsLanguageNode)
      options.HasLanguage = false;
    else if (qualNode.IsTypeNode)
      options.HasType = false;
    this.GetQualifier().Remove(qualNode);
    if (this._qualifier.Count != 0)
      return;
    options.HasQualifiers = false;
    this._qualifier = (List<XmpNode>) null;
  }

  public void RemoveQualifiers()
  {
    PropertyOptions options = this.Options;
    options.HasQualifiers = false;
    options.HasLanguage = false;
    options.HasType = false;
    this._qualifier = (List<XmpNode>) null;
  }

  public XmpNode FindQualifierByName(string expr)
  {
    return XmpNode.Find((IEnumerable<XmpNode>) this._qualifier, expr);
  }

  public bool HasChildren => this._children != null && this._children.Count > 0;

  public IIterator IterateChildren()
  {
    return this._children == null ? (IIterator) Enumerable.Empty<object>().Iterator<object>() : (IIterator) this.GetChildren().Iterator<XmpNode>();
  }

  public bool HasQualifier => this._qualifier != null && this._qualifier.Count > 0;

  public IIterator IterateQualifier()
  {
    return this._qualifier == null ? (IIterator) Enumerable.Empty<object>().Iterator<object>() : (IIterator) new XmpNode.Iterator391((IIterator) this.GetQualifier().Iterator<XmpNode>());
  }

  public object Clone() => this.Clone(false);

  public object Clone(bool skipEmpty)
  {
    PropertyOptions options;
    try
    {
      options = new PropertyOptions(this.Options.GetOptions());
    }
    catch (XmpException ex)
    {
      options = new PropertyOptions();
    }
    XmpNode destination = new XmpNode(this.Name, this.Value, options);
    this.CloneSubtree(destination, skipEmpty);
    if (skipEmpty && string.IsNullOrEmpty(destination.Value) && !destination.HasChildren)
      destination = (XmpNode) null;
    return (object) destination;
  }

  public void CloneSubtree(XmpNode destination, bool skipEmpty)
  {
    try
    {
      IIterator iterator1 = this.IterateChildren();
      while (iterator1.HasNext())
      {
        XmpNode xmpNode = (XmpNode) iterator1.Next();
        if (!skipEmpty || !string.IsNullOrEmpty(xmpNode.Value) || xmpNode.HasChildren)
        {
          XmpNode node = (XmpNode) xmpNode.Clone(skipEmpty);
          if (node != null)
            destination.AddChild(node);
        }
      }
      IIterator iterator2 = this.IterateQualifier();
      while (iterator2.HasNext())
      {
        XmpNode xmpNode = (XmpNode) iterator2.Next();
        if (!skipEmpty || !string.IsNullOrEmpty(xmpNode.Value) || xmpNode.HasChildren)
        {
          XmpNode qualNode = (XmpNode) xmpNode.Clone(skipEmpty);
          if (qualNode != null)
            destination.AddQualifier(qualNode);
        }
      }
    }
    catch (XmpException ex)
    {
    }
  }

  public string DumpNode(bool recursive)
  {
    StringBuilder result = new StringBuilder(512 /*0x0200*/);
    this.DumpNode(result, recursive, 0, 0);
    return result.ToString();
  }

  public int CompareTo(object xmpNode)
  {
    return !this.Options.IsSchemaNode ? string.CompareOrdinal(this.Name, ((XmpNode) xmpNode).Name) : string.CompareOrdinal(this.Value, ((XmpNode) xmpNode).Value);
  }

  public string Name { set; get; }

  public string Value { get; set; }

  public PropertyOptions Options
  {
    get => this._options ?? (this._options = new PropertyOptions());
    set => this._options = value;
  }

  public bool IsImplicit { get; set; }

  public bool HasAliases { get; set; }

  public bool IsAlias { get; set; }

  public bool HasValueChild { get; set; }

  public void Sort()
  {
    if (this.HasQualifier)
      this.GetQualifier().Sort((Comparison<XmpNode>) ((a, b) => XmpNode.QualifierOrderComparer.Default.Compare(a.Name, b.Name)));
    if (this._children == null)
      return;
    if (!this.Options.IsArray)
      this._children.Sort();
    foreach (XmpNode child in this._children)
      child.Sort();
  }

  private void DumpNode(StringBuilder result, bool recursive, int indent, int index)
  {
    for (int index1 = 0; index1 < indent; ++index1)
      result.Append('\t');
    if (this.Parent != null)
    {
      if (this.Options.IsQualifier)
      {
        result.Append('?');
        result.Append(this.Name);
      }
      else if (this.Parent.Options.IsArray)
      {
        result.Append('[');
        result.Append(index);
        result.Append(']');
      }
      else
        result.Append(this.Name);
    }
    else
    {
      result.Append("ROOT NODE");
      if (!string.IsNullOrEmpty(this.Name))
      {
        result.Append(" (");
        result.Append(this.Name);
        result.Append(')');
      }
    }
    if (!string.IsNullOrEmpty(this.Value))
    {
      result.Append(" = \"");
      result.Append(this.Value);
      result.Append('"');
    }
    if (this.Options.ContainsOneOf(-1))
    {
      result.Append("\t(");
      result.Append((object) this.Options);
      result.Append(" : ");
      result.Append(this.Options.GetOptionsString());
      result.Append(')');
    }
    result.Append('\n');
    if (recursive && this.HasQualifier)
    {
      int num = 0;
      foreach (XmpNode xmpNode in (IEnumerable<XmpNode>) this.GetQualifier().OrderBy<XmpNode, string>((Func<XmpNode, string>) (q => q.Name), (IComparer<string>) XmpNode.QualifierOrderComparer.Default))
        xmpNode.DumpNode(result, recursive, indent + 2, ++num);
    }
    if (!recursive || !this.HasChildren)
      return;
    int num1 = 0;
    foreach (XmpNode xmpNode in (IEnumerable<XmpNode>) this.GetChildren().OrderBy<XmpNode, XmpNode>((Func<XmpNode, XmpNode>) (c => c)))
      xmpNode.DumpNode(result, recursive, indent + 1, ++num1);
  }

  private bool IsLanguageNode => this.Name == "xml:lang";

  private bool IsTypeNode => this.Name == "rdf:type";

  private List<XmpNode> GetChildren() => this._children ?? (this._children = new List<XmpNode>(0));

  public IEnumerable<object> GetUnmodifiableChildren()
  {
    return (IEnumerable<object>) this.GetChildren().Cast<object>().ToList<object>();
  }

  private List<XmpNode> GetQualifier()
  {
    return this._qualifier ?? (this._qualifier = new List<XmpNode>(0));
  }

  private static XmpNode Find(IEnumerable<XmpNode> list, string expr)
  {
    return list == null ? (XmpNode) null : list.FirstOrDefault<XmpNode>((Func<XmpNode, bool>) (node => node.Name == expr));
  }

  private static XmpNode FindChild(
    List<XmpNode> children,
    ref Dictionary<string, XmpNode> lookup,
    string expr)
  {
    XmpNode child1 = (XmpNode) null;
    if (lookup == null)
    {
      if (children.Count > 9)
      {
        lookup = new Dictionary<string, XmpNode>(16 /*0x10*/);
        foreach (XmpNode child2 in children)
        {
          if (child2.Name != "rdf:li" && child2.Name != "[]")
            lookup.Add(child2.Name, child2);
        }
      }
      else
        child1 = XmpNode.Find((IEnumerable<XmpNode>) children, expr);
    }
    lookup?.TryGetValue(expr, out child1);
    return child1;
  }

  private void AssertChildNotExisting(string childName)
  {
    if (childName != "[]" && this.FindChildByName(childName) != null)
      throw new XmpException($"Duplicate property or field node '{childName}'", XmpErrorCode.BadXmp);
  }

  private void AssertQualifierNotExisting(string qualifierName)
  {
    if (qualifierName != "[]" && this.FindQualifierByName(qualifierName) != null)
      throw new XmpException($"Duplicate '{qualifierName}' qualifier", XmpErrorCode.BadXmp);
  }

  private sealed class Iterator391 : IIterator
  {
    private readonly IIterator _it;

    public Iterator391(IIterator it) => this._it = it;

    public bool HasNext() => this._it.HasNext();

    public object Next() => this._it.Next();

    public void Remove()
    {
      throw new NotSupportedException("remove() is not allowed due to the internal constraints");
    }
  }

  private sealed class QualifierOrderComparer : IComparer<string>
  {
    public static readonly XmpNode.QualifierOrderComparer Default = new XmpNode.QualifierOrderComparer();

    public int Compare(string x, string y)
    {
      if (string.Equals(x, y))
        return 0;
      switch (x)
      {
        case "xml:lang":
          return -1;
        case "rdf:type":
          return !(y == "xml:lang") ? -1 : 1;
        default:
          return 0;
      }
    }
  }
}
