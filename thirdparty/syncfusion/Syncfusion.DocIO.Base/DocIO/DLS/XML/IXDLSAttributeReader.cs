// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.XML.IXDLSAttributeReader
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.DocIO.DLS.XML;

public interface IXDLSAttributeReader
{
  bool HasAttribute(string name);

  string ReadString(string name);

  int ReadInt(string name);

  short ReadShort(string name);

  float ReadFloat(string name);

  bool ReadBoolean(string name);

  byte ReadByte(string name);

  Enum ReadEnum(string name, Type enumType);

  Color ReadColor(string name);

  DateTime ReadDateTime(string s);
}
