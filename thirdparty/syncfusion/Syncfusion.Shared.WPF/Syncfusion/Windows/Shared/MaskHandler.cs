// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.MaskHandler
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace Syncfusion.Windows.Shared;

internal class MaskHandler
{
  public static MaskHandler maskHandler = new MaskHandler();

  public bool MatchWithMask(MaskedTextBox maskedTextBox, string text)
  {
    if (maskedTextBox.IsReadOnly)
      return true;
    if (!string.IsNullOrEmpty(maskedTextBox.Mask))
    {
      if (maskedTextBox.OnValidating(new CancelEventArgs(false)))
        return true;
      int selectionStart = maskedTextBox.SelectionStart;
      if (selectionStart == maskedTextBox.Text.Length)
        return true;
      for (; selectionStart < maskedTextBox.Text.Length; ++selectionStart)
      {
        if (maskedTextBox.CharCollection != null && maskedTextBox.CharCollection[selectionStart].IsPromptCharacter.HasValue)
        {
          string regExpression = maskedTextBox.CharCollection[selectionStart].RegExpression;
          int num;
          if (regExpression != null && Regex.IsMatch(text, regExpression))
          {
            string str1 = maskedTextBox.MaskedText;
            if (maskedTextBox.CharCollection[selectionStart].IsUpper.HasValue)
            {
              bool? isUpper = maskedTextBox.CharCollection[selectionStart].IsUpper;
              text = (!isUpper.GetValueOrDefault() ? 0 : (isUpper.HasValue ? 1 : 0)) != 0 ? text.ToUpper() : text.ToLower();
            }
            if (str1.Length > 0)
              str1 = str1.Remove(selectionStart, 1);
            string str2 = str1.Insert(selectionStart, text);
            maskedTextBox.MaskedText = str2;
            maskedTextBox.SelectionStart = selectionStart + 1;
            maskedTextBox.CharCollection[selectionStart].IsPromptCharacter = new bool?(false);
            num = selectionStart + 1;
            return true;
          }
          num = selectionStart + 1;
          return true;
        }
        maskedTextBox.SelectionStart = selectionStart + 1;
      }
      maskedTextBox.OnValidated(EventArgs.Empty);
      return true;
    }
    if (maskedTextBox.OnValidating(new CancelEventArgs(false)))
      return true;
    string maskedText = maskedTextBox.MaskedText;
    string input;
    if (maskedText.Length == maskedTextBox.SelectionStart)
      input = maskedText + text;
    else if (maskedTextBox.SelectionStart == 0)
    {
      input = text;
    }
    else
    {
      StringBuilder stringBuilder = new StringBuilder(maskedText);
      if (!string.IsNullOrEmpty(maskedTextBox.SelectedText))
        stringBuilder.Remove(maskedTextBox.SelectionStart, maskedTextBox.SelectedText.Length);
      stringBuilder.Insert(maskedTextBox.SelectionStart, text);
      input = stringBuilder.ToString();
    }
    if (maskedTextBox.StringValidation != StringValidation.OnKeyPress || maskedTextBox.OnValidating(new CancelEventArgs(false)))
      return false;
    string message = "";
    bool bIsValidInput = Regex.IsMatch(input, maskedTextBox.ValidationString);
    string messageBoxText = bIsValidInput ? "String validation succeeded" : "String validation failed";
    if (!bIsValidInput)
    {
      switch (maskedTextBox.InvalidValueBehavior)
      {
        case InvalidInputBehavior.None:
          maskedTextBox.OnStringValidationCompleted(new StringValidationEventArgs(bIsValidInput, message, maskedTextBox.ValidationString));
          maskedTextBox.OnValidated(EventArgs.Empty);
          break;
        case InvalidInputBehavior.DisplayErrorMessage:
          int num1 = (int) MessageBox.Show(messageBoxText, "Invalid value", MessageBoxButton.OK);
          maskedTextBox.OnStringValidationCompleted(new StringValidationEventArgs(bIsValidInput, message, maskedTextBox.ValidationString));
          maskedTextBox.OnValidated(EventArgs.Empty);
          return true;
        case InvalidInputBehavior.ResetValue:
          maskedTextBox.OnStringValidationCompleted(new StringValidationEventArgs(bIsValidInput, message, maskedTextBox.ValidationString));
          maskedTextBox.OnValidated(EventArgs.Empty);
          return true;
      }
    }
    else
    {
      maskedTextBox.OnStringValidationCompleted(new StringValidationEventArgs(bIsValidInput, message, maskedTextBox.ValidationString));
      maskedTextBox.OnValidated(EventArgs.Empty);
    }
    return false;
  }

