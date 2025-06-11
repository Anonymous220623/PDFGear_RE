// Decompiled with JetBrains decompiler
// Type: pdfeditor.Utils.Validations.StringValidateRule
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System.Globalization;
using System.Linq;
using System.Windows.Controls;

#nullable disable
namespace pdfeditor.Utils.Validations;

public class StringValidateRule : ValidationRule
{
  public override ValidationResult Validate(object value, CultureInfo cultureInfo)
  {
    if (value == null)
      return new ValidationResult(false, (object) "The content cannot be spatial！");
    string source = value.ToString();
    if (string.IsNullOrWhiteSpace(source))
      return new ValidationResult(false, (object) "The content cannot be spatial！");
    return source.Count<char>() > 50 ? new ValidationResult(false, (object) "The input content  cannot exceed 50 characters!") : ValidationResult.ValidResult;
  }
}
