// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfFileSpecificationBase
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Primitives;
using System;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public abstract class PdfFileSpecificationBase : IPdfWrapper
{
  private PdfDictionary m_dictionary = new PdfDictionary();

  public PdfFileSpecificationBase(string fileName)
  {
    if (fileName == null)
      throw new ArgumentNullException(nameof (fileName));
    this.Initialize();
  }

  public abstract string FileName { get; set; }

  internal PdfDictionary Dictionary => this.m_dictionary;

  protected virtual void Initialize()
  {
    this.m_dictionary.SetProperty("Type", (IPdfPrimitive) new PdfName("Filespec"));
    this.m_dictionary.BeginSave += new SavePdfPrimitiveEventHandler(this.Dictionary_BeginSave);
  }

  protected abstract void Save();

  protected string FormatFileName(string fileName, bool flag)
  {
    switch (fileName)
    {
      case null:
        throw new ArgumentNullException(nameof (fileName));
      case "":
        throw new ArgumentException("fileName - string can not be empty");
      default:
        string str = fileName.Replace("\\", "/");
        if (str.Substring(0, 2) == "\\")
          str = str.Remove(1, 1);
        if (str.Substring(0, 1) != "/" && !flag)
          str = str;
        return str;
    }
  }

  private void Dictionary_BeginSave(object sender, SavePdfPrimitiveEventArgs ars) => this.Save();

  IPdfPrimitive IPdfWrapper.Element => (IPdfPrimitive) this.m_dictionary;
}
