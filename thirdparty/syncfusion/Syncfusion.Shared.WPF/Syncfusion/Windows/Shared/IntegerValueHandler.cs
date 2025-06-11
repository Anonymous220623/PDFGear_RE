// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.IntegerValueHandler
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace Syncfusion.Windows.Shared;

internal class IntegerValueHandler
{
  public static IntegerValueHandler integerValueHandler = new IntegerValueHandler();
  private NumberFormatInfo numberFormat;
  private int selectionStart;
  private int selectionEnd;
  private int selectionLength;
  private int caretPosition;
  private string unmaskedText;
  private string maskedText;
  private bool minusKeyValidationflag;

  public bool MatchWithMask(IntegerTextBox integerTextBox, string text)
  {
    if (integerTextBox.IsReadOnly)
      return true;
    this.InitializeValues(integerTextBox);
    this.unmaskedText = integerTextBox.Text;
    if (this.CharacterValidation(integerTextBox, text, this.minusKeyValidationflag) || this.TextEditingForMatchingMask(integerTextBox, text, this.minusKeyValidationflag) || integerTextBox.OnValidating(new CancelEventArgs(false)) || integerTextBox.ValueValidation != StringValidation.OnKeyPress)
      return true;
    string message = "";
    bool bIsValidInput = integerTextBox.ValidationValue == integerTextBox.Value.ToString();
    string messageBoxText = bIsValidInput ? "String validation succeeded" : "String validation failed";
    if (!bIsValidInput)
    {
      if (integerTextBox.InvalidValueBehavior == InvalidInputBehavior.DisplayErrorMessage)
      {
        int num = (int) MessageBox.Show(messageBoxText, "Invalid value", MessageBoxButton.OK);
        integerTextBox.OnValueValidationCompleted(new StringValidationEventArgs(bIsValidInput, message, integerTextBox.ValidationValue));
        integerTextBox.OnValidated(EventArgs.Empty);
        return true;
      }
      if (integerTextBox.InvalidValueBehavior == InvalidInputBehavior.ResetValue)
      {
        integerTextBox.OnValueValidationCompleted(new StringValidationEventArgs(bIsValidInput, message, integerTextBox.ValidationValue));
        integerTextBox.OnValidated(EventArgs.Empty);
        return true;
      }
      if (integerTextBox.InvalidValueBehavior == InvalidInputBehavior.None)
      {
        integerTextBox.OnValueValidationCompleted(new StringValidationEventArgs(bIsValidInput, message, integerTextBox.ValidationValue));
        integerTextBox.OnValidated(EventArgs.Empty);
      }
    }
    else
    {
      integerTextBox.OnValueValidationCompleted(new StringValidationEventArgs(bIsValidInput, message, integerTextBox.ValidationValue));
      integerTextBox.OnValidated(EventArgs.Empty);
    }
    return true;
  }

  private bool CharacterValidation(
    IntegerTextBox integerTextBox,
    string text,
    bool minusKeyValidationflag)
  {
    if (integerTextBox.mValue.HasValue)
    {
      long? mValue = integerTextBox.mValue;
      if ((mValue.GetValueOrDefault() != 0L ? 0 : (mValue.HasValue ? 1 : 0)) == 0)
        goto label_14;
    }
    if (text == "-")
    {
      integerTextBox.minusPressed = true;
      long result;
      if (long.TryParse(text + (object) 1, out result) && integerTextBox != null && result < integerTextBox.MinValue && integerTextBox.MinValidation == MinValidation.OnKeyPress)
      {
        integerTextBox.Value = new long?(integerTextBox.MinValue);
        return true;
      }
      if (integerTextBox.count % 2 == 0)
        integerTextBox.IsNegative = false;
      else
        integerTextBox.IsNegative = true;
      ++integerTextBox.count;
      integerTextBox.MaskedText = "-";
      if (!integerTextBox.UseNullOption)
        integerTextBox.Value = new long?(0L);
      integerTextBox.CaretIndex = 1;
      integerTextBox.IsNegative = true;
      return true;
    }
    if (integerTextBox.minusPressed)
    {
      integerTextBox.minusPressed = false;
      if (integerTextBox.count % 2 == 0)
        minusKeyValidationflag = true;
    }
label_14:
    if (!(text == "-") && !(text == "+"))
      return false;
    if (integerTextBox.mValue.HasValue)
    {
      long num1;
      if (text == "+")
      {
        long? nullable = integerTextBox.Value;
        num1 = (nullable.GetValueOrDefault() >= 1L ? 0 : (nullable.HasValue ? 1 : 0)) == 0 ? integerTextBox.mValue.Value : integerTextBox.mValue.Value * -1L;
      }
      else
        num1 = integerTextBox.mValue.Value * -1L;
      if (num1 > integerTextBox.MaxValue && integerTextBox.MaxValidation == MaxValidation.OnKeyPress)
      {
        if (!integerTextBox.MaxValueOnExceedMaxDigit)
          return true;
        num1 = integerTextBox.MaxValue;
      }
      if (num1 < integerTextBox.MinValue && integerTextBox.MinValidation == MinValidation.OnKeyPress)
      {
        if (!integerTextBox.MinValueOnExceedMinDigit)
          return true;
        num1 = integerTextBox.MinValue;
      }
      bool flag = false;
      if (integerTextBox.SelectedText.Length == integerTextBox.MaskedText.Length)
      {
        flag = true;
        if (text == "-")
        {
          integerTextBox.minusPressed = true;
          if (integerTextBox.UseNullOption)
            integerTextBox.SetValue(new bool?(false), new long?());
          else
            integerTextBox.Value = new long?(0L);
          integerTextBox.checktext = "-";
          integerTextBox.MaskedText = "-";
          integerTextBox.CaretIndex = 1;
          return true;
        }
      }
      if (integerTextBox.MaskedText.Length == integerTextBox.MaxLength)
        integerTextBox.MaskedText = integerTextBox.MaskedText;
      int selectionStart = integerTextBox.SelectionStart;
      integerTextBox.MaskedText = num1.ToString("N", (IFormatProvider) this.numberFormat);
      if (text != "+")
      {
        int num2;
        int num3;
        if (!integerTextBox.MaskedText.Contains("-"))
          num3 = num2 = selectionStart - 1;
        else
          num2 = num3 = selectionStart + 1;
        int num4 = num3;
        if (num4 < 0)
          num4 = 0;
        integerTextBox.SelectionStart = num4;
      }
      integerTextBox.SetValue(new bool?(false), new long?(num1));
      if (flag)
        integerTextBox.SelectAll();
    }
    integerTextBox.SelectionLength = 0;
    return true;
  }