  public ObservableCollection<CharacterProperties> CreateRegularExpression(
    MaskedTextBox maskedTextBox)
  {
    bool flag = false;
    bool? nullable = new bool?();
    ObservableCollection<CharacterProperties> regularExpression = new ObservableCollection<CharacterProperties>();
    string mask = maskedTextBox.Mask;
    CultureInfo culture = maskedTextBox.GetCulture();
    if (mask != null)
    {
      foreach (char ch in mask)
      {
        switch (ch)
        {
          case '#':
            CharacterProperties characterProperties1 = new CharacterProperties()
            {
              RegExpression = flag ? ch.ToString() : "[\\s\\d+-]",
              IsLiteral = flag,
              IsUpper = nullable,
              IsPromptCharacter = new bool?(false),
              IsOptional = new bool?(true)
            };
            regularExpression.Add(characterProperties1);
            flag = false;
            break;
          case '$':
            CharacterProperties characterProperties2 = new CharacterProperties()
            {
              RegExpression = flag ? ch.ToString() : culture.NumberFormat.CurrencySymbol,
              IsLiteral = true,
              IsUpper = nullable,
              IsPromptCharacter = new bool?(),
              IsOptional = new bool?()
            };
            regularExpression.Add(characterProperties2);
            flag = false;
            break;
          case '&':
            CharacterProperties characterProperties3 = new CharacterProperties()
            {
              RegExpression = flag ? ch.ToString() : "[\\p{Ll}\\p{Lu}\\p{Lt}\\p{Lm}\\p{Lo}]",
              IsLiteral = flag,
              IsUpper = nullable,
              IsPromptCharacter = new bool?(),
              IsOptional = new bool?()
            };
            regularExpression.Add(characterProperties3);
            flag = false;
            break;
          case ',':
            CharacterProperties characterProperties4 = new CharacterProperties()
            {
              RegExpression = flag ? ch.ToString() : culture.NumberFormat.NumberGroupSeparator,
              IsLiteral = true,
              IsUpper = nullable,
              IsPromptCharacter = new bool?(),
              IsOptional = new bool?()
            };
            regularExpression.Add(characterProperties4);
            flag = false;
            break;
          case '.':
            CharacterProperties characterProperties5 = new CharacterProperties()
            {
              RegExpression = flag ? ch.ToString() : culture.NumberFormat.NumberDecimalSeparator,
              IsLiteral = true,
              IsUpper = nullable,
              IsPromptCharacter = new bool?(),
              IsOptional = new bool?()
            };
            regularExpression.Add(characterProperties5);
            flag = false;
            break;
          case '/':
            CharacterProperties characterProperties6 = new CharacterProperties()
            {
              IsLiteral = true,
              IsUpper = nullable,
              IsPromptCharacter = new bool?(),
              IsOptional = new bool?()
            };
            characterProperties6.RegExpression = !flag ? (maskedTextBox.TimeSeparator != string.Empty ? maskedTextBox.DateSeparator[0].ToString() : "/") : ch.ToString();
            regularExpression.Add(characterProperties6);
            flag = false;
            break;
          case '0':
            CharacterProperties characterProperties7 = new CharacterProperties()
            {
              RegExpression = flag ? ch.ToString() : "\\d",
              IsLiteral = flag,
              IsUpper = nullable,
              IsPromptCharacter = new bool?(),
              IsOptional = new bool?(false)
            };
            regularExpression.Add(characterProperties7);
            flag = false;
            break;
          case '9':
            CharacterProperties characterProperties8 = new CharacterProperties()
            {
              RegExpression = flag ? ch.ToString() : "[\\s\\d]",
              IsLiteral = flag,
              IsUpper = nullable,
              IsPromptCharacter = new bool?(),
              IsOptional = new bool?(false)
            };
            regularExpression.Add(characterProperties8);
            flag = false;
            break;
          case ':':
            CharacterProperties characterProperties9 = new CharacterProperties()
            {
              IsLiteral = true,
              IsUpper = nullable,
              IsPromptCharacter = new bool?(),
              IsOptional = new bool?()
            };
            if (flag)
              characterProperties9.RegExpression = ch.ToString();
            else if (maskedTextBox.TimeSeparator != null)
              characterProperties9.RegExpression = maskedTextBox.TimeSeparator != string.Empty ? maskedTextBox.TimeSeparator[0].ToString() : ":";
            regularExpression.Add(characterProperties9);
            flag = false;
            break;
          case '<':
            nullable = new bool?(false);
            break;
          case '>':
            nullable = new bool?(true);
            break;
          case '?':
            CharacterProperties characterProperties10 = new CharacterProperties()
            {
              RegExpression = flag ? ch.ToString() : "[\\sa-zA-Z]",
              IsLiteral = flag,
              IsUpper = nullable,
              IsPromptCharacter = new bool?(false),
              IsOptional = new bool?(true)
            };
            regularExpression.Add(characterProperties10);
            flag = false;
            break;
          case 'A':
            CharacterProperties characterProperties11 = new CharacterProperties()
            {
              RegExpression = flag ? ch.ToString() : "\\w",
              IsLiteral = flag,
              IsUpper = nullable,
              IsPromptCharacter = new bool?(),
              IsOptional = new bool?(false)
            };
            regularExpression.Add(characterProperties11);
            flag = false;
            break;
          case 'C':
            CharacterProperties characterProperties12 = new CharacterProperties()
            {
              RegExpression = flag ? ch.ToString() : "[\\s\\p{Ll}\\p{Lu}\\p{Lt}\\p{Lm}\\p{Lo}]?",
              IsLiteral = flag,
              IsUpper = nullable,
              IsPromptCharacter = new bool?(false),
              IsOptional = new bool?(true)
            };
            regularExpression.Add(characterProperties12);
            flag = false;
            break;
          case 'L':
            CharacterProperties characterProperties13 = new CharacterProperties()
            {
              RegExpression = flag ? ch.ToString() : "[a-zA-Z]",
              IsLiteral = flag,
              IsUpper = nullable,
              IsPromptCharacter = new bool?(),
              IsOptional = new bool?(false)
            };
            regularExpression.Add(characterProperties13);
            flag = false;
            break;
          case '\\':
            if (!flag)
            {
              flag = true;
              break;
            }
            CharacterProperties characterProperties14 = new CharacterProperties()
            {
              RegExpression = ch.ToString(),
              IsLiteral = true,
              IsUpper = nullable,
              IsPromptCharacter = new bool?(),
              IsOptional = new bool?()
            };
            regularExpression.Add(characterProperties14);
            flag = false;
            break;
          default:
            CharacterProperties characterProperties15 = new CharacterProperties()
            {
              RegExpression = ch.ToString(),
              IsLiteral = true,
              IsUpper = nullable,
              IsPromptCharacter = new bool?(),
              IsOptional = new bool?()
            };
            regularExpression.Add(characterProperties15);
            flag = false;
            break;
        }
      }
    }
    return regularExpression;
  }

