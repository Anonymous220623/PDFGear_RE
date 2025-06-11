// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.ReferenceFileSpecification
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Interactive;
using Syncfusion.Pdf.Primitives;
using System;
using System.IO;

#nullable disable
namespace Syncfusion.Pdf;

internal class ReferenceFileSpecification : PdfFileSpecificationBase
{
  private string m_fileName = string.Empty;
  private PdfFilePathType m_path;

  public ReferenceFileSpecification(string fileName, PdfFilePathType path)
    : base(fileName)
  {
    this.m_path = path;
    this.FileName = fileName;
  }

  internal ReferenceFileSpecification(string fileName)
    : base(fileName)
  {
    this.m_fileName = fileName;
  }

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
          if (this.m_path == PdfFilePathType.Absolute)
          {
            this.m_fileName = value;
            break;
          }
          if (this.m_path != PdfFilePathType.Relative)
            break;
          this.m_fileName = Path.GetFullPath(value);
          break;
      }
    }
  }

  protected override void Save()
  {
    this.Dictionary.SetProperty("UF", (IPdfPrimitive) new PdfString(this.FormatFileName(this.FileName, this.m_path == PdfFilePathType.Relative)));
  }
}
