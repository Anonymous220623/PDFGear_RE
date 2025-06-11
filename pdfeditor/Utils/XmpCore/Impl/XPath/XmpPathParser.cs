// Decompiled with JetBrains decompiler
// Type: XmpCore.Impl.XPath.XmpPathParser
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

#nullable disable
namespace XmpCore.Impl.XPath;

public static class XmpPathParser
{
  public static XmpPath ExpandXPath(string schemaNs, string path)
  {
    if (schemaNs == null || path == null)
      throw new XmpException("Parameter must not be null", XmpErrorCode.BadParam);
    XmpPath expandedXPath = new XmpPath();
    PathPosition pos = new PathPosition() { Path = path };
    XmpPathParser.ParseRootNode(schemaNs, pos, expandedXPath);
    while (pos.StepEnd < path.Length)
    {
      pos.StepBegin = pos.StepEnd;
      XmpPathParser.SkipPathDelimiter(path, pos);
      pos.StepEnd = pos.StepBegin;
      XmpPathSegment segment = path[pos.StepBegin] != '[' ? XmpPathParser.ParseStructSegment(pos) : XmpPathParser.ParseIndexSegment(pos);
      switch (segment.Kind)
      {
        case XmpPathStepType.StructFieldStep:
          if (segment.Name[0] == '@')
          {
            segment.Name = "?" + segment.Name.Substring(1);
            if (segment.Name != "?xml:lang")
              throw new XmpException("Only xml:lang allowed with '@'", XmpErrorCode.BadXPath);
          }
          if (segment.Name[0] == '?')
          {
            ++pos.NameStart;
            segment.Kind = XmpPathStepType.QualifierStep;
          }
          XmpPathParser.VerifyQualName(pos.Path.Substring(pos.NameStart, pos.NameEnd - pos.NameStart));
          break;
        case XmpPathStepType.FieldSelectorStep:
          if (segment.Name[1] == '@')
          {
            segment.Name = "[?" + segment.Name.Substring(2);
            if (!segment.Name.StartsWith("[?xml:lang="))
              throw new XmpException("Only xml:lang allowed with '@'", XmpErrorCode.BadXPath);
          }
          if (segment.Name[1] == '?')
          {
            ++pos.NameStart;
            segment.Kind = XmpPathStepType.QualSelectorStep;
            XmpPathParser.VerifyQualName(pos.Path.Substring(pos.NameStart, pos.NameEnd - pos.NameStart));
            break;
          }
          break;
      }
      expandedXPath.Add(segment);
    }
    return expandedXPath;
  }

  private static void SkipPathDelimiter(string path, PathPosition pos)
  {
    if (path[pos.StepBegin] == '/')
    {
      ++pos.StepBegin;
      if (pos.StepBegin >= path.Length)
        throw new XmpException("Empty XmpPath segment", XmpErrorCode.BadXPath);
    }
    if (path[pos.StepBegin] != '*')
      return;
    ++pos.StepBegin;
    if (pos.StepBegin >= path.Length || path[pos.StepBegin] != '[')
      throw new XmpException("Missing '[' after '*'", XmpErrorCode.BadXPath);
  }

  private static XmpPathSegment ParseStructSegment(PathPosition pos)
  {
    pos.NameStart = pos.StepBegin;
    while (pos.StepEnd < pos.Path.Length && "/[*".IndexOf(pos.Path[pos.StepEnd]) < 0)
      ++pos.StepEnd;
    pos.NameEnd = pos.StepEnd;
    if (pos.StepEnd == pos.StepBegin)
      throw new XmpException("Empty XmpPath segment", XmpErrorCode.BadXPath);
    return new XmpPathSegment(pos.Path.Substring(pos.StepBegin, pos.StepEnd - pos.StepBegin), XmpPathStepType.StructFieldStep);
  }

  private static XmpPathSegment ParseIndexSegment(PathPosition pos)
  {
    ++pos.StepEnd;
    XmpPathSegment indexSegment;
    if ('0' <= pos.Path[pos.StepEnd] && pos.Path[pos.StepEnd] <= '9')
    {
      while (pos.StepEnd < pos.Path.Length && '0' <= pos.Path[pos.StepEnd] && pos.Path[pos.StepEnd] <= '9')
        ++pos.StepEnd;
      indexSegment = new XmpPathSegment((string) null, XmpPathStepType.ArrayIndexStep);
    }
    else
    {
      while (pos.StepEnd < pos.Path.Length && pos.Path[pos.StepEnd] != ']' && pos.Path[pos.StepEnd] != '=')
        ++pos.StepEnd;
      if (pos.StepEnd >= pos.Path.Length)
        throw new XmpException("Missing ']' or '=' for array index", XmpErrorCode.BadXPath);
      if (pos.Path[pos.StepEnd] == ']')
      {
        if (pos.Path.Substring(pos.StepBegin, pos.StepEnd - pos.StepBegin) != "[last()")
          throw new XmpException("Invalid non-numeric array index", XmpErrorCode.BadXPath);
        indexSegment = new XmpPathSegment((string) null, XmpPathStepType.ArrayLastStep);
      }
      else
      {
        pos.NameStart = pos.StepBegin + 1;
        pos.NameEnd = pos.StepEnd;
        ++pos.StepEnd;
        char ch = pos.Path[pos.StepEnd];
        switch (ch)
        {
          case '"':
          case '\'':
            for (++pos.StepEnd; pos.StepEnd < pos.Path.Length; ++pos.StepEnd)
            {
              if ((int) pos.Path[pos.StepEnd] == (int) ch)
              {
                if (pos.StepEnd + 1 < pos.Path.Length && (int) pos.Path[pos.StepEnd + 1] == (int) ch)
                  ++pos.StepEnd;
                else
                  break;
              }
            }
            if (pos.StepEnd >= pos.Path.Length)
              throw new XmpException("No terminating quote for array selector", XmpErrorCode.BadXPath);
            ++pos.StepEnd;
            indexSegment = new XmpPathSegment((string) null, XmpPathStepType.FieldSelectorStep);
            break;
          default:
            throw new XmpException("Invalid quote in array selector", XmpErrorCode.BadXPath);
        }
      }
    }
    if (pos.StepEnd >= pos.Path.Length || pos.Path[pos.StepEnd] != ']')
      throw new XmpException("Missing ']' for array index", XmpErrorCode.BadXPath);
    ++pos.StepEnd;
    indexSegment.Name = pos.Path.Substring(pos.StepBegin, pos.StepEnd - pos.StepBegin);
    return indexSegment;
  }

