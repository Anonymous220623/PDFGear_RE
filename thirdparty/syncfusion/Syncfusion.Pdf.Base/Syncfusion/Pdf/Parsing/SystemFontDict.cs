// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontDict
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontDict : SystemFontCFFTable
{
  private readonly int length;

  protected Dictionary<SystemFontOperatorDescriptor, SystemFontOperandsCollection> Data { get; private set; }

  public long SkipOffset => this.Offset + (long) this.length;

  private static bool IsOperator(byte b) => (byte) 0 <= b && b <= (byte) 21;

  private static bool IsTwoByteOperator(byte b) => b == (byte) 12;

  public SystemFontDict(SystemFontCFFFontFile file, long offset, int length)
    : base(file, offset)
  {
    this.length = length;
  }

  protected int GetInt(SystemFontOperatorDescriptor op)
  {
    SystemFontOperandsCollection operandsCollection;
    if (this.Data.TryGetValue(op, out operandsCollection))
      return operandsCollection.GetLastAsInt();
    return op.DefaultValue != null ? (int) op.DefaultValue : throw new ArgumentException("Operator not found");
  }

  protected SystemFontOperandsCollection GetOperands(SystemFontOperatorDescriptor op)
  {
    SystemFontOperandsCollection operands;
    if (!this.Data.TryGetValue(op, out operands))
      throw new ArgumentException("Operator not found");
    return operands;
  }

  protected double GetNumber(SystemFontOperatorDescriptor op)
  {
    SystemFontOperandsCollection operandsCollection;
    if (this.Data.TryGetValue(op, out operandsCollection))
      return operandsCollection.GetLastAsReal();
    return op.DefaultValue != null ? (double) op.DefaultValue : throw new ArgumentException("Operator not found");
  }

  protected SystemFontPostScriptArray GetArray(SystemFontOperatorDescriptor op)
  {
    SystemFontOperandsCollection operandsCollection;
    if (this.Data.TryGetValue(op, out operandsCollection))
    {
      SystemFontPostScriptArray array = new SystemFontPostScriptArray();
      while (operandsCollection.Count > 0)
        array.Add(operandsCollection.GetFirst());
      return array;
    }
    return op.DefaultValue != null ? (SystemFontPostScriptArray) op.DefaultValue : throw new ArgumentException("Operator not found");
  }

  public override void Read(SystemFontCFFFontReader reader)
  {
    byte[] numArray = new byte[this.length];
    reader.Read(numArray, this.length);
    SystemFontEncodedDataReader encodedDataReader = new SystemFontEncodedDataReader(numArray, SystemFontByteEncoding.DictByteEncodings);
    this.Data = new Dictionary<SystemFontOperatorDescriptor, SystemFontOperandsCollection>();
    SystemFontOperandsCollection operandsCollection = new SystemFontOperandsCollection();
    while (!encodedDataReader.EndOfFile)
    {
      byte b = encodedDataReader.Peek(0);
      if (SystemFontDict.IsOperator(b))
      {
        SystemFontOperatorDescriptor key;
        if (SystemFontDict.IsTwoByteOperator(b))
          key = new SystemFontOperatorDescriptor(SystemFontHelper.CreateByteArray(encodedDataReader.Read(), encodedDataReader.Read()));
        else
          key = new SystemFontOperatorDescriptor(encodedDataReader.Read());
        this.Data[key] = operandsCollection;
        operandsCollection = new SystemFontOperandsCollection();
      }
      else
        operandsCollection.AddLast(encodedDataReader.ReadOperand());
    }
  }
}
