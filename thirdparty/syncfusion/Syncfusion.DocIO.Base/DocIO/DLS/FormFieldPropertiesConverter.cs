// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.FormFieldPropertiesConverter
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.ReaderWriter.Biff_Records;
using System;
using System.Globalization;

#nullable disable
namespace Syncfusion.DocIO.DLS;

internal class FormFieldPropertiesConverter
{
  public static void ReadFormFieldProperties(WFormField formField, FormField frmField)
  {
    formField.Name = frmField.Title;
    if (frmField.Title == null)
      return;
    formField.Help = frmField.Help;
    formField.MacroOnEnd = frmField.MacroOnEnd;
    formField.MacroOnStart = frmField.MacroOnStart;
    formField.StatusBarHelp = frmField.Tooltip;
    formField.Value = frmField.Value;
    formField.Params = (int) frmField.Params;
    if (frmField.FieldType == FieldType.FieldFormDropDown)
    {
      WDropDownFormField wdropDownFormField = formField as WDropDownFormField;
      wdropDownFormField.DefaultDropDownValue = frmField.DefaultDropDownValue;
      wdropDownFormField.DropDownSelectedIndex = frmField.DropDownIndex;
      for (int index = 0; index < frmField.DropDownItems.Count; ++index)
        wdropDownFormField.DropDownItems.Add(frmField.DropDownItems[index]);
      if (frmField.DropDownItems.Count <= 0)
        return;
      wdropDownFormField.DropDownValue = frmField.DropDownValue;
    }
    else if (frmField.FieldType == FieldType.FieldFormCheckBox)
    {
      WCheckBox wcheckBox = formField as WCheckBox;
      wcheckBox.SetCheckBoxSizeValue(frmField.CheckBoxSize / 2);
      wcheckBox.DefaultCheckBoxValue = frmField.DefaultCheckBoxValue;
    }
    else
    {
      if (frmField.FieldType != FieldType.FieldFormTextInput)
        return;
      WTextFormField wtextFormField = formField as WTextFormField;
      wtextFormField.MaximumLength = frmField.MaxLength;
      wtextFormField.StringFormat = frmField.Format;
      wtextFormField.SetTextFormFieldType(frmField.TextFormFieldType);
      wtextFormField.DefaultText = frmField.DefaultTextInputValue;
      if (wtextFormField.Type != TextFormFieldType.RegularText)
        return;
      wtextFormField.TextFormat = FormFieldPropertiesConverter.GetTextFormat(frmField.Format);
    }
  }

  public static void WriteFormFieldProperties(FormField frmField, WFormField formField)
  {
    frmField.Help = formField.Help;
    frmField.MacroOnEnd = formField.MacroOnEnd;
    frmField.MacroOnStart = formField.MacroOnStart;
    frmField.Params = (short) formField.Params;
    frmField.Title = formField.Name;
    frmField.Tooltip = formField.StatusBarHelp;
    frmField.Value = formField.Value;
    if (formField.FormFieldType == FormFieldType.DropDown)
    {
      WDropDownFormField wdropDownFormField = formField as WDropDownFormField;
      for (int index = 0; index < wdropDownFormField.DropDownItems.Count; ++index)
        frmField.DropDownItems.Add(wdropDownFormField.DropDownItems[index].Text);
      frmField.DefaultDropDownValue = wdropDownFormField.DefaultDropDownValue;
      frmField.DropDownIndex = wdropDownFormField.DropDownSelectedIndex;
      if (wdropDownFormField.DropDownItems.Count <= 0)
        return;
      if (wdropDownFormField.DropDownItems.Count <= wdropDownFormField.DropDownSelectedIndex)
        throw new ArgumentException($"DropDownItem with index {(object) wdropDownFormField.DropDownSelectedIndex} doesn't exist");
      frmField.DropDownValue = wdropDownFormField.DropDownValue;
    }
    else if (formField.FormFieldType == FormFieldType.CheckBox)
    {
      WCheckBox wcheckBox = formField as WCheckBox;
      frmField.CheckBoxSize = wcheckBox.CheckBoxSize * 2;
      frmField.DefaultCheckBoxValue = wcheckBox.DefaultCheckBoxValue;
    }
    else
    {
      if (formField.FormFieldType != FormFieldType.TextInput)
        return;
      WTextFormField formField1 = formField as WTextFormField;
      frmField.MaxLength = formField1.MaximumLength;
      frmField.TextFormFieldType = formField1.Type;
      if (formField1.Type == TextFormFieldType.RegularText)
      {
        frmField.Format = FormFieldPropertiesConverter.GetStringTextFormat(formField1);
        frmField.DefaultTextInputValue = FormFieldPropertiesConverter.FormatText(formField1.TextFormat, formField1.DefaultText);
      }
      else
      {
        frmField.Format = formField1.StringFormat;
        frmField.DefaultTextInputValue = formField1.DefaultText;
      }
    }
  }

  private static TextFormat GetTextFormat(string formFieldFormat)
  {
    switch (formFieldFormat)
    {
      case "UPPERCASE":
        return TextFormat.Uppercase;
      case "LOWERCASE":
        return TextFormat.Lowercase;
      case "FIRST CAPITAL":
        return TextFormat.FirstCapital;
      case "TITLE CASE":
        return TextFormat.Titlecase;
      default:
        return TextFormat.None;
    }
  }