  public string CreateDisplayText(MaskedTextBox maskedTextBox)
  {
    if (!(maskedTextBox.Mask != string.Empty))
      return maskedTextBox.Value;
    int index1 = 0;
    string displayText1 = string.Empty;
    for (int index2 = 0; index2 < maskedTextBox.CharCollection.Count; ++index2)
    {
      CharacterProperties characterProperties1 = maskedTextBox.CharCollection[index2];
      if (characterProperties1.IsLiteral)
      {
        string str1 = displayText1;
        string str2;
        if (characterProperties1.IsUpper.HasValue)
        {
          bool? isUpper = characterProperties1.IsUpper;
          str2 = (!isUpper.GetValueOrDefault() ? 0 : (isUpper.HasValue ? 1 : 0)) != 0 ? characterProperties1.RegExpression.ToUpper() : characterProperties1.RegExpression.ToLower();
        }
        else
          str2 = characterProperties1.RegExpression.ToString();
        displayText1 = str1 + str2;
        characterProperties1.IsPromptCharacter = new bool?();
      }
      else if (maskedTextBox.mValue != null)
      {
        if (index1 < maskedTextBox.mValue.ToString().Length)
        {
          char c = maskedTextBox.mValue.ToString()[index1];
          string regExpression = characterProperties1.RegExpression;
          if (Regex.IsMatch(c.ToString(), regExpression))
          {
            string str3 = displayText1;
            string str4;
            if (characterProperties1.IsUpper.HasValue)
            {
              bool? isUpper = characterProperties1.IsUpper;
              str4 = (!isUpper.GetValueOrDefault() ? 0 : (isUpper.HasValue ? 1 : 0)) != 0 ? char.ToUpper(c).ToString() : char.ToLower(c).ToString();
            }
            else
              str4 = c.ToString();
            displayText1 = str3 + str4;
            ++index1;
            characterProperties1.IsPromptCharacter = new bool?(false);
          }
          else
          {
            string displayText2 = displayText1 + (object) maskedTextBox.PromptChar;
            characterProperties1.IsPromptCharacter = new bool?(true);
            for (int index3 = index2 + 1; index3 < maskedTextBox.CharCollection.Count; ++index3)
            {
              CharacterProperties characterProperties2 = maskedTextBox.CharCollection[index3];
              if (characterProperties2.IsLiteral)
              {
                string str5 = displayText2;
                string str6;
                if (characterProperties2.IsUpper.HasValue)
                {
                  bool? isUpper = characterProperties2.IsUpper;
                  str6 = (!isUpper.GetValueOrDefault() ? 0 : (isUpper.HasValue ? 1 : 0)) != 0 ? characterProperties2.RegExpression.ToUpper() : characterProperties2.RegExpression.ToLower();
                }
                else
                  str6 = characterProperties2.RegExpression.ToString();
                displayText2 = str5 + str6;
                characterProperties2.IsPromptCharacter = new bool?();
              }
              else
                displayText2 += characterProperties2.IsLiteral ? characterProperties2.RegExpression : maskedTextBox.PromptChar.ToString();
            }
            return displayText2;
          }
        }
        else
        {
          displayText1 += (string) (object) maskedTextBox.PromptChar;
          characterProperties1.IsPromptCharacter = new bool?(true);
        }
      }
      else
      {
        displayText1 += (string) (object) maskedTextBox.PromptChar;
        characterProperties1.IsPromptCharacter = new bool?(true);
      }
    }
    return displayText1;
  }

