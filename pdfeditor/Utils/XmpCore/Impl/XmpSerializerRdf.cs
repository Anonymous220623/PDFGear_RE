// Decompiled with JetBrains decompiler
// Type: XmpCore.Impl.XmpSerializerRdf
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Sharpen;
using System.Collections.Generic;
using System.IO;
using System.Text;
using XmpCore.Options;

#nullable disable
namespace XmpCore.Impl;

public sealed class XmpSerializerRdf
{
  private const int DefaultPad = 2048 /*0x0800*/;
  private const string PacketHeader = "<?xpacket begin=\"\uFEFF\" id=\"W5M0MpCehiHzreSzNTczkc9d\"?>";
  private const string PacketTrailer = "<?xpacket end=\"";
  private const string PacketTrailer2 = "\"?>";
  private const string RdfXmpmetaStart = "<x:xmpmeta xmlns:x=\"adobe:ns:meta/\" x:xmptk=\"";
  private const string RdfXmpmetaEnd = "</x:xmpmeta>";
  private const string RdfRdfStart = "<rdf:RDF xmlns:rdf=\"http://www.w3.org/1999/02/22-rdf-syntax-ns#\">";
  private const string RdfRdfEnd = "</rdf:RDF>";
  private const string RdfSchemaStart = "<rdf:Description rdf:about=";
  private const string RdfSchemaEnd = "</rdf:Description>";
  private const string RdfStructStart = "<rdf:Description";
  private const string RdfStructEnd = "</rdf:Description>";
  private const string RdfEmptyStruct = "<rdf:Description/>";
  private static readonly ICollection<object> RdfAttrQualifier = (ICollection<object>) new HashSet<object>((IEnumerable<object>) new string[5]
  {
    "xml:lang",
    "rdf:resource",
    "rdf:ID",
    "rdf:bagID",
    "rdf:nodeID"
  });
  private XmpMeta _xmp;
  private Stream _stream;
  private StreamWriter _writer;
  private SerializeOptions _options;
  private int _unicodeSize = 1;
  private int _padding;
  private long _startPos;

  public void Serialize(IXmpMeta xmp, Stream stream, SerializeOptions options)
  {
    try
    {
      this._stream = stream;
      this._startPos = this._stream.Position;
      this._writer = new StreamWriter(this._stream, options.GetEncoding());
      this._xmp = (XmpMeta) xmp;
      this._options = options;
      this._padding = options.Padding;
      this.CheckOptionsConsistence();
      string str = this.SerializeAsRdf();
      this._writer.Flush();
      this.AddPadding(str.Length);
      this.Write(str);
      this._writer.Flush();
    }
    catch (IOException ex)
    {
      throw new XmpException("Error writing to the OutputStream", XmpErrorCode.Unknown);
    }
  }

  private void AddPadding(int tailLength)
  {
    if (this._options.ExactPacketLength)
    {
      int num = checked ((int) (this._stream.Position - this._startPos)) + tailLength * this._unicodeSize;
      if (num > this._padding)
        throw new XmpException("Can't fit into specified packet size", XmpErrorCode.BadSerialize);
      this._padding -= num;
    }
    this._padding /= this._unicodeSize;
    int length = this._options.Newline.Length;
    if (this._padding >= length)
    {
      for (this._padding -= length; this._padding >= 100 + length; this._padding -= 100 + length)
      {
        this.WriteChars(100, ' ');
        this.WriteNewline();
      }
      this.WriteChars(this._padding, ' ');
      this.WriteNewline();
    }
    else
      this.WriteChars(this._padding, ' ');
  }

