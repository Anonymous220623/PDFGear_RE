// Decompiled with JetBrains decompiler
// Type: pdfconverter.Utils.TextNumberValidationRules
// Assembly: pdfconverter, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 691C26A2-7651-46C3-AA3C-2839EDA9722B
// Assembly location: C:\Program Files\PDFgear\pdfconverter.exe

using System.Globalization;
using System.Windows.Controls;

#nullable disable
namespace pdfconverter.Utils;

internal class TextNumberValidationRules : ValidationRule
{
  public override ValidationResult Validate(object value, CultureInfo cultureInfo)
  {
    int result = 0;
    string s = (value ?? (object) "").ToString();
    return !string.IsNullOrWhiteSpace(s) && int.TryParse(s, out result) ? ValidationResult.ValidResult : new ValidationResult(false, (object) "Please enter valid data");
  }
}