  public string CreateValueFromText(MaskedTextBox maskedTextBox)
  {
    if (!string.IsNullOrEmpty(maskedTextBox.Mask))
    {
      string valueFromText = string.Empty;
      for (int index = 0; index < maskedTextBox.CharCollection.Count; ++index)
      {
        CharacterProperties characterProperties = maskedTextBox.CharCollection[index];
        if (characterProperties.IsLiteral)
        {
          if (maskedTextBox.Text != null && (maskedTextBox.TextMaskFormat == MaskFormat.IncludeLiterals || maskedTextBox.TextMaskFormat == MaskFormat.IncludePromptAndLiterals) && maskedTextBox.Text.Length > index)
            valueFromText += (string) (object) maskedTextBox.Text[index];
        }
        else
        {
          bool? isPromptCharacter = characterProperties.IsPromptCharacter;
          if ((isPromptCharacter.GetValueOrDefault() ? 0 : (isPromptCharacter.HasValue ? 1 : 0)) != 0)
          {
            if (maskedTextBox.Text != null && maskedTextBox.Text.Length > index)
              valueFromText += (string) (object) maskedTextBox.Text[index];
          }
          else if (maskedTextBox.Text != null && (maskedTextBox.TextMaskFormat == MaskFormat.IncludePrompt || maskedTextBox.TextMaskFormat == MaskFormat.IncludePromptAndLiterals) && maskedTextBox.Text.Length > index)
            valueFromText += (string) (object) maskedTextBox.Text[index];
        }
      }
      if (maskedTextBox.PromptChar == ' ' && maskedTextBox.TextMaskFormat == MaskFormat.ExcludePromptAndLiterals)
        valueFromText = valueFromText.TrimEnd(maskedTextBox.PromptChar);
      return valueFromText;
    }
    if (maskedTextBox.Mask != string.Empty && maskedTextBox.CharCollection.Count < maskedTextBox.Text.Length)
    {
      int caretIndex = maskedTextBox.CaretIndex;
      maskedTextBox.Text = maskedTextBox.Text.Remove(maskedTextBox.CaretIndex, maskedTextBox.Text.Length - maskedTextBox.CharCollection.Count);
      maskedTextBox.CaretIndex = caretIndex;
    }
    return maskedTextBox.Text;
  }

  public bool HandleKeyDown(MaskedTextBox maskedTextBox, KeyEventArgs eventArgs)
  {
    switch (eventArgs.Key)
    {
      case Key.Back:
        return this.HandleBackSpaceKey(maskedTextBox);
      case Key.Return:
        if (maskedTextBox.EnterToMoveNext)
        {
          TraversalRequest request = new TraversalRequest(FocusNavigationDirection.Next);
          if (Keyboard.FocusedElement is UIElement focusedElement)
            focusedElement.MoveFocus(request);
          return true;
        }
        if (maskedTextBox.Mask == string.Empty)
        {
          eventArgs.Handled = !maskedTextBox.AcceptsReturn;
          break;
        }
        break;
      case Key.Space:
        return true;
      case Key.Delete:
        return this.HandleDeleteKey(maskedTextBox);
    }
    return false;
  }

  public bool HandlePaste(MaskedTextBox maskedTextBox)
  {
    if (maskedTextBox.IsReadOnly)
      return true;
    try
    {
      int selectionStart1 = maskedTextBox.SelectionStart;
      string text = Clipboard.GetText();
      int length = text.Length;
      int index = 0;
      int selectionStart2 = maskedTextBox.SelectionStart;
      if (maskedTextBox.CharCollection != null && maskedTextBox.Mask != string.Empty)
      {
        string maskedText = maskedTextBox.Text;
        while (index < length && selectionStart1 < maskedText.Length)
        {
          if (maskedTextBox.CharCollection[selectionStart1].IsLiteral)
            ++selectionStart1;
          else if (Regex.IsMatch(text[index].ToString(), maskedTextBox.CharCollection[selectionStart1].RegExpression))
          {
            if (maskedText.Length > 0)
              maskedText = maskedText.Remove(selectionStart1, 1);
            maskedText = maskedText.Insert(selectionStart1, text[index].ToString());
            maskedTextBox.CharCollection[selectionStart1].IsPromptCharacter = new bool?(false);
            ++index;
            ++selectionStart1;
            ++selectionStart2;
          }
          else
            ++index;
        }
        maskedTextBox.SelectionLength = 0;
        string str = MaskHandler.maskHandler.ValueFromMaskedText(maskedTextBox, maskedTextBox.TextMaskFormat, maskedText, maskedTextBox.CharCollection);
        maskedTextBox.SetValue(new bool?(false), (object) str);
        maskedTextBox.MaskedText = maskedText;
        maskedTextBox.SelectionStart = selectionStart1;
        return true;
      }
      this.HandleValidationString(maskedTextBox, text);
      return true;
    }
    catch (COMException ex)
    {
      return false;
    }
  }