  private void CheckOptionsConsistence()
  {
    if (this._options.EncodeUtf16Be || this._options.EncodeUtf16Le)
      this._unicodeSize = 2;
    if (this._options.ExactPacketLength)
    {
      if (this._options.OmitPacketWrapper || this._options.IncludeThumbnailPad)
        throw new XmpException("Inconsistent options for exact size serialize", XmpErrorCode.BadOptions);
      if ((this._options.Padding & this._unicodeSize - 1) != 0)
        throw new XmpException("Exact size must be a multiple of the Unicode element", XmpErrorCode.BadOptions);
    }
    else if (this._options.ReadOnlyPacket)
    {
      if (this._options.OmitPacketWrapper || this._options.IncludeThumbnailPad)
        throw new XmpException("Inconsistent options for read-only packet", XmpErrorCode.BadOptions);
      this._padding = 0;
    }
    else if (this._options.OmitPacketWrapper)
    {
      if (this._options.IncludeThumbnailPad)
        throw new XmpException("Inconsistent options for non-packet serialize", XmpErrorCode.BadOptions);
      this._padding = 0;
    }
    else
    {
      if (this._padding == 0)
        this._padding = 2048 /*0x0800*/ * this._unicodeSize;
      if (!this._options.IncludeThumbnailPad || this._xmp.DoesPropertyExist("http://ns.adobe.com/xap/1.0/", "Thumbnails"))
        return;
      this._padding += 10000 * this._unicodeSize;
    }
  }

  private string SerializeAsRdf()
  {
    int num = 0;
    if (!this._options.OmitPacketWrapper)
    {
      this.WriteIndent(num);
      this.Write("<?xpacket begin=\"\uFEFF\" id=\"W5M0MpCehiHzreSzNTczkc9d\"?>");
      this.WriteNewline();
    }
    if (!this._options.OmitXmpMetaElement)
    {
      this.WriteIndent(num);
      this.Write("<x:xmpmeta xmlns:x=\"adobe:ns:meta/\" x:xmptk=\"");
      this.Write(XmpMetaFactory.VersionInfo.Message);
      this.Write("\">");
      this.WriteNewline();
      ++num;
    }
    this.WriteIndent(num);
    this.Write("<rdf:RDF xmlns:rdf=\"http://www.w3.org/1999/02/22-rdf-syntax-ns#\">");
    this.WriteNewline();
    if (this._options.UseCanonicalFormat)
      this.SerializeCanonicalRdfSchemas(num);
    else
      this.SerializeCompactRdfSchemas(num);
    this.WriteIndent(num);
    this.Write("</rdf:RDF>");
    this.WriteNewline();
    if (!this._options.OmitXmpMetaElement)
    {
      this.WriteIndent(num - 1);
      this.Write("</x:xmpmeta>");
      this.WriteNewline();
    }
    StringBuilder stringBuilder = new StringBuilder();
    if (!this._options.OmitPacketWrapper)
    {
      for (int baseIndent = this._options.BaseIndent; baseIndent > 0; --baseIndent)
        stringBuilder.Append(this._options.Indent);
      stringBuilder.Append("<?xpacket end=\"");
      stringBuilder.Append(this._options.ReadOnlyPacket ? 'r' : 'w');
      stringBuilder.Append("\"?>");
    }
    return stringBuilder.ToString();
  }

  private void SerializeCanonicalRdfSchemas(int level)
  {
    if (this._xmp.GetRoot().GetChildrenLength() > 0)
    {
      this.StartOuterRdfDescription(this._xmp.GetRoot(), level);
      IIterator iterator = this._xmp.GetRoot().IterateChildren();
      while (iterator.HasNext())
        this.SerializeCanonicalRdfSchema((XmpNode) iterator.Next(), level);
      this.EndOuterRdfDescription(level);
    }
    else
    {
      this.WriteIndent(level + 1);
      this.Write("<rdf:Description rdf:about=");
      this.WriteTreeName();
      this.Write("/>");
      this.WriteNewline();
    }
  }

  private void WriteTreeName()
  {
    this.Write('"');
    string name = this._xmp.GetRoot().Name;
    if (name != null)
      this.AppendNodeValue(name, true);
    this.Write('"');
  }

