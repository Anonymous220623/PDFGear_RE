// Decompiled with JetBrains decompiler
// Type: Syncfusion.Calculate.LinearRegression
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Calculate;

internal class LinearRegression : IDisposable
{
  private const char BMARKER = '\u0092';
  internal CalcEngine Engine;
  private string TIC = "\"";

  internal LinearRegression(CalcEngine engine) => this.Engine = engine;

  internal void ComputeLinest(
    double[] y,
    double[] x,
    out double m,
    out double b,
    out string errorValue)
  {
    errorValue = string.Empty;
    m = 0.0;
    b = 1.0;
    int length = x.GetLength(0);
    double num1 = 0.0;
    double num2 = 0.0;
    double num3 = 0.0;
    double num4 = 0.0;
    for (int index = 0; index < x.Length; ++index)
    {
      if (x[index].ToString() != double.NaN.ToString() && y[index].ToString() != double.NaN.ToString())
      {
        num1 += x[index] * y[index];
        num2 += x[index];
        num3 += y[index];
        num4 += x[index] * x[index];
      }
      else
        errorValue = !this.Engine.RethrowLibraryComputationExceptions ? this.Engine.ErrorStrings[1].ToString() : throw new ArgumentException(this.Engine.FormulaErrorStrings[this.Engine.bad_formula]);
    }
    b = (num1 - num2 * num3 / (double) length) / (num4 - num2 * num2 / (double) length);
    double num5 = 0.0;
    double num6 = 0.0;
    double num7 = 0.0;
    double num8 = 0.0;
    for (int index = 0; index < length; ++index)
    {
      num7 += x[index];
      num8 += y[index];
    }
    double num9 = num7 / (double) length;
    double num10 = num8 / (double) length;
    for (int index = 0; index < length; ++index)
    {
      double num11 = x[index] - num9;
      num5 += num11 * (y[index] - num10);
      num6 += num11 * num11;
    }
    m = num10 - num5 / num6 * num9;
  }

  internal void SplitRange(string range, ref string rangeValue, ref string logicalValue)
  {
    for (int index = 0; index < range.Length; ++index)
    {
      range = range.Replace('\u0092'.ToString(), string.Empty);
      while (index != range.Length && char.IsDigit(range[index]) | range[index] == ':' | range[index] == '!' | this.Engine.IsUpper(range[index]))
        rangeValue += (string) (object) range[index++];
      while (index != range.Length && range[index] == '"' | char.IsLetter(range[index]) | char.IsDigit(range[index]) | range[index] == ',' | range[index] == '~')
        logicalValue += (string) (object) range[index++];
    }
  }

  internal void PerfromArrayMultiplication(double[,] a, double[,] b, out double[,] mult)
  {
    int length1 = a.GetLength(0);
    a.GetLength(1);
    int length2 = b.GetLength(0);
    int length3 = b.GetLength(1);
    mult = new double[length1, length3];
    for (int index1 = 0; index1 <= length1 - 1; ++index1)
    {
      for (int index2 = 0; index2 <= length3 - 1; ++index2)
      {
        for (int index3 = 0; index3 <= length2 - 1; ++index3)
          mult[index1, index2] += a[index1, index3] * b[index3, index2];
      }
    }
  }

  internal void ComputeMultipleRegression(
    double[] y,
    double[,] X,
    int row,
    int col,
    out double b,
    out string errorValue,
    out double[] coefficients)
  {
    errorValue = string.Empty;
    b = 1.0;
    double[,] mult1 = new double[row, row];
    double[,] numArray = new double[col, row];
    for (int index1 = 0; index1 < col; ++index1)
    {
      for (int index2 = 0; index2 < row; ++index2)
        numArray[index1, index2] = X[index2, index1];
    }
    this.PerfromArrayMultiplication(numArray, X, out mult1);
    double[,] iMatrix = new double[mult1.GetLength(0), mult1.GetLength(1)];
    this.Engine.GetCofactor(mult1, out iMatrix);
    double[,] mult2 = new double[iMatrix.GetLength(0), numArray.GetLength(1)];
    this.PerfromArrayMultiplication(iMatrix, numArray, out mult2);
    int length1 = mult2.GetLength(0);
    y.GetLength(0);
    int length2 = mult2.GetLength(1);
    coefficients = new double[length1];
    for (int index3 = 0; index3 <= length1 - 1; ++index3)
    {
      for (int index4 = 0; index4 <= length2 - 1; ++index4)
        coefficients[index3] += mult2[index3, index4] * y[index4];
    }
    b = coefficients[coefficients.Length - 1];
  }

