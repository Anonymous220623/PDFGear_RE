// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontInterpreter
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections.Generic;
using System.Globalization;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontInterpreter
{
  private readonly Dictionary<string, SystemFontType1Font> fonts;
  private readonly SystemFontPostScriptDictionary systemDict;

  internal SystemFontOperandsCollection Operands { get; private set; }

  internal Stack<SystemFontPostScriptDictionary> DictionaryStack { get; private set; }

  internal Stack<SystemFontPostScriptArray> ArrayStack { get; private set; }

  internal SystemFontPostScriptDictionary CurrentDictionary => this.DictionaryStack.Peek();

  internal SystemFontPostScriptArray CurrentArray => this.ArrayStack.Peek();

  internal SystemFontPostScriptReader Reader { get; private set; }

  internal SystemFontPostScriptDictionary SystemDict => this.systemDict;

  internal SystemFontPostScriptArray RD { get; set; }

  internal SystemFontPostScriptArray ND { get; set; }

  internal SystemFontPostScriptArray NP { get; set; }

  public Dictionary<string, SystemFontType1Font> Fonts => this.fonts;

  private static object ParseOperand(SystemFontToken token, SystemFontPostScriptReader reader)
  {
    switch (token)
    {
      case SystemFontToken.Operator:
        return (object) SystemFontPostScriptOperator.FindOperator(reader.Result);
      case SystemFontToken.Integer:
        return (object) SystemFontInterpreter.ParseInt(reader.Result);
      case SystemFontToken.Real:
        return (object) SystemFontInterpreter.ParseReal(reader.Result);
      case SystemFontToken.Name:
        return (object) reader.Result;
      case SystemFontToken.ArrayStart:
        return (object) SystemFontInterpreter.ParseArray(reader);
      case SystemFontToken.Keyword:
        return SystemFontKeywords.GetValue(reader.Result);
      case SystemFontToken.String:
        return (object) new SystemFontPostScriptString(reader.Result);
      case SystemFontToken.Boolean:
        return SystemFontInterpreter.ParseBool(reader.Result);
      case SystemFontToken.DictionaryStart:
        return (object) SystemFontInterpreter.ParseDictionary(reader);
      default:
        return (object) null;
    }
  }

  private static object ParseBool(string b) => (object) (b == "true");

  private static int ParseInt(string str)
  {
    return int.Parse(str, (IFormatProvider) CultureInfo.InvariantCulture.NumberFormat);
  }

  private static double ParseReal(string str)
  {
    return double.Parse(str, (IFormatProvider) CultureInfo.InvariantCulture.NumberFormat);
  }

  private static SystemFontPostScriptArray ParseArray(SystemFontPostScriptReader reader)
  {
    SystemFontPostScriptArray array = new SystemFontPostScriptArray();
    SystemFontToken token;
    while (!reader.EndOfFile && (token = reader.ReadToken()) != SystemFontToken.ArrayEnd)
    {
      object operand = SystemFontInterpreter.ParseOperand(token, reader);
      if (token != SystemFontToken.Operator || operand != null)
        array.Add(operand);
    }
    return array;
  }

  private static SystemFontPostScriptDictionary ParseDictionary(SystemFontPostScriptReader reader)
  {
    SystemFontPostScriptDictionary dictionary = new SystemFontPostScriptDictionary();
    SystemFontToken systemFontToken;
    while (!reader.EndOfFile && (systemFontToken = reader.ReadToken()) != SystemFontToken.ArrayEnd)
    {
      switch (systemFontToken)
      {
        case SystemFontToken.Name:
        case SystemFontToken.String:
          string result = reader.Result;
          dictionary[result] = SystemFontInterpreter.ParseOperand(reader.ReadToken(), reader);
          continue;
        default:
          int num = (int) reader.ReadToken();
          continue;
      }
    }
    return dictionary;
  }

  public SystemFontInterpreter()
  {
    this.fonts = new Dictionary<string, SystemFontType1Font>();
    this.systemDict = new SystemFontPostScriptDictionary();
    this.InitializeSystemDict();
  }

  public void Execute(byte[] data)
  {
    this.Operands = new SystemFontOperandsCollection();
    this.DictionaryStack = new Stack<SystemFontPostScriptDictionary>();
    this.ArrayStack = new Stack<SystemFontPostScriptArray>();
    this.Reader = new SystemFontPostScriptReader(data);
    while (!this.Reader.EndOfFile)
    {
      SystemFontToken token = this.Reader.ReadToken();
      switch (token)
      {
        case SystemFontToken.Operator:
          SystemFontPostScriptOperator.FindOperator(this.Reader.Result).Execute(this);
          continue;
        case SystemFontToken.Unknown:
          continue;
        default:
          this.Operands.AddLast(SystemFontInterpreter.ParseOperand(token, this.Reader));
          continue;
      }
    }
  }

  internal void ExecuteProcedure(SystemFontPostScriptArray proc)
  {
    if (proc == null)
      return;
    foreach (object obj in proc)
    {
      if (obj is SystemFontPostScriptOperator postScriptOperator)
        postScriptOperator.Execute(this);
      else
        this.Operands.AddLast(obj);
    }
  }

  private void InitializeSystemDict()
  {
    this.systemDict["FontDirectory"] = (object) new SystemFontPostScriptDictionary();
  }
}