  private void SerializeCompactRdfSchemas(int level)
  {
    this.WriteIndent(level + 1);
    this.Write("<rdf:Description rdf:about=");
    this.WriteTreeName();
    ICollection<object> usedPrefixes = (ICollection<object>) new HashSet<object>();
    usedPrefixes.Add((object) "xml");
    usedPrefixes.Add((object) "rdf");
    IIterator iterator1 = this._xmp.GetRoot().IterateChildren();
    while (iterator1.HasNext())
      this.DeclareUsedNamespaces((XmpNode) iterator1.Next(), usedPrefixes, level + 3);
    bool flag = true;
    IIterator iterator2 = this._xmp.GetRoot().IterateChildren();
    while (iterator2.HasNext())
    {
      XmpNode parentNode = (XmpNode) iterator2.Next();
      flag &= this.SerializeCompactRdfAttrProps(parentNode, level + 2);
    }
    if (!flag)
    {
      this.Write('>');
      this.WriteNewline();
      IIterator iterator3 = this._xmp.GetRoot().IterateChildren();
      while (iterator3.HasNext())
        this.SerializeCompactRdfElementProps((XmpNode) iterator3.Next(), level + 2);
      this.WriteIndent(level + 1);
      this.Write("</rdf:Description>");
      this.WriteNewline();
    }
    else
    {
      this.Write("/>");
      this.WriteNewline();
    }
  }

  private bool SerializeCompactRdfAttrProps(XmpNode parentNode, int indent)
  {
    bool flag = true;
    IIterator iterator = parentNode.IterateChildren();
    while (iterator.HasNext())
    {
      XmpNode node = (XmpNode) iterator.Next();
      if (XmpSerializerRdf.CanBeRdfAttrProp(node))
      {
        this.WriteNewline();
        this.WriteIndent(indent);
        this.Write(node.Name);
        this.Write("=\"");
        this.AppendNodeValue(node.Value, true);
        this.Write('"');
      }
      else
        flag = false;
    }
    return flag;
  }

  private void SerializeCompactRdfElementProps(XmpNode parentNode, int indent)
  {
    IIterator iterator1 = parentNode.IterateChildren();
    while (iterator1.HasNext())
    {
      XmpNode node = (XmpNode) iterator1.Next();
      if (!XmpSerializerRdf.CanBeRdfAttrProp(node))
      {
        bool flag1 = true;
        bool flag2 = true;
        string str = node.Name;
        if (str == "[]")
          str = "rdf:li";
        this.WriteIndent(indent);
        this.Write('<');
        this.Write(str);
        bool flag3 = false;
        bool hasRdfResourceQual = false;
        IIterator iterator2 = node.IterateQualifier();
        while (iterator2.HasNext())
        {
          XmpNode xmpNode = (XmpNode) iterator2.Next();
          if (!XmpSerializerRdf.RdfAttrQualifier.Contains((object) xmpNode.Name))
          {
            flag3 = true;
          }
          else
          {
            hasRdfResourceQual = xmpNode.Name == "rdf:resource";
            this.Write(' ');
            this.Write(xmpNode.Name);
            this.Write("=\"");
            this.AppendNodeValue(xmpNode.Value, true);
            this.Write('"');
          }
        }
        if (flag3)
          this.SerializeCompactRdfGeneralQualifier(indent, node);
        else if (!node.Options.IsCompositeProperty)
        {
          object[] objArray = this.SerializeCompactRdfSimpleProp(node);
          flag1 = (bool) objArray[0];
          flag2 = (bool) objArray[1];
        }
        else if (node.Options.IsArray)
          this.SerializeCompactRdfArrayProp(node, indent);
        else
          flag1 = this.SerializeCompactRdfStructProp(node, indent, hasRdfResourceQual);
        if (flag1)
        {
          if (flag2)
            this.WriteIndent(indent);
          this.Write("</");
          this.Write(str);
          this.Write('>');
          this.WriteNewline();
        }
      }
    }
  }

  private object[] SerializeCompactRdfSimpleProp(XmpNode node)
  {
    bool flag1 = true;
    bool flag2 = true;
    if (node.Options.IsUri)
    {
      this.Write(" rdf:resource=\"");
      this.AppendNodeValue(node.Value, true);
      this.Write("\"/>");
      this.WriteNewline();
      flag1 = false;
    }
    else if (string.IsNullOrEmpty(node.Value))
    {
      this.Write("/>");
      this.WriteNewline();
      flag1 = false;
    }
    else
    {
      this.Write('>');
      this.AppendNodeValue(node.Value, false);
      flag2 = false;
    }
    return new object[2]{ (object) flag1, (object) flag2 };
  }

