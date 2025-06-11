// Decompiled with JetBrains decompiler
// Type: HandyControl.Tools.RegexRule
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using HandyControl.Properties.Langs;
using System.Globalization;
using System.Windows.Controls;

#nullable disable
namespace HandyControl.Tools;

public class RegexRule : ValidationRule
{
  public TextType Type { get; set; }

  public string Pattern { get; set; }

  public string ErrorContent { get; set; } = Lang.FormatError;

  public override ValidationResult Validate(object value, CultureInfo cultureInfo)
  {
    if (!(value is string str))
      return this.CreateErrorValidationResult();
    if (!string.IsNullOrEmpty(this.Pattern))
    {
      if (!str.IsKindOf(this.Pattern))
        return this.CreateErrorValidationResult();
    }
    else if (this.Type != TextType.Common && !str.IsKindOf(this.Type))
      return this.CreateErrorValidationResult();
    return ValidationResult.ValidResult;
  }

  private ValidationResult CreateErrorValidationResult()
  {
    return new ValidationResult(false, (object) this.ErrorContent);
  }
}
