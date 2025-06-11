// Decompiled with JetBrains decompiler
// Type: XmpCore.Impl.XmpMetaParser
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using XmpCore.Options;

#nullable disable
namespace XmpCore.Impl;

public static class XmpMetaParser
{
  private static readonly object XmpRdf = new object();

  public static IXmpMeta Parse(Stream stream, ParseOptions options = null)
  {
    ParameterAsserts.AssertNotNull((object) stream);
    options = options ?? new ParseOptions();
    return XmpMetaParser.ParseXmlDoc(XmpMetaParser.ParseXmlFromInputStream(stream, options), options);
  }

  public static IXmpMeta Parse(byte[] bytes, ParseOptions options = null)
  {
    ParameterAsserts.AssertNotNull((object) bytes);
    options = options ?? new ParseOptions();
    return XmpMetaParser.ParseXmlDoc(XmpMetaParser.ParseXmlFromByteBuffer(new ByteBuffer(bytes), options), options);
  }

  public static IXmpMeta Parse(ByteBuffer byteBuffer, ParseOptions options = null)
  {
    ParameterAsserts.AssertNotNull((object) byteBuffer);
    options = options ?? new ParseOptions();
    return XmpMetaParser.ParseXmlDoc(XmpMetaParser.ParseXmlFromByteBuffer(byteBuffer, options), options);
  }

  public static IXmpMeta Parse(string xmlStr, ParseOptions options = null)
  {
    ParameterAsserts.AssertNotNullOrEmpty(xmlStr);
    options = options ?? new ParseOptions();
    return XmpMetaParser.ParseXmlDoc(XmpMetaParser.ParseXmlString(xmlStr, options), options);
  }

  public static IXmpMeta Parse(XDocument doc, ParseOptions options = null)
  {
    ParameterAsserts.AssertNotNull((object) doc);
    options = options ?? new ParseOptions();
    return XmpMetaParser.ParseXmlDoc(doc, options);
  }

  public static XDocument Extract(byte[] bytes, ParseOptions options = null)
  {
    ParameterAsserts.AssertNotNull((object) bytes);
    options = options ?? new ParseOptions();
    return XmpMetaParser.ParseXmlFromByteBuffer(new ByteBuffer(bytes), options);
  }

  private static IXmpMeta ParseXmlDoc(XDocument document, ParseOptions options)
  {
    foreach (XElement xelement in document.Descendants().Where<XElement>((Func<XElement, bool>) (d => d.Attributes().Count<XAttribute>() > 1)))
    {
      XElement node = xelement;
      IOrderedEnumerable<XAttribute> content = node.Attributes().OrderBy<XAttribute, bool>((Func<XAttribute, bool>) (n => !n.IsNamespaceDeclaration)).ThenBy<XAttribute, string>((Func<XAttribute, string>) (t => node.GetPrefixOfNamespace(t.Name.Namespace)), (IComparer<string>) StringComparer.Ordinal).ThenBy<XAttribute, string>((Func<XAttribute, string>) (s => s.Name.LocalName), (IComparer<string>) StringComparer.Ordinal);
      node.ReplaceAttributes((object) content);
    }
    object[] rootNode = XmpMetaParser.FindRootNode(document.Nodes(), options.RequireXmpMeta, new object[3]);
    if (rootNode == null || rootNode[1] != XmpMetaParser.XmpRdf)
      return (IXmpMeta) new XmpMeta();
    XmpMeta xmp = ParseRdf.Parse((XElement) rootNode[0], options);
    xmp.SetPacketHeader((string) rootNode[2]);
    return options.OmitNormalization ? (IXmpMeta) xmp : XmpNormalizer.Process(xmp, options);
  }

  private static XDocument ParseXmlFromInputStream(Stream stream, ParseOptions options)
  {
    if (!options.AcceptLatin1 && !options.FixControlChars && !options.DisallowDoctype)
      return XmpMetaParser.ParseStream(stream, options);
    try
    {
      return XmpMetaParser.ParseXmlFromByteBuffer(new ByteBuffer(stream), options);
    }
    catch (IOException ex)
    {
      throw new XmpException("Error reading the XML-file", XmpErrorCode.BadStream, (Exception) ex);
    }
  }