  private void SerializeCompactRdfArrayProp(XmpNode node, int indent)
  {
    this.Write('>');
    this.WriteNewline();
    this.EmitRdfArrayTag(node, true, indent + 1);
    if (node.Options.IsArrayAltText)
      XmpNodeUtils.NormalizeLangArray(node);
    this.SerializeCompactRdfElementProps(node, indent + 2);
    this.EmitRdfArrayTag(node, false, indent + 1);
  }

  private bool SerializeCompactRdfStructProp(XmpNode node, int indent, bool hasRdfResourceQual)
  {
    bool flag1 = false;
    bool flag2 = false;
    bool flag3 = true;
    IIterator iterator = node.IterateChildren();
    while (iterator.HasNext())
    {
      if (XmpSerializerRdf.CanBeRdfAttrProp((XmpNode) iterator.Next()))
        flag1 = true;
      else
        flag2 = true;
      if (flag1 & flag2)
        break;
    }
    if (hasRdfResourceQual & flag2)
      throw new XmpException("Can't mix rdf:resource qualifier and element fields", XmpErrorCode.BadRdf);
    if (!node.HasChildren)
    {
      this.Write(" rdf:parseType=\"Resource\"/>");
      this.WriteNewline();
      flag3 = false;
    }
    else if (!flag2)
    {
      this.SerializeCompactRdfAttrProps(node, indent + 1);
      this.Write("/>");
      this.WriteNewline();
      flag3 = false;
    }
    else if (!flag1)
    {
      this.Write(" rdf:parseType=\"Resource\">");
      this.WriteNewline();
      this.SerializeCompactRdfElementProps(node, indent + 1);
    }
    else
    {
      this.Write('>');
      this.WriteNewline();
      this.WriteIndent(indent + 1);
      this.Write("<rdf:Description");
      this.SerializeCompactRdfAttrProps(node, indent + 2);
      this.Write(">");
      this.WriteNewline();
      this.SerializeCompactRdfElementProps(node, indent + 1);
      this.WriteIndent(indent + 1);
      this.Write("</rdf:Description>");
      this.WriteNewline();
    }
    return flag3;
  }

  private void SerializeCompactRdfGeneralQualifier(int indent, XmpNode node)
  {
    this.Write(" rdf:parseType=\"Resource\">");
    this.WriteNewline();
    this.SerializeCanonicalRdfProperty(node, false, true, indent + 1);
    IIterator iterator = node.IterateQualifier();
    while (iterator.HasNext())
      this.SerializeCanonicalRdfProperty((XmpNode) iterator.Next(), false, false, indent + 1);
  }

  private void SerializeCanonicalRdfSchema(XmpNode schemaNode, int level)
  {
    IIterator iterator = schemaNode.IterateChildren();
    while (iterator.HasNext())
      this.SerializeCanonicalRdfProperty((XmpNode) iterator.Next(), this._options.UseCanonicalFormat, false, level + 2);
  }

  private void DeclareUsedNamespaces(XmpNode node, ICollection<object> usedPrefixes, int indent)
  {
    if (node.Options.IsSchemaNode)
      this.DeclareNamespace(node.Value.Substring(0, node.Value.Length - 1), node.Name, usedPrefixes, indent);
    else if (node.Options.IsStruct)
    {
      IIterator iterator = node.IterateChildren();
      while (iterator.HasNext())
        this.DeclareNamespace(((XmpNode) iterator.Next()).Name, (string) null, usedPrefixes, indent);
    }
    IIterator iterator1 = node.IterateChildren();
    while (iterator1.HasNext())
      this.DeclareUsedNamespaces((XmpNode) iterator1.Next(), usedPrefixes, indent);
    IIterator iterator2 = node.IterateQualifier();
    while (iterator2.HasNext())
    {
      XmpNode node1 = (XmpNode) iterator2.Next();
      this.DeclareNamespace(node1.Name, (string) null, usedPrefixes, indent);
      this.DeclareUsedNamespaces(node1, usedPrefixes, indent);
    }
  }

