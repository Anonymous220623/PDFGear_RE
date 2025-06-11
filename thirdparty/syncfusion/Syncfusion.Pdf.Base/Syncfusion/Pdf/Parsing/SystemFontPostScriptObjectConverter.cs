// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontPostScriptObjectConverter
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontPostScriptObjectConverter : ISystemFontConverter
{
  public object Convert(Type resultType, object value)
  {
    if (!(value is SystemFontPostScriptDictionary fromDict))
      return (object) null;
    if (!(Activator.CreateInstance(resultType) is SystemFontPostScriptObject instance))
      return (object) null;
    instance.Load(fromDict);
    return (object) instance;
  }
}