  private static string GetStringTextFormat(WTextFormField formField)
  {
    switch (formField.TextFormat)
    {
      case TextFormat.Uppercase:
        return "UPPERCASE";
      case TextFormat.Lowercase:
        return "LOWERCASE";
      case TextFormat.FirstCapital:
        return "FIRST CAPITAL";
      case TextFormat.Titlecase:
        return "TITLE CASE";
      default:
        return string.Empty;
    }
  }

  private static NumberFormat GetNumberFormat(string formFieldFormat)
  {
    switch (formFieldFormat)
    {
      case "0":
        return NumberFormat.WholeNumber;
      case "0,00":
        return NumberFormat.FloatingPoint;
      case "0%":
        return NumberFormat.WholeNumberPercent;
      case "0,00%":
        return NumberFormat.FloatingPointPercent;
      case "#�##0":
        return NumberFormat.WholeNumberWithSpace;
      case "#�##0,00":
        return NumberFormat.FloatingPointWithSpace;
      default:
        return formFieldFormat.StartsWithExt("#�##0,00 ") ? NumberFormat.CurrencyFormat : NumberFormat.None;
    }
  }

  private static string GetStringNumberFormat(NumberFormat numberFormat)
  {
    switch (numberFormat)
    {
      case NumberFormat.WholeNumber:
        return "0";
      case NumberFormat.FloatingPoint:
        return "0,00";
      case NumberFormat.WholeNumberPercent:
        return "0%";
      case NumberFormat.FloatingPointPercent:
        return "0,00%";
      case NumberFormat.WholeNumberWithSpace:
        return "#�##0";
      case NumberFormat.FloatingPointWithSpace:
        return "#�##0,00";
      case NumberFormat.CurrencyFormat:
        return $"#�##0.00 {CultureInfo.CurrentCulture.NumberFormat.CurrencySymbol};(#�##0.00 {CultureInfo.CurrentCulture.NumberFormat.CurrencySymbol})";
      default:
        return string.Empty;
    }
  }

  private static string GetDefaultNumberValue(NumberFormat numberFormat)
  {
    switch (numberFormat)
    {
      case NumberFormat.WholeNumber:
      case NumberFormat.WholeNumberWithSpace:
        return "0";
      case NumberFormat.FloatingPoint:
      case NumberFormat.FloatingPointWithSpace:
        return "0,00";
      case NumberFormat.WholeNumberPercent:
        return "0%";
      case NumberFormat.FloatingPointPercent:
        return "0,00%";
      case NumberFormat.CurrencyFormat:
        return "0,00 " + CultureInfo.CurrentCulture.NumberFormat.CurrencySymbol;
      default:
        return string.Empty;
    }
  }

  internal static string FormatText(TextFormat textFormat, string text)
  {
    if (text != string.Empty)
    {
      switch (textFormat)
      {
        case TextFormat.Uppercase:
          return text.ToUpper();
        case TextFormat.Lowercase:
          return text.ToLower();
        case TextFormat.FirstCapital:
          return text[0].ToString().ToUpper() + text.Remove(0, 1);
        case TextFormat.Titlecase:
          string[] strArray = text.Split(' ');
          for (int index = 0; index < strArray.Length; ++index)
          {
            string str1 = strArray[index];
            string str2 = str1[0].ToString().ToUpper() + str1.Remove(0, 1);
            strArray[index] = str1;
          }
          text = string.Empty;
          int length = strArray.Length;
          for (int index = 0; index < length; ++index)
          {
            text += strArray[index];
            if (index < length - 1)
              text += " ";
          }
          return text;
      }
    }
    return text;
  }

  private static string FormatNumberText(
    string format,
    NumberFormat numberFormat,
    string inputData)
  {
    if (numberFormat == NumberFormat.None)
      return inputData;
    inputData = inputData.Replace('.', ',');
    string empty = string.Empty;
    double num;
    try
    {
      num = Convert.ToDouble(inputData);
    }
    catch
    {
      return FormFieldPropertiesConverter.GetDefaultNumberValue(numberFormat);
    }
    switch (numberFormat)
    {
      case NumberFormat.WholeNumber:
      case NumberFormat.WholeNumberPercent:
        double dValue = numberFormat != NumberFormat.WholeNumberPercent ? Math.Floor(num) : Math.Floor(num * 100.0) / 100.0;
        return FormFieldPropertiesConverter.ConvertNumberToString(format, dValue);
      case NumberFormat.FloatingPoint:
      case NumberFormat.FloatingPointPercent:
        return FormFieldPropertiesConverter.ConvertNumberToString(format, num);
      case NumberFormat.WholeNumberWithSpace:
      case NumberFormat.FloatingPointWithSpace:
      case NumberFormat.CurrencyFormat:
        if (numberFormat == NumberFormat.WholeNumberWithSpace)
          num = Math.Floor(num);
        string str = FormFieldPropertiesConverter.ConvertNumberToString(format, num);
        if (num < 1000.0)
          str = str.Substring(1, str.Length - 1);
        return str;
      default:
        return format != string.Empty ? FormFieldPropertiesConverter.ConvertNumberToString(format, num) : string.Empty;
    }
  }

  private static string ConvertNumberToString(string format, double dValue)
  {
    double num1;
    if (format[format.Length - 1] == '%')
    {
      dValue *= 100.0;
      double num2 = Math.Round(dValue, 2);
      if (num2 > dValue)
        num2 -= 0.01;
      num1 = num2 / 100.0;
    }
    else
    {
      num1 = Math.Round(dValue, 2);
      if (num1 > dValue)
        num1 -= 0.01;
    }
    return num1.ToString(format, (IFormatProvider) CultureInfo.InvariantCulture);
  }
}
