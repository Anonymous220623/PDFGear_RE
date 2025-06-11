// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.PercentValueHandler
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

internal class PercentValueHandler
{
  private bool _negativeZeroPresent;
  public static PercentValueHandler percentValueHandler = new PercentValueHandler();
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
  private bool minusKeyValidationFlag;
  private bool separatorFlag;

  public bool MatchWithMask(PercentTextBox percentTextBox, string text)
  {
    if (percentTextBox.IsReadOnly)
      return true;
    this.InitializeValues(percentTextBox);
    if (this.CharacterValidation(percentTextBox, text))
      return true;
    this.GenerateUnmaskedText(percentTextBox, true);
    if (this.TextEditingForMatchingMask(percentTextBox, text))
      return true;
    if (double.TryParse(this.unmaskedText, out this.previewValue))
    {
      double num = this.previewValue;
      if (percentTextBox.IsNegative || this.minusKeyValidationFlag || this._negativeZeroPresent)
      {
        if (!(percentTextBox.SelectedText == percentTextBox.Text.ToString()) && !percentTextBox.SelectedText.Contains("-"))
        {
          this.previewValue *= -1.0;
          num = this.previewValue;
        }
        else
        {
          double? percentValue = percentTextBox.PercentValue;
          if (((percentValue.GetValueOrDefault() != 0.0 ? 0 : (percentValue.HasValue ? 1 : 0)) != 0 || percentTextBox.IsNull) && this.minusKeyValidationFlag)
          {
            this.previewValue *= -1.0;
            num = this.previewValue;
          }
        }
      }
      if (percentTextBox.PercentEditMode == PercentEditMode.PercentMode)
        num = this.previewValue / 100.0;
      if (num > percentTextBox.MaxValue && percentTextBox.MaxValidation == MaxValidation.OnKeyPress && !percentTextBox.ValidationOnLostFocus)
      {
        if (!percentTextBox.MaxValueOnExceedMaxDigit)
          return true;
        num = percentTextBox.MaxValue;
      }
      if (num < percentTextBox.MinValue && percentTextBox.MinValidation == MinValidation.OnKeyPress && !percentTextBox.ValidationOnLostFocus)
      {
        if (num <= percentTextBox.MinValue && percentTextBox.MinValue >= 0.0)
        {
          if (this.numberFormat != null)
          {
            if (percentTextBox.UseNullOption)
              this.unmaskedText = num.ToString("N", (IFormatProvider) this.numberFormat);
            if (this.unmaskedText.Length - (this.numberFormat.NumberDecimalDigits + 1) >= percentTextBox.MinValue.ToString().Length)
              num = percentTextBox.MinValue;
            else if (this.unmaskedText.Length - (this.numberFormat.NumberDecimalDigits + 1) <= percentTextBox.MinValue.ToString().Length)
            {
              percentTextBox.checktext += text;
              if (double.Parse(percentTextBox.checktext) > percentTextBox.MinValue)
              {
                percentTextBox.PercentValue = new double?(double.Parse(percentTextBox.checktext));
                percentTextBox.CaretIndex = percentTextBox.PercentValue.ToString().Length;
              }
              return true;
            }
          }
        }
        else if (num > percentTextBox.MinValue)
          percentTextBox.MaskedText = this.unmaskedText;
        else if (num >= percentTextBox.MinValue)
        {
          if (percentTextBox.MinValueOnExceedMinDigit && this.unmaskedText.Length - (this.numberFormat.NumberDecimalDigits + 1) > percentTextBox.MinValue.ToString().Length)
          {
            num = percentTextBox.MinValue;
          }
          else
          {
            if (this.unmaskedText.Length - (this.numberFormat.NumberDecimalDigits + 1) > percentTextBox.MinValue.ToString().Length)
              return true;
            percentTextBox.MaskedText = this.unmaskedText;
          }
        }
        else
        {
          if (!percentTextBox.MinValueOnExceedMinDigit)
            return true;
          num = percentTextBox.MinValue;
        }
      }
      if (percentTextBox.PercentEditMode == PercentEditMode.PercentMode)
      {
        if (percentTextBox.MaskedText == "-" && (percentTextBox.minusPressed || this.minusKeyValidationFlag) && num == 0.0)
        {
          percentTextBox.MaskedText += num.ToString("P", (IFormatProvider) this.numberFormat);
          this._negativeZeroPresent = true;
        }
        else
          percentTextBox.MaskedText = num.ToString("P", (IFormatProvider) this.numberFormat);
      }
      else if (percentTextBox.MaskedText == "-" && (percentTextBox.minusPressed || this.minusKeyValidationFlag) && num == 0.0)
      {
        percentTextBox.MaskedText += (num / 100.0).ToString("P", (IFormatProvider) this.numberFormat);
        this._negativeZeroPresent = true;
      }
      else
        percentTextBox.MaskedText = (num / 100.0).ToString("P", (IFormatProvider) this.numberFormat);
      this.maskedText = percentTextBox.MaskedText;
      percentTextBox.SetValue(new bool?(false), new double?(num));
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
      percentTextBox.SelectionStart = index1;
      percentTextBox.SelectionLength = 0;
      if (percentTextBox.IsNegative && this._negativeZeroPresent)
        this._negativeZeroPresent = false;
    }
    return percentTextBox.OnValidating(new CancelEventArgs(false)) || percentTextBox.ValueValidation != StringValidation.OnKeyPress || this.StringValidationOnKeyPress(percentTextBox, false);
  }