  public bool HandleKeyDown(IntegerTextBox integerTextBox, KeyEventArgs eventArgs)
  {
    if (eventArgs.Key == Key.Space)
      return true;
    switch (eventArgs.Key)
    {
      case Key.Back:
        integerTextBox.count = 1;
        return this.HandleBackSpaceKey(integerTextBox);
      case Key.Up:
        return this.HandleUpDownKey(integerTextBox, true);
      case Key.Down:
        return this.HandleUpDownKey(integerTextBox, false);
      case Key.Delete:
        integerTextBox.count = 1;
        return this.HandleDeleteKey(integerTextBox);
      default:
        return false;
    }
  }

  private void InitializeValues(IntegerTextBox integerTextBox)
  {
    this.numberFormat = integerTextBox.GetCulture().NumberFormat;
    this.selectionStart = 0;
    this.selectionEnd = 0;
    this.selectionLength = 0;
    this.caretPosition = 0;
    this.unmaskedText = "";
    this.maskedText = integerTextBox.MaskedText;
  }

  private void GenerateUnmaskedText(IntegerTextBox integerTextBox)
  {
    for (int index = 0; index <= this.maskedText.Length; ++index)
    {
      if (index == integerTextBox.SelectionStart)
      {
        this.selectionStart = this.unmaskedText.Length;
        this.caretPosition = this.unmaskedText.Length;
      }
      if (index == integerTextBox.SelectionStart + integerTextBox.SelectionLength)
        this.selectionEnd = this.unmaskedText.Length;
      if (index < this.maskedText.Length)
      {
        if (!char.IsDigit(this.maskedText[index]))
        {
          if (this.maskedText[index] == '-')
          {
            if (integerTextBox.Value.HasValue)
            {
              long? nullable = integerTextBox.Value;
              if ((nullable.GetValueOrDefault() != 0L ? 0 : (nullable.HasValue ? 1 : 0)) == 0)
                continue;
            }
          }
          else
            continue;
        }
        this.unmaskedText += (string) (object) this.maskedText[index];
      }
    }
    this.selectionLength = this.selectionEnd - this.selectionStart;
  }

  private bool TextEditingforBackSpace(IntegerTextBox integerTextBox)
  {
    if (this.selectionLength != 0)
    {
      this.unmaskedText = this.unmaskedText.Remove(this.selectionStart, this.selectionLength);
    }
    else
    {
      if (this.selectionStart == 0)
        return true;
      this.selectionLength = 1;
      this.unmaskedText = this.unmaskedText.Remove(this.selectionStart - 1, this.selectionLength);
    }
    this.selectionStart -= this.selectionLength;
    if (this.unmaskedText.Length != 0)
      return false;
    if (integerTextBox.UseNullOption)
      integerTextBox.SetValue(new bool?(true), new long?());
    else if (this.unmaskedText == "")
    {
      if (integerTextBox.MinValue.ToString() == "0" || integerTextBox.MinValue < 0L)
      {
        integerTextBox.MaskedText = "0";
        integerTextBox.Value = new long?(0L);
      }
      else
        integerTextBox.SetValue(new bool?(true), new long?(integerTextBox.MinValue));
    }
    return true;
  }

