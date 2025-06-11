// Decompiled with JetBrains decompiler
// Type: Syncfusion.Office.DocumentParser
// Assembly: Syncfusion.Compression.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: A9A7FF4E-A031-4867-8B1F-3311AA2A62FF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Compression.Base.dll

using System.Xml;

#nullable disable
namespace Syncfusion.Office;

internal abstract class DocumentParser
{
  internal abstract IOfficeRunFormat ParseMathControlFormat(
    XmlReader reader,
    IOfficeMathFunctionBase mathFunction);

  internal abstract void ParseMathRun(XmlReader reader, IOfficeMathRunElement mathParaItem);
}
