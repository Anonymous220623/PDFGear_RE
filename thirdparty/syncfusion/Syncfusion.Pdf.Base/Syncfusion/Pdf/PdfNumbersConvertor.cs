// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.PdfNumbersConvertor
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace Syncfusion.Pdf;

internal class PdfNumbersConvertor
{
  private const float LetterLimit = 26f;
  private const int AcsiiStartIndex = 64 /*0x40*/;

  public static string Convert(int intArabic, PdfNumberStyle numberStyle)
  {
    switch (numberStyle)
    {
      case PdfNumberStyle.None:
        return string.Empty;
      case PdfNumberStyle.Numeric:
        return intArabic.ToString();
      case PdfNumberStyle.LowerLatin:
        return PdfNumbersConvertor.ArabicToLetter(intArabic).ToLower();
      case PdfNumberStyle.LowerRoman:
        return PdfNumbersConvertor.ArabicToRoman(intArabic).ToLower();
      case PdfNumberStyle.UpperLatin:
        return PdfNumbersConvertor.ArabicToLetter(intArabic);
      case PdfNumberStyle.UpperRoman:
        return PdfNumbersConvertor.ArabicToRoman(intArabic);
      default:
        return string.Empty;
    }
  }

  private static string ArabicToRoman(int intArabic)
  {
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append(PdfNumbersConvertor.GenerateNumber(ref intArabic, 1000, "M"));
    stringBuilder.Append(PdfNumbersConvertor.GenerateNumber(ref intArabic, 900, "CM"));
    stringBuilder.Append(PdfNumbersConvertor.GenerateNumber(ref intArabic, 500, "D"));
    stringBuilder.Append(PdfNumbersConvertor.GenerateNumber(ref intArabic, 400, "CD"));
    stringBuilder.Append(PdfNumbersConvertor.GenerateNumber(ref intArabic, 100, "C"));
    stringBuilder.Append(PdfNumbersConvertor.GenerateNumber(ref intArabic, 90, "XC"));
    stringBuilder.Append(PdfNumbersConvertor.GenerateNumber(ref intArabic, 50, "L"));
    stringBuilder.Append(PdfNumbersConvertor.GenerateNumber(ref intArabic, 40, "XL"));
    stringBuilder.Append(PdfNumbersConvertor.GenerateNumber(ref intArabic, 10, "X"));
    stringBuilder.Append(PdfNumbersConvertor.GenerateNumber(ref intArabic, 9, "IX"));
    stringBuilder.Append(PdfNumbersConvertor.GenerateNumber(ref intArabic, 5, "V"));
    stringBuilder.Append(PdfNumbersConvertor.GenerateNumber(ref intArabic, 4, "IV"));
    stringBuilder.Append(PdfNumbersConvertor.GenerateNumber(ref intArabic, 1, "I"));
    return stringBuilder.ToString();
  }

  private static string ArabicToLetter(int arabic)
  {
    Stack<int> letter = PdfNumbersConvertor.ConvertToLetter((float) arabic);
    StringBuilder builder = new StringBuilder();
    while (letter.Count > 0)
    {
      int number = letter.Pop();
      PdfNumbersConvertor.AppendChar(builder, number);
    }
    return builder.ToString();
  }

  private static string GenerateNumber(ref int value, int magnitude, string letter)
  {
    StringBuilder stringBuilder = new StringBuilder();
    while (value >= magnitude)
    {
      value -= magnitude;
      stringBuilder.Append(letter);
    }
    return stringBuilder.ToString();
  }

  private static Stack<int> ConvertToLetter(float arabic)
  {
    if ((double) arabic <= 0.0)
      throw new ArgumentOutOfRangeException(nameof (arabic), "Value can not be less 0");
    Stack<int> letter = new Stack<int>();
    while ((double) (int) arabic > 26.0)
    {
      float num = arabic % 26f;
      if ((double) num == 0.0)
      {
        arabic = (float) ((double) arabic / 26.0 - 1.0);
        num = 26f;
      }
      else
        arabic /= 26f;
      letter.Push((int) num);
    }
    if ((double) arabic > 0.0)
      letter.Push((int) arabic);
    return letter;
  }

  private static void AppendChar(StringBuilder builder, int number)
  {
    if (builder == null)
      throw new ArgumentNullException(nameof (builder));
    if (number <= 0 || number > 26)
      throw new ArgumentOutOfRangeException(nameof (number), "Value can not be less 0 and greater 26");
    char ch = (char) (64 /*0x40*/ + number);
    builder.Append(ch);
  }
}