  public bool HandleKeyDown(PercentTextBox percentTextBox, KeyEventArgs eventArgs)
  {
    if (eventArgs.Key == Key.Space)
      return true;
    switch (eventArgs.Key)
    {
      case Key.Back:
        return this.HandleBackSpaceKey(percentTextBox);
      case Key.Up:
        return this.HandleUpDownKey(percentTextBox, true);
      case Key.Down:
        return this.HandleUpDownKey(percentTextBox, false);
      case Key.Delete:
        return this.HandleDeleteKey(percentTextBox);
      default:
        return false;
    }
  }

  public bool HandleBackSpaceKey(PercentTextBox percentTextBox)
  {
    if (percentTextBox.IsReadOnly)
      return true;
    this.InitializeValues(percentTextBox);
    if (percentTextBox.SelectionLength == 1)
    {
      if (!char.IsDigit(this.maskedText[percentTextBox.SelectionStart]))
      {
        percentTextBox.SelectionLength = 0;
        return true;
      }
    }
    else if (percentTextBox.SelectionLength == 0 && percentTextBox.SelectionStart != 0 && !char.IsDigit(this.maskedText[percentTextBox.SelectionStart - 1]))
    {
      if (this.maskedText[percentTextBox.SelectionStart - 1] == '-')
      {
        this.unmaskedText = percentTextBox.MaskedText;
        this.unmaskedText = this.maskedText.Remove(this.selectionStart, 1);
        percentTextBox.MaskedText = this.unmaskedText;
        this.maskedText = this.unmaskedText;
        int length = this.maskedText.Length;
        if (!string.IsNullOrEmpty(this.maskedText))
          this.unmaskedText = this.maskedText.Remove(length - 1, 1);
        double result;
        if (double.TryParse(this.unmaskedText, out result))
          percentTextBox.SetValue(new bool?(false), new double?(result));
        percentTextBox.SelectionLength = 0;
        return true;
      }
      if (this.maskedText[percentTextBox.SelectionStart - 1].ToString() == "," || this.maskedText[percentTextBox.SelectionStart - 1].ToString() == "%")
      {
        this.unmaskedText = "";
        --percentTextBox.SelectionStart;
        percentTextBox.SelectionLength = 0;
        return true;
      }
    }
    this.GenerateUnmaskedText(percentTextBox, false);
    return this.TextEditingForBackspace(percentTextBox) || this.GeneratePreviewValue(percentTextBox, true) || percentTextBox.OnValidating(new CancelEventArgs(false)) || percentTextBox.ValueValidation != StringValidation.OnKeyPress || this.StringValidationOnKeyPress(percentTextBox, true);
  }

  public bool HandleDeleteKey(PercentTextBox percentTextBox)
  {
    if (percentTextBox.IsReadOnly)
      return true;
    this.InitializeValues(percentTextBox);
    if (percentTextBox.SelectionLength <= 1 && percentTextBox.SelectionStart != this.maskedText.Length && !char.IsDigit(this.maskedText[percentTextBox.SelectionStart]))
    {
      if (this.maskedText[percentTextBox.SelectionStart] == '-')
      {
        this.unmaskedText = percentTextBox.MaskedText;
        this.unmaskedText = this.maskedText.Remove(this.selectionStart, 1);
        this.maskedText = this.unmaskedText;
        int length = this.maskedText.Length;
        percentTextBox.MaskedText = this.unmaskedText;
        this.unmaskedText = this.maskedText.Remove(length - 1, 1);
        double result;
        if (double.TryParse(this.unmaskedText, out result))
          percentTextBox.SetValue(new bool?(false), new double?(result));
        percentTextBox.SelectionLength = 0;
        return true;
      }
      if (this.maskedText[percentTextBox.SelectionStart].ToString() == "," || this.maskedText[percentTextBox.SelectionStart].ToString() == "%")
      {
        this.unmaskedText = "";
        ++percentTextBox.SelectionStart;
        percentTextBox.SelectionLength = 0;
        return true;
      }
    }
    this.GenerateUnmaskedText(percentTextBox, false);
    return !this.TextEditingForDelete(percentTextBox) && this.GeneratePreviewValue(percentTextBox, false) || true;
  }

