// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.CurrencyValueHandler
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

internal class CurrencyValueHandler
{
  public static CurrencyValueHandler currencyValueHandler = new CurrencyValueHandler();
  private bool selectedAll;
  private NumberFormatInfo numberFormat;
  private string maskedText;
  private int selectionStart;
  private int selectionEnd;
  private int selectionLength;
  private int separatorStart;
  private int separatorEnd;
  private int caretPosition;
  private string unmaskedText;
  private Decimal previewValue;
  private bool minusKeyValidationFlag;
  private bool valueAvailable;
  private double convertedValue;
  private int currencyFlag;
  private bool positionFlag;

  public bool MatchWithMask(CurrencyTextBox currencyTextBox, string text)
  {
    if (currencyTextBox.IsReadOnly)
      return true;
    this.InitializeValues(currencyTextBox);
    int selectionStart = currencyTextBox.SelectionStart;
    int length = currencyTextBox.Text.Length;
    if (this.CharacterValidation(currencyTextBox, text))
      return true;
    this.GenerateUnmaskedText(currencyTextBox, true);
    if (this.TextEditingForMatchingMask(currencyTextBox, text) || !Decimal.TryParse(this.unmaskedText, out this.previewValue))
      return true;
    if (!currencyTextBox.Text.Contains("-") && !currencyTextBox.Text.Contains("(") && !currencyTextBox.Text.Contains(")") && !this.selectedAll)
      currencyTextBox.IsNegative = false;
    if ((currencyTextBox.IsNegative || this.minusKeyValidationFlag) && !this.selectedAll)
      this.previewValue *= -1M;
    else if (this.minusKeyValidationFlag)
    {
      Decimal? nullable = currencyTextBox.Value;
      if ((!(nullable.GetValueOrDefault() == 0M) ? 0 : (nullable.HasValue ? 1 : 0)) != 0 || !currencyTextBox.Value.HasValue)
        this.previewValue *= -1M;
    }
    if (this.previewValue > currencyTextBox.MaxValue && currencyTextBox.MaxValidation == MaxValidation.OnKeyPress)
    {
      if (!currencyTextBox.MaxValueOnExceedMaxDigit)
        return true;
      this.previewValue = currencyTextBox.MaxValue;
    }
    if (this.previewValue < currencyTextBox.MinValue && currencyTextBox.MinValidation == MinValidation.OnKeyPress)
    {
      if (this.previewValue <= currencyTextBox.MinValue && currencyTextBox.MinValue >= 0M)
      {
        if (this.numberFormat != null)
        {
          if (currencyTextBox.UseNullOption)
            this.unmaskedText = this.previewValue.ToString("N", (IFormatProvider) this.numberFormat);
          if (this.unmaskedText.Length - (this.numberFormat.NumberDecimalDigits + 1) >= currencyTextBox.MinValue.ToString().Length)
            this.previewValue = currencyTextBox.MinValue;
          else if (this.unmaskedText.Length - (this.numberFormat.NumberDecimalDigits + 1) <= currencyTextBox.MinValue.ToString().Length)
          {
            currencyTextBox.checktext += text;
            if (Decimal.Parse(currencyTextBox.checktext) > currencyTextBox.MinValue)
            {
              currencyTextBox.Value = new Decimal?(Decimal.Parse(currencyTextBox.checktext));
              currencyTextBox.CaretIndex = currencyTextBox.Value.ToString().Length + 1;
            }
            return true;
          }
        }
      }
      else if (this.previewValue > currencyTextBox.MinValue)
        currencyTextBox.MaskedText = this.unmaskedText;
      else if (this.previewValue >= currencyTextBox.MinValue)
      {
        if (currencyTextBox.MinValueOnExceedMinDigit && this.unmaskedText.Length - (this.numberFormat.NumberDecimalDigits + 1) > currencyTextBox.MinValue.ToString().Length)
        {
          this.previewValue = currencyTextBox.MinValue;
        }
        else
        {
          if (this.unmaskedText.Length - (this.numberFormat.NumberDecimalDigits + 1) > currencyTextBox.MinValue.ToString().Length)
            return true;
          currencyTextBox.MaskedText = this.unmaskedText;
        }
      }
      else
      {
        if (!currencyTextBox.MinValueOnExceedMinDigit)
          return true;
        this.previewValue = currencyTextBox.MinValue;
      }
    }
    if (currencyTextBox.MaxLength != 0 && this.unmaskedText.Length > currencyTextBox.MaxLength && currencyTextBox.CurrencyDecimalDigits < currencyTextBox.MaxLength)
    {
      int num1 = currencyTextBox.CaretIndex;
      if (this.caretPosition > currencyTextBox.MaxLength || this.caretPosition != this.selectionStart)
        this.caretPosition = this.selectionStart + 1;
      int currencyDecimalDigits = currencyTextBox.CurrencyDecimalDigits;
      if (currencyDecimalDigits < 0)
      {
        this.previewValue = Decimal.Parse(this.unmaskedText.Remove(currencyTextBox.MaxLength - 3));
        ++this.caretPosition;
        currencyTextBox.CaretIndex = this.caretPosition;
      }
      else
      {
        Decimal num2 = Decimal.Parse(this.unmaskedText.Substring(this.unmaskedText.IndexOf(this.numberFormat.CurrencyDecimalSeparator, this.numberFormat.CurrencyDecimalSeparator.ToString().Length), currencyDecimalDigits + 1));
        if (currencyTextBox.IsValueChanged)
        {
          int num3 = (int) Decimal.Parse(this.unmaskedText);
          this.previewValue = currencyTextBox.MaxLength - currencyDecimalDigits - 1 <= 0 ? Decimal.Parse(num3.ToString().Substring(num3.ToString().Length - (currencyTextBox.MaxLength - currencyDecimalDigits))) + num2 : Decimal.Parse(num3.ToString().Substring(num3.ToString().Length - (currencyTextBox.MaxLength - currencyDecimalDigits - 1))) + num2;
          currencyTextBox.IsValueChanged = false;
          num1 = 1;
        }
        else if (currencyTextBox.MaxLength - currencyDecimalDigits - 1 > 0)
        {
          this.previewValue = Decimal.Parse(this.unmaskedText.Remove(currencyTextBox.MaxLength - currencyDecimalDigits - 1)) + num2;
          num1 = currencyTextBox.CaretIndex;
        }
        else
        {
          this.previewValue = Decimal.Parse(this.unmaskedText.Remove(currencyTextBox.MaxLength - currencyDecimalDigits)) + num2;
          ++this.caretPosition;
          num1 = currencyTextBox.CaretIndex;
        }
      }
      currencyTextBox.SetValue(new bool?(false), new Decimal?(this.previewValue));
      currencyTextBox.MaskedText = this.previewValue.ToString("C", (IFormatProvider) this.numberFormat);
      currencyTextBox.CaretIndex = num1;
      if (this.caretPosition == currencyTextBox.CaretIndex)
        ++this.caretPosition;
      currencyTextBox.CaretIndex = this.caretPosition;
      return true;
    }
    bool flag1 = false;
    if (!string.IsNullOrEmpty(currencyTextBox.Value.ToString()))
    {
      if (currencyTextBox.Value.HasValue)
      {
        Decimal? nullable = currencyTextBox.Value;
        if ((!(nullable.GetValueOrDefault() == 0M) ? 0 : (nullable.HasValue ? 1 : 0)) == 0)
          goto label_56;
      }
      else
        goto label_56;
    }
    flag1 = true;
label_56:
    currencyTextBox.SetValue(new bool?(false), new Decimal?(this.previewValue));
    currencyTextBox.MaskedText = this.previewValue.ToString("C", (IFormatProvider) this.numberFormat);
    if (this.maskedText.Contains("-"))
      currencyTextBox.IsNegative = true;
    this.maskedText = currencyTextBox.MaskedText;
    if (currencyTextBox.IsNegative && !currencyTextBox.Text.Contains("-") && this.maskedText.StartsWith("0"))
      this.maskedText = currencyTextBox.MaskedText = currencyTextBox.MaskedText.Insert(0, "-");
    if (flag1 && currencyTextBox.IsNegative && (currencyTextBox.CurrencyNegativePattern == 13 || currencyTextBox.CurrencyNegativePattern == 10 || currencyTextBox.CurrencyNegativePattern == 7 || currencyTextBox.CurrencyNegativePattern == 6))
      --selectionStart;
    bool flag2 = false;
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
    if (length != 0 && this.maskedText.Length > length && !flag1 && !flag2 && currencyTextBox.CurrencySymbol.Length != this.maskedText.Length - length)
      index1 = selectionStart + this.maskedText.Length - length;
    currencyTextBox.SelectionStart = index1;
    currencyTextBox.SelectionLength = 0;
    return currencyTextBox.OnValidating(new CancelEventArgs(false)) || currencyTextBox.ValueValidation != StringValidation.OnKeyPress || this.StringValidationOnKeyPress(currencyTextBox, false);
  }

