// Decompiled with JetBrains decompiler
// Type: Syncfusion.Calculate.ArrayParser
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

#nullable disable
namespace Syncfusion.Calculate;

internal class ArrayParser : IDisposable
{
  private const char BMARKER = '\u0092';
  private string validFunctionNameChars = "_";
  private char[] markers = new char[5]
  {
    '+',
    '-',
    '*',
    '/',
    '&'
  };
  private int length;
  internal ArrayParser.ArrayDelegate GetArrayRecordPosition;
  internal CalcEngine Engine;

  internal ArrayParser(CalcEngine engine) => this.Engine = engine;

  internal string[] SplitString(string formula)
  {
    string pattern = "([-+*/&])";
    formula = formula.Replace(" ", "").ToUpper();
    this.Engine.MarkNamedRanges(ref formula);
    return Regex.Split(formula, pattern);
  }

  internal string CalculateArraySize(
    string substring,
    ref int height,
    ref int width,
    ref int minHeight,
    ref int minWidth)
  {
    int length = substring.IndexOf(":");
    string empty = string.Empty;
    minWidth = 0;
    minHeight = 0;
    if (this.Engine.IsCellReference(substring))
    {
      if (length > -1)
      {
        int num1 = this.Engine.RowIndex(substring.Substring(0, length));
        int num2 = this.Engine.RowIndex(substring.Substring(length + 1));
        int col1 = this.Engine.ColIndex(substring.Substring(0, length));
        int col2 = this.Engine.ColIndex(substring.Substring(length + 1));
        int num3 = num2 - num1 + 1;
        int num4 = col2 - col1 + 1;
        if (num3 > height)
          num2 -= num3 - height;
        if (num4 > width)
          col2 -= num4 - width;
        if (num3 < height)
          minHeight = num3;
        if (num4 < width)
          minWidth = num4;
        substring = $"{RangeInfo.GetAlphaLabel(col1)}{num1.ToString()}:{RangeInfo.GetAlphaLabel(col2)}{num2.ToString()}";
      }
      else
      {
        minHeight = 1;
        minWidth = 1;
      }
    }
    else
    {
      int num5 = 1;
      int num6 = 1;
      if (substring.Contains(",") && !substring.Contains(";"))
        num6 = this.Engine.SplitArgsPreservingQuotedCommas(substring).Length;
      else if (substring.Contains(";") && !substring.Contains(","))
        num5 = this.Engine.SplitArguments(substring, ';').Length;
      if (num5 < height)
        minHeight = num5;
      if (num6 < width)
        minWidth = num6;
    }
    return substring;
  }