  public bool HandleUpDownKey(PercentTextBox percentTextBox, bool isUpKeyPressed)
  {
    if (percentTextBox.IsReadOnly || !percentTextBox.IsScrollingOnCircle)
      return true;
    if (percentTextBox.mValue.HasValue)
    {
      int num;
      if (!isUpKeyPressed)
      {
        if (percentTextBox.MinValidation == MinValidation.OnKeyPress)
        {
          double? mValue = percentTextBox.mValue;
          double scrollInterval = percentTextBox.ScrollInterval;
          double? nullable = mValue.HasValue ? new double?(mValue.GetValueOrDefault() - scrollInterval) : new double?();
          double minValue = percentTextBox.MinValue;
          num = nullable.GetValueOrDefault() >= minValue ? 0 : (nullable.HasValue ? 1 : 0);
        }
        else
          num = 0;
      }
      else if (percentTextBox.MaxValidation == MaxValidation.OnKeyPress)
      {
        double? mValue = percentTextBox.mValue;
        double? nullable = mValue.HasValue ? new double?(mValue.GetValueOrDefault() + 1.0) : new double?();
        double maxValue = percentTextBox.MaxValue;
        num = nullable.GetValueOrDefault() <= maxValue ? 0 : (nullable.HasValue ? 1 : 0);
      }
      else
        num = 0;
      if (num != 0 && !percentTextBox.ValidationOnLostFocus)
      {
        if (percentTextBox.ScrollInterval < 0.0)
        {
          PercentTextBox percentTextBox1 = percentTextBox;
          bool? IsReload = new bool?(true);
          double? nullable;
          if (!isUpKeyPressed)
          {
            double? mValue = percentTextBox.mValue;
            double scrollInterval = percentTextBox.ScrollInterval;
            nullable = mValue.HasValue ? new double?(mValue.GetValueOrDefault() - scrollInterval) : new double?();
          }
          else
          {
            double? mValue = percentTextBox.mValue;
            double scrollInterval = percentTextBox.ScrollInterval;
            nullable = mValue.HasValue ? new double?(mValue.GetValueOrDefault() + scrollInterval) : new double?();
          }
          percentTextBox1.SetValue(IsReload, nullable);
        }
        return true;
      }
      if (isUpKeyPressed)
      {
        double? mValue1 = percentTextBox.mValue;
        double scrollInterval1 = percentTextBox.ScrollInterval;
        double? nullable1 = mValue1.HasValue ? new double?(mValue1.GetValueOrDefault() + scrollInterval1) : new double?();
        double minValue1 = percentTextBox.MinValue;
        if ((nullable1.GetValueOrDefault() < minValue1 ? 0 : (nullable1.HasValue ? 1 : 0)) == 0)
        {
          if (percentTextBox.MinValidation == MinValidation.OnLostFocus)
          {
            double? mValue2 = percentTextBox.mValue;
            double minValue2 = percentTextBox.MinValue;
            if ((mValue2.GetValueOrDefault() >= minValue2 ? 0 : (mValue2.HasValue ? 1 : 0)) == 0)
              goto label_23;
          }
          else
            goto label_23;
        }
        PercentTextBox percentTextBox2 = percentTextBox;
        bool? IsReload = new bool?(true);
        double? mValue3 = percentTextBox.mValue;
        double scrollInterval2 = percentTextBox.ScrollInterval;
        double? nullable2 = mValue3.HasValue ? new double?(mValue3.GetValueOrDefault() + scrollInterval2) : new double?();
        percentTextBox2.SetValue(IsReload, nullable2);
      }
      else
      {
        PercentTextBox percentTextBox3 = percentTextBox;
        bool? IsReload = new bool?(true);
        double? mValue = percentTextBox.mValue;
        double scrollInterval = percentTextBox.ScrollInterval;
        double? nullable = mValue.HasValue ? new double?(mValue.GetValueOrDefault() - scrollInterval) : new double?();
        percentTextBox3.SetValue(IsReload, nullable);
      }
    }
label_23:
    return percentTextBox.OnValidating(new CancelEventArgs(false)) || percentTextBox.ValueValidation != StringValidation.OnKeyPress || this.StringValidationOnKeyPress(percentTextBox, false);
  }

  private void InitializeValues(PercentTextBox percentTextBox)
  {
    this.numberFormat = percentTextBox.GetCulture().NumberFormat;
    this.maskedText = percentTextBox.MaskedText;
    this.selectionStart = 0;
    this.selectionEnd = 0;
    this.selectionLength = 0;
    this.separatorStart = this.maskedText.IndexOf(this.numberFormat.PercentDecimalSeparator);
    this.separatorEnd = this.separatorStart + this.numberFormat.PercentDecimalSeparator.Length;
    this.caretPosition = percentTextBox.SelectionStart;
    this.unmaskedText = "";
    this.minusKeyValidationFlag = false;
    this.previewValue = 0.0;
    this.separatorFlag = false;
  }

  private void GenerateUnmaskedText(PercentTextBox percentTextBox, bool isMatchingMask)
  {
    for (int index = 0; index <= this.maskedText.Length; ++index)
    {
      if (index == percentTextBox.SelectionStart)
      {
        this.selectionStart = this.unmaskedText.Length;
        this.caretPosition = this.selectionStart;
      }
      if (index == percentTextBox.SelectionStart + percentTextBox.SelectionLength)
        this.selectionEnd = this.unmaskedText.Length;
      if (index == this.separatorEnd)
        this.separatorEnd = this.unmaskedText.Length;
      if (index == this.separatorStart)
      {
        this.separatorStart = this.unmaskedText.Length;
        this.unmaskedText += CultureInfo.CurrentCulture.NumberFormat.PercentDecimalSeparator.ToString();
      }
      if (index < this.maskedText.Length && (char.IsDigit(this.maskedText[index]) || !isMatchingMask && this.maskedText[index] == '-'))
      {
        if (isMatchingMask && this.unmaskedText.Length == 0)
        {
          if (this.maskedText[index] != '0' || (this.maskedText[index] != '0' || this.maskedText.Contains("-") ? (this.maskedText.IndexOf("-") == 0 ? 1 : 0) : (index == 0 ? 1 : 0)) != 0)
            this.unmaskedText += (string) (object) this.maskedText[index];
        }
        else
          this.unmaskedText += (string) (object) this.maskedText[index];
      }
    }
    this.selectionLength = this.selectionEnd - this.selectionStart;
    if (this.separatorStart >= 0)
      return;
    this.separatorStart = this.unmaskedText.Length;
    this.separatorEnd = this.unmaskedText.Length;
  }