  public bool HandleDeleteKey(MaskedTextBox maskedTextBox)
  {
    if (maskedTextBox.IsReadOnly || maskedTextBox.OnValidating(new CancelEventArgs(false)))
      return true;
    if (maskedTextBox.CharCollection != null && maskedTextBox.Mask != string.Empty)
    {
      int selectionStart1 = maskedTextBox.SelectionStart;
      string str1 = maskedTextBox.MaskedText;
      int num = maskedTextBox.SelectionStart + (maskedTextBox.tempSelectedLength == 0 ? 1 : maskedTextBox.SelectionLength);
      while (selectionStart1 <= num - 1 && selectionStart1 < maskedTextBox.Text.Length)
      {
        if (maskedTextBox.CharCollection[selectionStart1].IsPromptCharacter.HasValue)
        {
          bool? isPromptCharacter = maskedTextBox.CharCollection[selectionStart1].IsPromptCharacter;
          if ((!isPromptCharacter.GetValueOrDefault() ? 1 : (!isPromptCharacter.HasValue ? 1 : 0)) != 0)
          {
            bool? isOptional1 = maskedTextBox.CharCollection[selectionStart1].IsOptional;
            if ((!isOptional1.GetValueOrDefault() ? 1 : (!isOptional1.HasValue ? 1 : 0)) != 0)
              maskedTextBox.CharCollection[selectionStart1].IsPromptCharacter = new bool?(true);
            if (str1.Length > 0)
              str1 = str1.Remove(selectionStart1, 1);
            string str2 = str1;
            int startIndex = selectionStart1;
            bool? isOptional2 = maskedTextBox.CharCollection[selectionStart1].IsOptional;
            string str3 = (!isOptional2.GetValueOrDefault() ? 0 : (isOptional2.HasValue ? 1 : 0)) != 0 ? " " : maskedTextBox.PromptChar.ToString();
            str1 = str2.Insert(startIndex, str3);
          }
        }
        ++selectionStart1;
        if (selectionStart1 < maskedTextBox.CharCollection.Count && maskedTextBox.CharCollection[selectionStart1].IsLiteral)
          ++selectionStart1;
      }
      int selectionStart2 = maskedTextBox.SelectionStart;
      maskedTextBox.MaskedText = str1;
      maskedTextBox.SelectionStart = selectionStart2;
      maskedTextBox.CaretIndex = selectionStart1;
      maskedTextBox.SelectionLength = 0;
      maskedTextBox.SetValue(new bool?(), (object) MaskHandler.maskHandler.CreateValueFromText(maskedTextBox));
      maskedTextBox.OnValidated(EventArgs.Empty);
      return true;
    }
    string maskedText = maskedTextBox.MaskedText;
    if (maskedTextBox.MaskedText.Length == maskedTextBox.SelectionStart)
    {
      maskedTextBox.OnValidated(EventArgs.Empty);
      return true;
    }
    int count = maskedTextBox.SelectionLength == 0 ? 1 : maskedTextBox.SelectionLength;
    if (maskedTextBox.Text != null)
    {
      int selectionStart = maskedTextBox.SelectionStart;
      string str = maskedTextBox.MaskedText.Remove(maskedTextBox.SelectionStart, count);
      maskedTextBox.MaskedText = str;
      maskedTextBox.SelectionStart = selectionStart;
      return true;
    }
    if (maskedTextBox.StringValidation != StringValidation.OnKeyPress)
      return false;
    if (maskedTextBox.OnValidating(new CancelEventArgs(false)))
      return true;
    string message = "";
    bool bIsValidInput = Regex.IsMatch(maskedText, maskedTextBox.ValidationString);
    string messageBoxText = bIsValidInput ? "String validation succeeded" : "String validation failed";
    if (!bIsValidInput)
    {
      switch (maskedTextBox.InvalidValueBehavior)
      {
        case InvalidInputBehavior.None:
          maskedTextBox.OnStringValidationCompleted(new StringValidationEventArgs(bIsValidInput, message, maskedTextBox.ValidationString));
          maskedTextBox.OnValidated(EventArgs.Empty);
          break;
        case InvalidInputBehavior.DisplayErrorMessage:
          int num1 = (int) MessageBox.Show(messageBoxText, "Invalid value", MessageBoxButton.OK);
          maskedTextBox.OnStringValidationCompleted(new StringValidationEventArgs(bIsValidInput, message, maskedTextBox.ValidationString));
          maskedTextBox.OnValidated(EventArgs.Empty);
          return true;
        case InvalidInputBehavior.ResetValue:
          maskedTextBox.OnStringValidationCompleted(new StringValidationEventArgs(bIsValidInput, message, maskedTextBox.ValidationString));
          maskedTextBox.OnValidated(EventArgs.Empty);
          return true;
      }
    }
    else
    {
      maskedTextBox.OnStringValidationCompleted(new StringValidationEventArgs(bIsValidInput, message, maskedTextBox.ValidationString));
      maskedTextBox.OnValidated(EventArgs.Empty);
    }
    return false;
  }