  internal List<string[]> ResizeCellRange(string formula, string originalFormula)
  {
    List<string[]> strArrayList = new List<string[]>();
    string[] substrings = this.SplitString(formula);
    string empty = string.Empty;
    int height = this.GetHeight(substrings);
    int width = this.GetWidth(substrings);
    int minHeight = 0;
    int minWidth = 0;
    this.length = height * width;
    for (int index1 = 0; index1 < substrings.Length; ++index1)
    {
      if (substrings[index1].IndexOfAny(this.markers) > -1 || !this.Engine.IsCellReference(substrings[index1]))
      {
        if (substrings[index1].StartsWith("[") && substrings[index1].EndsWith("]"))
        {
          substrings[index1] = substrings[index1].Substring(1, substrings[index1].Length - 2);
          if (substrings[index1].Contains(","))
          {
            substrings[index1] = this.CalculateArraySize(substrings[index1], ref height, ref width, ref minHeight, ref minWidth);
            strArrayList.Add(this.Engine.SplitArgsPreservingQuotedCommas(substrings[index1]));
          }
          else if (substrings[index1].Contains(";"))
          {
            substrings[index1] = this.CalculateArraySize(substrings[index1], ref height, ref width, ref minHeight, ref minWidth);
            strArrayList.Add(this.Engine.SplitArguments(substrings[index1], ';'));
          }
        }
        else
        {
          strArrayList.Add(new string[1]
          {
            substrings[index1]
          });
          if (substrings[index1].IndexOfAny(this.markers) < 0)
            minHeight = 1;
        }
      }
      else
      {
        if (!char.IsLetterOrDigit(substrings[index1][0 + 1]) && this.Engine.IsCellReference(substrings[index1]))
          substrings[index1] = this.Engine.GetCellsFromArgs(substrings[index1], false)[0];
        substrings[index1] = this.CalculateArraySize(substrings[index1], ref height, ref width, ref minHeight, ref minWidth);
        strArrayList.Add(this.Engine.GetCellsFromArgs(substrings[index1]));
      }
      if (strArrayList[index1].Length < this.length && substrings[index1].IndexOfAny(this.markers) < 0)
      {
        string[] array = strArrayList[index1];
        int length = strArrayList[index1].Length;
        Array.Resize<string>(ref array, this.length);
        if (minWidth != 0 && minWidth < width)
        {
          int index2 = 0;
          int num = 0;
          int index3 = 0;
          while (index3 < this.length)
          {
            for (; num < width && index3 < this.length; ++index3)
            {
              if (index2 >= length)
                index2 = 0;
              array[index3] = strArrayList[index1][index2];
              ++num;
            }
            ++index2;
            num = 0;
          }
        }
        if (minHeight != 0 && minHeight < height)
        {
          int index4 = 0;
          for (int index5 = length; index5 < this.length; ++index5)
          {
            if (index4 >= length)
              index4 = 0;
            array[index5] = strArrayList[index1][index4];
            ++index4;
          }
        }
        strArrayList[index1] = array;
      }
    }
    return strArrayList;
  }

  internal string Parse(string formula, string originalFormula)
  {
    char ch = '\u007F';
    string args = formula;
    string str1 = string.Empty;
    string empty = string.Empty;
    string[] strArray = (string[]) null;
    if (formula.IndexOfAny(this.markers) < 0)
      return formula;
    if (args.Contains(CalcEngine.ParseArgumentSeparator.ToString()))
    {
      args = args.Replace("{", "\"{").Replace("}", "}\"");
      strArray = this.Engine.SplitArgsPreservingQuotedCommas(args);
      foreach (string str2 in strArray)
      {
        if (str2.IndexOfAny(this.markers) > -1 && !str2.Contains("\""))
        {
          args = str2;
          str1 = args;
        }
      }
      if (str1 == string.Empty && (strArray.Length != 1 && strArray[0].IndexOfAny(this.markers) < 0 || strArray[0].Contains("\"{") && strArray[0].Contains("}\"") || args.Contains(ch.ToString())))
        return formula;
    }
    List<string[]> strArrayList = this.ResizeCellRange(args.Replace("(", "").Replace(")", "").Replace("\"", ""), originalFormula);
    List<string> stringList = new List<string>();
    string str3 = string.Empty;
    string str4 = string.Empty;
    if (this.length == 0)
      this.length = strArrayList[0].Length;
    int count = strArrayList.Count;
    for (int index1 = 0; index1 < this.length; ++index1)
    {
      int num = index1;
      string str5 = "";
      for (int index2 = 0; index2 < count; ++index2)
      {
        if (strArrayList[index2].Length == 1)
          index1 = 0;
        str5 += strArrayList[index2][index1];
        index1 = num;
      }
      stringList.Add(str5);
    }
    foreach (string str6 in stringList)
      str4 = str4 + str6 + (object) CalcEngine.ParseArgumentSeparator;
    if (strArray != null && strArray.Length > 1)
    {
      for (int index = 0; index < strArray.Length; ++index)
      {
        if (strArray[index].Equals(str1))
        {
          if (index == 0)
            str3 = $"{str3}({str4.Substring(0, str4.Length - 1)}";
          else if (index != strArray.Length - 1)
            str3 = str3 + (object) CalcEngine.ParseArgumentSeparator + str4.Substring(0, str4.Length - 1);
          else
            str3 = $"{str3}{(object) CalcEngine.ParseArgumentSeparator}{str4.Substring(0, str4.Length - 1)})";
        }
        else
          str3 = index != 0 ? str3 + (object) CalcEngine.ParseArgumentSeparator + strArray[index] : str3 + strArray[index];
      }
    }
    else
      str3 = $"{str3}({str4.Substring(0, str4.Length - 1)})";
    return str3.Replace("\"", "");
  }