  private bool CharacterValidation(PercentTextBox percentTextBox, string text)
  {
    if (percentTextBox.mValue.HasValue)
    {
      double? mValue = percentTextBox.mValue;
      if ((mValue.GetValueOrDefault() != 0.0 ? 0 : (mValue.HasValue ? 1 : 0)) == 0)
        goto label_6;
    }
    if (text == "-")
    {
      percentTextBox.minusPressed = true;
      percentTextBox.MaskedText = "-";
      percentTextBox.PercentValue = new double?(0.0);
      percentTextBox.CaretIndex = 1;
      return true;
    }
    if (percentTextBox.minusPressed)
    {
      percentTextBox.minusPressed = false;
      this.minusKeyValidationFlag = true;
    }
label_6:
    if (text == "-" || text == "+")
    {
      double num1 = percentTextBox.mValue.Value;
      if (percentTextBox.mValue.HasValue)
      {
        double num2;
        if (text == "+")
        {
          double? percentValue = percentTextBox.PercentValue;
          num2 = (percentValue.GetValueOrDefault() >= 1.0 ? 0 : (percentValue.HasValue ? 1 : 0)) != 0 ? percentTextBox.mValue.Value * -1.0 : percentTextBox.mValue.Value * 1.0;
        }
        else
          num2 = percentTextBox.mValue.Value * -1.0;
        if (num2 > percentTextBox.MaxValue && percentTextBox.MaxValidation == MaxValidation.OnKeyPress && !percentTextBox.ValidationOnLostFocus)
        {
          if (!percentTextBox.MaxValueOnExceedMaxDigit)
            return true;
          num2 = percentTextBox.MaxValue;
        }
        if (num2 < percentTextBox.MinValue && percentTextBox.MinValidation == MinValidation.OnKeyPress && !percentTextBox.ValidationOnLostFocus)
        {
          if (!percentTextBox.MinValueOnExceedMinDigit)
            return true;
          num2 = percentTextBox.MinValue;
        }
        bool flag = false;
        if (percentTextBox.SelectedText.Length == percentTextBox.MaskedText.Length)
        {
          flag = true;
          if (text == "-")
          {
            percentTextBox.minusPressed = true;
            if (percentTextBox.UseNullOption)
              percentTextBox.SetValue(new bool?(false), new double?());
            else
              percentTextBox.PercentValue = new double?(0.0);
            percentTextBox.MaskedText = "-";
            percentTextBox.CaretIndex = 1;
            return true;
          }
        }
        int selectionStart = percentTextBox.SelectionStart;
        bool isNegative = percentTextBox.IsNegative;
        if (percentTextBox.PercentEditMode == PercentEditMode.DoubleMode)
          percentTextBox.MaskedText = (num2 / 100.0).ToString("P", (IFormatProvider) this.numberFormat);
        else
          percentTextBox.MaskedText = num2.ToString("P", (IFormatProvider) this.numberFormat);
        percentTextBox.SetValue(new bool?(false), new double?(num2));
        if (percentTextBox.MaskedText.Contains("-"))
          percentTextBox.SelectionStart = selectionStart + 1;
        else if (isNegative)
          percentTextBox.SelectionStart = selectionStart > 0 ? selectionStart - 1 : 0;
        if (flag)
          percentTextBox.SelectAll();
      }
      return true;
    }
    if (text == this.numberFormat.NumberDecimalSeparator || text == ".")
    {
      if (percentTextBox.percentDecimalDigits == 0)
      {
        if (percentTextBox.PercentDecimalDigits < 0 && percentTextBox.MinPercentDecimalDigits <= 0)
        {
          ++percentTextBox.percentDecimalDigits;
          percentTextBox.FormatText();
          percentTextBox.SelectionStart = this.separatorEnd + this.maskedText.Length + this.numberFormat.PercentDecimalSeparator.Length - 1;
        }
      }
      else if (percentTextBox.PercentDecimalDigits != 0)
      {
        percentTextBox.SelectionStart = this.separatorEnd;
        if (percentTextBox.Text == "" && (text == "." || text == this.numberFormat.PercentDecimalSeparator))
        {
          percentTextBox.MaskedText = $"0{this.numberFormat.PercentDecimalSeparator}00";
          percentTextBox.PercentValue = new double?(Convert.ToDouble(percentTextBox.MaskedText));
          percentTextBox.Select(2, 0);
        }
      }
      return true;
    }
    if (!percentTextBox.GroupSeperatorEnabled || !(text == this.numberFormat.NumberGroupSeparator))
      return false;
    if (percentTextBox.SelectionStart >= percentTextBox.Text.Length || !(percentTextBox.Text[percentTextBox.SelectionStart].ToString() == this.numberFormat.NumberGroupSeparator.ToString()))
      return true;
    ++percentTextBox.SelectionStart;
    return true;
  }