  private static XDocument ParseXmlFromByteBuffer(ByteBuffer buffer, ParseOptions options)
  {
    try
    {
      return XmpMetaParser.ParseStream(buffer.GetByteStream(), options);
    }
    catch (XmpException ex)
    {
      if (ex.ErrorCode == XmpErrorCode.BadXml || ex.ErrorCode == XmpErrorCode.BadStream)
      {
        if (options.AcceptLatin1)
          buffer = Latin1Converter.Convert(buffer);
        if (!options.FixControlChars)
          return XmpMetaParser.ParseStream(buffer.GetByteStream(), options);
        try
        {
          return XmpMetaParser.ParseTextReader((TextReader) new FixAsciiControlsReader(new StreamReader(buffer.GetByteStream(), buffer.GetEncoding())), options);
        }
        catch
        {
          throw new XmpException("Unsupported Encoding", XmpErrorCode.InternalFailure, (Exception) ex);
        }
      }
      else
        throw;
    }
  }

  private static XDocument ParseXmlString(string input, ParseOptions options)
  {
    try
    {
      using (StringReader reader = new StringReader(input))
        return XmpMetaParser.ParseTextReader((TextReader) reader, options);
    }
    catch (XmpException ex)
    {
      if (ex.ErrorCode == XmpErrorCode.BadXml && options.FixControlChars)
        return XmpMetaParser.ParseTextReader((TextReader) new FixAsciiControlsReader(new StreamReader((Stream) new MemoryStream(Encoding.UTF8.GetBytes(input)))), options);
      throw;
    }
  }

  private static XDocument ParseStream(Stream stream, ParseOptions options)
  {
    try
    {
      using (XmlReader reader = XmlReader.Create((TextReader) new StreamReader(stream), new XmlReaderSettings()
      {
        DtdProcessing = !options.DisallowDoctype ? DtdProcessing.Parse : DtdProcessing.Prohibit,
        MaxCharactersFromEntities = 10000000L
      }))
        return XDocument.Load(reader);
    }
    catch (XmlException ex)
    {
      throw new XmpException("XML parsing failure", XmpErrorCode.BadXml, (Exception) ex);
    }
    catch (IOException ex)
    {
      throw new XmpException("Error reading the XML-file", XmpErrorCode.BadStream, (Exception) ex);
    }
    catch (Exception ex)
    {
      throw new XmpException("XML Parser not correctly configured", XmpErrorCode.Unknown, ex);
    }
  }

  private static XDocument ParseTextReader(TextReader reader, ParseOptions options)
  {
    try
    {
      using (XmlReader reader1 = XmlReader.Create(reader, new XmlReaderSettings()
      {
        DtdProcessing = !options.DisallowDoctype ? DtdProcessing.Parse : DtdProcessing.Prohibit,
        MaxCharactersFromEntities = 10000000L
      }))
        return XDocument.Load(reader1);
    }
    catch (XmlException ex)
    {
      throw new XmpException("XML parsing failure", XmpErrorCode.BadXml, (Exception) ex);
    }
    catch (IOException ex)
    {
      throw new XmpException("Error reading the XML-file", XmpErrorCode.BadStream, (Exception) ex);
    }
    catch (Exception ex)
    {
      throw new XmpException("XML Parser not correctly configured", XmpErrorCode.Unknown, ex);
    }
  }

  private static object[] FindRootNode(
    IEnumerable<XNode> nodes,
    bool xmpmetaRequired,
    object[] result)
  {
    foreach (XNode node in nodes)
    {
      if (XmlNodeType.ProcessingInstruction == node.NodeType && ((XProcessingInstruction) node).Target == "xpacket")
        result[2] = (object) ((XProcessingInstruction) node).Data;
      else if (XmlNodeType.Element == node.NodeType)
      {
        XElement xelement = (XElement) node;
        string namespaceName = xelement.Name.NamespaceName;
        string localName = xelement.Name.LocalName;
        if ((localName == "xmpmeta" || localName == "xapmeta") && namespaceName == "adobe:ns:meta/")
          return XmpMetaParser.FindRootNode(xelement.Nodes(), false, result);
        if (!xmpmetaRequired && localName == "RDF" && namespaceName == "http://www.w3.org/1999/02/22-rdf-syntax-ns#")
        {
          if (result != null)
          {
            result[0] = (object) node;
            result[1] = XmpMetaParser.XmpRdf;
          }
          return result;
        }
        object[] rootNode = XmpMetaParser.FindRootNode(xelement.Nodes(), xmpmetaRequired, result);
        if (rootNode != null)
          return rootNode;
      }
    }
    return (object[]) null;
  }
}
