// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.XML.IXDLSContentReader
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.Xml;

#nullable disable
namespace Syncfusion.DocIO.DLS.XML;

public interface IXDLSContentReader
{
  string TagName { get; }

  XmlNodeType NodeType { get; }

  string GetAttributeValue(string name);

  bool ParseElementType(Type enumType, out Enum elementType);

  bool ReadChildElement(object value);

  object ReadChildElement(Type type);

  string ReadChildStringContent();

  byte[] ReadChildBinaryElement();

  XmlReader InnerReader { get; }

  IXDLSAttributeReader AttributeReader { get; }
}