  public bool HandleValidationString(MaskedTextBox maskedTextBox, string text)
  {
    if (maskedTextBox.OnValidating(new CancelEventArgs(false)))
      return true;
    string text1 = Clipboard.GetText();
    int length = Clipboard.GetText().Length;
    int index = 0;
    int selectionStart = maskedTextBox.SelectionStart;
    string input = maskedTextBox.MaskedText.Remove(maskedTextBox.SelectionStart, maskedTextBox.SelectionLength);
    if (input.Length == maskedTextBox.SelectionStart)
    {
      input += text;
      selectionStart += text.Length;
    }
    else if (maskedTextBox.SelectionStart == 0)
    {
      input = text + input;
      selectionStart += text.Length;
    }
    else
    {
      while (index < length && index < text.Length)
      {
        input = input.Insert(selectionStart, text1[index].ToString());
        ++index;
        ++selectionStart;
      }
    }
    if (maskedTextBox.MaxLength > 0 && input.Length > maskedTextBox.MaxLength)
      input = input.Remove(maskedTextBox.MaxLength);
    if (maskedTextBox.StringValidation == StringValidation.OnKeyPress && !maskedTextBox.OnValidating(new CancelEventArgs(false)))
    {
      string message = "";
      bool bIsValidInput = Regex.IsMatch(input, maskedTextBox.ValidationString);
      string messageBoxText = bIsValidInput ? "String validation succeeded" : "String validation failed";
      if (!bIsValidInput)
      {
        switch (maskedTextBox.InvalidValueBehavior)
        {
          case InvalidInputBehavior.None:
            maskedTextBox.OnStringValidationCompleted(new StringValidationEventArgs(bIsValidInput, message, maskedTextBox.ValidationString));
            maskedTextBox.OnValidated(EventArgs.Empty);
            break;
          case InvalidInputBehavior.DisplayErrorMessage:
            int num = (int) MessageBox.Show(messageBoxText, "Invalid value", MessageBoxButton.OK);
            maskedTextBox.OnStringValidationCompleted(new StringValidationEventArgs(bIsValidInput, message, maskedTextBox.ValidationString));
            maskedTextBox.OnValidated(EventArgs.Empty);
            return true;
          case InvalidInputBehavior.ResetValue:
            maskedTextBox.OnStringValidationCompleted(new StringValidationEventArgs(bIsValidInput, message, maskedTextBox.ValidationString));
            maskedTextBox.OnValidated(EventArgs.Empty);
            return true;
        }
      }
      else
      {
        maskedTextBox.OnStringValidationCompleted(new StringValidationEventArgs(bIsValidInput, message, maskedTextBox.ValidationString));
        maskedTextBox.OnValidated(EventArgs.Empty);
      }
      maskedTextBox.Value = input;
      maskedTextBox.MaskedText = input;
      maskedTextBox.CaretIndex = selectionStart;
      return false;
    }
    maskedTextBox.Value = input;
    maskedTextBox.MaskedText = input;
    maskedTextBox.CaretIndex = selectionStart;
    return false;
  }

  public bool HandleBackSpaceKey(MaskedTextBox maskedTextBox)
  {
    if (maskedTextBox.IsReadOnly || maskedTextBox.OnValidating(new CancelEventArgs(false)))
      return true;
    if (maskedTextBox.Mask != string.Empty)
    {
      int num = maskedTextBox.SelectionStart + (maskedTextBox.SelectionLength > 0 ? 1 : 0);
      string str = maskedTextBox.MaskedText;
      for (int index = maskedTextBox.SelectionStart + maskedTextBox.SelectionLength; num <= index && num > 0; ++num)
      {
        if (maskedTextBox.CharCollection[num - 1].IsPromptCharacter.HasValue)
        {
          bool? isPromptCharacter = maskedTextBox.CharCollection[num - 1].IsPromptCharacter;
          if ((!isPromptCharacter.GetValueOrDefault() ? 1 : (!isPromptCharacter.HasValue ? 1 : 0)) != 0)
          {
            bool? isOptional1 = maskedTextBox.CharCollection[num - 1].IsOptional;
            if ((!isOptional1.GetValueOrDefault() ? 1 : (!isOptional1.HasValue ? 1 : 0)) != 0)
              maskedTextBox.CharCollection[num - 1].IsPromptCharacter = new bool?(true);
            if (str.Length >= num - 1)
              str = str.Remove(num - 1, 1);
            if (str.Length >= num - 1)
            {
              bool? isOptional2 = maskedTextBox.CharCollection[num - 1].IsOptional;
              if ((!isOptional2.GetValueOrDefault() ? 0 : (isOptional2.HasValue ? 1 : 0)) != 0)
              {
                str = str.Insert(num - 1, " ");
                continue;
              }
            }
            if (str.Length >= num - 1)
              str = str.Insert(num - 1, maskedTextBox.PromptChar.ToString());
          }
        }
      }
      int selectionStart = maskedTextBox.SelectionStart;
      maskedTextBox.MaskedText = str;
      maskedTextBox.SelectionStart = selectionStart == 0 ? 0 : selectionStart - 1;
      maskedTextBox.SelectionLength = 0;
      maskedTextBox.SetValue(new bool?(), (object) MaskHandler.maskHandler.CreateValueFromText(maskedTextBox));
      maskedTextBox.OnValidated(EventArgs.Empty);
      return true;
    }
    string maskedText = maskedTextBox.MaskedText;
    if (maskedTextBox.SelectionStart == 0 && maskedTextBox.SelectionLength == 0)
    {
      maskedTextBox.OnValidated(EventArgs.Empty);
      return true;
    }
    if (maskedTextBox.SelectionLength == 0)
    {
      if (maskedTextBox.SelectionStart == 0)
        return true;
      int selectionStart = maskedTextBox.SelectionStart;
      string str = maskedTextBox.MaskedText;
      if (str.Length >= selectionStart - 1)
        str = str.Remove(maskedTextBox.SelectionStart - 1, 1);
      maskedTextBox.MaskedText = str;
      maskedTextBox.SelectionStart = selectionStart - 1;
      return true;
    }
    string input = maskedText.Remove(maskedTextBox.SelectionStart, maskedTextBox.SelectionLength);
    if (maskedTextBox.StringValidation != StringValidation.OnKeyPress)
      return false;
    if (maskedTextBox.OnValidating(new CancelEventArgs(false)))
      return true;
    string message = "";
    bool bIsValidInput = Regex.IsMatch(input, maskedTextBox.ValidationString);
    string messageBoxText = bIsValidInput ? "String validation succeeded" : "String validation failed";
    if (!bIsValidInput)
    {
      switch (maskedTextBox.InvalidValueBehavior)
      {
        case InvalidInputBehavior.None:
          maskedTextBox.OnStringValidationCompleted(new StringValidationEventArgs(bIsValidInput, message, maskedTextBox.ValidationString));
          maskedTextBox.OnValidated(EventArgs.Empty);
          break;
        case InvalidInputBehavior.DisplayErrorMessage:
          int num1 = (int) MessageBox.Show(messageBoxText, "Invalid value", MessageBoxButton.OK);
          maskedTextBox.OnStringValidationCompleted(new StringValidationEventArgs(bIsValidInput, message, maskedTextBox.ValidationString));
          maskedTextBox.OnValidated(EventArgs.Empty);
          return true;
        case InvalidInputBehavior.ResetValue:
          maskedTextBox.OnStringValidationCompleted(new StringValidationEventArgs(bIsValidInput, message, maskedTextBox.ValidationString));
          maskedTextBox.OnValidated(EventArgs.Empty);
          return true;
      }
    }
    else
    {
      maskedTextBox.OnStringValidationCompleted(new StringValidationEventArgs(bIsValidInput, message, maskedTextBox.ValidationString));
      maskedTextBox.OnValidated(EventArgs.Empty);
    }
    return false;
  }

