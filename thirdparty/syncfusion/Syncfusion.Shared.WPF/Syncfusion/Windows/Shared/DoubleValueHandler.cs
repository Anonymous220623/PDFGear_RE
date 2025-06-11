// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.DoubleValueHandler
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace Syncfusion.Windows.Shared;

internal class DoubleValueHandler
{
  public static DoubleValueHandler doubleValueHandler = new DoubleValueHandler();
  internal bool AllowSelectionStart;
  internal int count;
  private NumberFormatInfo numberFormat;
  private string maskedText;
  private int selectionStart;
  private int selectionEnd;
  private int selectionLength;
  private int separatorStart;
  private int separatorEnd;
  private int caretPosition;
  private string unmaskedText;
  private double previewValue;
  private int negflag;
  private int groupSeperator;
  private bool minusKeyValidationflag;
  private int selectedLength;

  public bool MatchWithMask(DoubleTextBox doubleTextBox, string text)
  {
    if (doubleTextBox.IsReadOnly)
      return true;
    this.InitializeValues(doubleTextBox);
    doubleTextBox.negativeFlag = false;
    int caretIndex = doubleTextBox.CaretIndex;
    if (!string.IsNullOrEmpty(doubleTextBox.SelectedText))
      this.selectedLength = doubleTextBox.SelectedText.Length;
    if (this.CharacterValidation(doubleTextBox, text))
      return true;
    this.GenerateUnmaskedText(doubleTextBox, true, false);
    if (this.TextEditingForMatchingMask(doubleTextBox, text))
      return true;
    if (this.numberFormat.NumberDecimalSeparator != CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator)
      this.unmaskedText = this.unmaskedText.Replace(this.numberFormat.NumberDecimalSeparator, CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
    if (double.TryParse(this.unmaskedText, out this.previewValue))
    {
      string str1 = this.unmaskedText;
      if (!string.IsNullOrEmpty(this.numberFormat.NumberGroupSeparator))
        str1 = str1.Replace(this.numberFormat.NumberGroupSeparator, string.Empty);
      if (str1.Replace(this.numberFormat.NumberDecimalSeparator, string.Empty).Length > 15 && doubleTextBox.MaxValidation == MaxValidation.OnLostFocus)
      {
        if (!doubleTextBox.IsNegative)
        {
          if (!this.minusKeyValidationflag)
            goto label_18;
        }
        if (!doubleTextBox.negativeFlag)
          this.unmaskedText = "-" + this.unmaskedText;
label_18:
        try
        {
          int length1 = doubleTextBox.MaskedText.Length;
          this.selectionStart = doubleTextBox.SelectionStart;
          if (!string.IsNullOrEmpty(this.numberFormat.NumberDecimalSeparator) && this.unmaskedText.Substring(this.unmaskedText.IndexOf(this.numberFormat.NumberDecimalSeparator)).Length - 1 > doubleTextBox.numberDecimalDigits)
          {
            this.previewValue = double.Parse(this.unmaskedText.Remove(this.unmaskedText.Length - (this.unmaskedText.Substring(this.unmaskedText.IndexOf(this.numberFormat.NumberDecimalSeparator)).Length - 1 - doubleTextBox.numberDecimalDigits)));
            doubleTextBox.MaskedText = this.previewValue.ToString("N", (IFormatProvider) this.numberFormat);
            doubleTextBox.SetValue(new bool?(false), new double?(this.previewValue));
          }
          doubleTextBox.SelectionStart = this.selectionStart + 1;
          int length2 = doubleTextBox.MaskedText.Length;
          int num = length2 - length1;
          if (num == 0 && doubleTextBox.MaskedText[this.selectionStart - 1].ToString() == this.numberFormat.CurrencyDecimalSeparator)
            doubleTextBox.SelectionStart = this.selectionStart + 1;
          else if (this.selectionStart + 1 == length2)
          {
            doubleTextBox.SelectionStart = this.selectionStart + 1;
          }
          else
          {
            switch (num)
            {
              case 1:
                doubleTextBox.SelectionStart = this.selectionStart + 1;
                break;
              case 2:
                doubleTextBox.SelectionStart = this.selectionStart + 2;
                break;
            }
          }
        }
        catch
        {
        }
        return true;
      }
      if (!doubleTextBox.IsNegative && !this.minusKeyValidationflag)
      {
        if (this.maskedText.Contains("-"))
        {
          double? nullable = doubleTextBox.Value;
          if ((nullable.GetValueOrDefault() != 0.0 ? 0 : (nullable.HasValue ? 1 : 0)) == 0)
            goto label_36;
        }
        else
          goto label_36;
      }
      if (!(doubleTextBox.SelectedText == doubleTextBox.Text.ToString()) && !doubleTextBox.SelectedText.Contains("-"))
      {
        this.previewValue *= -1.0;
      }
      else
      {
        double? nullable = doubleTextBox.Value;
        if (((nullable.GetValueOrDefault() != 0.0 ? 0 : (nullable.HasValue ? 1 : 0)) != 0 || doubleTextBox.IsNull) && this.minusKeyValidationflag)
          this.previewValue *= -1.0;
      }
label_36:
      if (this.previewValue > doubleTextBox.MaxValue && doubleTextBox.MaxValidation == MaxValidation.OnKeyPress)
      {
        if (!doubleTextBox.MaxValueOnExceedMaxDigit)
          return true;
        this.previewValue = doubleTextBox.MaxValue;
      }
      if (this.previewValue < doubleTextBox.MinValue && doubleTextBox.MinValidation == MinValidation.OnKeyPress)
      {
        if (this.previewValue <= doubleTextBox.MinValue && doubleTextBox.MinValue >= 0.0)
        {
          if (this.numberFormat != null)
          {
            if (doubleTextBox.UseNullOption)
              this.unmaskedText = this.previewValue.ToString("N", (IFormatProvider) this.numberFormat);
            if (this.unmaskedText.Length - (this.numberFormat.NumberDecimalDigits + 1) >= doubleTextBox.MinValue.ToString("N", (IFormatProvider) this.numberFormat).Length)
              this.previewValue = doubleTextBox.MinValue;
            else if (this.unmaskedText.Length - (this.numberFormat.NumberDecimalDigits + 1) <= doubleTextBox.MinValue.ToString("N", (IFormatProvider) this.numberFormat).Length)
            {
              doubleTextBox.checktext += text;
              if (double.Parse(doubleTextBox.checktext) >= doubleTextBox.MinValue)
              {
                doubleTextBox.Value = double.Parse(doubleTextBox.checktext) <= doubleTextBox.MaxValue ? new double?(double.Parse(doubleTextBox.checktext)) : new double?(doubleTextBox.MaxValue);
                doubleTextBox.CaretIndex = doubleTextBox.Value.ToString().Length;
                doubleTextBox.checktext = "";
              }
              return true;
            }
          }
        }
        else if (this.previewValue > doubleTextBox.MinValue)
          doubleTextBox.MaskedText = this.unmaskedText;
        else if (this.previewValue >= doubleTextBox.MinValue)
        {
          if (doubleTextBox.MinValueOnExceedMinDigit && this.unmaskedText.Length - (this.numberFormat.NumberDecimalDigits + 1) > doubleTextBox.MinValue.ToString().Length)
          {
            this.previewValue = doubleTextBox.MinValue;
          }
          else
          {
            if (this.unmaskedText.Length - (this.numberFormat.NumberDecimalDigits + 1) > doubleTextBox.MinValue.ToString().Length)
              return true;
            doubleTextBox.MaskedText = this.unmaskedText;
          }
        }
        else
        {
          if (!doubleTextBox.MinValueOnExceedMinDigit)
            return true;
          this.previewValue = doubleTextBox.MinValue;
        }
      }
      else if (this.previewValue >= doubleTextBox.MinValue && doubleTextBox.MinValidation == MinValidation.OnKeyPress && doubleTextBox.checktext != "" && double.Parse(doubleTextBox.checktext) == 0.0)
        doubleTextBox.checktext = "";
      if (doubleTextBox.MaxLength != 0 && this.unmaskedText.Length > doubleTextBox.MaxLength && doubleTextBox.NumberDecimalDigits <= doubleTextBox.MaxLength)
      {
        int numberDecimalDigits = doubleTextBox.NumberDecimalDigits;
        if (numberDecimalDigits < 0 && doubleTextBox.MaxLength > 3)
        {
          this.previewValue = double.Parse(this.unmaskedText.Remove(doubleTextBox.MaxLength - 3));
          ++this.caretPosition;
          doubleTextBox.CaretIndex = this.caretPosition;
        }
        else
          this.previewValue = double.Parse(this.unmaskedText.Remove(doubleTextBox.MaxLength - 1 - numberDecimalDigits));
        doubleTextBox.SetValue(new bool?(false), new double?(this.previewValue));
        doubleTextBox.MaskedText = this.previewValue.ToString("N", (IFormatProvider) this.numberFormat);
        if (this.caretPosition == doubleTextBox.CaretIndex)
          ++this.caretPosition;
        doubleTextBox.CaretIndex = this.caretPosition;
        return true;
      }
      if (doubleTextBox.checktext != "" && this.previewValue >= doubleTextBox.MinValue)
        ++this.caretPosition;
      double num1 = double.Parse(doubleTextBox.checktext + this.previewValue.ToString());
      if (num1 <= doubleTextBox.MaxValue)
        this.previewValue = num1;
      if (this.unmaskedText.Length > 1 && this.maskedText.Length - 1 == this.selectionStart && doubleTextBox.NumberDecimalDigits > 0 && !this.maskedText.Contains("-") && !this.maskedText.StartsWith("."))
      {
        this.numberFormat = doubleTextBox.GetCulture().NumberFormat;
        string str2 = this.numberFormat == null || !string.IsNullOrEmpty(this.numberFormat.NumberDecimalSeparator) ? "." : this.numberFormat.NumberDecimalSeparator;
        if (this.unmaskedText.Contains(str2) && double.Parse(this.unmaskedText).ToString().Length - 1 - double.Parse(this.unmaskedText).ToString().IndexOf(str2) != doubleTextBox.numberDecimalDigits)
          this.previewValue = double.Parse(this.unmaskedText.Remove(this.unmaskedText.Length - 1));
      }
      doubleTextBox.MaskedText = this.previewValue.ToString("N", (IFormatProvider) this.numberFormat);
      doubleTextBox.SetValue(new bool?(false), new double?(this.previewValue));
      double? nullable1 = doubleTextBox.Value;
      if ((nullable1.GetValueOrDefault() != 0.0 ? 0 : (nullable1.HasValue ? 1 : 0)) != 0 && this.maskedText.Contains("-"))
      {
        if (this.selectedLength != this.maskedText.Length && !doubleTextBox.MaskedText.Contains("-"))
          doubleTextBox.MaskedText = "-" + doubleTextBox.MaskedText;
        if (text == this.numberFormat.NumberDecimalSeparator || text == doubleTextBox.NumberDecimalSeparator)
          doubleTextBox.CaretIndex = doubleTextBox.MaskedText.IndexOf(doubleTextBox.NumberDecimalSeparator == "" ? this.numberFormat.NumberDecimalSeparator : doubleTextBox.NumberDecimalSeparator) + 1;
        else
          doubleTextBox.CaretIndex = caretIndex + text.Length;
        return true;
      }
      this.maskedText = doubleTextBox.MaskedText;
      if (!string.IsNullOrEmpty(this.maskedText) && this.maskedText[0] != '0' && this.selectionStart != 0 && this.selectionLength != 0 && this.selectionStart + text.Length + this.groupSeperator != this.caretPosition)
        --this.caretPosition;
      int index1 = 0;
      for (int index2 = 0; index2 < this.maskedText.Length && index2 != this.caretPosition && index1 != this.maskedText.Length; ++index2)
      {
        if (char.IsDigit(this.maskedText[index1]))
        {
          ++index1;
        }
        else
        {
          for (int index3 = index1; index3 < this.maskedText.Length && !char.IsDigit(this.maskedText[index3]); ++index3)
            ++index1;
          --index2;
        }
      }
      if (this.maskedText.StartsWith("0") && !this.maskedText.Contains("-") && this.separatorEnd < this.caretPosition && this.caretPosition != index1 && (doubleTextBox.NumberDecimalDigits == doubleTextBox.MaximumNumberDecimalDigits || doubleTextBox.NumberDecimalDigits >= this.selectionStart) && this.caretPosition == this.selectionStart + 1 || this.selectionStart != this.selectionEnd)
        index1 = this.caretPosition;
      doubleTextBox.SelectionStart = index1;
      doubleTextBox.SelectionLength = 0;
      if (!doubleTextBox.OnValidating(new CancelEventArgs(false)) && doubleTextBox.ValueValidation == StringValidation.OnKeyPress)
        return this.StringValidationOnKeyPress(doubleTextBox, false);
    }
    return true;
  }

  public bool HandleKeyDown(DoubleTextBox doubleTextBox, KeyEventArgs eventArgs)
  {
    if (eventArgs.Key == Key.Space)
      return true;
    if (eventArgs.Key == Key.Right || eventArgs.Key == Key.Left)
      DoubleValueHandler.doubleValueHandler.AllowSelectionStart = false;
    switch (eventArgs.Key)
    {
      case Key.Back:
        doubleTextBox.count = 1;
        return this.HandleBackSpaceKey(doubleTextBox);
      case Key.Up:
        return this.HandleUpDownKey(doubleTextBox, true);
      case Key.Down:
        return this.HandleUpDownKey(doubleTextBox, false);
      case Key.Delete:
        doubleTextBox.count = 1;
        return this.HandleDeleteKey(doubleTextBox);
      default:
        return false;
    }
  }

  public bool HandleBackSpaceKey(DoubleTextBox doubleTextBox)
  {
    if (doubleTextBox.IsReadOnly)
      return true;
    this.InitializeValues(doubleTextBox);
    if (doubleTextBox.SelectionLength == 1)
    {
      if (!char.IsDigit(this.maskedText[doubleTextBox.SelectionStart]) && this.maskedText[doubleTextBox.SelectionStart] == '-')
      {
        DoubleTextBox doubleTextBox1 = doubleTextBox;
        double? nullable1 = doubleTextBox.Value;
        double? nullable2 = nullable1.HasValue ? new double?(nullable1.GetValueOrDefault() * -1.0) : new double?();
        doubleTextBox1.Value = nullable2;
        doubleTextBox.SelectionLength = 0;
        return true;
      }
    }
    else if (doubleTextBox.SelectionLength == 0 && doubleTextBox.SelectionStart == 1 && doubleTextBox.SelectionStart != this.maskedText.Length - 2 && !char.IsDigit(this.maskedText[doubleTextBox.SelectionStart - 1]) && this.maskedText[0] == '-')
    {
      DoubleTextBox doubleTextBox2 = doubleTextBox;
      double? nullable3 = doubleTextBox.Value;
      double? nullable4 = nullable3.HasValue ? new double?(nullable3.GetValueOrDefault() * -1.0) : new double?();
      doubleTextBox2.Value = nullable4;
      doubleTextBox.SelectionLength = 0;
      return true;
    }
    if (doubleTextBox.SelectionLength == 0 && doubleTextBox.SelectionStart != 0)
    {
      string str = this.numberFormat != null ? this.numberFormat.NumberGroupSeparator : CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator;
      if (doubleTextBox.NumberGroupSeparator == string.Empty && this.maskedText[doubleTextBox.SelectionStart - 1].ToString() == str || this.maskedText[doubleTextBox.SelectionStart - 1].ToString() == str)
      {
        this.unmaskedText = "";
        --doubleTextBox.SelectionStart;
        return true;
      }
    }
    this.GenerateUnmaskedText(doubleTextBox, false, true);
    if (this.TextEditingForBackspace(doubleTextBox))
      return true;
    if (this.numberFormat.NumberDecimalSeparator != CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator)
      this.unmaskedText = this.unmaskedText.Replace(this.numberFormat.NumberDecimalSeparator, CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
    bool flag = false;
    if (double.TryParse(this.unmaskedText, out this.previewValue))
    {
      if (this.MinMaxValidation(doubleTextBox, true))
        return true;
      doubleTextBox.MaskedText = this.previewValue.ToString("N", (IFormatProvider) this.numberFormat);
      this.maskedText = doubleTextBox.MaskedText;
      doubleTextBox.SetValue(new bool?(false), new double?(this.previewValue));
      if (this.negflag == 0)
      {
        int index1 = 0;
        for (int index2 = 0; index2 < this.unmaskedText.Length && index2 != this.caretPosition && index1 != this.maskedText.Length; ++index2)
        {
          if (char.IsDigit(this.maskedText[index1]))
          {
            ++index1;
          }
          else
          {
            for (int index3 = index1; index3 < this.maskedText.Length; ++index3)
            {
              if (index1 == this.maskedText.IndexOf(this.numberFormat.NumberDecimalSeparator))
                flag = true;
              if (!char.IsDigit(this.maskedText[index3]))
                ++index1;
              else
                break;
            }
            if (!flag)
              --index2;
            flag = false;
          }
        }
        doubleTextBox.SelectionStart = index1;
      }
      else
        doubleTextBox.SelectionStart = 1;
      doubleTextBox.SelectionLength = 0;
      this.negflag = 0;
    }
    else
    {
      if (this.previewValue == 0.0)
      {
        if (doubleTextBox.IsExceedDecimalDigits && this.numberFormat != null)
        {
          doubleTextBox.numberDecimalDigits = doubleTextBox.MinimumNumberDecimalDigits >= 0 ? doubleTextBox.MinimumNumberDecimalDigits : (doubleTextBox.NumberDecimalDigits >= 0 ? doubleTextBox.NumberDecimalDigits : CultureInfo.CurrentCulture.NumberFormat.NumberDecimalDigits);
          this.numberFormat.NumberDecimalDigits = doubleTextBox.numberDecimalDigits;
        }
        if (doubleTextBox.UseNullOption)
          doubleTextBox.SetValue(new bool?(true), new double?());
        else if (doubleTextBox.MinValidation == MinValidation.OnLostFocus || doubleTextBox.MinValue <= 0.0)
          doubleTextBox.SetValue(new bool?(true), new double?(0.0));
        else
          doubleTextBox.SetValue(new bool?(true), doubleTextBox.MinValueOnExceedMinDigit ? new double?(doubleTextBox.MinValue) : doubleTextBox.Value);
        return true;
      }
      this.numberFormat = doubleTextBox.GetCulture().NumberFormat;
      doubleTextBox.MaskedText = this.previewValue.ToString("N", (IFormatProvider) this.numberFormat);
      this.maskedText = doubleTextBox.MaskedText;
      doubleTextBox.SetValue(new bool?(false), new double?(this.previewValue));
    }
    return doubleTextBox.OnValidating(new CancelEventArgs(false)) || doubleTextBox.ValueValidation != StringValidation.OnKeyPress || this.StringValidationOnKeyPress(doubleTextBox, true);
  }

  public bool HandleDeleteKey(DoubleTextBox doubleTextBox)
  {
    if (doubleTextBox.IsReadOnly)
      return true;
    this.InitializeValues(doubleTextBox);
    if (doubleTextBox.SelectionLength <= 1 && doubleTextBox.SelectionStart != this.maskedText.Length)
    {
      if (!char.IsDigit(this.maskedText[doubleTextBox.SelectionStart]) && this.maskedText[doubleTextBox.SelectionStart] == '-')
      {
        double? nullable1 = doubleTextBox.Value;
        if ((nullable1.GetValueOrDefault() >= 0.0 ? 0 : (nullable1.HasValue ? 1 : 0)) == 0 || doubleTextBox.MaxValue >= 0.0)
        {
          DoubleTextBox doubleTextBox1 = doubleTextBox;
          double? nullable2 = doubleTextBox.Value;
          double? nullable3 = nullable2.HasValue ? new double?(nullable2.GetValueOrDefault() * -1.0) : new double?();
          doubleTextBox1.Value = nullable3;
          doubleTextBox.SelectionLength = 0;
          return true;
        }
      }
      if (doubleTextBox.NumberFormat != null && doubleTextBox.SelectionStart == this.maskedText.Length - (doubleTextBox.NumberFormat.NumberDecimalDigits + 2) && this.maskedText[doubleTextBox.SelectionStart] == '0' && doubleTextBox.SelectionStart == 0)
        this.negflag = 1;
      if (doubleTextBox.NumberFormat == null)
      {
        if (this.maskedText[doubleTextBox.SelectionStart] == '0' && doubleTextBox.SelectionStart == 0)
        {
          this.negflag = 1;
          this.unmaskedText = "";
          ++doubleTextBox.SelectionStart;
          return true;
        }
        if (doubleTextBox.SelectionStart == 1)
          this.negflag = 1;
      }
      string str = this.numberFormat != null ? this.numberFormat.NumberGroupSeparator : CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator;
      if (doubleTextBox.NumberGroupSeparator == string.Empty && this.maskedText[doubleTextBox.SelectionStart].ToString() == str || this.maskedText[doubleTextBox.SelectionStart].ToString() == str)
      {
        this.unmaskedText = "";
        ++doubleTextBox.SelectionStart;
        doubleTextBox.SelectionLength = 0;
        return true;
      }
    }
    this.GenerateUnmaskedText(doubleTextBox, false, false);
    if (this.TextEditingForDelete(doubleTextBox))
      return true;
    if (this.numberFormat.NumberDecimalSeparator != CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator)
      this.unmaskedText = this.unmaskedText.Replace(this.numberFormat.NumberDecimalSeparator, CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
    if (double.TryParse(this.unmaskedText, out this.previewValue))
    {
      if (this.MinMaxValidation(doubleTextBox, false))
        return true;
      doubleTextBox.MaskedText = this.previewValue.ToString("N", (IFormatProvider) this.numberFormat);
      this.maskedText = doubleTextBox.MaskedText;
      doubleTextBox.SetValue(new bool?(false), new double?(this.previewValue));
      if (this.negflag == 0)
      {
        int index1 = 0;
        for (int index2 = 0; index2 < this.unmaskedText.Length && index2 != this.caretPosition && index1 != this.maskedText.Length; ++index2)
        {
          if (char.IsDigit(this.maskedText[index1]))
          {
            ++index1;
          }
          else
          {
            for (int index3 = index1; index3 < this.maskedText.Length && !char.IsDigit(this.maskedText[index3]); ++index3)
              ++index1;
            --index2;
          }
        }
        if (!this.AllowSelectionStart)
        {
          doubleTextBox.SelectionStart = index1;
          this.selectionStart = index1;
        }
        else if (this.selectionStart == this.separatorEnd && this.separatorEnd <= this.selectionStart && doubleTextBox.MinimumNumberDecimalDigits == -1 && doubleTextBox.MaximumNumberDecimalDigits == -1)
          doubleTextBox.SelectionStart = index1;
        else if (index1 > 0)
          doubleTextBox.SelectionStart = index1 + 1;
      }
      else
      {
        double? nullable = doubleTextBox.Value;
        if ((nullable.GetValueOrDefault() >= 0.0 ? 0 : (nullable.HasValue ? 1 : 0)) != 0)
        {
          ++doubleTextBox.SelectionStart;
          ++this.selectionStart;
        }
        else
        {
          doubleTextBox.SelectionStart = 1;
          this.selectionStart = 1;
        }
      }
      doubleTextBox.SelectionLength = 0;
      this.negflag = 0;
    }
    else
    {
      if (this.previewValue == 0.0)
      {
        if (doubleTextBox.IsExceedDecimalDigits && this.numberFormat != null)
        {
          doubleTextBox.numberDecimalDigits = doubleTextBox.MinimumNumberDecimalDigits >= 0 ? doubleTextBox.MinimumNumberDecimalDigits : (doubleTextBox.NumberDecimalDigits >= 0 ? doubleTextBox.NumberDecimalDigits : CultureInfo.CurrentCulture.NumberFormat.NumberDecimalDigits);
          this.numberFormat.NumberDecimalDigits = doubleTextBox.numberDecimalDigits;
        }
        if (doubleTextBox.UseNullOption)
          doubleTextBox.SetValue(new bool?(true), new double?());
        else if (doubleTextBox.MinValidation == MinValidation.OnLostFocus || doubleTextBox.MinValue <= 0.0)
          doubleTextBox.SetValue(new bool?(true), new double?(0.0));
        else
          doubleTextBox.SetValue(new bool?(true), doubleTextBox.MinValueOnExceedMinDigit ? new double?(doubleTextBox.MinValue) : doubleTextBox.Value);
        return true;
      }
      doubleTextBox.MaskedText = this.previewValue.ToString("N", (IFormatProvider) this.numberFormat);
      this.maskedText = doubleTextBox.MaskedText;
      doubleTextBox.SetValue(new bool?(false), new double?(this.previewValue));
    }
    return doubleTextBox.OnValidating(new CancelEventArgs(false)) || doubleTextBox.ValueValidation != StringValidation.OnKeyPress || this.StringValidationOnKeyPress(doubleTextBox, false);
  }

  public bool HandleUpDownKey(DoubleTextBox doubleTextBox, bool isUpKeyPressed)
  {
    if (doubleTextBox.IsReadOnly)
      return true;
    if (doubleTextBox.mValue.HasValue)
    {
      int num;
      if (!isUpKeyPressed)
      {
        double? mValue = doubleTextBox.mValue;
        double scrollInterval = doubleTextBox.ScrollInterval;
        double? nullable = mValue.HasValue ? new double?(mValue.GetValueOrDefault() - scrollInterval) : new double?();
        double minValue = doubleTextBox.MinValue;
        num = (nullable.GetValueOrDefault() >= minValue ? 0 : (nullable.HasValue ? 1 : 0)) == 0 ? 0 : (doubleTextBox.MinValidation == MinValidation.OnKeyPress ? 1 : 0);
      }
      else
      {
        double? mValue = doubleTextBox.mValue;
        double scrollInterval = doubleTextBox.ScrollInterval;
        double? nullable = mValue.HasValue ? new double?(mValue.GetValueOrDefault() + scrollInterval) : new double?();
        double maxValue = doubleTextBox.MaxValue;
        num = (nullable.GetValueOrDefault() <= maxValue ? 0 : (nullable.HasValue ? 1 : 0)) == 0 ? 0 : (doubleTextBox.MaxValidation == MaxValidation.OnKeyPress ? 1 : 0);
      }
      if (num != 0)
        return true;
      if (isUpKeyPressed)
      {
        double? mValue = doubleTextBox.mValue;
        double scrollInterval = doubleTextBox.ScrollInterval;
        double? nullable = mValue.HasValue ? new double?(mValue.GetValueOrDefault() + scrollInterval) : new double?();
        double minValue = doubleTextBox.MinValue;
        if ((nullable.GetValueOrDefault() >= minValue ? 0 : (nullable.HasValue ? 1 : 0)) != 0 && doubleTextBox.MinValidation == MinValidation.OnKeyPress)
          return true;
      }
      DoubleTextBox doubleTextBox1 = doubleTextBox;
      bool? IsReload = new bool?(true);
      double? nullable1;
      if (!isUpKeyPressed)
      {
        double? mValue = doubleTextBox.mValue;
        double scrollInterval = doubleTextBox.ScrollInterval;
        nullable1 = mValue.HasValue ? new double?(mValue.GetValueOrDefault() - scrollInterval) : new double?();
      }
      else
      {
        double? mValue = doubleTextBox.mValue;
        double scrollInterval = doubleTextBox.ScrollInterval;
        nullable1 = mValue.HasValue ? new double?(mValue.GetValueOrDefault() + scrollInterval) : new double?();
      }
      bool flag = doubleTextBox1.SetValue(IsReload, nullable1);
      NumberFormatInfo numberFormat = doubleTextBox.GetCulture().NumberFormat;
      if (flag)
      {
        if (doubleTextBox.Culture.Name == "vi-VN" && doubleTextBox.NumberGroupSeparator == string.Empty)
          doubleTextBox.Text = doubleTextBox.Value.Value.ToString("N", (IFormatProvider) numberFormat).Replace(".", "");
        else
          doubleTextBox.Text = doubleTextBox.Value.Value.ToString("N", (IFormatProvider) numberFormat);
      }
    }
    else if (isUpKeyPressed)
    {
      doubleTextBox.SetValue(new bool?(true), new double?(doubleTextBox.ScrollInterval));
    }
    else
    {
      doubleTextBox.mValue = new double?(0.0);
      double? mValue1 = doubleTextBox.mValue;
      double scrollInterval1 = doubleTextBox.ScrollInterval;
      double? nullable2 = mValue1.HasValue ? new double?(mValue1.GetValueOrDefault() - scrollInterval1) : new double?();
      double minValue = doubleTextBox.MinValue;
      if ((nullable2.GetValueOrDefault() < minValue ? 0 : (nullable2.HasValue ? 1 : 0)) != 0)
      {
        DoubleTextBox doubleTextBox2 = doubleTextBox;
        bool? IsReload = new bool?(true);
        double? mValue2 = doubleTextBox.mValue;
        double scrollInterval2 = doubleTextBox.ScrollInterval;
        double? nullable3 = mValue2.HasValue ? new double?(mValue2.GetValueOrDefault() - scrollInterval2) : new double?();
        doubleTextBox2.SetValue(IsReload, nullable3);
      }
      else
        doubleTextBox.SetValue(new bool?(true), doubleTextBox.mValue);
    }
    return doubleTextBox.OnValidating(new CancelEventArgs(false)) || doubleTextBox.ValueValidation != StringValidation.OnKeyPress || this.StringValidationOnKeyPress(doubleTextBox, false);
  }

  private void InitializeValues(DoubleTextBox doubleTextBox)
  {
    this.numberFormat = doubleTextBox.GetCulture().NumberFormat;
    this.count = 0;
    this.maskedText = doubleTextBox.MaskedText;
    this.selectionStart = 0;
    this.selectionEnd = 0;
    this.selectionLength = 0;
    this.separatorStart = this.maskedText.IndexOf(this.numberFormat.NumberDecimalSeparator);
    this.separatorEnd = this.separatorStart + this.numberFormat.NumberDecimalSeparator.Length;
    this.negflag = 0;
    this.caretPosition = doubleTextBox.SelectionStart;
    this.unmaskedText = "";
    this.groupSeperator = 0;
    this.minusKeyValidationflag = false;
    this.selectedLength = 0;
    this.previewValue = 0.0;
  }

  private void GenerateUnmaskedText(
    DoubleTextBox doubleTextBox,
    bool isMatchingMask,
    bool isBackspaceKeyPressed)
  {
    for (int index = 0; index <= this.maskedText.Length; ++index)
    {
      if (index == doubleTextBox.SelectionStart)
      {
        this.selectionStart = this.unmaskedText.Length;
        this.caretPosition = this.selectionStart;
      }
      if (index == doubleTextBox.SelectionStart + doubleTextBox.SelectionLength)
        this.selectionEnd = this.unmaskedText.Length;
      if (index == this.separatorEnd)
        this.separatorEnd = this.unmaskedText.Length;
      if (index == this.separatorStart)
      {
        this.separatorStart = this.unmaskedText.Length;
        this.unmaskedText += this.numberFormat.NumberDecimalSeparator.ToString();
      }
      if (index < this.maskedText.Length)
      {
        if (char.IsDigit(this.maskedText[index]))
        {
          if (isMatchingMask && this.unmaskedText.Length == 0)
          {
            if (this.maskedText[index] != '0' || this.maskedText[index] == '0' && index == 0 || this.maskedText.Contains("-") && (this.maskedText[index] != '0' || this.maskedText[index] == '0' && index == 1))
              this.unmaskedText += (string) (object) this.maskedText[index];
          }
          else
            this.unmaskedText += (string) (object) this.maskedText[index];
        }
        if (isMatchingMask && this.maskedText[index].ToString() == doubleTextBox.NumberGroupSeparator)
          ++this.groupSeperator;
      }
    }
    this.selectionLength = this.selectionEnd - this.selectionStart;
    if (isBackspaceKeyPressed || this.separatorStart >= 0)
      return;
    this.separatorStart = this.unmaskedText.Length;
    this.separatorEnd = this.unmaskedText.Length;
  }

  private bool CharacterValidation(DoubleTextBox doubleTextBox, string text)
  {
    if (doubleTextBox.mValue.HasValue)
    {
      double? mValue = doubleTextBox.mValue;
      if ((mValue.GetValueOrDefault() != 0.0 ? 0 : (mValue.HasValue ? 1 : 0)) == 0 && !object.Equals((object) doubleTextBox.mValue, (object) double.NaN))
        goto label_10;
    }
    if (text == "-" && (doubleTextBox.MinValidation == MinValidation.OnLostFocus || doubleTextBox.MinValidation == MinValidation.OnKeyPress && doubleTextBox.MinValue < 0.0))
    {
      doubleTextBox.minusPressed = true;
      if (doubleTextBox.count % 2 == 0)
      {
        doubleTextBox.IsNegative = false;
      }
      else
      {
        double? nullable = doubleTextBox.Value;
        if ((nullable.GetValueOrDefault() != 0.0 ? 1 : (!nullable.HasValue ? 1 : 0)) != 0)
          doubleTextBox.IsNegative = true;
      }
      ++doubleTextBox.count;
      doubleTextBox.MaskedText = "-";
      doubleTextBox.Value = new double?(0.0);
      doubleTextBox.CaretIndex = 1;
      return true;
    }
    if (doubleTextBox.minusPressed && text != ".")
    {
      doubleTextBox.minusPressed = false;
      this.minusKeyValidationflag = true;
    }
label_10:
    if (text == "-" || text == "+")
    {
      if (doubleTextBox.mValue.HasValue)
      {
        double num = doubleTextBox.mValue.Value;
        if (text == "+")
        {
          double? nullable = doubleTextBox.Value;
          if ((nullable.GetValueOrDefault() >= 1.0 ? 0 : (nullable.HasValue ? 1 : 0)) != 0)
            goto label_15;
        }
        if (!(text == CultureInfo.CurrentCulture.NumberFormat.NegativeSign))
          goto label_16;
label_15:
        num = doubleTextBox.mValue.Value * -1.0;
label_16:
        if (num > doubleTextBox.MaxValue && doubleTextBox.MaxValidation == MaxValidation.OnKeyPress)
        {
          if (doubleTextBox.MaxValueOnExceedMaxDigit)
            num = doubleTextBox.MaxValue;
          else if (doubleTextBox.SelectedText.Length != doubleTextBox.Text.Length)
            return true;
        }
        if (this.selectedLength == doubleTextBox.MaskedText.Length)
        {
          this.selectedLength = 0;
          if (text == "-" && (doubleTextBox.MinValidation == MinValidation.OnLostFocus || doubleTextBox.MinValidation == MinValidation.OnKeyPress && doubleTextBox.MinValue < 0.0))
          {
            doubleTextBox.minusPressed = true;
            if (doubleTextBox.UseNullOption)
              doubleTextBox.SetValue(new bool?(false), new double?());
            else
              doubleTextBox.Value = new double?(0.0);
            doubleTextBox.MaskedText = "-";
            doubleTextBox.CaretIndex = 1;
            return true;
          }
        }
        if (doubleTextBox.MinValidation == MinValidation.OnKeyPress)
        {
          if (num > doubleTextBox.MinValue)
          {
            if (doubleTextBox.MinValueOnExceedMinDigit && num.ToString().Length > doubleTextBox.MinValue.ToString().Length && num < doubleTextBox.MinValue)
            {
              num = doubleTextBox.MinValue;
            }
            else
            {
              if (num.ToString().Length > doubleTextBox.MinValue.ToString().Length && (num < doubleTextBox.MinValue || num > doubleTextBox.MaxValue))
                return true;
              doubleTextBox.MaskedText = num.ToString();
            }
          }
          else
            num = doubleTextBox.MinValue;
        }
        int selectionStart = doubleTextBox.SelectionStart;
        bool isNegative = doubleTextBox.IsNegative;
        doubleTextBox.MaskedText = num.ToString("N", (IFormatProvider) this.numberFormat);
        if (doubleTextBox.MaskedText.Contains("-"))
          doubleTextBox.SelectionStart = selectionStart + 1;
        else if (isNegative)
          doubleTextBox.SelectionStart = selectionStart > 0 ? selectionStart - 1 : 0;
        doubleTextBox.SetValue(new bool?(false), new double?(num));
      }
      return true;
    }
    if (text == this.numberFormat.NumberDecimalSeparator || text == ".")
    {
      if (doubleTextBox.numberDecimalDigits == 0)
      {
        if ((doubleTextBox.MaximumNumberDecimalDigits > 0 || doubleTextBox.MinimumNumberDecimalDigits >= 0) && doubleTextBox.NumberDecimalDigits < 0)
        {
          ++doubleTextBox.numberDecimalDigits;
          doubleTextBox.FormatText();
          doubleTextBox.SelectionStart = this.separatorEnd + this.maskedText.Length + this.numberFormat.NumberDecimalSeparator.Length;
        }
      }
      else
      {
        doubleTextBox.FormatText();
        this.separatorEnd = doubleTextBox.MaskedText.IndexOf(this.numberFormat.NumberDecimalSeparator) + this.numberFormat.NumberDecimalSeparator.Length;
        doubleTextBox.SelectionStart = this.separatorEnd;
      }
      if ((doubleTextBox.Text == "" || this.selectedLength == doubleTextBox.MaskedText.Length) && (text == "." || text == this.numberFormat.NumberDecimalSeparator))
      {
        this.UpdateNumberDecimalDigits(doubleTextBox);
        this.unmaskedText = doubleTextBox.numberDecimalDigits == 0 ? "0" : $"0{this.numberFormat.NumberDecimalSeparator}{"".PadRight(doubleTextBox.numberDecimalDigits, '0')}";
        doubleTextBox.MaskedText = this.unmaskedText;
        doubleTextBox.Value = new double?(Convert.ToDouble(doubleTextBox.MaskedText.Replace(this.numberFormat.NumberDecimalSeparator, CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator)));
        doubleTextBox.Select(doubleTextBox.Text.IndexOf(doubleTextBox.NumberDecimalSeparator == "" ? this.numberFormat.NumberDecimalSeparator : doubleTextBox.NumberDecimalSeparator) + 1, doubleTextBox.Text.Length - doubleTextBox.Text.IndexOf(doubleTextBox.NumberDecimalSeparator));
        doubleTextBox.CaretIndex = doubleTextBox.MaskedText.IndexOf(doubleTextBox.NumberDecimalSeparator == "" ? this.numberFormat.NumberDecimalSeparator : doubleTextBox.NumberDecimalSeparator) + 1;
        if (this.selectedLength == doubleTextBox.MaskedText.Length)
          this.selectedLength = 0;
      }
      if (doubleTextBox.MaskedText == "-" && (doubleTextBox.minusPressed || this.minusKeyValidationflag))
      {
        doubleTextBox.MaskedText += doubleTextBox.Value.Value.ToString("N", (IFormatProvider) this.numberFormat);
        doubleTextBox.CaretIndex = doubleTextBox.MaskedText.IndexOf(doubleTextBox.NumberDecimalSeparator == "" ? this.numberFormat.NumberDecimalSeparator : doubleTextBox.NumberDecimalSeparator) + 1;
      }
      return true;
    }
    if (!(text == this.numberFormat.NumberGroupSeparator))
      return false;
    if (doubleTextBox.SelectionStart >= doubleTextBox.Text.Length || !(doubleTextBox.Text[doubleTextBox.SelectionStart].ToString() == this.numberFormat.NumberGroupSeparator.ToString()))
      return true;
    ++doubleTextBox.SelectionStart;
    return true;
  }

  private bool TextEditingForMatchingMask(DoubleTextBox doubleTextBox, string text)
  {
    if (text != string.Empty)
    {
      if (this.selectionStart <= this.separatorStart && this.selectionEnd >= this.separatorEnd && char.IsDigit(text[0]))
      {
        for (int separatorEnd = this.separatorEnd; separatorEnd < this.selectionEnd; ++separatorEnd)
        {
          if (separatorEnd != this.unmaskedText.Length)
          {
            this.unmaskedText = this.unmaskedText.Remove(separatorEnd, 1);
            --this.selectionEnd;
            --separatorEnd;
          }
        }
        this.unmaskedText = this.unmaskedText.Remove(this.selectionStart, this.separatorStart - this.selectionStart);
        this.caretPosition = this.selectionStart;
        this.unmaskedText = this.unmaskedText.Insert(this.selectionStart, text);
        if (this.unmaskedText[0] != '0')
          this.caretPosition += text.Length;
      }
      else if (this.selectionStart <= this.separatorStart && this.selectionEnd < this.separatorEnd)
      {
        this.unmaskedText = this.unmaskedText.Remove(this.selectionStart, this.selectionLength);
        if (this.selectedLength == 0 && this.unmaskedText.Length == 1)
        {
          double? nullable = doubleTextBox.Value;
          if ((nullable.GetValueOrDefault() != 0.0 ? 0 : (nullable.HasValue ? 1 : 0)) != 0)
            this.unmaskedText = this.unmaskedText.Remove(this.selectionStart, 1);
        }
        this.caretPosition = this.selectionStart;
        if (this.unmaskedText.Length > 0 && this.unmaskedText[0] == '0')
        {
          if (this.selectionStart == 1)
          {
            this.unmaskedText = this.unmaskedText.Insert(0, text);
            this.unmaskedText = this.unmaskedText.Remove(this.selectionStart, text.Length);
          }
          if (this.selectionStart == 0)
          {
            this.unmaskedText = this.unmaskedText.Insert(0, text);
            this.caretPosition += text.Length;
          }
        }
        else
        {
          this.unmaskedText = this.unmaskedText.Insert(this.selectionStart, text);
          this.caretPosition += text.Length;
        }
      }
      else if (this.selectionStart == this.selectionEnd)
      {
        bool flag = true;
        if (doubleTextBox.numberDecimalDigits > 0)
        {
          int num1 = DoubleTextBox.CountDecimalDigits(this.unmaskedText, (DependencyObject) doubleTextBox);
          int num2 = doubleTextBox.MaximumNumberDecimalDigits >= 0 ? doubleTextBox.MaximumNumberDecimalDigits : (doubleTextBox.NumberDecimalDigits >= 0 ? doubleTextBox.NumberDecimalDigits : CultureInfo.CurrentCulture.NumberFormat.NumberDecimalDigits);
          if ((doubleTextBox.IsExceedDecimalDigits ? (num1 == num2 ? 1 : 0) : (num1 == doubleTextBox.numberDecimalDigits ? 1 : 0)) != 0)
            flag = true;
          else if (this.selectionStart != this.unmaskedText.Length || num2 > doubleTextBox.numberDecimalDigits)
            flag = false;
        }
        if (this.selectionStart == this.unmaskedText.Length && flag && !string.IsNullOrEmpty(this.unmaskedText))
        {
          this.unmaskedText = this.unmaskedText.Insert(this.unmaskedText.Length - 1, text[0].ToString());
          this.unmaskedText = this.unmaskedText.Remove(this.unmaskedText.Length - 1, 1);
        }
        else if (!doubleTextBox.IsExceedDecimalDigits && this.selectionStart < this.unmaskedText.Length && this.numberFormat != null && this.selectionStart > this.unmaskedText.IndexOf(this.numberFormat.NumberDecimalSeparator) && doubleTextBox.MaximumNumberDecimalDigits < 0)
        {
          this.unmaskedText = this.unmaskedText.Insert(this.selectionStart, text[0].ToString());
          string s = doubleTextBox.NumberGroupSeparator == string.Empty ? this.numberFormat.NumberGroupSeparator : doubleTextBox.NumberGroupSeparator;
          if (this.maskedText.Contains<char>('-'))
            this.unmaskedText = this.unmaskedText.Remove(doubleTextBox.CaretIndex - doubleTextBox.MaskedText.Count<char>((Func<char, bool>) (p => p.ToString() == s)), 1);
        }
        else if (this.selectionStart != this.unmaskedText.Length)
        {
          if (this.unmaskedText.Contains(this.numberFormat.NumberDecimalSeparator) && this.selectionStart >= this.separatorEnd && this.unmaskedText.Remove(0, this.selectionStart).All<char>((Func<char, bool>) (c => c == '0')))
            this.unmaskedText = this.unmaskedText.Remove(this.selectionStart, 1);
          this.unmaskedText = this.unmaskedText.Insert(this.selectionStart, text[0].ToString());
        }
        else
        {
          if (!doubleTextBox.IsExceedDecimalDigits)
            return true;
          if (doubleTextBox.MaximumNumberDecimalDigits > 0 || doubleTextBox.numberDecimalDigits > doubleTextBox.MaximumNumberDecimalDigits)
          {
            if (this.unmaskedText.Length <= doubleTextBox.SelectionStart)
            {
              this.unmaskedText = this.unmaskedText.Insert(this.unmaskedText.Length, "0");
              this.unmaskedText = this.unmaskedText.Remove(this.unmaskedText.Length - 1);
              this.unmaskedText = this.unmaskedText.Insert(this.unmaskedText.Length, text[0].ToString());
            }
            else
            {
              this.unmaskedText = this.unmaskedText.Insert(doubleTextBox.SelectionStart - 1, "0");
              this.unmaskedText = this.unmaskedText.Remove(doubleTextBox.SelectionStart - 1);
              this.unmaskedText = this.unmaskedText.Insert(doubleTextBox.SelectionStart - 1, text[0].ToString());
            }
            double? nullable1 = doubleTextBox.Value;
            if ((nullable1.GetValueOrDefault() <= -1.0 ? 0 : (nullable1.HasValue ? 1 : 0)) != 0)
            {
              double? nullable2 = doubleTextBox.Value;
              if ((nullable2.GetValueOrDefault() >= 1.0 ? 0 : (nullable2.HasValue ? 1 : 0)) != 0)
                this.caretPosition = this.selectionStart + 1;
            }
            this.UpdateNumberDecimalDigits(doubleTextBox);
            this.numberFormat = doubleTextBox.GetCulture().NumberFormat;
          }
        }
      }
      else if (char.IsDigit(text[0]))
      {
        int num3 = 0;
        int num4 = 0;
        int index = 0;
        for (int selectionStart = this.selectionStart; selectionStart < this.selectionEnd && this.unmaskedText.Length > selectionStart && num4 < this.selectionEnd - this.selectionStart; ++selectionStart)
        {
          this.unmaskedText = this.unmaskedText.Remove(selectionStart, 1);
          ++num4;
          if (index < text.Length)
          {
            this.unmaskedText = this.unmaskedText.Insert(selectionStart, text[index].ToString());
            ++index;
          }
          else
          {
            ++num3;
            --selectionStart;
          }
        }
        this.caretPosition = this.selectionStart + text.Length + this.groupSeperator;
      }
      else if (!doubleTextBox.IsExceedDecimalDigits)
      {
        doubleTextBox.negativeFlag = true;
        this.unmaskedText = doubleTextBox.MaskedText;
      }
      if (doubleTextBox.IsExceedDecimalDigits && this.unmaskedText.Contains(this.numberFormat.NumberDecimalSeparator))
        this.UpdateNumberDecimalDigits(doubleTextBox);
    }
    return false;
  }

  private bool TextEditingForBackspace(DoubleTextBox doubleTextBox)
  {
    if (this.selectionStart <= this.separatorStart && this.selectionEnd >= this.separatorEnd && this.unmaskedText.Length > 0)
    {
      if (this.selectionLength < this.unmaskedText.Length)
      {
        for (int separatorEnd = this.separatorEnd; separatorEnd < this.selectionEnd; ++separatorEnd)
        {
          if (separatorEnd != this.unmaskedText.Length)
          {
            this.unmaskedText = this.unmaskedText.Remove(separatorEnd, 1);
            --this.selectionEnd;
            --separatorEnd;
          }
        }
        this.unmaskedText = this.unmaskedText.Remove(this.selectionStart, this.separatorStart - this.selectionStart);
        this.caretPosition = this.selectionStart == 0 ? this.selectionStart + 1 : this.selectionStart;
        if (doubleTextBox.IsExceedDecimalDigits)
          this.UpdateNumberDecimalDigits(doubleTextBox);
      }
      else if (this.selectionLength == this.unmaskedText.Length)
      {
        this.unmaskedText = "";
        double? nullable = doubleTextBox.Value;
        if ((nullable.GetValueOrDefault() != 0.0 ? 1 : (!nullable.HasValue ? 1 : 0)) != 0)
        {
          if (doubleTextBox.UseNullOption)
          {
            double? nullValue = doubleTextBox.NullValue;
            if ((nullValue.GetValueOrDefault() != 0.0 ? 0 : (nullValue.HasValue ? 1 : 0)) == 0)
              goto label_79;
          }
          doubleTextBox.SelectionStart = 1;
        }
      }
    }
    else
    {
      if (doubleTextBox.CaretIndex == 0 && !this.maskedText.Contains(this.numberFormat.NumberDecimalSeparator) && doubleTextBox.SelectedText == string.Empty)
        return true;
      if (this.selectionStart <= this.separatorStart && this.selectionEnd < this.separatorEnd)
      {
        if (this.selectionLength == 0)
        {
          if (this.selectionStart != 0)
          {
            this.selectionLength = 1;
            if (doubleTextBox.MinValue == double.Parse(this.unmaskedText.Replace(this.numberFormat.NumberDecimalSeparator, CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator)))
            {
              if (doubleTextBox.UseNullOption && this.unmaskedText.Length > 0)
                this.unmaskedText = this.unmaskedText.Remove(this.selectionStart - 1, this.selectionLength);
              this.caretPosition = this.selectionStart - 1;
            }
            else if (this.unmaskedText.Length > 0 && !doubleTextBox.MaskedText.Contains<char>('\t'))
            {
              this.unmaskedText = this.unmaskedText.Remove(this.selectionStart - 1, this.selectionLength);
              this.caretPosition = this.selectionStart - 1;
              if (doubleTextBox.SelectionStart == 2)
                this.negflag = 1;
            }
            else if (doubleTextBox.MaskedText.Contains<char>('\t') && doubleTextBox.AcceptsTab && doubleTextBox.AcceptsTab)
            {
              int selectionStart = doubleTextBox.SelectionStart;
              doubleTextBox.MaskedText = doubleTextBox.MaskedText.Remove(doubleTextBox.SelectionStart - 1, 1);
              doubleTextBox.SelectionStart = selectionStart - 1;
            }
          }
          else
          {
            if (doubleTextBox.MaskedText.StartsWith("\t") && doubleTextBox != null && doubleTextBox.AcceptsTab)
            {
              for (int index = 0; index < this.unmaskedText.Length; ++index)
              {
                if (doubleTextBox.MaskedText[index] == '\t')
                {
                  doubleTextBox.MaskedText = doubleTextBox.MaskedText.Remove(index, 1);
                  break;
                }
              }
            }
            return true;
          }
        }
        else if (this.unmaskedText.Length > 0)
        {
          if (this.maskedText[doubleTextBox.SelectionStart] == '-')
          {
            DoubleTextBox doubleTextBox1 = doubleTextBox;
            double? nullable1 = doubleTextBox.Value;
            double? nullable2 = nullable1.HasValue ? new double?(nullable1.GetValueOrDefault() * -1.0) : new double?();
            doubleTextBox1.Value = nullable2;
          }
          this.unmaskedText = this.unmaskedText.Remove(this.selectionStart, this.selectionLength);
          this.caretPosition = this.selectionStart;
          if (doubleTextBox.SelectionStart > 0 && doubleTextBox.Text[doubleTextBox.SelectionStart - 1] == '-')
            this.negflag = 1;
        }
      }
      else if (this.separatorStart < 0)
      {
        if (this.selectionStart >= 0)
        {
          if (this.selectionStart == 0 && this.selectionLength >= 1 && this.maskedText[doubleTextBox.SelectionStart] == '-')
          {
            DoubleTextBox doubleTextBox2 = doubleTextBox;
            double? nullable3 = doubleTextBox.Value;
            double? nullable4 = nullable3.HasValue ? new double?(nullable3.GetValueOrDefault() * -1.0) : new double?();
            doubleTextBox2.Value = nullable4;
          }
          if (this.selectionLength >= 1 && this.unmaskedText.Length > 0)
          {
            if (this.selectionLength == this.unmaskedText.Length && doubleTextBox.MinValue.ToString() != "" && doubleTextBox.MinValue > 0.0)
            {
              this.unmaskedText = doubleTextBox.MinValue.ToString();
            }
            else
            {
              this.unmaskedText = this.unmaskedText.Remove(this.selectionStart, this.selectionLength);
              this.caretPosition = this.selectionStart;
              if (doubleTextBox.SelectionStart == 1)
                this.negflag = 1;
            }
          }
          else if (this.selectionLength == 0 && this.selectionStart == this.unmaskedText.Length && this.unmaskedText.Length > 0)
          {
            if (double.Parse(this.unmaskedText.Replace(this.numberFormat.NumberDecimalSeparator, CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator)) == doubleTextBox.MinValue)
            {
              this.caretPosition = this.selectionStart - 1;
            }
            else
            {
              this.unmaskedText = this.unmaskedText.Remove(this.selectionStart - 1, 1);
              this.caretPosition = this.selectionStart;
            }
          }
          else if (this.selectionLength == 0 && this.unmaskedText.Length > 0 && this.selectionStart != this.unmaskedText.Length && this.selectionStart != 0 && this.maskedText[doubleTextBox.SelectionStart - 1] != ',')
          {
            if (doubleTextBox.SelectionStart == 2)
            {
              this.unmaskedText = this.unmaskedText.Remove(this.selectionStart - 1, 1);
              this.negflag = 1;
            }
            else
            {
              this.unmaskedText = this.unmaskedText.Remove(this.selectionStart - 1, 1);
              this.caretPosition = this.selectionStart - 1;
            }
          }
          else
          {
            if (this.selectionLength == 0 && this.selectionStart != this.unmaskedText.Length && this.selectionStart != 0 && this.maskedText[doubleTextBox.SelectionStart - 1] == ',')
            {
              this.unmaskedText = "";
              --doubleTextBox.SelectionStart;
              return true;
            }
            if (this.selectionStart != 0 && this.unmaskedText.Length > 0)
            {
              this.unmaskedText = this.unmaskedText.Remove(this.selectionStart, 1);
              this.caretPosition = this.selectionStart;
            }
          }
        }
      }
      else if (this.selectionStart == this.selectionEnd && this.unmaskedText.Length > 0)
      {
        if (this.selectionStart != this.separatorEnd && !doubleTextBox.MaskedText.Contains<char>('\t'))
        {
          this.unmaskedText = this.unmaskedText.Remove(this.selectionStart - 1, 1);
          if (doubleTextBox.IsExceedDecimalDigits)
            this.UpdateNumberDecimalDigits(doubleTextBox);
          this.caretPosition = this.selectionStart - 1;
        }
        else
        {
          if (doubleTextBox.MaskedText.Contains<char>('\t') && doubleTextBox.AcceptsTab && doubleTextBox != null)
          {
            int selectionStart = doubleTextBox.SelectionStart;
            if (char.IsDigit(this.maskedText[doubleTextBox.SelectionStart - 1]))
              doubleTextBox.MaskedText = doubleTextBox.MaskedText.Remove(doubleTextBox.SelectionStart - 1, 1);
            doubleTextBox.SelectionStart = selectionStart - 1;
          }
          else
            --doubleTextBox.SelectionStart;
          return true;
        }
      }
      else if (this.unmaskedText.Length > 0)
      {
        for (int index = 0; index < doubleTextBox.SelectionLength; ++index)
          this.unmaskedText = this.unmaskedText.Remove(this.selectionStart, 1);
        if (doubleTextBox.IsExceedDecimalDigits)
          this.UpdateNumberDecimalDigits(doubleTextBox);
        else
          --this.caretPosition;
      }
    }
label_79:
    return false;
  }

  private bool TextEditingForDelete(DoubleTextBox doubleTextBox)
  {
    if (this.selectionStart <= this.separatorStart && this.selectionEnd >= this.separatorEnd && this.unmaskedText.Length > 0)
    {
      if (this.numberFormat != null && this.selectionLength < this.unmaskedText.Length)
      {
        for (int separatorEnd = this.separatorEnd; separatorEnd < this.selectionEnd; ++separatorEnd)
        {
          if (separatorEnd != this.unmaskedText.Length)
          {
            this.unmaskedText = this.unmaskedText.Remove(separatorEnd, 1);
            --this.selectionEnd;
            --separatorEnd;
          }
        }
        this.unmaskedText = this.unmaskedText.Remove(this.selectionStart, this.separatorStart - this.selectionStart);
        this.caretPosition = this.selectionStart == 0 ? this.selectionStart + 1 : this.selectionStart;
        if (doubleTextBox.IsExceedDecimalDigits)
          this.UpdateNumberDecimalDigits(doubleTextBox);
      }
      else if (this.selectionLength == this.unmaskedText.Length)
      {
        this.unmaskedText = "";
        double? nullable = doubleTextBox.Value;
        if ((nullable.GetValueOrDefault() != 0.0 ? 1 : (!nullable.HasValue ? 1 : 0)) != 0)
        {
          if (doubleTextBox.UseNullOption)
          {
            double? nullValue = doubleTextBox.NullValue;
            if ((nullValue.GetValueOrDefault() != 0.0 ? 0 : (nullValue.HasValue ? 1 : 0)) == 0)
              goto label_14;
          }
          doubleTextBox.SelectionStart = 1;
        }
      }
label_14:
      if (this.selectionLength == this.unmaskedText.Length && doubleTextBox.MinValue.ToString() != "" && doubleTextBox.MinValue > 0.0)
        this.unmaskedText = doubleTextBox.MinValue.ToString();
    }
    else if (this.selectionStart <= this.separatorStart && this.selectionEnd < this.separatorEnd && this.unmaskedText.Length > 0)
    {
      if (this.selectionLength == 0)
      {
        if (this.selectionStart != this.separatorStart)
        {
          this.selectionLength = 1;
          this.unmaskedText = this.unmaskedText.Remove(this.selectionStart, this.selectionLength);
          if (this.negflag == 1)
          {
            this.caretPosition = this.selectionStart + 1;
            this.negflag = 0;
          }
          if (doubleTextBox.SelectionStart == 1)
            this.negflag = 1;
        }
        else
        {
          ++doubleTextBox.SelectionStart;
          this.AllowSelectionStart = true;
          return true;
        }
      }
      else
      {
        if (this.selectionStart == 0 && this.maskedText[doubleTextBox.SelectionStart] == '-')
        {
          DoubleTextBox doubleTextBox1 = doubleTextBox;
          double? nullable1 = doubleTextBox.Value;
          double? nullable2 = nullable1.HasValue ? new double?(nullable1.GetValueOrDefault() * -1.0) : new double?();
          doubleTextBox1.Value = nullable2;
        }
        this.unmaskedText = this.unmaskedText.Remove(this.selectionStart, this.selectionLength);
        this.caretPosition = this.selectionStart;
        if (doubleTextBox.SelectionStart == 1)
          this.negflag = 1;
      }
    }
    else if (this.unmaskedText.Length > 0)
    {
      if (this.selectionStart == this.selectionEnd)
      {
        if (this.selectionStart == this.unmaskedText.Length)
          return true;
        if (this.numberFormat != null && this.unmaskedText != string.Empty)
        {
          this.AllowSelectionStart = this.unmaskedText.Length >= this.selectionStart && (this.unmaskedText[this.selectionStart - 1].ToString() == this.numberFormat.NumberDecimalSeparator || this.unmaskedText[this.selectionStart - 1].ToString() == doubleTextBox.NumberDecimalSeparator);
          if (this.unmaskedText[this.selectionStart].ToString() != this.numberFormat.NumberDecimalSeparator && this.unmaskedText[this.selectionStart].ToString() != doubleTextBox.NumberDecimalSeparator)
          {
            this.unmaskedText = this.unmaskedText.Remove(this.selectionStart, 1);
            if (this.selectionStart >= this.separatorEnd && doubleTextBox.MinimumNumberDecimalDigits == -1 && doubleTextBox.MaximumNumberDecimalDigits == -1)
              this.unmaskedText = this.unmaskedText.Insert(this.selectionStart, "0");
          }
        }
        if (doubleTextBox.IsExceedDecimalDigits)
          this.UpdateNumberDecimalDigits(doubleTextBox);
        this.caretPosition = this.selectionEnd != this.selectionStart || this.separatorEnd > this.selectionStart || doubleTextBox.MinimumNumberDecimalDigits != -1 || doubleTextBox.MaximumNumberDecimalDigits != -1 ? this.selectionStart - 1 : this.selectionStart;
      }
      else
      {
        for (int index = 0; index < doubleTextBox.SelectionLength; ++index)
        {
          if (this.unmaskedText.Length > 0 && this.numberFormat != null && !string.IsNullOrEmpty(this.unmaskedText) && this.unmaskedText[this.selectionStart].ToString() != this.numberFormat.NumberDecimalSeparator && (!doubleTextBox.IsExceedDecimalDigits || doubleTextBox.IsExceedDecimalDigits && this.unmaskedText[this.selectionStart - 1].ToString() != doubleTextBox.NumberDecimalSeparator))
            this.unmaskedText = this.unmaskedText.Remove(this.selectionStart, 1);
        }
        if (doubleTextBox.IsExceedDecimalDigits)
        {
          this.UpdateNumberDecimalDigits(doubleTextBox);
          --this.caretPosition;
        }
      }
    }
    return false;
  }

  private void UpdateNumberDecimalDigits(DoubleTextBox doubleTextBox)
  {
    this.count = DoubleTextBox.CountDecimalDigits(this.unmaskedText, (DependencyObject) doubleTextBox);
    doubleTextBox.UpdateNumberDecimalDigits(this.count);
    if (this.numberFormat == null || this.numberFormat.NumberDecimalDigits == doubleTextBox.numberDecimalDigits)
      return;
    this.numberFormat.NumberDecimalDigits = doubleTextBox.numberDecimalDigits;
  }

  private bool MinMaxValidation(DoubleTextBox doubleTextBox, bool isBackspaceKeyPressed)
  {
    if (doubleTextBox.MaskedText.Length >= 15 && doubleTextBox.MaxValidation == MaxValidation.OnLostFocus)
    {
      if (doubleTextBox.IsNegative)
      {
        if (!doubleTextBox.negativeFlag)
          this.unmaskedText = "-" + this.unmaskedText;
      }
      try
      {
        Decimal num1 = Decimal.Parse(this.unmaskedText.Replace(this.numberFormat.NumberDecimalSeparator, CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator));
        int length1 = doubleTextBox.MaskedText.Length;
        this.selectionStart = doubleTextBox.SelectionStart;
        doubleTextBox.MaskedText = num1.ToString("N", (IFormatProvider) this.numberFormat);
        int length2 = doubleTextBox.MaskedText.Length;
        if (length1 != 0)
        {
          int num2 = length1 - length2;
          if (num2 == 0 && (isBackspaceKeyPressed ? doubleTextBox.MaskedText[this.selectionStart - 2].ToString() : doubleTextBox.MaskedText[this.selectionStart - 1].ToString()) == this.numberFormat.CurrencyDecimalSeparator)
            doubleTextBox.SelectionStart = isBackspaceKeyPressed ? this.selectionStart - 1 : this.selectionStart;
          else if ((isBackspaceKeyPressed ? this.selectionStart : this.selectionStart + 1) == length2)
          {
            doubleTextBox.SelectionStart = isBackspaceKeyPressed ? this.selectionStart - 1 : this.selectionStart + 1;
          }
          else
          {
            switch (num2)
            {
              case 1:
                doubleTextBox.SelectionStart = isBackspaceKeyPressed ? this.selectionStart - 1 : this.selectionStart;
                break;
              case 2:
                doubleTextBox.SelectionStart = isBackspaceKeyPressed ? this.selectionStart - 2 : this.selectionStart - 1;
                break;
            }
          }
        }
      }
      catch
      {
      }
      return true;
    }
    if (this.previewValue == 0.0)
    {
      if (doubleTextBox.IsExceedDecimalDigits && this.numberFormat != null)
      {
        doubleTextBox.numberDecimalDigits = doubleTextBox.MinimumNumberDecimalDigits >= 0 ? doubleTextBox.MinimumNumberDecimalDigits : (doubleTextBox.NumberDecimalDigits >= 0 ? doubleTextBox.NumberDecimalDigits : CultureInfo.CurrentCulture.NumberFormat.NumberDecimalDigits);
        this.numberFormat.NumberDecimalDigits = doubleTextBox.numberDecimalDigits;
      }
      if (doubleTextBox.UseNullOption)
        doubleTextBox.SetValue(new bool?(true), new double?());
      else if (isBackspaceKeyPressed)
      {
        if (doubleTextBox.MinValidation == MinValidation.OnLostFocus || doubleTextBox.MinValue <= 0.0)
        {
          if (doubleTextBox.SelectionStart != 0)
            --doubleTextBox.SelectionStart;
          doubleTextBox.SetValue(new bool?(true), new double?(0.0));
        }
        else
        {
          if (doubleTextBox.SelectionStart != 0)
            --doubleTextBox.SelectionStart;
          doubleTextBox.SetValue(new bool?(true), new double?(doubleTextBox.MinValue));
        }
      }
      else
      {
        if (this.selectionLength == 0)
          ++doubleTextBox.SelectionStart;
        doubleTextBox.SetValue(new bool?(true), new double?(0.0));
      }
      return true;
    }
    if (doubleTextBox.IsNegative)
      this.previewValue *= -1.0;
    this.numberFormat = doubleTextBox.GetCulture().NumberFormat;
    if (this.previewValue > doubleTextBox.MaxValue && doubleTextBox.MaxValidation == MaxValidation.OnKeyPress)
    {
      if (!doubleTextBox.MaxValueOnExceedMaxDigit)
        return true;
      this.previewValue = doubleTextBox.MaxValue;
    }
    if ((isBackspaceKeyPressed ? (this.previewValue <= doubleTextBox.MinValue ? 1 : 0) : (this.previewValue < doubleTextBox.MinValue ? 1 : 0)) != 0 && doubleTextBox.MinValidation == MinValidation.OnKeyPress)
    {
      if ((isBackspaceKeyPressed ? (this.previewValue < doubleTextBox.MinValue ? 1 : 0) : (this.previewValue <= doubleTextBox.MinValue ? 1 : 0)) != 0 && doubleTextBox.MinValue >= 0.0)
      {
        if (this.numberFormat != null)
        {
          if (this.unmaskedText.Length - (this.numberFormat.NumberDecimalDigits + 1) >= doubleTextBox.MinValue.ToString().Length)
          {
            if (!doubleTextBox.MinValueOnExceedMinDigit)
              return true;
            this.previewValue = doubleTextBox.MinValue;
          }
          else if (this.unmaskedText.Length - (this.numberFormat.NumberDecimalDigits + 1) <= doubleTextBox.MinValue.ToString().Length)
          {
            if (doubleTextBox.MinValueOnExceedMinDigit)
            {
              this.previewValue = doubleTextBox.MinValue;
            }
            else
            {
              if (this.previewValue < doubleTextBox.MinValue)
                return true;
              doubleTextBox.MaskedText = this.unmaskedText;
            }
          }
          else if (!isBackspaceKeyPressed)
            return true;
        }
        else if (isBackspaceKeyPressed)
          return true;
      }
      else if (this.previewValue > doubleTextBox.MinValue)
        doubleTextBox.MaskedText = this.unmaskedText;
      else if (this.previewValue >= doubleTextBox.MinValue)
      {
        if (doubleTextBox.MinValueOnExceedMinDigit && this.unmaskedText.Length - (this.numberFormat.NumberDecimalDigits + 1) > doubleTextBox.MinValue.ToString().Length)
        {
          this.previewValue = doubleTextBox.MinValue;
        }
        else
        {
          if (this.unmaskedText.Length - (this.numberFormat.NumberDecimalDigits + 1) > doubleTextBox.MinValue.ToString().Length)
            return true;
          doubleTextBox.MaskedText = this.unmaskedText;
        }
      }
      else
      {
        if (!doubleTextBox.MinValueOnExceedMinDigit)
          return true;
        this.previewValue = doubleTextBox.MinValue;
      }
    }
    return false;
  }

  private bool StringValidationOnKeyPress(DoubleTextBox doubleTextBox, bool isBackspaceKeyPressed)
  {
    string message = "";
    bool bIsValidInput = doubleTextBox.ValidationValue == doubleTextBox.Value.ToString();
    string messageBoxText = bIsValidInput ? "String validation succeeded" : "String validation failed";
    if (!bIsValidInput)
    {
      if (doubleTextBox.InvalidValueBehavior == InvalidInputBehavior.DisplayErrorMessage)
      {
        int num = (int) MessageBox.Show(messageBoxText, "Invalid value", MessageBoxButton.OK);
        doubleTextBox.OnValueValidationCompleted(new StringValidationEventArgs(bIsValidInput, message, doubleTextBox.ValidationValue));
        doubleTextBox.OnValidated(EventArgs.Empty);
        return true;
      }
      if (doubleTextBox.InvalidValueBehavior == InvalidInputBehavior.ResetValue)
      {
        doubleTextBox.OnValueValidationCompleted(new StringValidationEventArgs(bIsValidInput, message, doubleTextBox.ValidationValue));
        doubleTextBox.OnValidated(EventArgs.Empty);
        return true;
      }
      if (doubleTextBox.InvalidValueBehavior == InvalidInputBehavior.None)
      {
        doubleTextBox.OnValueValidationCompleted(new StringValidationEventArgs(bIsValidInput, message, doubleTextBox.ValidationValue));
        doubleTextBox.OnValidated(EventArgs.Empty);
        if (isBackspaceKeyPressed)
          return true;
      }
    }
    else
    {
      doubleTextBox.OnValueValidationCompleted(new StringValidationEventArgs(bIsValidInput, message, doubleTextBox.ValidationValue));
      doubleTextBox.OnValidated(EventArgs.Empty);
    }
    return !isBackspaceKeyPressed;
  }
}
