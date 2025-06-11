// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.PdfConfig
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.ComponentModel;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf;

[ToolboxBitmap(typeof (PdfConfig), "ToolBoxIcons.Pdf.bmp")]
public class PdfConfig : Component
{
  public PdfConfig()
  {
    try
    {
      AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(PdfBaseAssembly.AssemblyResolver);
    }
    finally
    {
      AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(PdfBaseAssembly.AssemblyResolver);
    }
  }

  public string Copyright => "Syncfusion, Inc. 2001 - 2006";
}