  internal string ParseLibraryFormula(string formula)
  {
    if (formula.Length > 0)
      formula = formula.Substring(2, formula.Length - 3);
    string str1 = formula;
    formula = this.Engine.CheckForNamedRange(formula);
    if (formula.StartsWith("{") && formula.EndsWith("}") || this.Engine.findNamedRange && this.Engine.IsRange(formula))
      return this.ParseDimensionalArray(formula);
    Hashtable strings = this.Engine.SaveStrings(ref formula);
    formula = formula.Replace(" ", "").Replace("{", "\"[").Replace("}", "]\"");
    int num1 = formula.IndexOf(')');
    if (num1 == -1)
    {
      formula = this.Parse(formula, str1);
    }
    else
    {
      for (; num1 > -1; num1 = formula.IndexOf(')'))
      {
        int num2 = 0;
        int num3;
        for (num3 = num1 - 1; num3 > -1 && (formula[num3] != '(' || num2 != 0); --num3)
        {
          if (formula[num3] == ')')
            ++num2;
        }
        if (num3 == -1)
          return this.Engine.ErrorStrings[0].ToString();
        int index = num3 - 1;
        while (index > -1 && (char.IsLetterOrDigit(formula[index]) || this.validFunctionNameChars.IndexOf(formula[index]) > -1 || formula[index].Equals(CalcEngine.ParseDecimalSeparator)))
          --index;
        int length = num3 - index - 1;
        if (length > 0 && this.Engine.LibraryFunctions[(object) formula.Substring(index + 1, length)] != null)
        {
          string str2 = this.Parse(formula.Substring(num3, num1 - num3 + 1), str1);
          formula = formula.Substring(0, index + 1) + formula.Substring(index + 1, length) + str2.Replace('(', '{').Replace(')', '}') + formula.Substring(num1 + 1);
        }
        else if (length == 0)
        {
          string str3 = string.Empty;
          if (num3 > 0)
            str3 = formula.Substring(0, num3);
          string str4 = str3 + (object) '{' + formula.Substring(num3 + 1, num1 - num3 - 1) + (object) '}';
          if (num1 < formula.Length)
            str4 += formula.Substring(num1 + 1);
          formula = str4;
          if (!str4.Contains("(") && !str4.Contains(")"))
            formula = this.Parse(formula, str1).Replace('(', '{').Replace(')', '}');
        }
        else
          formula = formula.Replace('(', '{').Replace(')', '}');
      }
    }
    if (strings != null && strings.Count > 0)
      this.Engine.SetStrings(ref formula, strings);
    formula = formula.Replace('{', '(').Replace('}', ')');
    if (this.IsMultiCellArray(str1))
      formula = this.ParseMultiCellArray(formula, str1);
    return formula;
  }

  internal string ComputeInteriorFunction(string arg, string label, int computedLevel)
  {
    string str = string.Empty;
    switch (label)
    {
      case "LEN":
        str = this.ComputeLen(arg, computedLevel);
        break;
      case "ROW":
        str = this.ComputeRow(arg, computedLevel);
        break;
      case "COLUMN":
        str = this.ComputeColumn(arg, computedLevel);
        break;
      case "IF":
        str = this.ComputeIF(arg, computedLevel);
        break;
    }
    return computedLevel > 0 && label != "IF" ? $"{{{str}}}" : str;
  }