  private bool TextEditingForMatchingMask(
    IntegerTextBox integerTextBox,
    string text,
    bool minusKeyValidationflag)
  {
    if (this.unmaskedText != "")
    {
      if (this.unmaskedText.Length == 1)
      {
        if (this.unmaskedText == "0")
        {
          this.unmaskedText = text;
          goto label_13;
        }
      }
      else if (this.unmaskedText[0] == '0' && this.unmaskedText.Length == 1)
      {
        this.unmaskedText = text + this.unmaskedText;
        goto label_13;
      }
    }
    this.unmaskedText = "";
    this.GenerateUnmaskedText(integerTextBox);
    if (this.unmaskedText.Length <= 0 || this.unmaskedText[0] != '0' || this.selectionStart != 0 || this.selectionLength != 0)
      this.unmaskedText = this.unmaskedText.Remove(this.selectionStart, this.selectionLength);
    if (text != string.Empty && char.IsDigit(text[0]))
    {
      if (this.selectionStart == 0 && text == "0")
      {
        if (integerTextBox.Text.Length == this.selectionLength || this.unmaskedText == "")
          this.unmaskedText = this.unmaskedText.Insert(this.selectionStart, text);
      }
      else
        this.unmaskedText = this.unmaskedText.Insert(this.selectionStart, text);
    }
label_13:
    if (text != string.Empty && char.IsDigit(text[0]))
    {
      if (this.unmaskedText.Contains("-"))
        this.unmaskedText = this.unmaskedText.Remove(0, 1);
      long result;
      if (!long.TryParse(this.unmaskedText, NumberStyles.Number, (IFormatProvider) this.numberFormat, out result))
        return true;
      if (integerTextBox.MaxValidation == MaxValidation.OnLostFocus && integerTextBox.SelectedText != "")
      {
        long? mValue = integerTextBox.mValue;
        if ((mValue.GetValueOrDefault() != 0L ? 1 : (!mValue.HasValue ? 1 : 0)) != 0 && !integerTextBox.IsNegative)
        {
          result = result;
          goto label_25;
        }
      }
      if (integerTextBox.IsNegative || minusKeyValidationflag)
      {
        if (!(integerTextBox.SelectedText == integerTextBox.Text.ToString()) && !integerTextBox.SelectedText.Contains("-"))
        {
          result *= -1L;
        }
        else
        {
          long? nullable = integerTextBox.Value;
          if (((nullable.GetValueOrDefault() != 0L ? 0 : (nullable.HasValue ? 1 : 0)) != 0 || integerTextBox.IsNull) && minusKeyValidationflag)
            result *= -1L;
        }
      }
label_25:
      if (result > integerTextBox.MaxValue && integerTextBox.MaxValidation == MaxValidation.OnKeyPress)
      {
        if (!integerTextBox.MaxValueOnExceedMaxDigit)
          return true;
        result = integerTextBox.MaxValue;
      }
      if (result <= integerTextBox.MinValue && integerTextBox.MinValidation == MinValidation.OnKeyPress)
      {
        if (result <= integerTextBox.MinValue && integerTextBox.MinValue >= 0L)
        {
          if (this.numberFormat != null)
          {
            if (integerTextBox.UseNullOption)
              this.unmaskedText = result.ToString("N", (IFormatProvider) this.numberFormat);
            if (this.unmaskedText.Length - (this.numberFormat.NumberDecimalDigits + 1) >= integerTextBox.MinValue.ToString().Length)
              result = integerTextBox.MinValue;
            else if (this.unmaskedText.Length - (this.numberFormat.NumberDecimalDigits + 1) <= integerTextBox.MinValue.ToString().Length)
            {
              integerTextBox.checktext += text;
              if ((long) int.Parse(integerTextBox.checktext) >= integerTextBox.MinValue)
              {
                integerTextBox.Value = new long?((long) int.Parse(integerTextBox.checktext));
                integerTextBox.CaretIndex = integerTextBox.Value.ToString().Length;
                integerTextBox.checktext = "";
              }
              return true;
            }
          }
        }
        else if (result > integerTextBox.MinValue)
          integerTextBox.MaskedText = this.unmaskedText;
        else if (result >= integerTextBox.MinValue)
        {
          if (integerTextBox.MinValueOnExceedMinDigit && this.unmaskedText.Length - (this.numberFormat.NumberDecimalDigits + 1) > integerTextBox.MinValue.ToString().Length)
          {
            result = integerTextBox.MinValue;
          }
          else
          {
            if (this.unmaskedText.Length - (this.numberFormat.NumberDecimalDigits + 1) > integerTextBox.MinValue.ToString().Length)
              return true;
            integerTextBox.MaskedText = this.unmaskedText;
          }
        }
        else
        {
          if (!integerTextBox.MinValueOnExceedMinDigit)
            return true;
          result = integerTextBox.MinValue;
        }
      }
      if (integerTextBox.MaxLength != 0 && this.unmaskedText.Length > integerTextBox.MaxLength)
      {
        this.caretPosition = integerTextBox.CaretIndex;
        result = result >= 0L ? long.Parse(this.unmaskedText.Remove(integerTextBox.MaxLength)) : long.Parse(this.unmaskedText.Remove(integerTextBox.MaxLength)) * -1L;
        integerTextBox.SetValue(new bool?(false), new long?(result));
        integerTextBox.MaskedText = result.ToString("N", (IFormatProvider) this.numberFormat);
        ++this.caretPosition;
        integerTextBox.CaretIndex = this.caretPosition;
        return true;
      }
      if (integerTextBox.checktext != "" && result >= integerTextBox.MinValue)
        ++this.selectionStart;
      if (result.ToString().Contains("-"))
        integerTextBox.checktext = "";
      result = long.Parse(integerTextBox.checktext + result.ToString());
      integerTextBox.SetValue(new bool?(true), new long?(result));
      integerTextBox.MaskedText = integerTextBox.Value.Value.ToString("N", (IFormatProvider) this.numberFormat);
      integerTextBox.checktext = "";
      if (this.unmaskedText != "")
      {
        if (this.unmaskedText[0] == '-')
        {
          long? oldValue = integerTextBox.OldValue;
          if ((oldValue.GetValueOrDefault() != 0L ? 0 : (oldValue.HasValue ? 1 : 0)) != 0)
          {
            integerTextBox.MaskedText = this.unmaskedText;
            this.maskedText = integerTextBox.MaskedText;
            integerTextBox.SelectionStart = 2;
            return true;
          }
        }
        if (this.unmaskedText[0] == '0')
        {
          long? nullable = integerTextBox.Value;
          if ((nullable.GetValueOrDefault() >= 0L ? 0 : (nullable.HasValue ? 1 : 0)) != 0)
          {
            char ch = '-';
            integerTextBox.MaskedText = ch.ToString() + this.unmaskedText;
          }
          else
            integerTextBox.MaskedText = this.unmaskedText;
        }
        this.maskedText = integerTextBox.MaskedText;
      }
      int num1 = -1;
      int num2 = 0;
      bool flag1 = false;
      bool flag2 = false;
      int num3 = 0;
      for (int index = 0; index < this.maskedText.Length; ++index)
      {
        if (char.IsDigit(this.maskedText[index]))
          ++num1;
        if (this.maskedText.Contains("-") && this.maskedText.Length == 2 && !flag1)
        {
          ++num1;
          flag1 = true;
          num3 = 2;
        }
        if (num1 == this.selectionStart)
        {
          num2 = index;
          flag2 = true;
        }
        if (flag2 && char.IsDigit(this.maskedText[index]))
          ++num3;
        if (flag2 && num3 == text.Length)
          break;
      }
      integerTextBox.SelectionStart = num2 + num3;
      integerTextBox.SelectionLength = 0;
    }
    return false;
  }

