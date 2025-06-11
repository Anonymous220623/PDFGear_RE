// Decompiled with JetBrains decompiler
// Type: PDFKit.PdfResourceDictionary
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using PDFKit.Utils.StampUtils;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

#nullable disable
namespace PDFKit;

public class PdfResourceDictionary : ResourceDictionary
{
  public PdfResourceDictionary()
  {
    foreach (string formObjectName in (IEnumerable<string>) StampFormObjectDrawingHelper.FormObjectNames)
    {
      string str = formObjectName.Replace(" ", "_");
      Size geometrySize;
      Geometry geometry = StampFormObjectDrawingHelper.GetGeometry(formObjectName, out geometrySize);
      this.Add((object) ("StampFormObjectGeometry." + str), (object) geometry);
      this.Add((object) $"StampFormObjectGeometry.{str}.Width", (object) geometrySize.Width);
      this.Add((object) $"StampFormObjectGeometry.{str}.Height", (object) geometrySize.Height);
    }
  }
}
