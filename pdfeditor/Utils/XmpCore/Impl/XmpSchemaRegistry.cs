// Decompiled with JetBrains decompiler
// Type: XmpCore.Impl.XmpSchemaRegistry
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Sharpen;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using XmpCore.Options;

#nullable disable
namespace XmpCore.Impl;

public sealed class XmpSchemaRegistry : IXmpSchemaRegistry
{
  private readonly Dictionary<string, string> _namespaceToPrefixMap = new Dictionary<string, string>();
  private readonly Dictionary<string, string> _prefixToNamespaceMap = new Dictionary<string, string>();
  private readonly Dictionary<string, IXmpAliasInfo> _aliasMap = new Dictionary<string, IXmpAliasInfo>();
  private readonly Regex _p = new Regex("[/*?\\[\\]]");
  private readonly object _lock = new object();

  public XmpSchemaRegistry()
  {
    try
    {
      this.RegisterStandardNamespaces();
      this.RegisterStandardAliases();
    }
    catch (XmpException ex)
    {
      throw new Exception("The XMPSchemaRegistry cannot be initialized!", (Exception) ex);
    }
  }

  public string RegisterNamespace(string namespaceUri, string suggestedPrefix)
  {
    lock (this._lock)
    {
      ParameterAsserts.AssertSchemaNs(namespaceUri);
      ParameterAsserts.AssertPrefix(suggestedPrefix);
      if (suggestedPrefix[suggestedPrefix.Length - 1] != ':')
        suggestedPrefix += ":";
      if (!Utils.IsXmlNameNs(suggestedPrefix.Substring(0, suggestedPrefix.Length - 1)))
        throw new XmpException("The prefix is a bad XML name", XmpErrorCode.BadXml);
      string str;
      if (this._namespaceToPrefixMap.TryGetValue(namespaceUri, out str))
        return str;
      if (this._prefixToNamespaceMap.ContainsKey(suggestedPrefix))
      {
        string key = suggestedPrefix;
        int num = 1;
        while (this._prefixToNamespaceMap.ContainsKey(key))
        {
          key = $"{suggestedPrefix.Substring(0, suggestedPrefix.Length - 1)}_{num.ToString()}_:";
          ++num;
        }
        suggestedPrefix = key;
      }
      this._prefixToNamespaceMap[suggestedPrefix] = namespaceUri;
      this._namespaceToPrefixMap[namespaceUri] = suggestedPrefix;
      return suggestedPrefix;
    }
  }

  public void DeleteNamespace(string namespaceUri)
  {
    lock (this._lock)
    {
      string namespacePrefix = this.GetNamespacePrefix(namespaceUri);
      if (namespacePrefix == null)
        return;
      this._namespaceToPrefixMap.Remove(namespaceUri);
      this._prefixToNamespaceMap.Remove(namespacePrefix);
    }
  }

  public string GetNamespacePrefix(string namespaceUri)
  {
    lock (this._lock)
    {
      string str;
      return this._namespaceToPrefixMap.TryGetValue(namespaceUri, out str) ? str : (string) null;
    }
  }

  public string GetNamespaceUri(string namespacePrefix)
  {
    lock (this._lock)
    {
      if (namespacePrefix != null && !namespacePrefix.EndsWith(":"))
        namespacePrefix += ":";
      string str;
      return this._prefixToNamespaceMap.TryGetValue(namespacePrefix, out str) ? str : (string) null;
    }
  }

  public IDictionary<string, string> Namespaces
  {
    get
    {
      lock (this._lock)
        return (IDictionary<string, string>) new Dictionary<string, string>((IDictionary<string, string>) this._namespaceToPrefixMap);
    }
  }

  public IDictionary<string, string> Prefixes
  {
    get
    {
      lock (this._lock)
        return (IDictionary<string, string>) new Dictionary<string, string>((IDictionary<string, string>) this._prefixToNamespaceMap);
    }
  }