  public bool HandleBackSpaceKey(IntegerTextBox integerTextBox)
  {
    if (integerTextBox.IsReadOnly)
      return true;
    this.InitializeValues(integerTextBox);
    if (integerTextBox.SelectionLength == 1)
    {
      if (!char.IsDigit(this.maskedText[integerTextBox.SelectionStart]))
      {
        if (this.maskedText[integerTextBox.SelectionStart] == '-')
        {
          IntegerTextBox integerTextBox1 = integerTextBox;
          long? nullable1 = integerTextBox.Value;
          long? nullable2 = nullable1.HasValue ? new long?(nullable1.GetValueOrDefault() * -1L) : new long?();
          integerTextBox1.Value = nullable2;
        }
        integerTextBox.SelectionLength = 0;
        return true;
      }
    }
    else if (integerTextBox.SelectionLength == 0 && integerTextBox.SelectionStart != 0 && !char.IsDigit(this.maskedText[integerTextBox.SelectionStart - 1]))
    {
      if (this.maskedText[0] == '-')
      {
        IntegerTextBox integerTextBox2 = integerTextBox;
        long? mValue = integerTextBox.mValue;
        long? nullable3 = mValue.HasValue ? new long?(-1L * mValue.GetValueOrDefault()) : new long?();
        integerTextBox2.Value = nullable3;
        integerTextBox.SelectionLength = 0;
        long? nullable4 = integerTextBox.Value;
        if ((nullable4.GetValueOrDefault() != 0L ? 0 : (nullable4.HasValue ? 1 : 0)) != 0)
        {
          this.maskedText = this.maskedText.Remove(0, 1);
          integerTextBox.MaskedText = this.maskedText;
        }
        return true;
      }
      --integerTextBox.SelectionStart;
      IntegerTextBox integerTextBox3 = integerTextBox;
      long? mValue1 = integerTextBox.mValue;
      long? nullable = mValue1.HasValue ? new long?(mValue1.GetValueOrDefault()) : new long?();
      integerTextBox3.Value = nullable;
      integerTextBox.SelectionLength = 0;
      return true;
    }
    this.GenerateUnmaskedText(integerTextBox);
    if (this.TextEditingforBackSpace(integerTextBox))
      return true;
    long result;
    if (long.TryParse(this.unmaskedText, NumberStyles.Number, (IFormatProvider) this.numberFormat, out result))
    {
      if (integerTextBox.IsNegative)
        result = result;
      if (result > integerTextBox.MaxValue && integerTextBox.MaxValidation == MaxValidation.OnKeyPress)
      {
        if (!integerTextBox.MaxValueOnExceedMaxDigit)
          return true;
        result = integerTextBox.MaxValue;
      }
      if (result < integerTextBox.MinValue && integerTextBox.MinValidation == MinValidation.OnKeyPress)
      {
        if (!integerTextBox.MinValueOnExceedMinDigit)
          return true;
        result = integerTextBox.MinValue;
      }
      integerTextBox.SetValue(new bool?(false), new long?(result));
      integerTextBox.MaskedText = result.ToString("N", (IFormatProvider) this.numberFormat);
      this.maskedText = integerTextBox.MaskedText;
      integerTextBox.CaretIndex = this.selectionStart + this.selectionLength;
      if (integerTextBox.MaskedText.Contains(integerTextBox.NumberGroupSeparator))
      {
        int num = 0;
        foreach (char ch in integerTextBox.MaskedText)
        {
          if (integerTextBox.NumberFormat != null)
          {
            if (ch.ToString() == integerTextBox.NumberFormat.NumberGroupSeparator)
              ++num;
          }
          else if (ch.ToString() == integerTextBox.NumberGroupSeparator || ch == ',')
            ++num;
          else if (ch.ToString() == integerTextBox.NumberGroupSeparator || ch == ' ')
            ++num;
        }
        integerTextBox.CaretIndex += num;
      }
      int num1 = -1;
      int index;
      for (index = 0; index < this.maskedText.Length; ++index)
      {
        if (char.IsDigit(this.maskedText[index]))
          ++num1;
        if (num1 == this.selectionStart)
          break;
      }
      if (this.unmaskedText[0] == '-')
      {
        if (this.unmaskedText.Length == this.selectionStart)
          integerTextBox.SelectionStart = index;
        else
          integerTextBox.SelectionStart = index - 1;
      }
      else
        integerTextBox.SelectionStart = index;
      integerTextBox.SelectionLength = 0;
      if (integerTextBox.OnValidating(new CancelEventArgs(false)) || integerTextBox.ValueValidation != StringValidation.OnKeyPress)
        return true;
      string message = "";
      bool bIsValidInput = integerTextBox.ValidationValue == integerTextBox.Value.ToString();
      string messageBoxText = bIsValidInput ? "String validation succeeded" : "String validation failed";
      if (!bIsValidInput)
      {
        if (integerTextBox.InvalidValueBehavior == InvalidInputBehavior.DisplayErrorMessage)
        {
          int num2 = (int) MessageBox.Show(messageBoxText, "Invalid value", MessageBoxButton.OK);
          integerTextBox.OnValueValidationCompleted(new StringValidationEventArgs(bIsValidInput, message, integerTextBox.ValidationValue));
          integerTextBox.OnValidated(EventArgs.Empty);
          return true;
        }
        if (integerTextBox.InvalidValueBehavior == InvalidInputBehavior.ResetValue)
        {
          integerTextBox.OnValueValidationCompleted(new StringValidationEventArgs(bIsValidInput, message, integerTextBox.ValidationValue));
          integerTextBox.OnValidated(EventArgs.Empty);
          return true;
        }
        if (integerTextBox.InvalidValueBehavior == InvalidInputBehavior.None)
        {
          integerTextBox.OnValueValidationCompleted(new StringValidationEventArgs(bIsValidInput, message, integerTextBox.ValidationValue));
          integerTextBox.OnValidated(EventArgs.Empty);
          return true;
        }
      }
      else
      {
        integerTextBox.OnValueValidationCompleted(new StringValidationEventArgs(bIsValidInput, message, integerTextBox.ValidationValue));
        integerTextBox.OnValidated(EventArgs.Empty);
      }
      return false;
    }
    if (this.unmaskedText == "-")
    {
      if (integerTextBox.UseNullOption)
      {
        integerTextBox.MaskedText = "";
        integerTextBox.SetValue(new bool?(true), new long?());
      }
      else
      {
        integerTextBox.MaskedText = "0";
        integerTextBox.Value = new long?(0L);
      }
    }
    return true;
  }