  private static void ParseRootNode(string schemaNs, PathPosition pos, XmpPath expandedXPath)
  {
    while (pos.StepEnd < pos.Path.Length && "/[*".IndexOf(pos.Path[pos.StepEnd]) < 0)
      ++pos.StepEnd;
    if (pos.StepEnd == pos.StepBegin)
      throw new XmpException("Empty initial XmpPath step", XmpErrorCode.BadXPath);
    string str = XmpPathParser.VerifyXPathRoot(schemaNs, pos.Path.Substring(pos.StepBegin, pos.StepEnd - pos.StepBegin));
    IXmpAliasInfo alias = XmpMetaFactory.SchemaRegistry.FindAlias(str);
    if (alias == null)
    {
      expandedXPath.Add(new XmpPathSegment(schemaNs, XmpPathStepType.SchemaNode));
      XmpPathSegment segment = new XmpPathSegment(str, XmpPathStepType.StructFieldStep);
      expandedXPath.Add(segment);
    }
    else
    {
      expandedXPath.Add(new XmpPathSegment(alias.Namespace, XmpPathStepType.SchemaNode));
      expandedXPath.Add(new XmpPathSegment(XmpPathParser.VerifyXPathRoot(alias.Namespace, alias.PropName), XmpPathStepType.StructFieldStep)
      {
        IsAlias = true,
        AliasForm = alias.AliasForm.GetOptions()
      });
      if (alias.AliasForm.IsArrayAltText)
      {
        expandedXPath.Add(new XmpPathSegment("[?xml:lang='x-default']", XmpPathStepType.QualSelectorStep)
        {
          IsAlias = true,
          AliasForm = alias.AliasForm.GetOptions()
        });
      }
      else
      {
        if (!alias.AliasForm.IsArray)
          return;
        expandedXPath.Add(new XmpPathSegment("[1]", XmpPathStepType.ArrayIndexStep)
        {
          IsAlias = true,
          AliasForm = alias.AliasForm.GetOptions()
        });
      }
    }
  }

  private static void VerifyQualName(string qualName)
  {
    int length = qualName.IndexOf(':');
    string str = length > 0 ? qualName.Substring(0, length) : throw new XmpException("Ill-formed qualified name", XmpErrorCode.BadXPath);
    if (!Utils.IsXmlNameNs(str))
      throw new XmpException("Ill-formed qualified name", XmpErrorCode.BadXPath);
    if (XmpMetaFactory.SchemaRegistry.GetNamespaceUri(str) == null)
      throw new XmpException("Unknown namespace prefix for qualified name", XmpErrorCode.BadXPath);
  }

  private static void VerifySimpleXmlName(string name)
  {
    if (!Utils.IsXmlName(name))
      throw new XmpException("Bad XML name", XmpErrorCode.BadXPath);
  }

  private static string VerifyXPathRoot(string schemaNs, string rootProp)
  {
    if (string.IsNullOrEmpty(schemaNs))
      throw new XmpException("Schema namespace URI is required", XmpErrorCode.BadSchema);
    if (rootProp[0] == '?' || rootProp[0] == '@')
      throw new XmpException("Top level name must not be a qualifier", XmpErrorCode.BadXPath);
    if (rootProp.IndexOf('/') >= 0 || rootProp.IndexOf('[') >= 0)
      throw new XmpException("Top level name must be simple", XmpErrorCode.BadXPath);
    string namespacePrefix1 = XmpMetaFactory.SchemaRegistry.GetNamespacePrefix(schemaNs);
    if (namespacePrefix1 == null)
      throw new XmpException("Unregistered schema namespace URI", XmpErrorCode.BadSchema);
    int num = rootProp.IndexOf(':');
    if (num < 0)
    {
      XmpPathParser.VerifySimpleXmlName(rootProp);
      return namespacePrefix1 + rootProp;
    }
    XmpPathParser.VerifySimpleXmlName(rootProp.Substring(0, num));
    XmpPathParser.VerifySimpleXmlName(rootProp.Substring(num));
    string str = rootProp.Substring(0, num + 1);
    string namespacePrefix2 = XmpMetaFactory.SchemaRegistry.GetNamespacePrefix(schemaNs);
    if (namespacePrefix2 == null)
      throw new XmpException("Unknown schema namespace prefix", XmpErrorCode.BadSchema);
    if (str != namespacePrefix2)
      throw new XmpException("Schema namespace URI and prefix mismatch", XmpErrorCode.BadSchema);
    return rootProp;
  }
}
