// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfResetAction
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Primitives;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public class PdfResetAction : PdfFormAction
{
  public override bool Include
  {
    get => base.Include;
    set
    {
      if (base.Include == value)
        return;
      base.Include = value;
      this.Dictionary.SetNumber("Flags", base.Include ? 0 : 1);
    }
  }

  protected override void Initialize()
  {
    base.Initialize();
    this.Dictionary.SetProperty("S", (IPdfPrimitive) new PdfName("ResetForm"));
  }
}