  private void DeclareNamespace(
    string prefix,
    string ns,
    ICollection<object> usedPrefixes,
    int indent)
  {
    if (ns == null)
    {
      QName qname = new QName(prefix);
      if (!qname.HasPrefix)
        return;
      prefix = qname.Prefix;
      ns = XmpMetaFactory.SchemaRegistry.GetNamespaceUri(prefix + ":");
      this.DeclareNamespace(prefix, ns, usedPrefixes, indent);
    }
    if (usedPrefixes.Contains((object) prefix))
      return;
    this.WriteNewline();
    this.WriteIndent(indent);
    this.Write("xmlns:");
    this.Write(prefix);
    this.Write("=\"");
    this.Write(ns);
    this.Write('"');
    usedPrefixes.Add((object) prefix);
  }

  private void StartOuterRdfDescription(XmpNode schemaNode, int level)
  {
    this.WriteIndent(level + 1);
    this.Write("<rdf:Description rdf:about=");
    this.WriteTreeName();
    ICollection<object> usedPrefixes = (ICollection<object>) new HashSet<object>();
    usedPrefixes.Add((object) "xml");
    usedPrefixes.Add((object) "rdf");
    this.DeclareUsedNamespaces(schemaNode, usedPrefixes, level + 3);
    this.Write('>');
    this.WriteNewline();
  }

  private void EndOuterRdfDescription(int level)
  {
    this.WriteIndent(level + 1);
    this.Write("</rdf:Description>");
    this.WriteNewline();
  }

  private void SerializeCanonicalRdfProperty(
    XmpNode node,
    bool useCanonicalRdf,
    bool emitAsRdfValue,
    int indent)
  {
    bool flag1 = true;
    bool flag2 = true;
    string str = node.Name;
    if (emitAsRdfValue)
      str = "rdf:value";
    else if (str == "[]")
      str = "rdf:li";
    this.WriteIndent(indent);
    this.Write('<');
    this.Write(str);
    bool flag3 = false;
    bool flag4 = false;
    IIterator iterator1 = node.IterateQualifier();
    while (iterator1.HasNext())
    {
      XmpNode xmpNode = (XmpNode) iterator1.Next();
      if (!XmpSerializerRdf.RdfAttrQualifier.Contains((object) xmpNode.Name))
      {
        flag3 = true;
      }
      else
      {
        flag4 = xmpNode.Name == "rdf:resource";
        if (!emitAsRdfValue)
        {
          this.Write(' ');
          this.Write(xmpNode.Name);
          this.Write("=\"");
          this.AppendNodeValue(xmpNode.Value, true);
          this.Write('"');
        }
      }
    }
    if (flag3 && !emitAsRdfValue)
    {
      if (flag4)
        throw new XmpException("Can't mix rdf:resource and general qualifiers", XmpErrorCode.BadRdf);
      if (useCanonicalRdf)
      {
        this.Write(">");
        this.WriteNewline();
        ++indent;
        this.WriteIndent(indent);
        this.Write("<rdf:Description");
        this.Write(">");
      }
      else
        this.Write(" rdf:parseType=\"Resource\">");
      this.WriteNewline();
      this.SerializeCanonicalRdfProperty(node, useCanonicalRdf, true, indent + 1);
      IIterator iterator2 = node.IterateQualifier();
      while (iterator2.HasNext())
      {
        XmpNode node1 = (XmpNode) iterator2.Next();
        if (!XmpSerializerRdf.RdfAttrQualifier.Contains((object) node1.Name))
          this.SerializeCanonicalRdfProperty(node1, useCanonicalRdf, false, indent + 1);
      }
      if (useCanonicalRdf)
      {
        this.WriteIndent(indent);
        this.Write("</rdf:Description>");
        this.WriteNewline();
        --indent;
      }
    }
    else if (!node.Options.IsCompositeProperty)
    {
      if (node.Options.IsUri)
      {
        this.Write(" rdf:resource=\"");
        this.AppendNodeValue(node.Value, true);
        this.Write("\"/>");
        this.WriteNewline();
        flag1 = false;
      }
      else if (string.IsNullOrEmpty(node.Value))
      {
        this.Write("/>");
        this.WriteNewline();
        flag1 = false;
      }
      else
      {
        this.Write('>');
        this.AppendNodeValue(node.Value, false);
        flag2 = false;
      }
    }
    else if (node.Options.IsArray)
    {
      this.Write('>');
      this.WriteNewline();
      this.EmitRdfArrayTag(node, true, indent + 1);
      if (node.Options.IsArrayAltText)
        XmpNodeUtils.NormalizeLangArray(node);
      IIterator iterator3 = node.IterateChildren();
      while (iterator3.HasNext())
        this.SerializeCanonicalRdfProperty((XmpNode) iterator3.Next(), useCanonicalRdf, false, indent + 2);
      this.EmitRdfArrayTag(node, false, indent + 1);
    }
    else if (!flag4)
    {
      if (!node.HasChildren)
      {
        if (useCanonicalRdf)
        {
          this.Write(">");
          this.WriteNewline();
          this.WriteIndent(indent + 1);
          this.Write("<rdf:Description/>");
        }
        else
        {
          this.Write(" rdf:parseType=\"Resource\"/>");
          flag1 = false;
        }
        this.WriteNewline();
      }
      else
      {
        if (useCanonicalRdf)
        {
          this.Write(">");
          this.WriteNewline();
          ++indent;
          this.WriteIndent(indent);
          this.Write("<rdf:Description");
          this.Write(">");
        }
        else
          this.Write(" rdf:parseType=\"Resource\">");
        this.WriteNewline();
        IIterator iterator4 = node.IterateChildren();
        while (iterator4.HasNext())
          this.SerializeCanonicalRdfProperty((XmpNode) iterator4.Next(), useCanonicalRdf, false, indent + 1);
        if (useCanonicalRdf)
        {
          this.WriteIndent(indent);
          this.Write("</rdf:Description>");
          this.WriteNewline();
          --indent;
        }
      }
    }
    else
    {
      IIterator iterator5 = node.IterateChildren();
      while (iterator5.HasNext())
      {
        XmpNode node2 = (XmpNode) iterator5.Next();
        if (!XmpSerializerRdf.CanBeRdfAttrProp(node2))
          throw new XmpException("Can't mix rdf:resource and complex fields", XmpErrorCode.BadRdf);
        this.WriteNewline();
        this.WriteIndent(indent + 1);
        this.Write(' ');
        this.Write(node2.Name);
        this.Write("=\"");
        this.AppendNodeValue(node2.Value, true);
        this.Write('"');
      }
      this.Write("/>");
      this.WriteNewline();
      flag1 = false;
    }
    if (!flag1)
      return;
    if (flag2)
      this.WriteIndent(indent);
    this.Write("</");
    this.Write(str);
    this.Write('>');
    this.WriteNewline();
  }