  internal bool IsMultiCellArray(string formula)
  {
    if (formula.StartsWith("(") && formula.EndsWith(")"))
      formula = formula.Substring(1, formula.Length - 2);
    bool flag = false;
    foreach (string str in this.SplitString(formula))
    {
      if (str.Contains("(") && !str.Contains(")") || !str.Contains("(") && str.Contains(")"))
      {
        flag = false;
        break;
      }
      if (this.Engine.IsRange(str.Replace("$", "")) || str.StartsWith("{") && str.EndsWith("}"))
        flag = true;
    }
    return flag;
  }

  internal string ParseMultiCellArray(string formula, string originalFormula)
  {
    string str = originalFormula;
    string[] substrings = this.SplitString(originalFormula);
    this.Engine.MarkNamedRanges(ref originalFormula);
    if (str != originalFormula)
      return this.ParseDimensionalArray(formula);
    if (this.Engine.cell != string.Empty)
    {
      int height = this.GetHeight(substrings);
      int width = this.GetWidth(substrings);
      int position = this.GetPosition(ref height, ref width);
      if (this.GetWidth(substrings) > width && position != -1 && position >= width)
        position += position / width * (this.GetWidth(substrings) - width);
      if (position >= 0)
      {
        if (formula.StartsWith("(") && formula.EndsWith(")"))
          formula = formula.Substring(1, formula.Length - 2);
        string[] strArray = this.Engine.SplitArgsPreservingQuotedCommas(formula);
        if (strArray.Length > position)
          formula = strArray[position];
      }
      else
        formula = this.Engine.ErrorStrings[0].ToString();
    }
    return formula;
  }

  internal string ParseDimensionalArray(string formula)
  {
    formula = formula.Replace(" ", "");
    string str1 = formula;
    if (formula.IndexOfAny(this.markers) > -1)
    {
      formula = formula.Replace("{", "\"[").Replace("}", "]\"");
      formula = this.Parse(formula, str1).Replace("(", "{").Replace(")", "}");
    }
    if (formula.StartsWith("{") && formula.EndsWith("}"))
      formula = formula.Substring(1, formula.Length - 2);
    if (formula.Length == 1)
      return formula;
    int length1 = formula.IndexOf(":");
    if (this.Engine.IsCellReference(formula) && length1 > -1)
    {
      string[] cellsFromArgs = this.Engine.GetCellsFromArgs(formula);
      string str2 = string.Empty;
      string str3 = ",";
      int num1 = this.Engine.RowIndex(formula.Substring(0, length1));
      int num2 = this.Engine.RowIndex(formula.Substring(length1 + 1));
      int num3 = this.Engine.ColIndex(formula.Substring(0, length1));
      int num4 = this.Engine.ColIndex(formula.Substring(length1 + 1));
      int height = num2 - num1 + 1;
      int width = num4 - num3 + 1;
      if (width == 1)
        str3 = ";";
      else if (height == 1)
        str3 = ",";
      foreach (string str4 in cellsFromArgs)
        str2 = str2 + str4 + str3;
      formula = str2.Substring(0, str2.Length - 1).Replace(" ", "");
      str1 = formula;
      if (height > 1 && width > 1)
        return this.ParseRangeArray(formula, height, width);
    }
    if (str1.Contains(",") && !str1.Contains(";"))
      return this.ParseHorizontalArray(formula);
    if (str1.Contains(";") && !str1.Contains(","))
      return this.ParseVerticalArray(formula);
    if (str1.IndexOfAny(this.markers) > -1)
    {
      string[] substrings = this.SplitString(str1);
      return this.ParseRangeArray(formula, this.GetHeight(substrings), this.GetWidth(substrings));
    }
    string[] strArray1 = this.Engine.SplitArguments(formula, ';');
    List<string[]> strArrayList = new List<string[]>();
    List<string> stringList = new List<string>();
    int length2 = strArray1.Length;
    int width1 = 0;
    foreach (string args in strArray1)
    {
      string[] strArray2 = this.Engine.SplitArgsPreservingQuotedCommas(args);
      strArrayList.Add(strArray2);
      width1 = strArray2.Length;
    }
    for (int index1 = 0; index1 < length2; ++index1)
    {
      for (int index2 = 0; index2 < width1; ++index2)
        stringList.Add(strArrayList[index1][index2]);
    }
    if (this.Engine.cell == string.Empty)
      return stringList[0];
    int num = width1;
    int position = this.GetPosition(ref length2, ref width1);
    if (num > width1 && position != -1 && position >= width1)
      position += position / width1 * (num - width1);
    formula = position <= -1 ? this.Engine.ErrorStrings[0].ToString() : stringList[position];
    return formula;
  }

