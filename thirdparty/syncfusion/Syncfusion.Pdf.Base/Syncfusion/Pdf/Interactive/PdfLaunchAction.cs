// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfLaunchAction
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Primitives;
using System;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public class PdfLaunchAction : PdfAction
{
  private ReferenceFileSpecification m_fileSpecification;
  private PdfFilePathType m_pathType = PdfFilePathType.Absolute;

  public PdfLaunchAction(string fileName)
  {
    this.m_fileSpecification = fileName != null ? new ReferenceFileSpecification(fileName, this.m_pathType) : throw new ArgumentNullException(nameof (fileName));
  }

  public PdfLaunchAction(string fileName, PdfFilePathType path)
  {
    if (fileName == null)
      throw new ArgumentNullException(nameof (fileName));
    this.m_pathType = path;
    this.m_fileSpecification = new ReferenceFileSpecification(fileName, this.m_pathType);
  }

  internal PdfLaunchAction(string fileName, bool loaded)
  {
    if (!loaded)
      return;
    this.m_fileSpecification = fileName != null ? new ReferenceFileSpecification(fileName) : throw new ArgumentNullException(nameof (fileName));
  }

  public string FileName
  {
    get => this.m_fileSpecification.FileName;
    set
    {
      switch (value)
      {
        case null:
          throw new ArgumentNullException(nameof (FileName));
        case "":
          throw new ArgumentException("File name can not be empty");
        default:
          if (!(this.m_fileSpecification.FileName != value))
            break;
          this.m_fileSpecification.FileName = value;
          break;
      }
    }
  }

  protected override void Initialize()
  {
    base.Initialize();
    this.Dictionary.BeginSave += new SavePdfPrimitiveEventHandler(this.Dictionary_BeginSave);
    this.Dictionary.SetProperty("S", (IPdfPrimitive) new PdfName("Launch"));
  }

  private void Dictionary_BeginSave(object sender, SavePdfPrimitiveEventArgs ars)
  {
    this.Dictionary.SetProperty("F", (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) this.m_fileSpecification));
  }
}
