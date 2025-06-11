// Decompiled with JetBrains decompiler
// Type: HandyControl.Tools.NoBlankTextRule
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Properties.Langs;
using System.Globalization;
using System.Windows.Controls;

#nullable disable
namespace HandyControl.Tools;

public class NoBlankTextRule : ValidationRule
{
  public string ErrorContent { get; set; } = Lang.IsNecessary;

  public override ValidationResult Validate(object value, CultureInfo cultureInfo)
  {
    if (!(value is string str))
      return new ValidationResult(false, (object) Lang.FormatError);
    return string.IsNullOrEmpty(str) ? new ValidationResult(false, (object) this.ErrorContent) : ValidationResult.ValidResult;
  }
}