  internal string ParseRangeArray(string formula, int height, int width)
  {
    string[] strArray = this.Engine.SplitArgsPreservingQuotedCommas(formula);
    if (this.Engine.cell == string.Empty)
      return strArray[0];
    int num = width;
    int position = this.GetPosition(ref height, ref width);
    if (num > width && position != -1 && position >= width)
      position += position / width * (num - width);
    formula = position <= -1 ? this.Engine.ErrorStrings[0].ToString() : strArray[position];
    return formula;
  }

  internal string ParseHorizontalArray(string formula)
  {
    int height = 1048576 /*0x100000*/;
    string[] strArray = this.Engine.SplitArgsPreservingQuotedCommas(formula);
    int length = strArray.Length;
    if (this.Engine.cell == string.Empty)
      return strArray[0];
    int position = this.GetPosition(ref height, ref length);
    int index = position > -1 ? position % length : position;
    formula = index <= -1 ? this.Engine.ErrorStrings[0].ToString() : strArray[index];
    return formula;
  }

  internal string ParseVerticalArray(string formula)
  {
    int width = 16384 /*0x4000*/;
    string[] strArray = this.Engine.SplitArguments(formula, ';');
    int length = strArray.Length;
    if (this.Engine.cell == string.Empty)
      return strArray[0];
    int position = this.GetPosition(ref length, ref width);
    int index = position > -1 ? position / width : position;
    formula = index <= -1 ? this.Engine.ErrorStrings[0].ToString() : strArray[index];
    return formula;
  }

  internal int GetHeight(string[] substrings)
  {
    int result1 = 0;
    string empty = string.Empty;
    int result2 = 0;
    substrings[0] = substrings[0].Replace("$", "");
    if (this.Engine.IsCellReference(substrings[0]))
      result1 = int.TryParse(this.Engine.ComputeRows(substrings[0]), out result1) ? result1 : 0;
    else if (substrings[0].Contains(",") && !substrings[0].Contains(";"))
      result1 = 1;
    else if (substrings[0].Contains(";") && !substrings[0].Contains(","))
      result1 = this.Engine.SplitArguments(substrings[0], ';').Length;
    for (int index = 0; index < substrings.Length; ++index)
    {
      if (result1 == 0)
        result1 = 1;
      substrings[index] = substrings[index].Replace("$", "");
      if (this.Engine.IsCellReference(substrings[index]))
        result2 = int.TryParse(this.Engine.ComputeRows(substrings[index]), out result2) ? result2 : 0;
      else if (substrings[index].Contains(",") && !substrings[index].Contains(";"))
        result2 = 1;
      else if (substrings[index].Contains(";") && !substrings[index].Contains(","))
        result2 = this.Engine.SplitArguments(substrings[index], ';').Length;
      if (result2 != 0 && (result1 == 1 || result2 < result1 && result2 != 1))
        result1 = result2;
    }
    return result1;
  }

