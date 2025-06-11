// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.Type4
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Primitives;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class Type4 : Function
{
  private PdfStream m_stream;
  private string[] m_filter;
  private int m_stackPointer;
  private List<PostScriptOperators> operators;
  private double[] m_stackValue;
  private int[] m_stackType;
  private int m_currentType;

  internal int ResultantValue => this.Range.Count / 2;

  internal string[] Filter
  {
    get => this.m_filter;
    set => this.m_filter = value;
  }

  public Type4(PdfDictionary dictionary)
    : base(dictionary)
  {
    this.m_stream = dictionary as PdfStream;
    this.SetFilterName(dictionary);
    if (this.m_stream == null)
      return;
    this.Load(this.m_stream);
  }

  private void Load(PdfStream stream)
  {
    this.operators = new List<PostScriptOperators>();
    byte[] decodedStream = this.GetDecodedStream(stream);
    this.ReadPostScriptOperator(Encoding.UTF8.GetString(decodedStream, 0, decodedStream.Length), out this.operators);
  }

  private byte[] GetDecodedStream(PdfStream stream)
  {
    MemoryStream memoryStream = new MemoryStream();
    if (this.Filter == null)
      return stream.Data;
    for (int index = 0; index < this.Filter.Length; ++index)
    {
      switch (this.Filter[index])
      {
        case "A85":
        case "ASCII85Decode":
          memoryStream = this.DecodeASCII85Stream(stream.InternalStream);
          memoryStream.Position = 0L;
          break;
        default:
          try
          {
            memoryStream = this.DecodeFlateStream(stream.InternalStream);
            break;
          }
          catch
          {
            break;
          }
      }
    }
    return memoryStream.ToArray();
  }

  protected override double[] PerformFunction(double[] clippedInputValues)
  {
    double[] numArray = new double[this.ResultantValue];
    this.resetStacks(clippedInputValues);
    this.PerformPostScriptFunction(this.operators);
    if (this.Domain.Count / 2 == 1)
    {
      int index1 = 0;
      for (int index2 = this.Range.Count / 2; index1 < index2; ++index1)
        numArray[index1] = this.m_stackValue[index1];
    }
    else
    {
      int index3 = 0;
      for (int index4 = this.Range.Count / 2; index3 < index4; ++index3)
        numArray[index3] = this.m_stackValue[index3];
    }
    return numArray;
  }

  private void SetFilterName(PdfDictionary dictionary)
  {
    if (!dictionary.Items.ContainsKey(new PdfName("Filter")))
      return;
    PdfName pdfName = dictionary.Items[new PdfName("Filter")] as PdfName;
    if (pdfName != (PdfName) null)
    {
      this.m_filter = new string[1];
      this.m_filter[0] = pdfName.Value;
    }
    else if (dictionary.Items[new PdfName("Filter")] is PdfArray pdfArray1)
    {
      this.m_filter = new string[pdfArray1.Count];
      for (int index = 0; index < pdfArray1.Count; ++index)
        this.m_filter[index] = (pdfArray1[index] as PdfName).Value;
    }
    else
    {
      if (!(dictionary.Items[new PdfName("Filter")] as PdfReferenceHolder != (PdfReferenceHolder) null))
        return;
      PdfArray pdfArray = (dictionary.Items[new PdfName("Filter")] as PdfReferenceHolder).Object as PdfArray;
      this.m_filter = new string[pdfArray.Count];
      for (int index = 0; index < pdfArray.Count; ++index)
        this.m_filter[index] = (pdfArray[index] as PdfName).Value;
    }
  }

  private MemoryStream DecodeFlateStream(MemoryStream encodedStream)
  {
    encodedStream.Position = 0L;
    encodedStream.ReadByte();
    encodedStream.ReadByte();
    DeflateStream deflateStream = new DeflateStream((Stream) encodedStream, CompressionMode.Decompress, true);
    byte[] buffer = new byte[4096 /*0x1000*/];
    MemoryStream memoryStream = new MemoryStream();
    while (true)
    {
      int count = deflateStream.Read(buffer, 0, 4096 /*0x1000*/);
      if (count > 0)
        memoryStream.Write(buffer, 0, count);
      else
        break;
    }
    return memoryStream;
  }

  private MemoryStream DecodeASCII85Stream(MemoryStream encodedStream)
  {
    byte[] buffer = new ASCII85().decode(encodedStream.ToArray());
    MemoryStream memoryStream = new MemoryStream(buffer, 0, buffer.Length, true, true);
    memoryStream.Position = 0L;
    return memoryStream;
  }

  private void ReadPostScriptOperator(string code, out List<PostScriptOperators> op)
  {
    List<PostScriptOperators> postScriptOperatorsList = new List<PostScriptOperators>();
    int num = 0;
    string str1 = "";
    if (code.Contains("\n"))
      code = code.Replace("\n", " ");
    if (code.Contains("  "))
      code = code.Replace("  ", " ");
    if (code.Length > 0 && code[0] == '{' && code[code.Length - 1] == '}')
      code = code.Substring(1, code.Length - 2);
    if (code.Length > 0 && code[0] == ' ')
      code = code.Substring(1, code.Length - 1);
    if (code.Length > 0 && code[code.Length - 1] == ' ')
      code = code.Substring(0, code.Length - 1);
    string[] strArray = code.Split(' ');
    for (int index = 0; index < strArray.Length; ++index)
    {
      if (strArray[index].Contains("{"))
        ++num;
      if (strArray[index].Contains("}"))
      {
        --num;
        if (num == 0)
        {
          string str2 = str1 + strArray[index];
          postScriptOperatorsList.Add(new PostScriptOperators(PostScriptOperatorTypes.OPERATOR, str2));
          str1 = "";
          continue;
        }
      }
      if (num > 0)
      {
        str1 = str1 + strArray[index] + " ";
      }
      else
      {
        char[] charArray = strArray[index].ToCharArray();
        try
        {
          switch (charArray[0])
          {
            case '+':
            case '-':
            case '0':
            case '1':
            case '2':
            case '3':
            case '4':
            case '5':
            case '6':
            case '7':
            case '8':
            case '9':
              postScriptOperatorsList.Add(new PostScriptOperators(PostScriptOperatorTypes.NUMBER, strArray[index]));
              continue;
            default:
              postScriptOperatorsList.Add(new PostScriptOperators(PostScriptOperatorTypes.OPERATOR, strArray[index]));
              continue;
          }
        }
        catch
        {
        }
      }
    }
    op = postScriptOperatorsList;
  }

  private double Pop()
  {
    --this.m_stackPointer;
    double num = 0.0;
    if (this.m_stackPointer >= 0)
    {
      num = this.m_stackValue[this.m_stackPointer];
      this.m_currentType = this.m_stackType[this.m_stackPointer];
    }
    return num;
  }

  private void Push(double a, int type)
  {
    if (this.m_stackPointer <= 99 && this.m_stackPointer >= 0)
    {
      this.m_stackValue[this.m_stackPointer] = a;
      this.m_stackType[this.m_stackPointer] = type;
    }
    ++this.m_stackPointer;
  }

  private void Copy(double a)
  {
    int length1 = (int) a;
    if (this.m_currentType != 2 || length1 <= 0)
      return;
    double[] numArray1 = new double[length1];
    int[] numArray2 = new int[length1];
    for (int index = 0; index < numArray1.Length; ++index)
    {
      numArray1[index] = this.Pop();
      numArray2[index] = this.m_currentType;
    }
    for (int length2 = numArray1.Length; length2 > 0; --length2)
      this.Push(numArray1[length2 - 1], numArray2[length2 - 1]);
    for (int length3 = numArray1.Length; length3 > 0; --length3)
      this.Push(numArray1[length3 - 1], numArray2[length3 - 1]);
  }

  private void Duplicate()
  {
    double a = this.Pop();
    int currentType = this.m_currentType;
    this.Push(a, currentType);
    this.Push(a, currentType);
  }

  private void Index()
  {
    int length1 = (int) this.Pop();
    if (length1 == 0)
    {
      this.Duplicate();
    }
    else
    {
      if (length1 <= 0)
        return;
      double[] numArray1 = new double[length1];
      int[] numArray2 = new int[length1];
      for (int index = 0; index < numArray1.Length; ++index)
      {
        numArray1[index] = this.Pop();
        numArray2[index] = this.m_currentType;
      }
      double a = this.Pop();
      int currentType = this.m_currentType;
      this.Push(a, currentType);
      for (int length2 = numArray1.Length; length2 > 0; --length2)
        this.Push(numArray1[length2 - 1], numArray2[length2 - 1]);
      this.Push(a, currentType);
    }
  }

  private void Rotate()
  {
    int length1 = (int) this.Pop();
    int num = (int) this.Pop();
    if (num > this.m_stackPointer)
      num = this.m_stackPointer;
    if (length1 > 0)
    {
      double[] numArray1 = new double[length1];
      int[] numArray2 = new int[length1];
      if (num - length1 <= 0)
        return;
      double[] numArray3 = new double[num - length1];
      int[] numArray4 = new int[num - length1];
      for (int index = 0; index < numArray1.Length; ++index)
      {
        numArray1[index] = this.Pop();
        numArray2[index] = this.m_currentType;
      }
      for (int index = 0; index < numArray3.Length; ++index)
      {
        numArray3[index] = this.Pop();
        numArray4[index] = this.m_currentType;
      }
      for (int length2 = numArray1.Length; length2 > 0; --length2)
        this.Push(numArray1[length2 - 1], numArray2[length2 - 1]);
      for (int length3 = numArray3.Length; length3 > 0; --length3)
        this.Push(numArray3[length3 - 1], numArray4[length3 - 1]);
    }
    else
    {
      if (length1 >= 0)
        return;
      int length4 = -length1;
      double[] numArray5 = new double[num - length4];
      int[] numArray6 = new int[num - length4];
      double[] numArray7 = new double[length4];
      int[] numArray8 = new int[length4];
      for (int index = 0; index < numArray5.Length; ++index)
      {
        numArray5[index] = this.Pop();
        numArray6[index] = this.m_currentType;
      }
      for (int index = 0; index < numArray7.Length; ++index)
      {
        numArray7[index] = this.Pop();
        numArray8[index] = this.m_currentType;
      }
      for (int length5 = numArray5.Length; length5 > 0; --length5)
        this.Push(numArray5[length5 - 1], numArray6[length5 - 1]);
      for (int length6 = numArray7.Length; length6 > 0; --length6)
        this.Push(numArray7[length6 - 1], numArray8[length6 - 1]);
    }
  }

  private void PerformPostScriptFunction(List<PostScriptOperators> op)
  {
    List<PostScriptOperators> op1 = new List<PostScriptOperators>();
    List<PostScriptOperators> op2 = new List<PostScriptOperators>();
    foreach (PostScriptOperators postScriptOperators in op)
    {
      if (postScriptOperators.Operatortype == PostScriptOperatorTypes.NUMBER)
      {
        float result;
        float.TryParse(postScriptOperators.Operand, out result);
        this.Push((double) result, 2);
      }
      else
      {
        if (postScriptOperators.Operand.Contains("{") && postScriptOperators.Operand.Contains("}"))
        {
          if (op1.Count == 0)
            this.ReadPostScriptOperator(postScriptOperators.Operand, out op1);
          else if (op2.Count == 0)
            this.ReadPostScriptOperator(postScriptOperators.Operand, out op2);
        }
        double num1;
        switch (postScriptOperators.Operand)
        {
          case "if":
            if (this.Pop() > 0.0)
              this.PerformPostScriptFunction(op1);
            op1.Clear();
            continue;
          case "ifelse":
            if (this.Pop() > 0.0)
              this.PerformPostScriptFunction(op1);
            else
              this.PerformPostScriptFunction(op2);
            op1.Clear();
            op2.Clear();
            continue;
          case "jz":
            this.Pop();
            num1 = this.Pop();
            continue;
          case "j":
            num1 = this.Pop();
            continue;
          case "abs":
            double a1 = this.Pop();
            if (a1 < 0.0)
            {
              this.Push(-a1, 2);
              continue;
            }
            this.Push(a1, 2);
            continue;
          case "add":
            this.Push(this.Pop() + this.Pop(), 2);
            continue;
          case "and":
            double num2 = this.Pop();
            int currentType1 = this.m_currentType;
            double num3 = this.Pop();
            int currentType2 = this.m_currentType;
            if (currentType1 == 2 && currentType2 == 2)
            {
              this.Push((double) ((int) num2 & (int) num3), 2);
              continue;
            }
            if (currentType1 == 4 && currentType2 == 4)
            {
              this.Push((double) ((int) num2 & (int) num3), 4);
              continue;
            }
            continue;
          case "atan":
            this.Push(Math.Atan(this.Pop()), 2);
            continue;
          case "bitshift":
            int a2 = (int) this.Pop();
            int currentType3 = this.m_currentType;
            int num4 = (int) this.Pop();
            int currentType4 = this.m_currentType;
            if (a2 > 0)
              a2 <<= num4;
            if (a2 < 0)
              a2 >>= -num4;
            this.Push((double) a2, 2);
            continue;
          case "ceiling":
            this.Push(Math.Ceiling(this.Pop()), this.m_currentType);
            continue;
          case "copy":
            this.Copy(this.Pop());
            continue;
          case "cos":
            this.Push(Math.Cos(this.Pop()), this.m_currentType);
            continue;
          case "cvi":
            this.Push((double) (int) this.Pop(), this.m_currentType);
            continue;
          case "cvr":
            this.Push(this.Pop(), 2);
            continue;
          case "div":
            double num5 = this.Pop();
            this.Push(this.Pop() / num5, 2);
            continue;
          case "dup":
            this.Duplicate();
            continue;
          case "eq":
            double num6 = this.Pop();
            if (this.Pop() == num6)
            {
              this.Push(1.0, 4);
              continue;
            }
            this.Push(0.0, 4);
            continue;
          case "exch":
            double a3 = this.Pop();
            int currentType5 = this.m_currentType;
            double a4 = this.Pop();
            int currentType6 = this.m_currentType;
            this.Push(a3, currentType5);
            this.Push(a4, currentType6);
            continue;
          case "exp":
            double y = this.Pop();
            this.Push(Math.Pow(this.Pop(), y), 2);
            continue;
          case "false":
            this.Push(0.0, 4);
            continue;
          case "floor":
            this.Push(Math.Floor(this.Pop()), 2);
            continue;
          case "ge":
            double num7 = this.Pop();
            this.Push(this.Pop() >= num7 ? 1.0 : 0.0, 4);
            continue;
          case "gt":
            double num8 = this.Pop();
            this.Push(this.Pop() > num8 ? 1.0 : 0.0, 4);
            continue;
          case "idiv":
            double num9 = this.Pop();
            this.Push(this.Pop() / num9, 2);
            continue;
          case "index":
            this.Index();
            continue;
          case "le":
            double num10 = this.Pop();
            this.Push(this.Pop() <= num10 ? 1.0 : 0.0, 4);
            continue;
          case "ln":
            this.Push(Math.Log(this.Pop()), 2);
            continue;
          case "log":
            this.Push(Math.Log(this.Pop()), 2);
            continue;
          case "lt":
            double num11 = this.Pop();
            this.Push(this.Pop() < num11 ? 1.0 : 0.0, 4);
            continue;
          case "mod":
            double num12 = this.Pop();
            this.Push(this.Pop() % num12, 2);
            continue;
          case "mul":
            double num13 = this.Pop();
            this.Push(this.Pop() * num13, 2);
            continue;
          case "ne":
            double num14 = this.Pop();
            this.Push(this.Pop() != num14 ? 1.0 : 0.0, 4);
            continue;
          case "neg":
            double num15 = this.Pop();
            this.Push(num15 != 0.0 ? -num15 : num15, 2);
            continue;
          case "not":
            double num16 = this.Pop();
            int currentType7 = this.m_currentType;
            if (num16 == 0.0 && currentType7 == 1)
            {
              this.Push(1.0, 4);
              continue;
            }
            if (num16 == 1.0 && currentType7 == 1)
            {
              this.Push(0.0, 4);
              continue;
            }
            this.Push((double) ~(int) num16, 2);
            continue;
          case "or":
            double num17 = this.Pop();
            int currentType8 = this.m_currentType;
            double num18 = this.Pop();
            int currentType9 = this.m_currentType;
            if (currentType9 == 4 && currentType8 == 4)
            {
              this.Push((double) ((int) num18 | (int) num17), 4);
              continue;
            }
            if (currentType9 == 2 && currentType8 == 2)
            {
              this.Push((double) ((int) num18 | (int) num17), 2);
              continue;
            }
            continue;
          case "pop":
            this.Pop();
            continue;
          case "roll":
            this.Rotate();
            continue;
          case "round":
            this.Push(Math.Round(this.Pop()), 2);
            continue;
          case "sin":
            this.Push(Math.Sin(this.Pop()), 2);
            continue;
          case "sqrt":
            this.Push(Math.Sqrt(this.Pop()), 2);
            continue;
          case "sub":
            double num19 = this.Pop();
            this.Push(this.Pop() - num19, 2);
            continue;
          case "true":
            this.Push(1.0, 4);
            continue;
          case "truncate":
            double num20 = this.Pop();
            this.Push(num20 < 0.0 ? Math.Ceiling(num20) : Math.Floor(num20), 2);
            continue;
          case "xor":
            double num21 = this.Pop();
            int currentType10 = this.m_currentType;
            double num22 = this.Pop();
            int currentType11 = this.m_currentType;
            if (currentType11 == 4 && currentType10 == 4)
            {
              this.Push((double) ((int) num22 ^ (int) num21), 4);
              continue;
            }
            if (currentType11 == 2 && currentType10 == 2)
            {
              this.Push((double) ((int) num22 ^ (int) num21), 2);
              continue;
            }
            continue;
          default:
            if (postScriptOperators.Operatortype == PostScriptOperatorTypes.NUMBER)
            {
              float result;
              float.TryParse(postScriptOperators.Operand, out result);
              this.Push((double) result, 2);
              continue;
            }
            continue;
        }
      }
    }
  }

  public void resetStacks(double[] values)
  {
    this.m_stackValue = new double[100];
    this.m_stackType = new int[100];
    this.m_stackPointer = 0;
    for (int index = 0; index < 100; ++index)
      this.m_stackValue[index] = 0.0;
    for (int index = 0; index < 100; ++index)
      this.m_stackType[index] = 0;
    int length = values.Length;
    for (int index = 0; index < length; ++index)
      this.Push(values[index], 2);
  }
}
