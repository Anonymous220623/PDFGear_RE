// Decompiled with JetBrains decompiler
// Type: pdfeditor.Utils.ToolbarContextMenuValueEqualityComparer
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Patagames.Pdf;
using pdfeditor.Models.Menus;
using pdfeditor.ViewModels;
using System;
using System.Windows.Media;

#nullable disable
namespace pdfeditor.Utils;

public static class ToolbarContextMenuValueEqualityComparer
{
  public static bool MenuValueEquals(
    AnnotationMode mode,
    ContextMenuItemType type,
    object x,
    object y)
  {
    if (x == null && y == null || object.Equals(x, y))
      return true;
    if (x == null)
    {
      object obj = y;
      y = x;
      x = obj;
    }
    switch (type)
    {
      case ContextMenuItemType.StrokeColor:
      case ContextMenuItemType.FillColor:
        return ToolbarContextMenuValueEqualityComparer.ColorValueEquals(mode, type, x, y);
      case ContextMenuItemType.StrokeThickness:
        return ToolbarContextMenuValueEqualityComparer.StrokeThicknessValueEquals(mode, type, x, y);
      default:
        return false;
    }
  }

  private static bool ColorValueEquals(
    AnnotationMode mode,
    ContextMenuItemType type,
    object x,
    object y)
  {
    Color? color1 = ConvertToColor(x);
    Color? color2 = ConvertToColor(y);
    if (object.Equals((object) color1, (object) color2))
      return true;
    if (!color1.HasValue || !color2.HasValue)
      return false;
    if (color1.Value.A == (byte) 0 && color2.Value.A == (byte) 0)
      return true;
    return Math.Abs((int) color1.Value.R - (int) color2.Value.R) <= 2 && Math.Abs((int) color1.Value.G - (int) color2.Value.G) <= 2 && Math.Abs((int) color1.Value.B - (int) color2.Value.B) <= 2 && Math.Abs((int) color1.Value.A - (int) color2.Value.A) <= 2;

    static Color? ConvertToColor(object obj)
    {
      Color? color1 = new Color?();
      switch (obj)
      {
        case FS_COLOR color3:
          color1 = new Color?(color3.ToColor());
          break;
        case Color color4:
          color1 = new Color?(color4);
          break;
        case string str:
          string lowerInvariant = str.Trim().ToLowerInvariant();
          try
          {
            color1 = new Color?((Color) ColorConverter.ConvertFromString(lowerInvariant));
            break;
          }
          catch
          {
            break;
          }
      }
      return color1;
    }
  }

  private static bool StrokeThicknessValueEquals(
    AnnotationMode mode,
    ContextMenuItemType type,
    object x,
    object y)
  {
    try
    {
      return (double) Convert.ToSingle(x) == (double) Convert.ToSingle(y);
    }
    catch
    {
    }
    return false;
  }
}