  internal int GetWidth(string[] substrings)
  {
    int result1 = 0;
    string empty = string.Empty;
    int result2 = 0;
    substrings[0] = substrings[0].Replace("$", "");
    if (this.Engine.IsCellReference(substrings[0]))
      result1 = int.TryParse(this.Engine.ComputeColumns(substrings[0]), out result1) ? result1 : 0;
    else if (substrings[0].Contains(",") && !substrings[0].Contains(";"))
      result1 = this.Engine.SplitArgsPreservingQuotedCommas(substrings[0]).Length;
    else if (substrings[0].Contains(";") && !substrings[0].Contains(","))
      result1 = 1;
    for (int index = 0; index < substrings.Length; ++index)
    {
      substrings[index] = substrings[index].Replace("$", "");
      if (result1 == 0)
        result1 = 1;
      if (this.Engine.IsCellReference(substrings[index]))
        result2 = int.TryParse(this.Engine.ComputeColumns(substrings[index]), out result2) ? result2 : 0;
      else if (substrings[index].Contains(",") && !substrings[0].Contains(";"))
        result2 = this.Engine.SplitArgsPreservingQuotedCommas(substrings[index]).Length;
      else if (substrings[index].Contains(";") && !substrings[0].Contains(","))
        result2 = 1;
      if (result2 != 0 && (result1 == 1 || result2 < result1 && result2 != 1))
        result1 = result2;
    }
    return result1;
  }

  internal int GetPosition(ref int height, ref int width)
  {
    int row = this.Engine.RowIndex(this.Engine.cell);
    if (row == -1 && this.Engine.grid is ISheetData)
      row = ((ISheetData) this.Engine.grid).GetFirstRow();
    int col = this.Engine.ColIndex(this.Engine.cell);
    if (col == -1 && this.Engine.grid is ISheetData)
      col = ((ISheetData) this.Engine.grid).GetFirstColumn();
    return this.GetArrayRecordPosition(row, col, ref height, ref width, this.Engine.grid);
  }

  internal string ComputeLen(string arg, int computedLevel)
  {
    string empty1 = string.Empty;
    string len = string.Empty;
    string empty2 = string.Empty;
    if (arg.IndexOf(':') > -1 && computedLevel > 0)
    {
      foreach (string cellsFromArg in this.Engine.GetCellsFromArgs(arg))
      {
        string str1 = this.Engine.GetValueFromArg(cellsFromArg).Replace("\"", string.Empty);
        if (str1 != string.Empty)
        {
          string str2 = str1.Length.ToString();
          len = len + str2 + (object) CalcEngine.ParseArgumentSeparator;
        }
      }
      len = len.Remove(len.Length - 1);
    }
    else if (computedLevel > 0)
    {
      foreach (string preservingQuotedComma in this.Engine.SplitArgsPreservingQuotedCommas(arg))
      {
        string str = this.Engine.GetValueFromArg(preservingQuotedComma).Replace("\"", string.Empty).Length.ToString();
        len = len + str + (object) CalcEngine.ParseArgumentSeparator;
      }
      len = len.Remove(len.Length - 1);
    }
    else if (arg.IndexOf(':') > -1)
    {
      if (this.Engine.cell != string.Empty)
      {
        string[] cellsFromArgs = this.Engine.GetCellsFromArgs(arg);
        int result1;
        int height = int.TryParse(this.Engine.ComputeRows(arg), out result1) ? result1 : 0;
        int result2 = int.TryParse(this.Engine.ComputeColumns(arg), out result2) ? result2 : 0;
        int position = this.GetPosition(ref height, ref result2);
        if (position >= 0 && cellsFromArgs.Length > position)
        {
          string str = this.Engine.GetValueFromArg(cellsFromArgs[position]).Replace("\"", string.Empty);
          if (str != string.Empty)
            len = str.Length.ToString();
        }
        else
          len = this.Engine.ErrorStrings[0].ToString();
      }
      else
        len = string.Empty;
    }
    else
    {
      string str = this.Engine.GetValueFromArg(this.Engine.SplitArgsPreservingQuotedCommas(arg)[0]).Replace("\"", string.Empty);
      if (str != string.Empty)
        len = str.Length.ToString();
    }
    return len;
  }