  public bool HandleKeyDown(CurrencyTextBox currencyTextBox, KeyEventArgs eventArgs)
  {
    if (eventArgs.Key == Key.Space)
      return true;
    switch (eventArgs.Key)
    {
      case Key.Back:
        return this.HandleBackSpaceKey(currencyTextBox);
      case Key.Up:
        return this.HandleUpDownKey(currencyTextBox, true);
      case Key.Down:
        return this.HandleUpDownKey(currencyTextBox, false);
      case Key.Delete:
        return this.HandleDeleteKey(currencyTextBox);
      default:
        return false;
    }
  }

  public bool HandleBackSpaceKey(CurrencyTextBox currencyTextBox)
  {
    if (currencyTextBox.IsReadOnly)
      return true;
    this.InitializeValues(currencyTextBox);
    if (currencyTextBox.SelectionLength == 1)
    {
      if (!char.IsDigit(this.maskedText[currencyTextBox.SelectionStart]))
      {
        currencyTextBox.SelectionLength = 0;
        return true;
      }
    }
    else if (currencyTextBox.SelectionLength == 0 && currencyTextBox.SelectionStart != 0)
    {
      if (!char.IsDigit(this.maskedText[currencyTextBox.SelectionStart - 1]))
      {
        if (this.maskedText[currencyTextBox.SelectionStart - 1] == '(')
        {
          this.unmaskedText = currencyTextBox.MaskedText;
          this.unmaskedText = this.maskedText.Remove(this.selectionStart, 1);
          this.unmaskedText = this.unmaskedText.Remove(this.unmaskedText.Length - 1, 1);
          currencyTextBox.MaskedText = this.unmaskedText;
          this.unmaskedText = this.unmaskedText.Remove(this.selectionStart, 1);
          currencyTextBox.Value = new Decimal?(Decimal.Parse(this.unmaskedText, NumberStyles.Any, (IFormatProvider) this.numberFormat));
          currencyTextBox.SelectionLength = 0;
          return true;
        }
        if (this.numberFormat != null && (this.maskedText[currencyTextBox.SelectionStart - 1].ToString() == this.numberFormat.CurrencyGroupSeparator || this.maskedText[currencyTextBox.SelectionStart - 1].ToString() == this.numberFormat.CurrencySymbol))
        {
          this.unmaskedText = "";
          --currencyTextBox.SelectionStart;
          currencyTextBox.SelectionLength = 0;
          return true;
        }
      }
      else if (this.numberFormat != null)
      {
        if (this.numberFormat.CurrencySymbol != string.Empty)
        {
          if (currencyTextBox.SelectionStart == 2 && this.maskedText[currencyTextBox.SelectionStart - 1].ToString() == "0")
          {
            this.unmaskedText = "";
            --currencyTextBox.SelectionStart;
            currencyTextBox.SelectionLength = 0;
            return true;
          }
        }
        else if (currencyTextBox.SelectionStart == 1 && this.maskedText[currencyTextBox.SelectionStart - 1].ToString() == "0")
        {
          this.unmaskedText = "";
          --currencyTextBox.SelectionStart;
          currencyTextBox.SelectionLength = 0;
          return true;
        }
      }
    }
    this.GenerateUnmaskedText(currencyTextBox, false);
    if (this.TextEditingForBackspace(currencyTextBox))
      return true;
    this.valueAvailable = double.TryParse(this.unmaskedText, out this.convertedValue);
    if (!this.valueAvailable && currencyTextBox.UseNullOption)
    {
      currencyTextBox.SetValue(new bool?(true), new Decimal?());
      return true;
    }
    bool flag = false;
    if (Decimal.TryParse(this.unmaskedText, out this.previewValue))
    {
      if (this.ValueValidation(currencyTextBox))
        return true;
      if (this.currencyFlag == 0)
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
              if (index1 == this.maskedText.IndexOf(this.numberFormat.CurrencyDecimalSeparator))
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
        currencyTextBox.SelectionStart = index1;
        currencyTextBox.SelectionLength = 0;
      }
      else
      {
        currencyTextBox.SelectionStart = currencyTextBox.CurrencyPositivePattern == 1 || currencyTextBox.CurrencyPositivePattern == 3 ? 0 : 1;
        currencyTextBox.SelectionLength = 0;
      }
    }
    else
    {
      currencyTextBox.MaskedText = this.previewValue.ToString("C", (IFormatProvider) this.numberFormat);
      this.maskedText = currencyTextBox.MaskedText;
      currencyTextBox.SetValue(new bool?(false), new Decimal?(this.previewValue));
    }
    return currencyTextBox.OnValidating(new CancelEventArgs(false)) || currencyTextBox.ValueValidation != StringValidation.OnKeyPress || this.StringValidationOnKeyPress(currencyTextBox, true);
  }

  public bool HandleDeleteKey(CurrencyTextBox currencyTextBox)
  {
    if (currencyTextBox.IsReadOnly)
      return true;
    this.InitializeValues(currencyTextBox);
    if (currencyTextBox.SelectionLength <= 1 && currencyTextBox.SelectionStart != this.maskedText.Length)
    {
      if (!char.IsDigit(this.maskedText[currencyTextBox.SelectionStart]) && (this.maskedText[currencyTextBox.SelectionStart] == '(' || this.maskedText[currencyTextBox.SelectionStart] == ')'))
      {
        this.unmaskedText = currencyTextBox.MaskedText;
        this.unmaskedText = this.maskedText.Remove(this.selectionStart, 1);
        this.unmaskedText = this.unmaskedText.Remove(this.unmaskedText.Length - 1, 1);
        currencyTextBox.MaskedText = this.unmaskedText;
        this.unmaskedText = this.unmaskedText.Remove(this.selectionStart, 1);
        currencyTextBox.Value = new Decimal?(Decimal.Parse(this.unmaskedText, NumberStyles.Any, (IFormatProvider) this.numberFormat));
        currencyTextBox.SelectionLength = 0;
        return true;
      }
      if (this.numberFormat != null && (this.maskedText[currencyTextBox.SelectionStart].ToString() == this.numberFormat.CurrencyGroupSeparator || this.maskedText[currencyTextBox.SelectionStart].ToString() == this.numberFormat.CurrencySymbol))
      {
        this.unmaskedText = "";
        ++currencyTextBox.SelectionStart;
        currencyTextBox.SelectionLength = 0;
        return true;
      }
      if (this.numberFormat.CurrencySymbol != null && currencyTextBox.SelectionStart != 0 && (this.maskedText[currencyTextBox.SelectionStart - 1].ToString() == this.numberFormat.CurrencySymbol && this.maskedText[currencyTextBox.SelectionStart].ToString() == "0" || (currencyTextBox.Culture.Name == "ar-SA" || currencyTextBox.Culture.Name == "he-IL") && (this.maskedText[currencyTextBox.SelectionStart].ToString() == " " || this.maskedText[currencyTextBox.SelectionStart].ToString() == "0")))
      {
        this.unmaskedText = "";
        ++currencyTextBox.SelectionStart;
        currencyTextBox.SelectionLength = 0;
        return true;
      }
    }
    this.GenerateUnmaskedText(currencyTextBox, false);
    if (this.TextEditingForDelete(currencyTextBox))
      return true;
    this.valueAvailable = double.TryParse(this.unmaskedText, out this.convertedValue);
    if (!this.valueAvailable && currencyTextBox.UseNullOption)
    {
      currencyTextBox.SetValue(new bool?(true), new Decimal?());
      return true;
    }
    if (Decimal.TryParse(this.unmaskedText, out this.previewValue))
    {
      if (this.ValueValidation(currencyTextBox))
        return true;
      if (this.currencyFlag == 0)
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
        if (this.positionFlag)
        {
          currencyTextBox.SelectionStart = index1 - 1;
          this.selectionStart = index1 - 1;
          this.positionFlag = false;
        }
        else
        {
          currencyTextBox.SelectionStart = index1;
          this.selectionStart = index1;
        }
        currencyTextBox.SelectionLength = 0;
      }
      else if (this.numberFormat != null)
      {
        if (this.numberFormat.CurrencySymbol != string.Empty || currencyTextBox.Text[this.selectionStart] == '0')
        {
          currencyTextBox.SelectionStart = 1;
          this.selectionStart = 1;
          currencyTextBox.SelectionLength = 0;
        }
        else
        {
          currencyTextBox.SelectionStart = 0;
          this.selectionStart = 0;
          currencyTextBox.SelectionLength = 0;
        }
      }
      else
      {
        currencyTextBox.SelectionStart = 1;
        this.selectionStart = 1;
        currencyTextBox.SelectionLength = 0;
      }
    }
    else
    {
      currencyTextBox.MaskedText = this.previewValue.ToString("C", (IFormatProvider) this.numberFormat);
      this.maskedText = currencyTextBox.MaskedText;
      currencyTextBox.SetValue(new bool?(false), new Decimal?(this.previewValue));
    }
    return true;
  }

  public bool HandleUpDownKey(CurrencyTextBox currencyTextBox, bool isUpKeyPressed)
  {
    if (currencyTextBox.IsReadOnly || !currencyTextBox.IsScrollingOnCircle || !currencyTextBox.mValue.HasValue)
      return true;
    int num1;
    if (currencyTextBox.MaxLength - currencyTextBox.CurrencyDecimalDigits - 1 <= 0)
    {
      int length;
      if (!isUpKeyPressed)
      {
        Decimal? nullable = currencyTextBox.Value;
        Decimal scrollInterval = (Decimal) currencyTextBox.ScrollInterval;
        length = ((long) (nullable.HasValue ? new Decimal?(nullable.GetValueOrDefault() - scrollInterval) : new Decimal?()).Value).ToString().Length;
      }
      else
      {
        Decimal? nullable = currencyTextBox.Value;
        Decimal scrollInterval = (Decimal) currencyTextBox.ScrollInterval;
        length = ((long) (nullable.HasValue ? new Decimal?(nullable.GetValueOrDefault() + scrollInterval) : new Decimal?()).Value).ToString().Length;
      }
      int currencyDecimalDigits = currencyTextBox.CurrencyDecimalDigits;
      num1 = length + currencyDecimalDigits;
    }
    else
    {
      int length;
      if (!isUpKeyPressed)
      {
        Decimal? nullable = currencyTextBox.Value;
        Decimal scrollInterval = (Decimal) currencyTextBox.ScrollInterval;
        length = ((long) (nullable.HasValue ? new Decimal?(nullable.GetValueOrDefault() - scrollInterval) : new Decimal?()).Value).ToString().Length;
      }
      else
      {
        Decimal? nullable = currencyTextBox.Value;
        Decimal scrollInterval = (Decimal) currencyTextBox.ScrollInterval;
        length = ((long) (nullable.HasValue ? new Decimal?(nullable.GetValueOrDefault() + scrollInterval) : new Decimal?()).Value).ToString().Length;
      }
      int currencyDecimalDigits = currencyTextBox.CurrencyDecimalDigits;
      num1 = length + currencyDecimalDigits + 1;
    }
    int num2;
    if (!isUpKeyPressed)
    {
      Decimal? mValue = currencyTextBox.mValue;
      Decimal scrollInterval = (Decimal) currencyTextBox.ScrollInterval;
      Decimal? nullable = mValue.HasValue ? new Decimal?(mValue.GetValueOrDefault() - scrollInterval) : new Decimal?();
      Decimal minValue = currencyTextBox.MinValue;
      num2 = (!(nullable.GetValueOrDefault() < minValue) ? 0 : (nullable.HasValue ? 1 : 0)) == 0 ? 0 : (currencyTextBox.MinValidation == MinValidation.OnKeyPress ? 1 : 0);
    }
    else
    {
      Decimal? mValue = currencyTextBox.mValue;
      Decimal? nullable = mValue.HasValue ? new Decimal?(Decimal.op_Increment(mValue.GetValueOrDefault())) : new Decimal?();
      Decimal maxValue = currencyTextBox.MaxValue;
      num2 = (!(nullable.GetValueOrDefault() > maxValue) ? 0 : (nullable.HasValue ? 1 : 0)) == 0 ? 0 : (currencyTextBox.MaxValidation == MaxValidation.OnKeyPress ? 1 : 0);
    }
    if (num2 != 0)
      return true;
    if (currencyTextBox.MaxLength != 0)
    {
      int num3 = num1;
      Decimal? nullable1 = currencyTextBox.Value;
      Decimal scrollInterval = (Decimal) currencyTextBox.ScrollInterval;
      Decimal? nullable2 = nullable1.HasValue ? new Decimal?(nullable1.GetValueOrDefault() - scrollInterval) : new Decimal?();
      int num4 = (!(nullable2.GetValueOrDefault() < 0M) ? 0 : (nullable2.HasValue ? 1 : 0)) != 0 ? currencyTextBox.MaxLength + 1 : currencyTextBox.MaxLength;
      if (num3 > num4)
        goto label_22;
    }
    CurrencyTextBox currencyTextBox1 = currencyTextBox;
    bool? IsReload = new bool?(true);
    Decimal? nullable3;
    if (!isUpKeyPressed)
    {
      Decimal? mValue = currencyTextBox.mValue;
      Decimal scrollInterval = (Decimal) currencyTextBox.ScrollInterval;
      nullable3 = mValue.HasValue ? new Decimal?(mValue.GetValueOrDefault() - scrollInterval) : new Decimal?();
    }
    else
    {
      Decimal? mValue = currencyTextBox.mValue;
      Decimal scrollInterval = (Decimal) currencyTextBox.ScrollInterval;
      nullable3 = mValue.HasValue ? new Decimal?(mValue.GetValueOrDefault() + scrollInterval) : new Decimal?();
    }
    currencyTextBox1.SetValue(IsReload, nullable3);
label_22:
    return currencyTextBox.OnValidating(new CancelEventArgs(false)) || currencyTextBox.ValueValidation != StringValidation.OnKeyPress || this.StringValidationOnKeyPress(currencyTextBox, false);
  }

  private void InitializeValues(CurrencyTextBox currencyTextBox)
  {
    this.numberFormat = currencyTextBox.GetCulture().NumberFormat;
    this.maskedText = currencyTextBox.MaskedText;
    this.selectionStart = 0;
    this.selectionEnd = 0;
    this.selectionLength = 0;
    this.separatorStart = this.maskedText.IndexOf(this.numberFormat.CurrencyDecimalSeparator);
    this.separatorEnd = this.separatorStart + this.numberFormat.CurrencyDecimalSeparator.Length;
    this.caretPosition = currencyTextBox.SelectionStart;
    this.unmaskedText = "";
    this.minusKeyValidationFlag = false;
    this.previewValue = 0.0M;
    this.currencyFlag = 0;
    this.positionFlag = false;
  }

  private void GenerateUnmaskedText(CurrencyTextBox currencyTextBox, bool isMatchingMask)
  {
    for (int index = 0; index <= this.maskedText.Length; ++index)
    {
      if (index == currencyTextBox.SelectionStart)
      {
        this.selectionStart = this.unmaskedText.Length;
        this.caretPosition = this.selectionStart;
      }
      if (index == currencyTextBox.SelectionStart + currencyTextBox.SelectionLength)
        this.selectionEnd = this.unmaskedText.Length;
      if (index == this.separatorEnd)
        this.separatorEnd = this.unmaskedText.Length;
      if (index == this.separatorStart)
      {
        this.separatorStart = this.unmaskedText.Length;
        this.unmaskedText += CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator.ToString();
      }
      if (index < this.maskedText.Length && char.IsDigit(this.maskedText[index]))
      {
        if (isMatchingMask && this.unmaskedText.Length == 0)
        {
          if (this.maskedText[index] != '0' || this.maskedText[0] == '-')
            this.unmaskedText += (string) (object) this.maskedText[index];
        }
        else
          this.unmaskedText += (string) (object) this.maskedText[index];
      }
    }
    this.selectionLength = this.selectionEnd - this.selectionStart;
    if (!isMatchingMask || this.separatorStart >= 0)
      return;
    this.separatorStart = this.unmaskedText.Length;
    this.separatorEnd = this.unmaskedText.Length;
  }

  private bool CharacterValidation(CurrencyTextBox currencyTextBox, string text)
  {
    if (currencyTextBox.mValue.HasValue)
    {
      Decimal? mValue = currencyTextBox.mValue;
      if ((!(mValue.GetValueOrDefault() == 0M) ? 0 : (mValue.HasValue ? 1 : 0)) == 0)
        goto label_11;
    }
    switch (text)
    {
      case "-":
        currencyTextBox.minusPressed = true;
        currencyTextBox.MaskedText = "-";
        currencyTextBox.Value = new Decimal?(0M);
        currencyTextBox.CaretIndex = 1;
        return true;
      case ".":
        if (currencyTextBox.SelectionStart <= 1)
          currencyTextBox.CaretIndex += 2;
        if (currencyTextBox.Text == "" && (text == "." || text == currencyTextBox.CurrencyDecimalSeparator))
        {
          currencyTextBox.MaskedText = $"0{currencyTextBox.CurrencyDecimalSeparator}00";
          currencyTextBox.Value = new Decimal?(Convert.ToDecimal(currencyTextBox.MaskedText));
          currencyTextBox.Select(3, 0);
          break;
        }
        break;
      default:
        if (!(text == currencyTextBox.CurrencyDecimalSeparator.ToString()))
        {
          if (currencyTextBox.minusPressed)
          {
            currencyTextBox.minusPressed = false;
            this.minusKeyValidationFlag = true;
            this.selectedAll = false;
            break;
          }
          break;
        }
        goto case ".";
    }
label_11:
    if (text == "-" || text == "+")
    {
      if (currencyTextBox.mValue.HasValue)
      {
        Decimal num1 = currencyTextBox.mValue.Value;
        Decimal num2;
        if (text == "+")
        {
          Decimal? nullable = currencyTextBox.Value;
          num2 = (!(nullable.GetValueOrDefault() < 1M) ? 0 : (nullable.HasValue ? 1 : 0)) != 0 ? currencyTextBox.mValue.Value * -1M : currencyTextBox.mValue.Value * 1M;
        }
        else
          num2 = currencyTextBox.mValue.Value * -1M;
        if (num2 > currencyTextBox.MaxValue && currencyTextBox.MaxValidation == MaxValidation.OnKeyPress)
        {
          if (!currencyTextBox.MaxValueOnExceedMaxDigit)
            return true;
          num2 = currencyTextBox.MaxValue;
        }
        if (num2 < currencyTextBox.MinValue && currencyTextBox.MinValidation == MinValidation.OnKeyPress)
        {
          if (!currencyTextBox.MinValueOnExceedMinDigit)
            return true;
          num2 = currencyTextBox.MinValue;
        }
        bool flag = false;
        if (currencyTextBox.SelectedText.Length == currencyTextBox.MaskedText.Length)
        {
          flag = true;
          if (text == "-")
          {
            currencyTextBox.minusPressed = true;
            if (currencyTextBox.UseNullOption)
              currencyTextBox.SetValue(new bool?(false), new Decimal?());
            else
              currencyTextBox.Value = new Decimal?(0M);
            currencyTextBox.MaskedText = "-";
            currencyTextBox.CaretIndex = 1;
            return true;
          }
        }
        int selectionStart = currencyTextBox.SelectionStart;
        bool isNegative = currencyTextBox.IsNegative;
        currencyTextBox.MaskedText = num2.ToString("C", (IFormatProvider) this.numberFormat);
        currencyTextBox.SetValue(new bool?(false), new Decimal?(num2));
        if (flag)
        {
          currencyTextBox.SelectAll();
          if (currencyTextBox.UseNullOption)
          {
            if (currencyTextBox.MinValue <= 0M)
            {
              currencyTextBox.SetValue(new bool?(true), new Decimal?());
            }
            else
            {
              currencyTextBox.SetValue(new bool?(true), new Decimal?(currencyTextBox.MinValue));
              currencyTextBox.SelectionStart = 0;
            }
          }
          else
          {
            currencyTextBox.SetValue(new bool?(true), new Decimal?(currencyTextBox.MinValue <= 0M ? 0M : currencyTextBox.MinValue));
            currencyTextBox.SelectionStart = 0;
            this.selectedAll = true;
          }
          return true;
        }
        if (currencyTextBox.MaskedText.Contains("-") || currencyTextBox.MaskedText.Contains("("))
          currencyTextBox.SelectionStart = selectionStart + 1;
        else if (isNegative)
          currencyTextBox.SelectionStart = selectionStart > 0 ? selectionStart - 1 : 0;
      }
      return true;
    }
    if ((text != "-" || text != "+") && currencyTextBox.mValue.HasValue)
      this.selectedAll = currencyTextBox.SelectedText.Length == currencyTextBox.Text.Length;
    if (!(text == this.numberFormat.CurrencyDecimalSeparator) && !(text == this.numberFormat.NumberGroupSeparator) && (!(text == CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator) || currencyTextBox.CurrencyDecimalDigits == 0))
      return false;
    if (currencyTextBox.currencyDecimalDigits == 0)
    {
      if (currencyTextBox.CurrencyDecimalDigits < 0 && currencyTextBox.MinimumCurrencyDecimalDigits <= 0)
      {
        ++currencyTextBox.currencyDecimalDigits;
        currencyTextBox.FormatText();
        if (currencyTextBox.IsNegative && currencyTextBox.Text[0] == '0')
        {
          currencyTextBox.Text = "-" + currencyTextBox.Text;
          currencyTextBox.MaskedText = currencyTextBox.Text;
        }
        currencyTextBox.SelectionStart = currencyTextBox.MaskedText.IndexOf(this.numberFormat.CurrencyDecimalSeparator) + this.numberFormat.CurrencyDecimalSeparator.Length;
      }
    }
    else if ((text == this.numberFormat.CurrencyDecimalSeparator || text == ".") && (currencyTextBox.MaskedText.Contains(this.numberFormat.CurrencyDecimalSeparator) || currencyTextBox.MaskedText.Contains(".")))
      currencyTextBox.SelectionStart = this.separatorEnd;
    if (text == CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator || text == this.numberFormat.CurrencyDecimalSeparator)
    {
      if (currencyTextBox.SelectionStart < currencyTextBox.Text.Length && (currencyTextBox.Text[currencyTextBox.SelectionStart].ToString() == this.numberFormat.NumberGroupSeparator.ToString() || currencyTextBox.Text[currencyTextBox.SelectionStart].ToString() == CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator.ToString()))
      {
        ++currencyTextBox.SelectionStart;
        return true;
      }
    }
    else
      currencyTextBox.SelectionStart = this.separatorEnd;
    return true;
  }

  private bool TextEditingForMatchingMask(CurrencyTextBox currencyTextBox, string text)
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
      this.TrimmingZerosinUnmaskedText();
      this.caretPosition = this.selectionStart;
      this.unmaskedText = this.unmaskedText.Insert(this.selectionStart, text);
      this.caretPosition += text.Length;
    }
    else if (this.selectionStart <= this.separatorStart && this.selectionEnd < this.separatorEnd)
    {
      this.unmaskedText = this.unmaskedText.Remove(this.selectionStart, this.selectionLength);
      this.caretPosition = this.selectionStart;
      this.unmaskedText = this.unmaskedText.Insert(this.selectionStart, text);
      this.caretPosition += text.Length;
    }
    else if (this.selectionStart == this.selectionEnd)
    {
      if (this.selectionStart == this.unmaskedText.Length && !string.IsNullOrEmpty(text))
      {
        if (currencyTextBox.MinimumCurrencyDecimalDigits < 0 && currencyTextBox.MaximumCurrencyDecimalDigits < 0 && currencyTextBox.CurrencyDecimalDigits < 0 || currencyTextBox.MaximumCurrencyDecimalDigits > 0 && currencyTextBox.MaximumCurrencyDecimalDigits > currencyTextBox.currencyDecimalDigits && currencyTextBox.CurrencyDecimalDigits < 0 || currencyTextBox.MinimumCurrencyDecimalDigits >= 0 && currencyTextBox.MaximumCurrencyDecimalDigits < 0 && currencyTextBox.CurrencyDecimalDigits < 0)
        {
          this.unmaskedText = this.unmaskedText.Insert(this.unmaskedText.Length, text[0].ToString());
          if ((int) this.unmaskedText[0] == (int) CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator[0])
            ++this.caretPosition;
        }
        else
        {
          this.unmaskedText = this.unmaskedText.Insert(this.unmaskedText.Length - 1, text[0].ToString());
          this.unmaskedText = this.unmaskedText.Remove(this.unmaskedText.Length - 1, 1);
        }
      }
      else
      {
        if (this.selectionStart == this.unmaskedText.Length || string.IsNullOrEmpty(text))
          return true;
        if (this.unmaskedText[this.unmaskedText.Length - 1] == '0')
          this.unmaskedText = this.unmaskedText.Remove(this.unmaskedText.Length - 1, 1);
        this.unmaskedText = this.unmaskedText.Insert(this.selectionStart, text[0].ToString());
        Decimal? nullable1 = currencyTextBox.Value;
        if ((!(nullable1.GetValueOrDefault() > -1M) ? 0 : (nullable1.HasValue ? 1 : 0)) != 0)
        {
          Decimal? nullable2 = currencyTextBox.Value;
          if ((!(nullable2.GetValueOrDefault() < 1M) ? 0 : (nullable2.HasValue ? 1 : 0)) != 0)
          {
            this.caretPosition = this.unmaskedText.StartsWith(currencyTextBox.CurrencyDecimalSeparator) || this.unmaskedText.StartsWith(this.numberFormat.NumberDecimalSeparator) ? this.selectionStart + 1 : this.selectionStart;
            if (currencyTextBox.IsNegative)
              this.caretPosition = currencyTextBox.SelectionStart - 1;
          }
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
      this.TrimmingZerosinUnmaskedText();
      this.caretPosition = this.unmaskedText.StartsWith(".") ? this.selectionStart + text.Length : this.selectionStart + text.Length - 1;
    }
    this.UpdateCurrencyDecimalDigits(currencyTextBox);
    return false;
  }

  private void UpdateCurrencyDecimalDigits(CurrencyTextBox currencyTextBox)
  {
    if (!currencyTextBox.IsExceedCurrencyDecimalDigits)
      return;
    int count = CurrencyTextBox.CountDecimalDigits(this.unmaskedText, (DependencyObject) currencyTextBox);
    if (count < 100)
      currencyTextBox.UpdateCurrencyDecimalDigits(count);
    if (this.numberFormat == null || this.numberFormat.CurrencyDecimalDigits == currencyTextBox.currencyDecimalDigits)
      return;
    this.numberFormat.CurrencyDecimalDigits = currencyTextBox.currencyDecimalDigits;
  }

  private void TrimmingZerosinUnmaskedText()
  {
    if (!this.unmaskedText.Contains<char>('.'))
      return;
    this.unmaskedText = this.unmaskedText.TrimEnd('0');
  }

  private bool TextEditingForBackspace(CurrencyTextBox currencyTextBox)
  {
    if (this.unmaskedText.Length == this.selectionLength)
    {
      if (currencyTextBox.UseNullOption)
        currencyTextBox.SetValue(new bool?(true), new Decimal?());
      else
        currencyTextBox.SetValue(new bool?(true), new Decimal?(0M));
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
      this.UpdateCurrencyDecimalDigits(currencyTextBox);
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
        if (this.caretPosition == 0)
          this.currencyFlag = 1;
      }
      else if (this.separatorStart < 0)
      {
        this.unmaskedText = this.unmaskedText.Remove(this.selectionStart - 1, 1);
        this.caretPosition = this.selectionStart - 1;
        this.UpdateCurrencyDecimalDigits(currencyTextBox);
      }
      else
      {
        this.unmaskedText = this.unmaskedText.Remove(this.selectionStart, this.selectionLength);
        this.UpdateCurrencyDecimalDigits(currencyTextBox);
        this.caretPosition = this.selectionStart;
      }
    }
    else if (this.selectionStart == this.selectionEnd)
    {
      if (this.selectionStart != this.separatorEnd)
      {
        this.unmaskedText = this.unmaskedText.Remove(this.selectionStart - 1, 1);
        this.UpdateCurrencyDecimalDigits(currencyTextBox);
        this.caretPosition = this.unmaskedText.Length == 0 || char.IsNumber(this.unmaskedText[this.unmaskedText.Length - 1]) || currencyTextBox.currencyDecimalDigits != 0 ? this.selectionStart - 1 : this.selectionStart - 2;
      }
      else
      {
        if (currencyTextBox.SelectionStart > 0)
          --currencyTextBox.SelectionStart;
        return true;
      }
    }
    else
    {
      if (this.selectionLength > 0)
        this.unmaskedText = this.unmaskedText.Remove(this.selectionStart, this.selectionLength);
      this.UpdateCurrencyDecimalDigits(currencyTextBox);
      this.caretPosition = this.selectionStart;
    }
    return false;
  }

  private bool TextEditingForDelete(CurrencyTextBox currencyTextBox)
  {
    if (this.unmaskedText.Length == this.selectionLength)
    {
      if (currencyTextBox.UseNullOption)
      {
        if (currencyTextBox.MinValue <= 0M)
        {
          currencyTextBox.SetValue(new bool?(true), new Decimal?());
        }
        else
        {
          currencyTextBox.SetValue(new bool?(true), new Decimal?(currencyTextBox.MinValue));
          currencyTextBox.SelectionStart = 0;
        }
      }
      else
      {
        currencyTextBox.SetValue(new bool?(true), new Decimal?(currencyTextBox.MinValue <= 0M ? 0M : currencyTextBox.MinValue));
        currencyTextBox.SelectionStart = 0;
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
      this.unmaskedText = this.unmaskedText.Remove(this.selectionStart, this.separatorStart - this.selectionStart);
      this.caretPosition = this.selectionStart;
      this.TrimmingZerosinUnmaskedText();
      this.UpdateCurrencyDecimalDigits(currencyTextBox);
    }
    else if (this.selectionStart <= this.separatorStart && this.selectionEnd < this.separatorEnd)
    {
      if (this.selectionLength == 0)
      {
        if (this.selectionStart != this.separatorStart)
        {
          this.selectionLength = 1;
          this.unmaskedText = this.unmaskedText.Remove(this.selectionStart, this.selectionLength);
          this.caretPosition = this.selectionStart;
          if (this.caretPosition == 0 && (this.maskedText[this.selectionStart].ToString() == this.numberFormat.CurrencySymbol || this.maskedText[this.selectionStart] == '0' && this.maskedText[this.caretPosition + 1].ToString() == this.numberFormat.CurrencyDecimalSeparator))
            this.currencyFlag = 1;
        }
        else
        {
          ++currencyTextBox.SelectionStart;
          return true;
        }
      }
      else
      {
        this.unmaskedText = this.unmaskedText.Remove(this.selectionStart, this.selectionLength);
        this.caretPosition = this.selectionStart;
      }
    }
    else if (this.selectionStart == this.selectionEnd && this.selectionStart < this.separatorEnd)
    {
      if (this.selectionStart == this.unmaskedText.Length)
        return true;
      this.unmaskedText = this.unmaskedText.Remove(this.selectionStart, 1);
      if (this.selectionStart > 0 && this.maskedText[this.selectionStart - 1].ToString() == this.numberFormat.CurrencyDecimalSeparator || (this.maskedText[this.selectionStart + 1].ToString() == this.numberFormat.CurrencyDecimalSeparator || this.maskedText[this.selectionStart].ToString() == this.numberFormat.CurrencyDecimalSeparator) && this.maskedText[0].ToString() == this.numberFormat.CurrencySymbol)
        this.positionFlag = true;
    }
    else if (this.selectionStart == this.selectionEnd && this.selectionStart >= this.separatorEnd)
    {
      if (this.selectionStart == this.unmaskedText.Length)
        return true;
      this.unmaskedText = this.unmaskedText.Remove(this.selectionStart, 1);
      this.unmaskedText = this.unmaskedText.Insert(this.selectionStart, "0");
      this.UpdateCurrencyDecimalDigits(currencyTextBox);
    }
    else
    {
      if (this.selectionLength > 0)
        this.unmaskedText = this.unmaskedText.Remove(this.selectionStart, this.selectionLength);
      this.caretPosition = this.selectionStart + this.selectionLength;
      this.UpdateCurrencyDecimalDigits(currencyTextBox);
    }
    return false;
  }

  private bool ValueValidation(CurrencyTextBox currencyTextBox)
  {
    if (currencyTextBox.UseNullOption && this.previewValue == 0M)
    {
      currencyTextBox.SetValue(new bool?(true), new Decimal?());
      return true;
    }
    if (currencyTextBox.IsNegative)
      this.previewValue *= -1M;
    if (this.previewValue > currencyTextBox.MaxValue && currencyTextBox.MaxValidation == MaxValidation.OnKeyPress)
    {
      if (!currencyTextBox.MaxValueOnExceedMaxDigit)
        return true;
      this.previewValue = currencyTextBox.MaxValue;
    }
    if (this.previewValue < currencyTextBox.MinValue && currencyTextBox.MinValidation == MinValidation.OnKeyPress)
    {
      if (!currencyTextBox.MinValueOnExceedMinDigit)
        return true;
      this.previewValue = currencyTextBox.MinValue;
    }
    currencyTextBox.SetValue(new bool?(false), new Decimal?(this.previewValue));
    currencyTextBox.MaskedText = this.previewValue.ToString("C", (IFormatProvider) this.numberFormat);
    this.maskedText = currencyTextBox.MaskedText;
    return false;
  }

  private bool StringValidationOnKeyPress(
    CurrencyTextBox currencyTextBox,
    bool isBackspaceKeyPressed)
  {
    string message = "";
    bool bIsValidInput = currencyTextBox.ValidationValue == currencyTextBox.Value.ToString();
    string messageBoxText = bIsValidInput ? "String validation succeeded" : "String validation failed";
    if (!bIsValidInput)
    {
      if (currencyTextBox.InvalidValueBehavior == InvalidInputBehavior.DisplayErrorMessage)
      {
        int num = (int) MessageBox.Show(messageBoxText, "Invalid value", MessageBoxButton.OK);
        currencyTextBox.OnValueValidationCompleted(new StringValidationEventArgs(bIsValidInput, message, currencyTextBox.ValidationValue));
        currencyTextBox.OnValidated(EventArgs.Empty);
        return true;
      }
      if (currencyTextBox.InvalidValueBehavior == InvalidInputBehavior.ResetValue)
      {
        currencyTextBox.OnValueValidationCompleted(new StringValidationEventArgs(bIsValidInput, message, currencyTextBox.ValidationValue));
        currencyTextBox.OnValidated(EventArgs.Empty);
        return true;
      }
      if (currencyTextBox.InvalidValueBehavior == InvalidInputBehavior.None)
      {
        currencyTextBox.OnValueValidationCompleted(new StringValidationEventArgs(bIsValidInput, message, currencyTextBox.ValidationValue));
        currencyTextBox.OnValidated(EventArgs.Empty);
        if (isBackspaceKeyPressed)
          return true;
      }
    }
    else
    {
      currencyTextBox.OnValueValidationCompleted(new StringValidationEventArgs(bIsValidInput, message, currencyTextBox.ValidationValue));
      currencyTextBox.OnValidated(EventArgs.Empty);
    }
    return !isBackspaceKeyPressed;
  }
}
