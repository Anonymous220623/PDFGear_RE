// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontFontInfo
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontFontInfo : SystemFontPostScriptObject
{
  private readonly SystemFontProperty<string> familyName;
  private readonly SystemFontProperty<string> weight;
  private readonly SystemFontProperty<double> italicAngle;

  public string FamilyName => this.familyName.GetValue();

  public string Weight => this.weight.GetValue();

  public double ItalicAngle => this.italicAngle.GetValue();

  public SystemFontFontInfo()
  {
    this.familyName = this.CreateProperty<string>(new SystemFontPropertyDescriptor()
    {
      Name = nameof (FamilyName)
    });
    this.weight = this.CreateProperty<string>(new SystemFontPropertyDescriptor()
    {
      Name = nameof (Weight)
    });
    this.italicAngle = this.CreateProperty<double>(new SystemFontPropertyDescriptor()
    {
      Name = nameof (ItalicAngle)
    }, 0.0);
  }
}