  internal string ComputeRow(string arg, int computedLevel)
  {
    string str = string.Empty;
    arg = arg.Replace("\"", "");
    string[] cellsFromArgs = this.Engine.GetCellsFromArgs(arg);
    int num1 = this.Engine.RowIndex(cellsFromArgs[0].ToString());
    int num2 = this.Engine.RowIndex(cellsFromArgs[cellsFromArgs.Length - 1].ToString());
    string row;
    if (computedLevel > 0)
    {
      for (int index = num1; index <= num2; ++index)
        str = str + index.ToString() + (object) CalcEngine.ParseArgumentSeparator;
      row = str.Remove(str.Length - 1);
    }
    else if (this.Engine.cell != string.Empty)
    {
      string[] strArray = new string[num2 - num1 + 1];
      int index1 = 0;
      for (int index2 = num1; index2 <= num2; ++index2)
      {
        strArray[index1] = index2.ToString();
        ++index1;
      }
      int result1;
      int height = int.TryParse(this.Engine.ComputeRows(arg), out result1) ? result1 : 0;
      int result2;
      int width = int.TryParse(this.Engine.ComputeColumns(arg), out result2) ? result2 : 0;
      if (width == 1)
        width = 16384 /*0x4000*/;
      int position = this.GetPosition(ref height, ref width);
      int index3 = position > -1 ? position / width : position;
      row = index3 <= -1 ? this.Engine.ErrorStrings[0].ToString() : strArray[index3].ToString();
    }
    else
      row = string.Empty;
    return row;
  }

  internal string ComputeColumn(string arg, int computedLevel)
  {
    string str = string.Empty;
    arg = arg.Replace("\"", "");
    string[] cellsFromArgs = this.Engine.GetCellsFromArgs(arg);
    int num1 = this.Engine.ColIndex(cellsFromArgs[0].ToString());
    int num2 = this.Engine.ColIndex(cellsFromArgs[cellsFromArgs.Length - 1].ToString());
    string column;
    if (computedLevel > 0)
    {
      for (int index = num1; index <= num2; ++index)
        str = str + index.ToString() + (object) CalcEngine.ParseArgumentSeparator;
      column = str.Remove(str.Length - 1);
    }
    else if (this.Engine.cell != string.Empty)
    {
      string[] strArray = new string[num2 - num1 + 1];
      int index1 = 0;
      for (int index2 = num1; index2 <= num2; ++index2)
      {
        strArray[index1] = index2.ToString();
        ++index1;
      }
      int result1;
      int height = int.TryParse(this.Engine.ComputeRows(arg), out result1) ? result1 : 0;
      int result2;
      int width = int.TryParse(this.Engine.ComputeColumns(arg), out result2) ? result2 : 0;
      if (height == 1)
        height = 1048576 /*0x100000*/;
      int position = this.GetPosition(ref height, ref width);
      int index3 = position > -1 ? position % width : position;
      column = index3 <= -1 ? this.Engine.ErrorStrings[0].ToString() : strArray[index3].ToString();
    }
    else
      column = string.Empty;
    return column;
  }

  internal string ComputeIF(string arg, int computedLevel)
  {
    string strArray = string.Empty;
    arg = arg.Replace('\u0092'.ToString(), string.Empty);
    string[] ss = this.Engine.SplitArgsPreservingQuotedCommas(arg);
    if (ss.GetLength(0) <= 3)
    {
      for (int index1 = 0; index1 < ss[0].Length; ++index1)
      {
        string empty1 = string.Empty;
        string empty2 = string.Empty;
        string empty3 = string.Empty;
        while (index1 != ss[0].Length && char.IsDigit(ss[0][index1]) | ss[0][index1] == ':' | ss[0][index1] == '!' | this.Engine.IsUpper(ss[0][index1]))
          empty1 += (string) (object) ss[0][index1++];
        while (index1 != ss[0].Length && ss[0][index1] == '"' | char.IsLetter(ss[0][index1]) | char.IsDigit(ss[0][index1]))
          empty2 += (string) (object) ss[0][index1++];
        string[] cellsFromArgs = this.Engine.GetCellsFromArgs(empty1);
        if (computedLevel > 0)
        {
          for (int index2 = 0; index2 <= cellsFromArgs.Length - 1; ++index2)
            strArray = this.PerformLogicalTest(this.Engine.GetValueFromArg('\u0092'.ToString() + cellsFromArgs[index2] + empty2 + '\u0092'.ToString()), strArray, computedLevel, ss);
        }
        else if (this.Engine.cell != string.Empty)
          strArray = this.PerformLogicalTest(this.Engine.GetValueFromArg('\u0092'.ToString() + empty1 + empty2 + '\u0092'.ToString()), strArray, computedLevel, ss);
        strArray = strArray.Remove(strArray.Length - 1);
      }
    }
    else
      strArray = string.Empty;
    if (strArray != string.Empty)
      strArray = $"\"{strArray}\"";
    return strArray;
  }