  private bool TextEditingforDelete(IntegerTextBox integerTextBox)
  {
    if (this.selectionLength != 0)
    {
      this.unmaskedText = this.unmaskedText.Remove(this.selectionStart, this.selectionLength);
    }
    else
    {
      if (this.selectionStart == this.unmaskedText.Length)
        return true;
      this.selectionLength = 1;
      this.unmaskedText = this.unmaskedText.Remove(this.selectionStart, this.selectionLength);
    }
    if (this.unmaskedText.Length != 0)
      return false;
    if (integerTextBox.UseNullOption)
    {
      integerTextBox.MaskedText = "";
      integerTextBox.SetValue(new bool?(true), new long?());
    }
    else if (this.unmaskedText == "")
    {
      if ((integerTextBox.MinValue.ToString() == "0" || integerTextBox.MinValue < 0L) && integerTextBox.MinValue != integerTextBox.MaxValue)
      {
        integerTextBox.MaskedText = "0";
        integerTextBox.Value = new long?(0L);
      }
      else
        integerTextBox.SetValue(new bool?(true), new long?(integerTextBox.MinValue));
    }
    return true;
  }

  public bool HandleDeleteKey(IntegerTextBox integerTextBox)
  {
    if (integerTextBox.IsReadOnly)
      return true;
    this.InitializeValues(integerTextBox);
    if (integerTextBox.SelectionLength <= 1 && integerTextBox.SelectionStart != this.maskedText.Length && !char.IsDigit(this.maskedText[integerTextBox.SelectionStart]))
    {
      if (this.maskedText[0] == '-')
      {
        IntegerTextBox integerTextBox1 = integerTextBox;
        long? mValue = integerTextBox.mValue;
        long? nullable1 = mValue.HasValue ? new long?(-1L * mValue.GetValueOrDefault()) : new long?();
        integerTextBox1.Value = nullable1;
        integerTextBox.SelectionLength = 0;
        long? nullable2 = integerTextBox.Value;
        if ((nullable2.GetValueOrDefault() != 0L ? 0 : (nullable2.HasValue ? 1 : 0)) != 0)
        {
          this.maskedText = this.maskedText.Remove(0, 1);
          integerTextBox.MaskedText = this.maskedText;
        }
        return true;
      }
      ++integerTextBox.SelectionStart;
      IntegerTextBox integerTextBox2 = integerTextBox;
      long? mValue1 = integerTextBox.mValue;
      long? nullable = mValue1.HasValue ? new long?(mValue1.GetValueOrDefault()) : new long?();
      integerTextBox2.Value = nullable;
      integerTextBox.SelectionLength = 0;
      return true;
    }
    this.GenerateUnmaskedText(integerTextBox);
    if (this.TextEditingforDelete(integerTextBox))
      return true;
    long result;
    if (long.TryParse(this.unmaskedText, NumberStyles.Number, (IFormatProvider) this.numberFormat, out result))
    {
      if (integerTextBox.IsNegative)
        result = result;
      if (result > integerTextBox.MaxValue && integerTextBox.MaxValidation == MaxValidation.OnKeyPress)
      {
        if (!integerTextBox.MaxValueOnExceedMaxDigit)
          return true;
        result = integerTextBox.MaxValue;
      }
      if (result < integerTextBox.MinValue && integerTextBox.MinValidation == MinValidation.OnKeyPress)
      {
        if (!integerTextBox.MinValueOnExceedMinDigit)
          return true;
        result = integerTextBox.MinValue;
      }
      integerTextBox.SetValue(new bool?(false), new long?(result));
      long? nullable = integerTextBox.Value;
      if ((nullable.GetValueOrDefault() != 0L ? 0 : (nullable.HasValue ? 1 : 0)) != 0)
      {
        if (integerTextBox.UseNullOption)
        {
          integerTextBox.Value = new long?(0L);
          integerTextBox.MaskedText = "";
        }
        else
          integerTextBox.MaskedText = this.unmaskedText;
      }
      integerTextBox.MaskedText = result.ToString("N", (IFormatProvider) this.numberFormat);
      this.maskedText = integerTextBox.MaskedText;
      int num1 = -1;
      int index;
      for (index = 0; index < this.maskedText.Length; ++index)
      {
        if (char.IsDigit(this.maskedText[index]))
          ++num1;
        if (num1 == this.selectionStart)
          break;
      }
      if (this.unmaskedText.Length == this.selectionStart)
        integerTextBox.SelectionStart = index;
      else if (this.unmaskedText[0] == '-')
        integerTextBox.SelectionStart = index - 1;
      else
        integerTextBox.SelectionStart = index;
      integerTextBox.SelectionLength = 0;
      if (integerTextBox.OnValidating(new CancelEventArgs(false)) || integerTextBox.ValueValidation != StringValidation.OnKeyPress)
        return true;
      string message = "";
      bool bIsValidInput = integerTextBox.ValidationValue == integerTextBox.Value.ToString();
      string messageBoxText = bIsValidInput ? "String validation succeeded" : "String validation failed";
      if (!bIsValidInput)
      {
        if (integerTextBox.InvalidValueBehavior == InvalidInputBehavior.DisplayErrorMessage)
        {
          int num2 = (int) MessageBox.Show(messageBoxText, "Invalid value", MessageBoxButton.OK);
          integerTextBox.OnValueValidationCompleted(new StringValidationEventArgs(bIsValidInput, message, integerTextBox.ValidationValue));
          integerTextBox.OnValidated(EventArgs.Empty);
          return true;
        }
        if (integerTextBox.InvalidValueBehavior == InvalidInputBehavior.ResetValue)
        {
          integerTextBox.OnValueValidationCompleted(new StringValidationEventArgs(bIsValidInput, message, integerTextBox.ValidationValue));
          integerTextBox.OnValidated(EventArgs.Empty);
          return true;
        }
        if (integerTextBox.InvalidValueBehavior == InvalidInputBehavior.None)
        {
          integerTextBox.OnValueValidationCompleted(new StringValidationEventArgs(bIsValidInput, message, integerTextBox.ValidationValue));
          integerTextBox.OnValidated(EventArgs.Empty);
        }
      }
      else
      {
        integerTextBox.OnValueValidationCompleted(new StringValidationEventArgs(bIsValidInput, message, integerTextBox.ValidationValue));
        integerTextBox.OnValidated(EventArgs.Empty);
      }
      return true;
    }
    if (this.unmaskedText == "-")
    {
      if (integerTextBox.UseNullOption)
        integerTextBox.SetValue(new bool?(true), new long?());
      else
        integerTextBox.MaskedText = "0";
    }
    return true;
  }