  private string displayTextValue(string displayText, string text, bool isNumeric)
  {
    return !isNumeric ? text + displayText : (displayText += text);
  }

  public string CoerceValue(MaskedTextBox maskedTextbox, string value, MaskFormat maskformat)
  {
    int index1 = 0;
    string text = string.Empty;
    if (value != string.Empty && maskedTextbox.IsNumeric)
    {
      string empty = string.Empty;
      for (int index2 = value.Length - 1; index2 >= 0; --index2)
        empty += (string) (object) value[index2];
      ObservableCollection<CharacterProperties> observableCollection = new ObservableCollection<CharacterProperties>();
      if (maskedTextbox.CharCollection != null)
      {
        for (int index3 = maskedTextbox.CharCollection.Count - 1; index3 >= 0; --index3)
          observableCollection.Add(maskedTextbox.CharCollection[index3]);
        maskedTextbox.CharCollection = observableCollection;
      }
      value = empty;
    }
    if (maskedTextbox.CharCollection != null)
    {
      for (int index4 = 0; index4 < maskedTextbox.CharCollection.Count; ++index4)
      {
        if (value != string.Empty)
        {
          if (maskedTextbox.CharCollection[index4].IsLiteral)
          {
            text = this.displayTextValue(maskedTextbox.CharCollection[index4].RegExpression, text, maskedTextbox.IsNumeric);
            maskedTextbox.CharCollection[index4].IsPromptCharacter = new bool?();
          }
          else
          {
            bool? isPromptCharacter = maskedTextbox.CharCollection[index4].IsPromptCharacter;
            if ((!isPromptCharacter.GetValueOrDefault() ? 0 : (isPromptCharacter.HasValue ? 1 : 0)) != 0)
            {
              text = this.displayTextValue(maskedTextbox.PromptChar.ToString(), text, maskedTextbox.IsNumeric);
              maskedTextbox.CharCollection[index4].IsPromptCharacter = new bool?(true);
            }
            else if (index1 < value.Length)
            {
              char ch = value.ToString()[index1];
              string regExpression = maskedTextbox.CharCollection[index4].RegExpression;
              if (regExpression != null)
              {
                string pattern = regExpression;
                if (Regex.IsMatch(ch.ToString(), pattern))
                {
                  ++index1;
                  text = this.displayTextValue(ch.ToString(), text, maskedTextbox.IsNumeric);
                  maskedTextbox.CharCollection[index4].IsPromptCharacter = new bool?(false);
                }
                else if (ch == '-' || ch == ' ' || ch == '$' || ch == '>' || ch == '<' || ch == '\\' || ch == '.' || ch == ':' || ch == ',' || ch == '(' || ch == ')' || ch == '/' || ch == '+')
                {
                  ++index1;
                  --index4;
                }
                else
                {
                  ++index1;
                  text = this.displayTextValue(maskedTextbox.PromptChar.ToString(), text, maskedTextbox.IsNumeric);
                  maskedTextbox.CharCollection[index4].IsPromptCharacter = new bool?(true);
                }
              }
            }
            else if (maskedTextbox.CharCollection[index4].IsLiteral)
            {
              text = this.displayTextValue(maskedTextbox.CharCollection[index4].RegExpression, text, maskedTextbox.IsNumeric);
              maskedTextbox.CharCollection[index4].IsPromptCharacter = new bool?();
            }
            else
            {
              bool? isOptional = maskedTextbox.CharCollection[index4].IsOptional;
              if ((!isOptional.GetValueOrDefault() ? 0 : (isOptional.HasValue ? 1 : 0)) != 0)
              {
                text = this.displayTextValue(" ", text, maskedTextbox.IsNumeric);
              }
              else
              {
                text = this.displayTextValue(maskedTextbox.PromptChar.ToString(), text, maskedTextbox.IsNumeric);
                maskedTextbox.CharCollection[index4].IsPromptCharacter = new bool?(true);
              }
            }
          }
        }
        else if (maskedTextbox.CharCollection[index4].IsLiteral)
        {
          text += maskedTextbox.CharCollection[index4].RegExpression;
          maskedTextbox.CharCollection[index4].IsPromptCharacter = new bool?();
        }
        else
        {
          bool? isOptional1 = maskedTextbox.CharCollection[index4].IsOptional;
          if ((!isOptional1.GetValueOrDefault() ? 0 : (isOptional1.HasValue ? 1 : 0)) != 0)
          {
            text += " ";
            maskedTextbox.CharCollection[index4].IsPromptCharacter = new bool?(false);
          }
          else
          {
            string str1 = text + (object) maskedTextbox.PromptChar;
            maskedTextbox.CharCollection[index4].IsPromptCharacter = new bool?(true);
            for (int index5 = index4 + 1; index5 < maskedTextbox.CharCollection.Count; ++index5)
            {
              if (maskedTextbox.CharCollection[index5].IsLiteral)
              {
                string str2 = str1;
                string str3;
                if (maskedTextbox.CharCollection[index5].IsUpper.HasValue)
                {
                  bool? isUpper = maskedTextbox.CharCollection[index5].IsUpper;
                  str3 = (!isUpper.GetValueOrDefault() ? 0 : (isUpper.HasValue ? 1 : 0)) != 0 ? maskedTextbox.CharCollection[index5].RegExpression.ToUpper() : maskedTextbox.CharCollection[index5].RegExpression.ToLower();
                }
                else
                  str3 = maskedTextbox.CharCollection[index5].RegExpression.ToString();
                str1 = str2 + str3;
                maskedTextbox.CharCollection[index5].IsPromptCharacter = new bool?();
              }
              else
              {
                bool? isOptional2 = maskedTextbox.CharCollection[index5].IsOptional;
                if ((!isOptional2.GetValueOrDefault() ? 1 : (!isOptional2.HasValue ? 1 : 0)) != 0)
                {
                  str1 += maskedTextbox.CharCollection[index5].IsLiteral ? maskedTextbox.CharCollection[index5].RegExpression : maskedTextbox.PromptChar.ToString();
                  maskedTextbox.CharCollection[index5].IsPromptCharacter = new bool?(true);
                }
                else
                {
                  bool? isOptional3 = maskedTextbox.CharCollection[index5].IsOptional;
                  if ((!isOptional3.GetValueOrDefault() ? 0 : (isOptional3.HasValue ? 1 : 0)) != 0)
                    str1 += " ";
                }
              }
            }
            return str1;
          }
        }
      }
    }
    return text;
  }

