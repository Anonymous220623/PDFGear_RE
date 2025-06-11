// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.XmlSerialization.MyXmlNamespaceManager
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System.Collections.Generic;
using System.Xml;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.XmlSerialization;

internal class MyXmlNamespaceManager : XmlNamespaceManager
{
  private string m_missingNamespacePrefix;
  private int m_missingNamespaceIndex;
  private Dictionary<string, string> m_missingNamespaces;
  private string DefaultMissingNamespacePrefix = "http://schemas.microsoft.com/office/spreadsheetml/2015/revision2/";

  internal string MissingNamespacePrefix
  {
    get => this.m_missingNamespacePrefix;
    set => this.m_missingNamespacePrefix = value;
  }

  internal int NextMissingNamespaceIndex
  {
    get => this.m_missingNamespaceIndex;
    set => this.m_missingNamespaceIndex = value;
  }

  internal Dictionary<string, string> MissingNamespaces
  {
    get => this.m_missingNamespaces;
    set => this.m_missingNamespaces = value;
  }

  internal MyXmlNamespaceManager(XmlNameTable nameTable)
    : this(nameTable, (string) null)
  {
  }

  internal MyXmlNamespaceManager(XmlNameTable nameTable, string missingNamespacePrefix)
    : base(nameTable)
  {
    this.MissingNamespacePrefix = string.IsNullOrEmpty(missingNamespacePrefix) ? this.DefaultMissingNamespacePrefix : missingNamespacePrefix;
    this.MissingNamespaces = new Dictionary<string, string>();
  }

  internal void AddMissingNamespace(string prefix)
  {
    if (string.IsNullOrEmpty(prefix))
      return;
    string uri;
    do
    {
      uri = this.MissingNamespacePrefix + this.NextMissingNamespaceIndex++.ToString();
    }
    while (this.LookupPrefix(uri) != null);
    this.AddNamespace(prefix, uri);
    this.MissingNamespaces.Add(prefix, uri);
  }

  public override bool HasNamespace(string prefix)
  {
    if (!base.HasNamespace(prefix))
      this.AddMissingNamespace(prefix);
    return base.HasNamespace(prefix);
  }

  public override string LookupNamespace(string prefix)
  {
    if (base.LookupNamespace(prefix) == null)
      this.AddMissingNamespace(prefix);
    return base.LookupNamespace(prefix);
  }
}
