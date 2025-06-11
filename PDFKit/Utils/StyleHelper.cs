// Decompiled with JetBrains decompiler
// Type: PDFKit.Utils.StyleHelper
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace PDFKit.Utils;

internal static class StyleHelper
{
  private static object scrollViewerStyleResourceKey;

  public static object ModernScrollViewerStyleKey
  {
    get
    {
      StyleHelper.InitializeStyle();
      return StyleHelper.scrollViewerStyleResourceKey is "NOTFOUND" ? (object) typeof (ScrollViewer) : StyleHelper.scrollViewerStyleResourceKey;
    }
  }

  public static Style GetScrollViewerStyle()
  {
    StyleHelper.InitializeStyle();
    return StyleHelper.scrollViewerStyleResourceKey is "NOTFOUND" ? (Style) null : Application.Current.TryFindResource(StyleHelper.scrollViewerStyleResourceKey) as Style;
  }

  private static void InitializeStyle()
  {
    if (StyleHelper.scrollViewerStyleResourceKey != null)
      return;
    try
    {
      Assembly assembly = ((IEnumerable<Assembly>) AppDomain.CurrentDomain.GetAssemblies()).FirstOrDefault<Assembly>((Func<Assembly, bool>) (c => c.Location.EndsWith("CommomLib.dll") || c.Location.EndsWith("CommonLib.dll")));
      PropertyInfo property = ((object) assembly != null ? (Type) assembly.DefinedTypes.FirstOrDefault<TypeInfo>((Func<TypeInfo, bool>) (c => c.Name == "ScrollBarHelper")) : (Type) null).GetProperty("ModernScrollViewerStyleKey");
      if (property != (PropertyInfo) null)
        StyleHelper.scrollViewerStyleResourceKey = property.GetValue((object) null);
    }
    catch
    {
    }
    if (StyleHelper.scrollViewerStyleResourceKey == null)
      StyleHelper.scrollViewerStyleResourceKey = (object) "NOTFOUND";
  }
}