  private bool TextEditingForMatchingMask(PercentTextBox percentTextBox, string text)
  {
    if (percentTextBox.MaxLength == 0 || this.maskedText.Length <= percentTextBox.MaxLength)
    {
      if (this.selectionStart <= this.separatorStart && this.selectionEnd >= this.separatorEnd)
      {
        for (int separatorEnd = this.separatorEnd; separatorEnd < this.selectionEnd; ++separatorEnd)
        {
          if (separatorEnd != this.unmaskedText.Length)
          {
            this.unmaskedText = this.unmaskedText.Remove(separatorEnd, 1);
            this.unmaskedText = this.unmaskedText.Insert(separatorEnd, "0");
          }
        }
        this.unmaskedText = this.unmaskedText.Remove(this.selectionStart, this.separatorStart - this.selectionStart);
        this.caretPosition = this.selectionStart;
        this.unmaskedText = this.unmaskedText.Insert(this.selectionStart, text);
        this.caretPosition += text.Length;
        this.TrimmingZerosinUnmaskedText();
      }
      else if (this.selectionStart <= this.separatorStart && this.selectionEnd < this.separatorEnd)
      {
        this.unmaskedText = this.unmaskedText.Remove(this.selectionStart, this.selectionLength);
        string str = percentTextBox.PercentDecimalSeparator == string.Empty ? this.numberFormat.NumberDecimalSeparator : percentTextBox.PercentDecimalSeparator;
        if (this.unmaskedText.IndexOf(str) == 1 && percentTextBox.CaretIndex == 0)
          this.unmaskedText = this.unmaskedText.Remove(this.selectionStart, text.Length);
        else if (percentTextBox.PercentDecimalDigits == 0 || percentTextBox.PercentDecimalDigits < 0 && percentTextBox.MinPercentDecimalDigits <= 0)
        {
          double? percentValue = percentTextBox.PercentValue;
          if ((percentValue.GetValueOrDefault() != 0.0 ? 0 : (percentValue.HasValue ? 1 : 0)) != 0 && !percentTextBox.IsNegative)
            this.unmaskedText = this.unmaskedText.Remove(this.selectionStart, text.Length);
        }
        this.caretPosition = this.selectionStart;
        if (!this.unmaskedText.StartsWith("0") || this.unmaskedText.IndexOf(str) != 1 || percentTextBox.CaretIndex != 1)
          this.caretPosition += text.Length;
        this.unmaskedText = this.unmaskedText.Insert(this.selectionStart, text);
      }
      else if (this.selectionStart == this.selectionEnd)
      {
        if (this.selectionStart == this.unmaskedText.Length && !string.IsNullOrEmpty(text))
        {
          if (percentTextBox.MinPercentDecimalDigits < 0 && percentTextBox.MaxPercentDecimalDigits < 0 && percentTextBox.PercentDecimalDigits < 0 || percentTextBox.MaxPercentDecimalDigits > 0 && percentTextBox.MaxPercentDecimalDigits > percentTextBox.percentDecimalDigits && percentTextBox.PercentDecimalDigits < 0 || percentTextBox.MinPercentDecimalDigits >= 0 && percentTextBox.MaxPercentDecimalDigits < 0 && percentTextBox.PercentDecimalDigits < 0)
          {
            this.unmaskedText = this.unmaskedText.Insert(this.unmaskedText.Length, text[0].ToString());
          }
          else
          {
            this.unmaskedText = this.unmaskedText.Insert(this.unmaskedText.Length - 1, text[0].ToString());
            this.unmaskedText = this.unmaskedText.Remove(this.unmaskedText.Length - 1, 1);
          }
          this.caretPosition = this.selectionStart;
        }
        else if (this.selectionStart < this.unmaskedText.Length && this.numberFormat != null && this.selectionStart > this.unmaskedText.IndexOf(this.numberFormat.NumberDecimalSeparator) && !string.IsNullOrEmpty(text))
        {
          if (this.unmaskedText[this.unmaskedText.Length - 1] == '0')
            this.unmaskedText = this.unmaskedText.Remove(this.unmaskedText.Length - 1, 1);
          this.unmaskedText = this.unmaskedText.Insert(this.selectionStart, text[0].ToString());
          string s = percentTextBox.PercentGroupSeparator == string.Empty ? this.numberFormat.PercentGroupSeparator : percentTextBox.PercentGroupSeparator;
          int num1 = percentTextBox.MaskedText.Count<char>((Func<char, bool>) (p => p.ToString() == s));
          if (percentTextBox.IsNegative || percentTextBox.MaskedText.Contains("-") && this.numberFormat.PercentNegativePattern == 0)
          {
            int num2 = num1 + 1;
          }
          if (this.caretPosition + this.maskedText.Substring(0, this.caretPosition).Count<char>((Func<char, bool>) (p => p.ToString() == ",")) == this.maskedText.IndexOf(percentTextBox.PercentageSymbol == string.Empty ? this.numberFormat.PercentSymbol : percentTextBox.PercentageSymbol))
            this.caretPosition += (percentTextBox.PercentageSymbol == string.Empty ? this.numberFormat.PercentSymbol : percentTextBox.PercentageSymbol).Length;
        }
        else
        {
          if (this.selectionStart == this.unmaskedText.Length || string.IsNullOrEmpty(text))
            return true;
          this.unmaskedText = this.unmaskedText.Insert(this.selectionStart, text[0].ToString());
          double? percentValue1 = percentTextBox.PercentValue;
          if ((percentValue1.GetValueOrDefault() <= -1.0 ? 0 : (percentValue1.HasValue ? 1 : 0)) != 0)
          {
            double? percentValue2 = percentTextBox.PercentValue;
            if ((percentValue2.GetValueOrDefault() >= 1.0 ? 0 : (percentValue2.HasValue ? 1 : 0)) != 0 && percentTextBox.PercentEditMode != PercentEditMode.PercentMode)
              this.caretPosition = this.selectionStart + 1;
          }
        }
      }
      else
      {
        int index = 0;
        for (int selectionStart = this.selectionStart; selectionStart < this.selectionEnd; ++selectionStart)
        {
          this.unmaskedText = this.unmaskedText.Remove(selectionStart, 1);
          this.unmaskedText = this.unmaskedText.Insert(selectionStart, index < text.Length ? text[index].ToString() : "0");
          ++index;
        }
        this.caretPosition = this.selectionStart;
        this.TrimmingZerosinUnmaskedText();
      }
    }
    else if (percentTextBox.MaxLength != 0)
    {
      if (percentTextBox.SelectedText != string.Empty)
      {
        string empty = string.Empty;
        if (!(percentTextBox.PercentGroupSeparator == string.Empty))
        {
          string percentGroupSeparator1 = percentTextBox.PercentGroupSeparator;
        }
        else
        {
          string percentGroupSeparator2 = this.numberFormat.PercentGroupSeparator;
        }
        if (!(percentTextBox.PercentageSymbol == string.Empty))
        {
          string percentageSymbol = percentTextBox.PercentageSymbol;
        }
        else
        {
          string percentSymbol = this.numberFormat.PercentSymbol;
        }
        this.maskedText.Remove(percentTextBox.SelectionStart, percentTextBox.SelectedText.Length);
        if (percentTextBox.Text == percentTextBox.SelectedText)
        {
          for (int separatorEnd = this.separatorEnd; separatorEnd < this.selectionEnd; ++separatorEnd)
          {
            if (separatorEnd != this.unmaskedText.Length)
            {
              this.unmaskedText = this.unmaskedText.Remove(separatorEnd, 1);
              this.unmaskedText = this.unmaskedText.Insert(separatorEnd, "0");
            }
          }
          this.unmaskedText = this.unmaskedText.Remove(this.selectionStart, this.separatorStart - this.selectionStart);
          this.caretPosition = this.selectionStart;
          this.unmaskedText = this.unmaskedText.Insert(this.selectionStart, text);
          this.caretPosition += text.Length;
          this.TrimmingZerosinUnmaskedText();
        }
        else if (this.selectionStart <= this.separatorStart && this.selectionEnd >= this.separatorEnd)
        {
          for (int separatorEnd = this.separatorEnd; separatorEnd < this.selectionEnd; ++separatorEnd)
          {
            if (separatorEnd != this.unmaskedText.Length)
            {
              this.unmaskedText = this.unmaskedText.Remove(separatorEnd, 1);
              this.unmaskedText = this.unmaskedText.Insert(separatorEnd, "0");
            }
          }
          this.unmaskedText = this.unmaskedText.Remove(this.selectionStart, this.separatorStart - this.selectionStart);
          this.caretPosition = this.selectionStart;
          this.unmaskedText = this.unmaskedText.Insert(this.selectionStart, text);
          this.caretPosition += text.Length;
          this.TrimmingZerosinUnmaskedText();
        }
        else if (this.selectionStart <= this.separatorStart && this.selectionEnd < this.separatorEnd)
        {
          this.unmaskedText = this.unmaskedText.Remove(this.selectionStart, this.selectionLength);
          this.caretPosition = this.selectionStart;
          this.unmaskedText = this.unmaskedText.Insert(this.selectionStart, text);
          this.caretPosition += text.Length;
        }
      }
      else if (this.selectionStart > this.separatorStart && this.unmaskedText.Length > this.selectionStart)
      {
        this.unmaskedText = this.unmaskedText.Remove(this.selectionStart, text.Length);
        this.unmaskedText = this.unmaskedText.Insert(this.selectionStart, text);
        this.caretPosition = this.selectionStart + text.Length;
      }
      else if (this.selectionStart == this.separatorStart)
        this.caretPosition = this.selectionStart + 1;
    }
    this.UpdatePercentDecimalDigits(percentTextBox);
    return false;
  }