  private void RegisterStandardNamespaces()
  {
    this.RegisterNamespace("http://www.w3.org/XML/1998/namespace", "xml");
    this.RegisterNamespace("http://www.w3.org/1999/02/22-rdf-syntax-ns#", "rdf");
    this.RegisterNamespace("http://purl.org/dc/elements/1.1/", "dc");
    this.RegisterNamespace("http://iptc.org/std/Iptc4xmpCore/1.0/xmlns/", "Iptc4xmpCore");
    this.RegisterNamespace("http://iptc.org/std/Iptc4xmpExt/2008-02-29/", "Iptc4xmpExt");
    this.RegisterNamespace("http://ns.adobe.com/DICOM/", "DICOM");
    this.RegisterNamespace("http://ns.useplus.org/ldf/xmp/1.0/", "plus");
    this.RegisterNamespace("adobe:ns:meta/", "x");
    this.RegisterNamespace("http://ns.adobe.com/iX/1.0/", "iX");
    this.RegisterNamespace("http://ns.adobe.com/xap/1.0/", "xmp");
    this.RegisterNamespace("http://ns.adobe.com/xap/1.0/rights/", "xmpRights");
    this.RegisterNamespace("http://ns.adobe.com/xap/1.0/mm/", "xmpMM");
    this.RegisterNamespace("http://ns.adobe.com/xap/1.0/bj/", "xmpBJ");
    this.RegisterNamespace("http://ns.adobe.com/xmp/note/", "xmpNote");
    this.RegisterNamespace("http://ns.adobe.com/pdf/1.3/", "pdf");
    this.RegisterNamespace("http://ns.adobe.com/pdfx/1.3/", "pdfx");
    this.RegisterNamespace("http://www.npes.org/pdfx/ns/id/", "pdfxid");
    this.RegisterNamespace("http://www.aiim.org/pdfa/ns/schema#", "pdfaSchema");
    this.RegisterNamespace("http://www.aiim.org/pdfa/ns/property#", "pdfaProperty");
    this.RegisterNamespace("http://www.aiim.org/pdfa/ns/type#", "pdfaType");
    this.RegisterNamespace("http://www.aiim.org/pdfa/ns/field#", "pdfaField");
    this.RegisterNamespace("http://www.aiim.org/pdfa/ns/id/", "pdfaid");
    this.RegisterNamespace("http://www.aiim.org/pdfa/ns/extension/", "pdfaExtension");
    this.RegisterNamespace("http://ns.adobe.com/photoshop/1.0/", "photoshop");
    this.RegisterNamespace("http://ns.adobe.com/album/1.0/", "album");
    this.RegisterNamespace("http://ns.adobe.com/exif/1.0/", "exif");
    this.RegisterNamespace("http://cipa.jp/exif/1.0/", "exifEX");
    this.RegisterNamespace("http://ns.adobe.com/exif/1.0/aux/", "aux");
    this.RegisterNamespace("http://ns.adobe.com/tiff/1.0/", "tiff");
    this.RegisterNamespace("http://ns.adobe.com/png/1.0/", "png");
    this.RegisterNamespace("http://ns.adobe.com/jpeg/1.0/", "jpeg");
    this.RegisterNamespace("http://ns.adobe.com/jp2k/1.0/", "jp2k");
    this.RegisterNamespace("http://ns.adobe.com/camera-raw-settings/1.0/", "crs");
    this.RegisterNamespace("http://ns.adobe.com/StockPhoto/1.0/", "bmsp");
    this.RegisterNamespace("http://ns.adobe.com/creatorAtom/1.0/", "creatorAtom");
    this.RegisterNamespace("http://ns.adobe.com/asf/1.0/", "asf");
    this.RegisterNamespace("http://ns.adobe.com/xmp/wav/1.0/", "wav");
    this.RegisterNamespace("http://ns.adobe.com/bwf/bext/1.0/", "bext");
    this.RegisterNamespace("http://ns.adobe.com/riff/info/", "riffinfo");
    this.RegisterNamespace("http://ns.adobe.com/xmp/1.0/Script/", "xmpScript");
    this.RegisterNamespace("http://ns.adobe.com/TransformXMP/", "txmp");
    this.RegisterNamespace("http://ns.adobe.com/swf/1.0/", "swf");
    this.RegisterNamespace("http://ns.adobe.com/ccv/1.0/", "ccv");
    this.RegisterNamespace("http://ns.adobe.com/xmp/1.0/DynamicMedia/", "xmpDM");
    this.RegisterNamespace("http://ns.adobe.com/xmp/transient/1.0/", "xmpx");
    this.RegisterNamespace("http://ns.adobe.com/xap/1.0/t/", "xmpT");
    this.RegisterNamespace("http://ns.adobe.com/xap/1.0/t/pg/", "xmpTPg");
    this.RegisterNamespace("http://ns.adobe.com/xap/1.0/g/", "xmpG");
    this.RegisterNamespace("http://ns.adobe.com/xap/1.0/g/img/", "xmpGImg");
    this.RegisterNamespace("http://ns.adobe.com/xap/1.0/sType/Font#", "stFnt");
    this.RegisterNamespace("http://ns.adobe.com/xap/1.0/sType/Dimensions#", "stDim");
    this.RegisterNamespace("http://ns.adobe.com/xap/1.0/sType/ResourceEvent#", "stEvt");
    this.RegisterNamespace("http://ns.adobe.com/xap/1.0/sType/ResourceRef#", "stRef");
    this.RegisterNamespace("http://ns.adobe.com/xap/1.0/sType/Version#", "stVer");
    this.RegisterNamespace("http://ns.adobe.com/xap/1.0/sType/Job#", "stJob");
    this.RegisterNamespace("http://ns.adobe.com/xap/1.0/sType/ManifestItem#", "stMfs");
    this.RegisterNamespace("http://ns.adobe.com/xmp/Identifier/qual/1.0/", "xmpidq");
  }

