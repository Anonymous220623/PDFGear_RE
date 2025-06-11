// Decompiled with JetBrains decompiler
// Type: Syncfusion.PdfViewer.Base.PostScriptObjectConverter
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.PdfViewer.Base;

internal class PostScriptObjectConverter : IConverter
{
  public object Convert(Type resultType, object value)
  {
    if (!(value is PostScriptDict fromDict))
      return (object) null;
    if (!(Activator.CreateInstance(resultType) is PostScriptObj instance))
      return (object) null;
    instance.Load(fromDict);
    return (object) instance;
  }
}
