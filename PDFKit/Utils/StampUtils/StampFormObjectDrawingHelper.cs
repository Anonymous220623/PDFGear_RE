// Decompiled with JetBrains decompiler
// Type: PDFKit.Utils.StampUtils.StampFormObjectDrawingHelper
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using Patagames.Pdf;
using Patagames.Pdf.Net;
using PDFKit.Utils.DrawingHelpers;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

#nullable disable
namespace PDFKit.Utils.StampUtils;

internal static class StampFormObjectDrawingHelper
{
  internal static readonly System.Collections.Generic.IReadOnlyList<string> FormObjectNames = (System.Collections.Generic.IReadOnlyList<string>) new string[6]
  {
    "Check",
    "Cancel",
    "RadioCheck",
    "CheckBox",
    "Indeterminate",
    "Indeterminate Fill"
  };
  private static readonly IReadOnlyDictionary<string, Func<(Geometry, Rect)>> geometryDict = (IReadOnlyDictionary<string, Func<(Geometry, Rect)>>) new Dictionary<string, Func<(Geometry, Rect)>>()
  {
    ["Check"] = (Func<(Geometry, Rect)>) (() => (Check_GeometryGenerator.Geometry, Check_GeometryGenerator.Viewbox)),
    ["Cancel"] = (Func<(Geometry, Rect)>) (() => (Cancel_GeometryGenerator.Geometry, Cancel_GeometryGenerator.Viewbox)),
    ["RadioCheck"] = (Func<(Geometry, Rect)>) (() => (RadioCheck_GeometryGenerator.Geometry, RadioCheck_GeometryGenerator.Viewbox)),
    ["CheckBox"] = (Func<(Geometry, Rect)>) (() => (CheckBox_GeometryGenerator.Geometry, CheckBox_GeometryGenerator.Viewbox)),
    ["Indeterminate"] = (Func<(Geometry, Rect)>) (() => (Indeterminate_GeometryGenerator.Geometry, Indeterminate_GeometryGenerator.Viewbox)),
    ["Indeterminate Fill"] = (Func<(Geometry, Rect)>) (() => (IndeterminateFill_GeometryGenerator.Geometry, IndeterminateFill_GeometryGenerator.Viewbox))
  };
  private static readonly IReadOnlyDictionary<string, Func<FS_RECTF, FS_COLOR, FS_COLOR, float, List<PdfPageObject>>> pathObjectCreaterDictionary = (IReadOnlyDictionary<string, Func<FS_RECTF, FS_COLOR, FS_COLOR, float, List<PdfPageObject>>>) new Dictionary<string, Func<FS_RECTF, FS_COLOR, FS_COLOR, float, List<PdfPageObject>>>()
  {
    ["Check"] = new Func<FS_RECTF, FS_COLOR, FS_COLOR, float, List<PdfPageObject>>(Check_GeometryGenerator.CreateGeometryPath),
    ["Cancel"] = new Func<FS_RECTF, FS_COLOR, FS_COLOR, float, List<PdfPageObject>>(Cancel_GeometryGenerator.CreateGeometryPath),
    ["RadioCheck"] = new Func<FS_RECTF, FS_COLOR, FS_COLOR, float, List<PdfPageObject>>(RadioCheck_GeometryGenerator.CreateGeometryPath),
    ["CheckBox"] = new Func<FS_RECTF, FS_COLOR, FS_COLOR, float, List<PdfPageObject>>(CheckBox_GeometryGenerator.CreateGeometryPath),
    ["Indeterminate"] = new Func<FS_RECTF, FS_COLOR, FS_COLOR, float, List<PdfPageObject>>(Indeterminate_GeometryGenerator.CreateGeometryPath),
    ["Indeterminate Fill"] = new Func<FS_RECTF, FS_COLOR, FS_COLOR, float, List<PdfPageObject>>(IndeterminateFill_GeometryGenerator.CreateGeometryPath)
  };

  public static Geometry GetGeometry(string name, out Size geometrySize)
  {
    Application current = Application.Current;
    if (current == null || !current.CheckAccess())
      throw new InvalidOperationException("Dispatcher");
    geometrySize = new Size(0.0, 0.0);
    Func<(Geometry, Rect)> func;
    if (!StampFormObjectDrawingHelper.geometryDict.TryGetValue(name, out func))
      return (Geometry) null;
    (Geometry geometry, Rect rect) = func();
    geometrySize = rect.Size;
    return geometry;
  }

  public static List<PdfPageObject> CreatePageObject(
    string name,
    FS_RECTF rect,
    FS_COLOR strokeColor,
    FS_COLOR fillColor,
    float strokeWidth)
  {
    Func<FS_RECTF, FS_COLOR, FS_COLOR, float, List<PdfPageObject>> func;
    if (StampFormObjectDrawingHelper.pathObjectCreaterDictionary.TryGetValue(name, out func))
    {
      try
      {
        return func(rect, strokeColor, fillColor, strokeWidth);
      }
      catch
      {
      }
    }
    return (List<PdfPageObject>) null;
  }
}