  public string ValueFromMaskedText(
    MaskedTextBox maskedTextBox,
    MaskFormat TextMaskFormat,
    string maskedText,
    ObservableCollection<CharacterProperties> CharCollection)
  {
    if (string.IsNullOrEmpty(maskedTextBox.Mask))
      return maskedTextBox.Text;
    string empty = string.Empty;
    for (int index = 0; index < CharCollection.Count; ++index)
    {
      CharacterProperties characterProperties = CharCollection[index];
      if (characterProperties.IsLiteral)
      {
        if ((TextMaskFormat == MaskFormat.IncludeLiterals || TextMaskFormat == MaskFormat.IncludePromptAndLiterals) && index < maskedText.Length && !maskedText[index].ToString().IsNullOrWhiteSpace())
          empty += (string) (object) maskedText[index];
      }
      else
      {
        bool? isPromptCharacter = characterProperties.IsPromptCharacter;
        if ((isPromptCharacter.GetValueOrDefault() ? 0 : (isPromptCharacter.HasValue ? 1 : 0)) != 0 || !characterProperties.IsPromptCharacter.HasValue)
        {
          bool? isOptional = characterProperties.IsOptional;
          if ((!isOptional.GetValueOrDefault() ? 0 : (isOptional.HasValue ? 1 : 0)) != 0 && index < maskedText.Length && (TextMaskFormat == MaskFormat.IncludeLiterals || TextMaskFormat == MaskFormat.IncludePromptAndLiterals) && !maskedText[index].ToString().IsNullOrWhiteSpace())
            empty += (string) (object) maskedText[index];
          else if (index < maskedText.Length && !maskedText[index].ToString().IsNullOrWhiteSpace())
            empty += (string) (object) maskedText[index];
        }
        else
        {
          bool? isOptional = characterProperties.IsOptional;
          if ((!isOptional.GetValueOrDefault() ? 0 : (isOptional.HasValue ? 1 : 0)) != 0 && index < maskedText.Length && (TextMaskFormat == MaskFormat.IncludeLiterals || TextMaskFormat == MaskFormat.IncludePromptAndLiterals))
          {
            if (index < maskedText.Length && !maskedText[index].ToString().IsNullOrWhiteSpace())
              empty += (string) (object) maskedText[index];
          }
          else if ((TextMaskFormat == MaskFormat.IncludePrompt || TextMaskFormat == MaskFormat.IncludePromptAndLiterals) && index < maskedText.Length && !maskedText[index].ToString().IsNullOrWhiteSpace())
            empty += (string) (object) maskedText[index];
        }
      }
    }
    return empty;
  }
}