  internal string PerformLogicalTest(
    string logicTest,
    string strArray,
    int computedLevel,
    string[] ss)
  {
    double result = 0.0;
    if (logicTest.ToUpper().Replace("\"", "").ToUpper().Equals("TRUE") || double.TryParse(logicTest, NumberStyles.Any, (IFormatProvider) null, out result) && result != 0.0)
    {
      logicTest = this.Engine.GetValueFromArg(ss[1]);
      if (string.IsNullOrEmpty(logicTest) && this.Engine.TreatStringsAsZero && computedLevel > 0)
        logicTest = "0";
      strArray = $"{strArray}{logicTest};";
    }
    else if (logicTest.ToUpper().Replace("\"", "").ToUpper().Equals("FALSE") || double.TryParse(logicTest, NumberStyles.Any, (IFormatProvider) null, out result) && result != 0.0)
    {
      logicTest = this.Engine.GetValueFromArg(ss[2]);
      if (string.IsNullOrEmpty(logicTest) && this.Engine.TreatStringsAsZero && computedLevel > 0)
        logicTest = "0";
      strArray = $"{strArray}{logicTest};";
    }
    return strArray;
  }

  internal string ComputeCountIF(
    string[] s1,
    char op,
    string criteria,
    bool isNumber,
    double compare,
    int computedLevel,
    int count)
  {
    Dictionary<string, int> dictionary = new Dictionary<string, int>();
    string str = string.Empty;
    for (int index1 = 0; index1 < criteria.Length; ++index1)
    {
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      string empty3 = string.Empty;
      criteria = criteria.Replace('\u0092'.ToString(), string.Empty);
      while (index1 != criteria.Length && char.IsDigit(criteria[index1]) | criteria[index1] == ':' | criteria[index1] == '!' | this.Engine.IsUpper(criteria[index1]))
        empty1 += (string) (object) criteria[index1++];
      while (index1 != criteria.Length && criteria[index1] == '"' | char.IsLetter(criteria[index1]) | char.IsDigit(criteria[index1]))
        empty2 += (string) (object) criteria[index1++];
      string[] cellsFromArgs = this.Engine.GetCellsFromArgs(empty1);
      for (int index2 = 0; index2 <= cellsFromArgs.Length - 1; ++index2)
      {
        int num = 0;
        string key = this.Engine.GetValueFromArg('\u0092'.ToString() + cellsFromArgs[index2] + empty2 + '\u0092'.ToString()).Replace("\"", string.Empty);
        if (!dictionary.ContainsKey(key))
        {
          for (int index3 = 0; index3 < count; ++index3)
          {
            if (this.Engine.CheckForCriteriaMatch(this.Engine.GetValueFromArg(s1[index3]).ToUpper(), op, key.ToUpper(), isNumber, compare))
              ++num;
          }
          dictionary.Add(key, num);
        }
        str = $"{str}{(dictionary.ContainsKey(key) ? dictionary[key].ToString() : num.ToString())};";
      }
    }
    return str.Remove(str.Length - 1);
  }

  public void Dispose() => this.Engine = (CalcEngine) null;

  internal delegate int ArrayDelegate(
    int row,
    int col,
    ref int height,
    ref int width,
    ICalcData calcData);
}