  private void EmitRdfArrayTag(XmpNode arrayNode, bool isStartTag, int indent)
  {
    if (!isStartTag && !arrayNode.HasChildren)
      return;
    this.WriteIndent(indent);
    this.Write(isStartTag ? "<rdf:" : "</rdf:");
    if (arrayNode.Options.IsArrayAlternate)
      this.Write("Alt");
    else
      this.Write(arrayNode.Options.IsArrayOrdered ? "Seq" : "Bag");
    if (isStartTag && !arrayNode.HasChildren)
      this.Write("/>");
    else
      this.Write(">");
    this.WriteNewline();
  }

  private void AppendNodeValue(string value, bool forAttribute)
  {
    if (value == null)
      value = string.Empty;
    this.Write(Utils.EscapeXml(value, forAttribute, true));
  }

  private static bool CanBeRdfAttrProp(XmpNode node)
  {
    return !node.HasQualifier && !node.Options.IsUri && !node.Options.IsCompositeProperty && node.Name != "[]";
  }

  private void WriteIndent(int times)
  {
    for (int index = this._options.BaseIndent + times; index > 0; --index)
      this._writer.Write(this._options.Indent);
  }

  private void Write(int c) => this._writer.Write(c);

  private void Write(char c) => this._writer.Write(c);

  private void Write(string str) => this._writer.Write(str);

  private void WriteChars(int number, char c)
  {
    for (; number > 0; --number)
      this._writer.Write(c);
  }

  private void WriteNewline() => this._writer.Write(this._options.Newline);
}