  private void UpdatePercentDecimalDigits(PercentTextBox percentTextBox)
  {
    if (!percentTextBox.IsExceedPercentDecimalDigits)
      return;
    int count = PercentTextBox.CountDecimalDigits(this.unmaskedText, (DependencyObject) percentTextBox);
    percentTextBox.UpdatePercentDecimalDigits(count);
    if (this.numberFormat == null || this.numberFormat.PercentDecimalDigits == percentTextBox.percentDecimalDigits)
      return;
    this.numberFormat.PercentDecimalDigits = percentTextBox.percentDecimalDigits;
  }

  private void TrimmingZerosinUnmaskedText()
  {
    if (!this.unmaskedText.Contains<char>('.'))
      return;
    this.unmaskedText = this.unmaskedText.TrimEnd('0');
  }

  private bool TextEditingForBackspace(PercentTextBox percentTextBox)
  {
    if (this.unmaskedText.Length == this.selectionLength)
    {
      if (percentTextBox.UseNullOption)
      {
        if (percentTextBox.MinValue.ToString() == "0" || percentTextBox.MinValue < 0.0)
        {
          percentTextBox.SetValue(new bool?(true), new double?());
        }
        else
        {
          percentTextBox.SetValue(new bool?(true), percentTextBox.NullValue);
          percentTextBox.SelectionStart = 0;
        }
      }
      else
      {
        percentTextBox.SetValue(new bool?(true), new double?(percentTextBox.MinValue.ToString() == "0" || percentTextBox.MinValue < 0.0 ? 0.0 : percentTextBox.MinValue));
        percentTextBox.SelectionStart = 0;
      }
      return true;
    }
    if (this.selectionStart <= this.separatorStart && this.selectionEnd >= this.separatorEnd)
    {
      for (int separatorEnd = this.separatorEnd; separatorEnd < this.selectionEnd; ++separatorEnd)
      {
        if (separatorEnd != this.unmaskedText.Length)
        {
          this.unmaskedText = this.unmaskedText.Remove(separatorEnd, 1);
          this.unmaskedText = this.unmaskedText.Insert(separatorEnd, "0");
        }
      }
      this.TrimmingZerosinUnmaskedText();
      if (this.separatorStart == this.separatorEnd)
      {
        this.unmaskedText = this.unmaskedText.Remove(this.selectionStart - 1, this.separatorStart - (this.selectionStart - 1));
        if (this.unmaskedText.Length == 0)
          this.unmaskedText = this.unmaskedText.Insert(0, "0");
      }
      else
        this.unmaskedText = this.unmaskedText.Remove(this.selectionStart, this.separatorStart - this.selectionStart);
      this.caretPosition = this.selectionStart;
    }
    else if (this.selectionStart <= this.separatorStart && this.selectionEnd < this.separatorEnd)
    {
      if (this.selectionLength == 0)
      {
        if (this.selectionStart == 0)
          return true;
        this.selectionLength = 1;
        this.unmaskedText = this.unmaskedText.Remove(this.selectionStart - 1, this.selectionLength);
        this.caretPosition = this.selectionStart - 1;
      }
      else
      {
        this.unmaskedText = this.unmaskedText.Remove(this.selectionStart, this.selectionLength);
        this.caretPosition = this.selectionStart;
      }
    }
    else if (this.selectionStart == this.selectionEnd)
    {
      if (this.selectionStart != this.separatorEnd)
      {
        this.unmaskedText = this.unmaskedText.Remove(this.selectionStart - 1, 1);
        this.caretPosition = this.selectionStart - 1;
        if (percentTextBox.percentDecimalDigits == 1 && percentTextBox.PercentDecimalDigits < 0 && this.separatorEnd == this.selectionStart - 1)
          this.caretPosition = this.selectionStart - 2;
      }
      else
      {
        --percentTextBox.SelectionStart;
        return true;
      }
    }
    else
    {
      for (int selectionStart = this.selectionStart; selectionStart < this.selectionEnd; ++selectionStart)
      {
        this.unmaskedText = this.unmaskedText.Remove(selectionStart, 1);
        this.unmaskedText = this.unmaskedText.Insert(selectionStart, "0");
      }
      this.caretPosition = this.selectionStart;
      if (this.separatorEnd == this.selectionStart && this.unmaskedText.Length == this.selectionEnd)
        this.caretPosition = this.selectionStart - 1;
      this.TrimmingZerosinUnmaskedText();
    }
    this.UpdatePercentDecimalDigits(percentTextBox);
    return false;
  }