  public IXmpAliasInfo ResolveAlias(string aliasNs, string aliasProp)
  {
    lock (this._lock)
    {
      string namespacePrefix = this.GetNamespacePrefix(aliasNs);
      IXmpAliasInfo xmpAliasInfo;
      return namespacePrefix == null ? (IXmpAliasInfo) null : (this._aliasMap.TryGetValue(namespacePrefix + aliasProp, out xmpAliasInfo) ? xmpAliasInfo : (IXmpAliasInfo) null);
    }
  }

  public IXmpAliasInfo FindAlias(string qname)
  {
    lock (this._lock)
    {
      IXmpAliasInfo xmpAliasInfo;
      return this._aliasMap.TryGetValue(qname, out xmpAliasInfo) ? xmpAliasInfo : (IXmpAliasInfo) null;
    }
  }

  public IEnumerable<IXmpAliasInfo> FindAliases(string aliasNs)
  {
    lock (this._lock)
    {
      string namespacePrefix = this.GetNamespacePrefix(aliasNs);
      List<IXmpAliasInfo> aliases = new List<IXmpAliasInfo>();
      if (namespacePrefix != null)
      {
        Iterator<string> iterator = this._aliasMap.Keys.Iterator<string>();
        while (iterator.HasNext())
        {
          string qname = iterator.Next();
          if (qname.StartsWith(namespacePrefix))
            aliases.Add(this.FindAlias(qname));
        }
      }
      return (IEnumerable<IXmpAliasInfo>) aliases;
    }
  }

  private void RegisterAlias(
    string aliasNs,
    string aliasProp,
    string actualNs,
    string actualProp,
    AliasOptions aliasForm)
  {
    lock (this._lock)
    {
      ParameterAsserts.AssertSchemaNs(aliasNs);
      ParameterAsserts.AssertPropName(aliasProp);
      ParameterAsserts.AssertSchemaNs(actualNs);
      ParameterAsserts.AssertPropName(actualProp);
      AliasOptions aliasOpts = aliasForm != null ? new AliasOptions(XmpNodeUtils.VerifySetOptions(aliasForm.ToPropertyOptions(), (object) null).GetOptions()) : new AliasOptions();
      if (this._p.IsMatch(aliasProp) || this._p.IsMatch(actualProp))
        throw new XmpException("Alias and actual property names must be simple", XmpErrorCode.BadXPath);
      string namespacePrefix1 = this.GetNamespacePrefix(aliasNs);
      string namespacePrefix2 = this.GetNamespacePrefix(actualNs);
      if (namespacePrefix1 == null)
        throw new XmpException("Alias namespace is not registered", XmpErrorCode.BadSchema);
      if (namespacePrefix2 == null)
        throw new XmpException("Actual namespace is not registered", XmpErrorCode.BadSchema);
      string key = namespacePrefix1 + aliasProp;
      if (this._aliasMap.ContainsKey(key))
        throw new XmpException("Alias is already existing", XmpErrorCode.BadParam);
      if (this._aliasMap.ContainsKey(namespacePrefix2 + actualProp))
        throw new XmpException("Actual property is already an alias, use the base property", XmpErrorCode.BadParam);
      this._aliasMap[key] = (IXmpAliasInfo) new XmpSchemaRegistry.XmpAliasInfo(actualNs, namespacePrefix2, actualProp, aliasOpts);
    }
  }

