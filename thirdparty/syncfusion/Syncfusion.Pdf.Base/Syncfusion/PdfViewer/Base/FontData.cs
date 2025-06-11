// Decompiled with JetBrains decompiler
// Type: Syncfusion.PdfViewer.Base.FontData
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.PdfViewer.Base;

internal class FontData : PostScriptObj
{
  private readonly KeyProperty<string> familyName;
  private readonly KeyProperty<string> weight;
  private readonly KeyProperty<double> italicAngle;

  public string FamilyName => this.familyName.GetValue();

  public string Weight => this.weight.GetValue();

  public double ItalicAngle => this.italicAngle.GetValue();

  public FontData()
  {
    this.familyName = this.CreateProperty<string>(new KeyPropertyDescriptor()
    {
      Name = nameof (FamilyName)
    });
    this.weight = this.CreateProperty<string>(new KeyPropertyDescriptor()
    {
      Name = nameof (Weight)
    });
    this.italicAngle = this.CreateProperty<double>(new KeyPropertyDescriptor()
    {
      Name = nameof (ItalicAngle)
    }, 0.0);
  }
}