  private bool TextEditingForDelete(PercentTextBox percentTextBox)
  {
    if (this.unmaskedText.Length == this.selectionLength)
    {
      if (percentTextBox.UseNullOption)
      {
        if (percentTextBox.MinValue.ToString() == "0" || percentTextBox.MinValue < 0.0)
        {
          percentTextBox.SetValue(new bool?(true), new double?());
        }
        else
        {
          percentTextBox.SetValue(new bool?(true), percentTextBox.NullValue);
          percentTextBox.SelectionStart = 0;
        }
      }
      else if ((percentTextBox.MinValue.ToString() == "0" || percentTextBox.MinValue < 0.0) && percentTextBox.MinValue != percentTextBox.MaxValue)
      {
        percentTextBox.SetValue(new bool?(true), new double?(0.0));
        percentTextBox.SelectionStart = 0;
      }
      else
      {
        percentTextBox.SetValue(new bool?(true), new double?(percentTextBox.MinValue));
        percentTextBox.SelectionStart = 0;
      }
      this.UpdatePercentDecimalDigits(percentTextBox);
      return true;
    }
    if (this.selectionStart <= this.separatorStart && this.selectionEnd >= this.separatorEnd)
    {
      for (int separatorEnd = this.separatorEnd; separatorEnd < this.selectionEnd; ++separatorEnd)
      {
        if (separatorEnd != this.unmaskedText.Length)
        {
          this.unmaskedText = this.unmaskedText.Remove(separatorEnd, 1);
          this.unmaskedText = this.unmaskedText.Insert(separatorEnd, "0");
        }
      }
      this.unmaskedText = this.unmaskedText.Remove(this.selectionStart, this.separatorStart - this.selectionStart);
      this.caretPosition = this.selectionStart;
      this.TrimmingZerosinUnmaskedText();
    }
    else if (this.selectionStart <= this.separatorStart && this.selectionEnd < this.separatorEnd)
    {
      if (this.selectionLength == 0)
      {
        if (this.selectionStart != this.separatorStart)
        {
          this.selectionLength = 1;
          this.unmaskedText = this.unmaskedText.Remove(this.selectionStart, this.selectionLength);
          if (this.numberFormat != null && !string.IsNullOrEmpty(this.unmaskedText))
            this.caretPosition = !(this.unmaskedText[0].ToString() == this.numberFormat.NumberDecimalSeparator) ? this.selectionStart : 1;
        }
        else
        {
          ++percentTextBox.SelectionStart;
          return true;
        }
      }
      else
      {
        this.unmaskedText = this.unmaskedText.Remove(this.selectionStart, this.selectionLength);
        this.caretPosition = this.selectionStart;
      }
    }
    else if (this.selectionStart == this.selectionEnd)
    {
      if (this.selectionStart == this.unmaskedText.Length)
        return true;
      this.unmaskedText = this.unmaskedText.Remove(this.selectionStart, 1);
      this.unmaskedText = this.unmaskedText.Insert(this.selectionStart, "0");
    }
    else
    {
      for (int selectionStart = this.selectionStart; selectionStart < this.selectionEnd; ++selectionStart)
      {
        this.unmaskedText = this.unmaskedText.Remove(selectionStart, 1);
        this.unmaskedText = this.unmaskedText.Insert(selectionStart, "0");
      }
      this.caretPosition = this.selectionStart + this.selectionLength;
      this.TrimmingZerosinUnmaskedText();
    }
    this.UpdatePercentDecimalDigits(percentTextBox);
    return false;
  }

