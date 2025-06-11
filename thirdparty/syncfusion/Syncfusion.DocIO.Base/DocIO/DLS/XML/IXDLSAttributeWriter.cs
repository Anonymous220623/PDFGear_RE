// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.XML.IXDLSAttributeWriter
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.DocIO.DLS.XML;

public interface IXDLSAttributeWriter
{
  void WriteValue(string name, float value);

  void WriteValue(string name, double value);

  void WriteValue(string name, int value);

  void WriteValue(string name, string value);

  void WriteValue(string name, Enum value);

  void WriteValue(string name, bool value);

  void WriteValue(string name, Color value);

  void WriteValue(string name, DateTime value);
}