  internal string ComputeXArg(
    double[] y,
    double[] x,
    string arg,
    double b,
    double m,
    bool padXValues,
    string errorValue,
    out double[] coefficients)
  {
    int length = arg.IndexOf(':');
    int num1 = this.Engine.RowIndex(arg.Substring(0, length));
    int num2 = this.Engine.RowIndex(arg.Substring(length + 1));
    int num3 = this.Engine.ColIndex(arg.Substring(0, length));
    int num4 = this.Engine.ColIndex(arg.Substring(length + 1, arg.Length - length - 1));
    coefficients = new double[num4 + 1];
    int num5 = num4 > num3 ? num4 - num3 + 1 : num3 - num4 + 1;
    if (num5 == 1)
      return !this.Engine.RethrowLibraryComputationExceptions ? this.Engine.ErrorStrings[2].ToString() : throw new ArgumentException(this.Engine.FormulaErrorStrings[this.Engine.bad_formula]);
    if (x.Length / num5 == y.Length)
    {
      int num6 = 0;
      double[,] X = new double[y.GetLength(0), num4 + 1];
      for (int col = num3; col <= num4; ++col)
      {
        arg = $"{RangeInfo.GetAlphaLabel(col)}{(object) num1}:{RangeInfo.GetAlphaLabel(col)}{(object) num2}";
        x = padXValues ? new double[y.GetLength(0)] : this.Engine.GetDoubleArray(arg);
        for (int index1 = num6; index1 <= col; ++index1)
        {
          for (int index2 = 0; index2 < y.GetLength(0); ++index2)
            X[index2, index1] = index1 != 0 ? x[index2] : 1.0;
        }
        num6 = col + 1;
        if (padXValues)
        {
          for (int index = 0; index < x.Length - 1; ++index)
            x[index] = (double) (index + 1);
        }
        if (col == num4)
        {
          this.ComputeMultipleRegression(y, X, y.GetLength(0), num4 + 1, out b, out errorValue, out coefficients);
          return errorValue != string.Empty ? errorValue : b.ToString();
        }
      }
    }
    return b.ToString();
  }

  internal string ComputeXArithmetic(
    double[] y,
    double[] x,
    string arg,
    double b,
    double m,
    bool padXValues,
    string errorValue,
    out double[] coefficients)
  {
    string empty1 = string.Empty;
    string empty2 = string.Empty;
    this.SplitRange(arg, ref empty1, ref empty2);
    coefficients = (double[]) null;
    if (empty1 != string.Empty && empty2 != string.Empty && empty2.IndexOfAny(this.Engine.tokens) > -1)
    {
      string[] strArray1 = empty2.Replace('\u0092'.ToString(), string.Empty).Split(new string[1]
      {
        this.TIC
      }, StringSplitOptions.RemoveEmptyEntries);
      List<string> stringList1 = new List<string>();
      List<string> stringList2 = new List<string>();
      int index1 = -1;
      int index2;
      for (int index3 = 0; index3 < strArray1.Length; index3 = index2 + 1)
      {
        stringList1.Add(strArray1[index3]);
        ++index1;
        if (index1 > 0 && stringList1[index1].Length != stringList1[index1 - 1].Length)
          errorValue = !this.Engine.RethrowLibraryComputationExceptions ? this.Engine.ErrorStrings[1].ToString() : throw new ArgumentException(this.Engine.FormulaErrorStrings[this.Engine.bad_formula]);
        index2 = index3 + 1;
        stringList2.Add(strArray1[index2]);
      }
      double[] numArray = (double[]) null;
      for (int index4 = 0; index4 < stringList1.Count; ++index4)
      {
        x = padXValues ? new double[y.GetLength(0)] : this.Engine.GetDoubleArray(empty1);
        if (padXValues)
        {
          for (int index5 = 0; index5 < x.Length - 1; ++index5)
            x[index5] = (double) (index5 + 1);
        }
        string[] strArray2 = this.Engine.SplitArgsPreservingQuotedCommas(stringList1[index4]);
        double[,] X = new double[y.GetLength(0), strArray2.Length + 1];
        int num = 0;
        for (int index6 = 0; index6 <= strArray2.Length - 1; ++index6)
        {
          if (numArray != null && index6 == strArray2.Length - 1)
          {
            x = numArray;
          }
          else
          {
            x = padXValues ? new double[y.GetLength(0)] : this.Engine.GetDoubleArray(empty1);
            if (padXValues)
            {
              for (int index7 = 0; index7 < x.Length - 1; ++index7)
                x[index7] = (double) (index7 + 1);
            }
          }
          for (int index8 = 0; index8 <= x.Length - 1; ++index8)
          {
            if (x[index8].ToString() != double.NaN.ToString() && y[index8].ToString() != double.NaN.ToString())
              x[index8] = double.Parse(this.Engine.ComputedValue($"n{x[index8].ToString()}n{strArray2[index6]}{stringList2[index4]}"));
          }
          if (index6 == strArray2.Length - 1 && stringList2.Count > 1)
            numArray = x;
          if (num <= strArray2.Length)
          {
            for (int index9 = num; index9 <= index6 + 1; ++index9)
            {
              for (int index10 = 0; index10 < y.GetLength(0); ++index10)
                X[index10, index9] = index9 != 0 ? x[index10] : 1.0;
            }
            num = index6 + 2;
          }
          if (index6 == strArray2.Length - 1 && index4 == stringList2.Count - 1)
          {
            this.ComputeMultipleRegression(y, X, y.GetLength(0), X.GetLength(1), out b, out errorValue, out coefficients);
            return errorValue != string.Empty ? errorValue : b.ToString();
          }
        }
      }
    }
    return b.ToString();
  }

  public void Dispose() => this.Engine = (CalcEngine) null;
}