  private bool GeneratePreviewValue(PercentTextBox percentTextBox, bool isBackspaceKeyPressed)
  {
    if (double.TryParse(this.unmaskedText, out this.previewValue))
    {
      if (percentTextBox.UseNullOption && this.previewValue == 0.0)
      {
        percentTextBox.SetValue(new bool?(true), new double?());
        return true;
      }
      if (percentTextBox.IsNegative)
        this.previewValue *= 1.0;
      if (percentTextBox.PercentEditMode == PercentEditMode.PercentMode)
        this.previewValue /= 100.0;
      if (this.previewValue > percentTextBox.MaxValue && percentTextBox.MaxValidation == MaxValidation.OnKeyPress && !percentTextBox.ValidationOnLostFocus)
      {
        if (!percentTextBox.MaxValueOnExceedMaxDigit)
          return true;
        this.previewValue = percentTextBox.MaxValue;
      }
      if (this.previewValue < percentTextBox.MinValue && percentTextBox.MinValidation == MinValidation.OnKeyPress && !percentTextBox.ValidationOnLostFocus)
      {
        if (!percentTextBox.MinValueOnExceedMinDigit)
          return true;
        this.previewValue = percentTextBox.MinValue;
      }
      if (percentTextBox.PercentEditMode == PercentEditMode.DoubleMode)
        percentTextBox.MaskedText = (this.previewValue / 100.0).ToString("P", (IFormatProvider) this.numberFormat);
      else
        percentTextBox.MaskedText = this.previewValue.ToString("P", (IFormatProvider) this.numberFormat);
      this.maskedText = percentTextBox.MaskedText;
      percentTextBox.SetValue(new bool?(false), new double?(this.previewValue));
      int index1 = 0;
      for (int index2 = 0; index2 < this.unmaskedText.Length && index2 != this.caretPosition && index1 != this.maskedText.Length; ++index2)
      {
        if (char.IsDigit(this.maskedText[index1]))
        {
          ++index1;
        }
        else
        {
          bool flag = false;
          for (int index3 = index1; index3 < this.maskedText.Length && !char.IsDigit(this.maskedText[index3]); ++index3)
          {
            if (isBackspaceKeyPressed && index1 == this.maskedText.IndexOf(this.numberFormat.PercentDecimalSeparator))
              this.separatorFlag = true;
            if (percentTextBox != null && percentTextBox.IsNegative && percentTextBox.NumberFormat == null && this.maskedText[index3] == '-')
            {
              flag = true;
              ++index1;
              break;
            }
            ++index1;
          }
          if ((isBackspaceKeyPressed ? (this.separatorFlag ? 0 : (!flag ? 1 : 0)) : (!flag ? 1 : 0)) != 0)
            --index2;
          if (isBackspaceKeyPressed)
            this.separatorFlag = false;
        }
      }
      percentTextBox.SelectionStart = index1;
      if (!isBackspaceKeyPressed)
        this.selectionStart = index1;
      percentTextBox.SelectionLength = 0;
    }
    else if (isBackspaceKeyPressed && this.unmaskedText.Length == 0 && percentTextBox.PercentDecimalDigits == 0)
    {
      percentTextBox.MaskedText = this.previewValue.ToString("P", (IFormatProvider) this.numberFormat);
      percentTextBox.SetValue(new bool?(false), new double?(this.previewValue));
    }
    return false;
  }

  private bool StringValidationOnKeyPress(PercentTextBox percentTextBox, bool isBackspaceKeyPressed)
  {
    string message = "";
    bool bIsValidInput = percentTextBox.ValidationValue == percentTextBox.PercentValue.ToString();
    string messageBoxText = bIsValidInput ? "String validation succeeded" : "String validation failed";
    if (!bIsValidInput)
    {
      if (percentTextBox.InvalidValueBehavior == InvalidInputBehavior.DisplayErrorMessage)
      {
        int num = (int) MessageBox.Show(messageBoxText, "Invalid value", MessageBoxButton.OK);
        percentTextBox.OnValueValidationCompleted(new StringValidationEventArgs(bIsValidInput, message, percentTextBox.ValidationValue));
        percentTextBox.OnValidated(EventArgs.Empty);
        return true;
      }
      if (percentTextBox.InvalidValueBehavior == InvalidInputBehavior.ResetValue)
      {
        percentTextBox.OnValueValidationCompleted(new StringValidationEventArgs(bIsValidInput, message, percentTextBox.ValidationValue));
        percentTextBox.OnValidated(EventArgs.Empty);
        return true;
      }
      if (percentTextBox.InvalidValueBehavior == InvalidInputBehavior.None)
      {
        percentTextBox.OnValueValidationCompleted(new StringValidationEventArgs(bIsValidInput, message, percentTextBox.ValidationValue));
        percentTextBox.OnValidated(EventArgs.Empty);
        if (isBackspaceKeyPressed)
          return true;
      }
    }
    else
    {
      percentTextBox.OnValueValidationCompleted(new StringValidationEventArgs(bIsValidInput, message, percentTextBox.ValidationValue));
      percentTextBox.OnValidated(EventArgs.Empty);
    }
    return !isBackspaceKeyPressed;
  }
}