  public IDictionary<string, IXmpAliasInfo> Aliases
  {
    get
    {
      lock (this._lock)
        return (IDictionary<string, IXmpAliasInfo>) new Dictionary<string, IXmpAliasInfo>((IDictionary<string, IXmpAliasInfo>) this._aliasMap);
    }
  }

  private void RegisterStandardAliases()
  {
    AliasOptions aliasForm1 = new AliasOptions()
    {
      IsArrayOrdered = true
    };
    AliasOptions aliasForm2 = new AliasOptions()
    {
      IsArrayAltText = true
    };
    this.RegisterAlias("http://ns.adobe.com/xap/1.0/", "Author", "http://purl.org/dc/elements/1.1/", "creator", aliasForm1);
    this.RegisterAlias("http://ns.adobe.com/xap/1.0/", "Authors", "http://purl.org/dc/elements/1.1/", "creator", (AliasOptions) null);
    this.RegisterAlias("http://ns.adobe.com/xap/1.0/", "Description", "http://purl.org/dc/elements/1.1/", "description", (AliasOptions) null);
    this.RegisterAlias("http://ns.adobe.com/xap/1.0/", "Format", "http://purl.org/dc/elements/1.1/", "format", (AliasOptions) null);
    this.RegisterAlias("http://ns.adobe.com/xap/1.0/", "Keywords", "http://purl.org/dc/elements/1.1/", "subject", (AliasOptions) null);
    this.RegisterAlias("http://ns.adobe.com/xap/1.0/", "Locale", "http://purl.org/dc/elements/1.1/", "language", (AliasOptions) null);
    this.RegisterAlias("http://ns.adobe.com/xap/1.0/", "Title", "http://purl.org/dc/elements/1.1/", "title", (AliasOptions) null);
    this.RegisterAlias("http://ns.adobe.com/xap/1.0/rights/", "Copyright", "http://purl.org/dc/elements/1.1/", "rights", (AliasOptions) null);
    this.RegisterAlias("http://ns.adobe.com/pdf/1.3/", "Author", "http://purl.org/dc/elements/1.1/", "creator", aliasForm1);
    this.RegisterAlias("http://ns.adobe.com/pdf/1.3/", "BaseURL", "http://ns.adobe.com/xap/1.0/", "BaseURL", (AliasOptions) null);
    this.RegisterAlias("http://ns.adobe.com/pdf/1.3/", "CreationDate", "http://ns.adobe.com/xap/1.0/", "CreateDate", (AliasOptions) null);
    this.RegisterAlias("http://ns.adobe.com/pdf/1.3/", "Creator", "http://ns.adobe.com/xap/1.0/", "CreatorTool", (AliasOptions) null);
    this.RegisterAlias("http://ns.adobe.com/pdf/1.3/", "ModDate", "http://ns.adobe.com/xap/1.0/", "ModifyDate", (AliasOptions) null);
    this.RegisterAlias("http://ns.adobe.com/pdf/1.3/", "Subject", "http://purl.org/dc/elements/1.1/", "description", aliasForm2);
    this.RegisterAlias("http://ns.adobe.com/pdf/1.3/", "Title", "http://purl.org/dc/elements/1.1/", "title", aliasForm2);
    this.RegisterAlias("http://ns.adobe.com/photoshop/1.0/", "Author", "http://purl.org/dc/elements/1.1/", "creator", aliasForm1);
    this.RegisterAlias("http://ns.adobe.com/photoshop/1.0/", "Caption", "http://purl.org/dc/elements/1.1/", "description", aliasForm2);
    this.RegisterAlias("http://ns.adobe.com/photoshop/1.0/", "Copyright", "http://purl.org/dc/elements/1.1/", "rights", aliasForm2);
    this.RegisterAlias("http://ns.adobe.com/photoshop/1.0/", "Keywords", "http://purl.org/dc/elements/1.1/", "subject", (AliasOptions) null);
    this.RegisterAlias("http://ns.adobe.com/photoshop/1.0/", "Marked", "http://ns.adobe.com/xap/1.0/rights/", "Marked", (AliasOptions) null);
    this.RegisterAlias("http://ns.adobe.com/photoshop/1.0/", "Title", "http://purl.org/dc/elements/1.1/", "title", aliasForm2);
    this.RegisterAlias("http://ns.adobe.com/photoshop/1.0/", "WebStatement", "http://ns.adobe.com/xap/1.0/rights/", "WebStatement", (AliasOptions) null);
    this.RegisterAlias("http://ns.adobe.com/tiff/1.0/", "Artist", "http://purl.org/dc/elements/1.1/", "creator", aliasForm1);
    this.RegisterAlias("http://ns.adobe.com/tiff/1.0/", "Copyright", "http://purl.org/dc/elements/1.1/", "rights", (AliasOptions) null);
    this.RegisterAlias("http://ns.adobe.com/tiff/1.0/", "DateTime", "http://ns.adobe.com/xap/1.0/", "ModifyDate", (AliasOptions) null);
    this.RegisterAlias("http://ns.adobe.com/exif/1.0/", "DateTimeDigitized", "http://ns.adobe.com/xap/1.0/", "CreateDate", (AliasOptions) null);
    this.RegisterAlias("http://ns.adobe.com/tiff/1.0/", "ImageDescription", "http://purl.org/dc/elements/1.1/", "description", (AliasOptions) null);
    this.RegisterAlias("http://ns.adobe.com/tiff/1.0/", "Software", "http://ns.adobe.com/xap/1.0/", "CreatorTool", (AliasOptions) null);
    this.RegisterAlias("http://ns.adobe.com/png/1.0/", "Author", "http://purl.org/dc/elements/1.1/", "creator", aliasForm1);
    this.RegisterAlias("http://ns.adobe.com/png/1.0/", "Copyright", "http://purl.org/dc/elements/1.1/", "rights", aliasForm2);
    this.RegisterAlias("http://ns.adobe.com/png/1.0/", "CreationTime", "http://ns.adobe.com/xap/1.0/", "CreateDate", (AliasOptions) null);
    this.RegisterAlias("http://ns.adobe.com/png/1.0/", "Description", "http://purl.org/dc/elements/1.1/", "description", aliasForm2);
    this.RegisterAlias("http://ns.adobe.com/png/1.0/", "ModificationTime", "http://ns.adobe.com/xap/1.0/", "ModifyDate", (AliasOptions) null);
    this.RegisterAlias("http://ns.adobe.com/png/1.0/", "Software", "http://ns.adobe.com/xap/1.0/", "CreatorTool", (AliasOptions) null);
    this.RegisterAlias("http://ns.adobe.com/png/1.0/", "Title", "http://purl.org/dc/elements/1.1/", "title", aliasForm2);
  }

  private sealed class XmpAliasInfo : IXmpAliasInfo
  {
    public XmpAliasInfo(
      string actualNs,
      string actualPrefix,
      string actualProp,
      AliasOptions aliasOpts)
    {
      this.Namespace = actualNs;
      this.Prefix = actualPrefix;
      this.PropName = actualProp;
      this.AliasForm = aliasOpts;
    }

    public string Namespace { get; }

    public string Prefix { get; }

    public string PropName { get; }

    public AliasOptions AliasForm { get; }

    public override string ToString()
    {
      return $"{this.Prefix}{this.PropName} NS({this.Namespace}), FORM ({this.AliasForm})";
    }
  }
}
