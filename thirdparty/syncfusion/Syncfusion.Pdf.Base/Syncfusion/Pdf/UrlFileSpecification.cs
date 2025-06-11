// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.UrlFileSpecification
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Interactive;
using Syncfusion.Pdf.Primitives;
using System;

#nullable disable
namespace Syncfusion.Pdf;

internal class UrlFileSpecification(string fileName) : PdfFileSpecificationBase(fileName)
{
  private string m_fileName = string.Empty;

  public override string FileName
  {
    get => this.m_fileName;
    set
    {
      switch (value)
      {
        case null:
          throw new ArgumentNullException(nameof (FileName));
        case "":
          throw new ArgumentException("FileName can't be empty");
        default:
          if (!(this.m_fileName != value))
            break;
          this.m_fileName = value;
          break;
      }
    }
  }

  protected override void Initialize()
  {
    base.Initialize();
    this.Dictionary.SetProperty("FS", (IPdfPrimitive) new PdfName("URL"));
  }

  protected override void Save()
  {
    this.Dictionary.SetProperty("F", (IPdfPrimitive) new PdfString(this.FileName));
  }
}