  public bool HandleUpDownKey(IntegerTextBox integerTextBox, bool isUpKeyPressed)
  {
    if (!isUpKeyPressed)
    {
      if (integerTextBox.IsReadOnly && !integerTextBox.IsScrollingOnCircle)
        return true;
      if (integerTextBox.IsReadOnly || !integerTextBox.IsScrollingOnCircle)
        return false;
      if (integerTextBox.mValue.HasValue)
      {
        long? mValue1 = integerTextBox.mValue;
        long scrollInterval1 = (long) integerTextBox.ScrollInterval;
        long? nullable1 = mValue1.HasValue ? new long?(mValue1.GetValueOrDefault() - scrollInterval1) : new long?();
        long minValue = integerTextBox.MinValue;
        if ((nullable1.GetValueOrDefault() >= minValue ? 0 : (nullable1.HasValue ? 1 : 0)) != 0 && integerTextBox.MinValidation != MinValidation.OnLostFocus)
        {
          if (integerTextBox.ScrollInterval < 0)
          {
            IntegerTextBox integerTextBox1 = integerTextBox;
            bool? IsReload = new bool?(true);
            long? mValue2 = integerTextBox.mValue;
            long scrollInterval2 = (long) integerTextBox.ScrollInterval;
            long? nullable2 = mValue2.HasValue ? new long?(mValue2.GetValueOrDefault() - scrollInterval2) : new long?();
            integerTextBox1.SetValue(IsReload, nullable2);
          }
          return true;
        }
        if (integerTextBox.IsNegative)
        {
          if (integerTextBox.MaxLength != 0)
          {
            long? mValue3 = integerTextBox.mValue;
            long scrollInterval3 = (long) integerTextBox.ScrollInterval;
            if ((mValue3.HasValue ? new long?(mValue3.GetValueOrDefault() - scrollInterval3) : new long?()).ToString().Length > integerTextBox.MaxLength + 1)
              goto label_31;
          }
          IntegerTextBox integerTextBox2 = integerTextBox;
          bool? IsReload = new bool?(true);
          long? mValue4 = integerTextBox.mValue;
          long scrollInterval4 = (long) integerTextBox.ScrollInterval;
          long? nullable3 = mValue4.HasValue ? new long?(mValue4.GetValueOrDefault() - scrollInterval4) : new long?();
          integerTextBox2.SetValue(IsReload, nullable3);
        }
        else
        {
          if (integerTextBox.MaxLength != 0)
          {
            long? mValue5 = integerTextBox.mValue;
            long scrollInterval5 = (long) integerTextBox.ScrollInterval;
            if ((mValue5.HasValue ? new long?(mValue5.GetValueOrDefault() - scrollInterval5) : new long?()).ToString().Length > integerTextBox.MaxLength)
              goto label_31;
          }
          IntegerTextBox integerTextBox3 = integerTextBox;
          bool? IsReload = new bool?(true);
          long? mValue6 = integerTextBox.mValue;
          long scrollInterval6 = (long) integerTextBox.ScrollInterval;
          long? nullable4 = mValue6.HasValue ? new long?(mValue6.GetValueOrDefault() - scrollInterval6) : new long?();
          integerTextBox3.SetValue(IsReload, nullable4);
        }
      }
    }
    else
    {
      if (integerTextBox.IsReadOnly || !integerTextBox.IsScrollingOnCircle)
        return true;
      if (integerTextBox.mValue.HasValue)
      {
        long? mValue7 = integerTextBox.mValue;
        long? nullable5 = mValue7.HasValue ? new long?(mValue7.GetValueOrDefault() + 1L) : new long?();
        long maxValue = integerTextBox.MaxValue;
        if ((nullable5.GetValueOrDefault() <= maxValue ? 0 : (nullable5.HasValue ? 1 : 0)) != 0 && integerTextBox.MaxValidation != MaxValidation.OnLostFocus)
        {
          if (integerTextBox.ScrollInterval < 0)
          {
            IntegerTextBox integerTextBox4 = integerTextBox;
            bool? IsReload = new bool?(true);
            long? mValue8 = integerTextBox.mValue;
            long scrollInterval = (long) integerTextBox.ScrollInterval;
            long? nullable6 = mValue8.HasValue ? new long?(mValue8.GetValueOrDefault() + scrollInterval) : new long?();
            integerTextBox4.SetValue(IsReload, nullable6);
          }
          return true;
        }
        if (integerTextBox.IsNegative)
        {
          if (integerTextBox.MaxLength != 0)
          {
            long? mValue9 = integerTextBox.mValue;
            long scrollInterval = (long) integerTextBox.ScrollInterval;
            if ((mValue9.HasValue ? new long?(mValue9.GetValueOrDefault() + scrollInterval) : new long?()).ToString().Length > integerTextBox.MaxLength + 1)
              goto label_31;
          }
          IntegerTextBox integerTextBox5 = integerTextBox;
          bool? IsReload = new bool?(true);
          long? mValue10 = integerTextBox.mValue;
          long scrollInterval7 = (long) integerTextBox.ScrollInterval;
          long? nullable7 = mValue10.HasValue ? new long?(mValue10.GetValueOrDefault() + scrollInterval7) : new long?();
          integerTextBox5.SetValue(IsReload, nullable7);
        }
        else
        {
          if (integerTextBox.MaxLength != 0)
          {
            long? mValue11 = integerTextBox.mValue;
            long scrollInterval = (long) integerTextBox.ScrollInterval;
            if ((mValue11.HasValue ? new long?(mValue11.GetValueOrDefault() + scrollInterval) : new long?()).ToString().Length > integerTextBox.MaxLength)
              goto label_31;
          }
          IntegerTextBox integerTextBox6 = integerTextBox;
          bool? IsReload = new bool?(true);
          long? mValue12 = integerTextBox.mValue;
          long scrollInterval8 = (long) integerTextBox.ScrollInterval;
          long? nullable8 = mValue12.HasValue ? new long?(mValue12.GetValueOrDefault() + scrollInterval8) : new long?();
          integerTextBox6.SetValue(IsReload, nullable8);
        }
      }
    }
label_31:
    if (integerTextBox.OnValidating(new CancelEventArgs(false)) || integerTextBox.ValueValidation != StringValidation.OnKeyPress)
      return true;
    string message = "";
    bool bIsValidInput = integerTextBox.ValidationValue == integerTextBox.Value.ToString();
    string messageBoxText = bIsValidInput ? "String validation succeeded" : "String validation failed";
    if (!bIsValidInput)
    {
      if (integerTextBox.InvalidValueBehavior == InvalidInputBehavior.DisplayErrorMessage)
      {
        int num = (int) MessageBox.Show(messageBoxText, "Invalid value", MessageBoxButton.OK);
        integerTextBox.OnValueValidationCompleted(new StringValidationEventArgs(bIsValidInput, message, integerTextBox.ValidationValue));
        integerTextBox.OnValidated(EventArgs.Empty);
        return true;
      }
      if (integerTextBox.InvalidValueBehavior == InvalidInputBehavior.ResetValue)
      {
        integerTextBox.OnValueValidationCompleted(new StringValidationEventArgs(bIsValidInput, message, integerTextBox.ValidationValue));
        integerTextBox.OnValidated(EventArgs.Empty);
        return true;
      }
      if (integerTextBox.InvalidValueBehavior == InvalidInputBehavior.None)
      {
        integerTextBox.OnValueValidationCompleted(new StringValidationEventArgs(bIsValidInput, message, integerTextBox.ValidationValue));
        integerTextBox.OnValidated(EventArgs.Empty);
      }
    }
    else
    {
      integerTextBox.OnValueValidationCompleted(new StringValidationEventArgs(bIsValidInput, message, integerTextBox.ValidationValue));
      integerTextBox.OnValidated(EventArgs.Empty);
    }
    return true;
  }
}
