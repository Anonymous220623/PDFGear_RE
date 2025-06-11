// Decompiled with JetBrains decompiler
// Type: Syncfusion.PdfViewer.Base.FontInterpreter
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections.Generic;
using System.Globalization;

#nullable disable
namespace Syncfusion.PdfViewer.Base;

internal class FontInterpreter
{
  private readonly Dictionary<string, BaseType1Font> fonts;
  private readonly PostScriptDict systemDict;

  internal OperandCollector Operands { get; private set; }

  internal Stack<PostScriptDict> DictionaryStack { get; private set; }

  internal Stack<PostScriptArray> ArrayStack { get; private set; }

  internal PostScriptDict CurrentDictionary => this.DictionaryStack.Peek();

  internal PostScriptArray CurrentArray => this.ArrayStack.Peek();

  internal PostScriptParser Reader { get; private set; }

  internal PostScriptDict SystemDict => this.systemDict;

  internal PostScriptArray RD { get; set; }

  internal PostScriptArray ND { get; set; }

  internal PostScriptArray NP { get; set; }

  public Dictionary<string, BaseType1Font> Fonts => this.fonts;

  private static object ParseOperand(Token token, PostScriptParser reader)
  {
    switch (token)
    {
      case Token.Operator:
        return (object) PostScriptOperators.FindOperator(reader.Result);
      case Token.Integer:
        return (object) FontInterpreter.ParseInt(reader.Result);
      case Token.Real:
        return (object) FontInterpreter.ParseReal(reader.Result);
      case Token.Name:
        return (object) reader.Result;
      case Token.ArrayStart:
        return (object) FontInterpreter.ParseArray(reader);
      case Token.Keyword:
        return PdfKeywords.GetValue(reader.Result);
      case Token.String:
        return (object) new PostScriptStrHelper(reader.Result);
      case Token.Boolean:
        return FontInterpreter.ParseBool(reader.Result);
      case Token.DictionaryStart:
        return (object) FontInterpreter.ParseDictionary(reader);
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

  private static PostScriptArray ParseArray(PostScriptParser reader)
  {
    PostScriptArray array = new PostScriptArray();
    Token token;
    while (!reader.EndOfFile && (token = reader.ReadToken()) != Token.ArrayEnd)
    {
      object operand = FontInterpreter.ParseOperand(token, reader);
      if (token != Token.Operator || operand != null)
        array.Add(operand);
    }
    return array;
  }

  private static PostScriptDict ParseDictionary(PostScriptParser reader)
  {
    PostScriptDict dictionary = new PostScriptDict();
    Token token;
    while (!reader.EndOfFile && (token = reader.ReadToken()) != Token.ArrayEnd)
    {
      switch (token)
      {
        case Token.Name:
        case Token.String:
          string result = reader.Result;
          dictionary[result] = FontInterpreter.ParseOperand(reader.ReadToken(), reader);
          continue;
        default:
          int num = (int) reader.ReadToken();
          continue;
      }
    }
    return dictionary;
  }

  public FontInterpreter()
  {
    this.fonts = new Dictionary<string, BaseType1Font>();
    this.systemDict = new PostScriptDict();
    this.InitializeSystemDict();
  }

  public void Execute(byte[] data)
  {
    this.Operands = new OperandCollector();
    this.DictionaryStack = new Stack<PostScriptDict>();
    this.ArrayStack = new Stack<PostScriptArray>();
    this.Reader = new PostScriptParser(data);
    while (!this.Reader.EndOfFile)
    {
      Token token = this.Reader.ReadToken();
      switch (token)
      {
        case Token.Operator:
          PostScriptOperators.FindOperator(this.Reader.Result).Execute(this);
          continue;
        case Token.Unknown:
          continue;
        default:
          this.Operands.AddLast(FontInterpreter.ParseOperand(token, this.Reader));
          continue;
      }
    }
  }

  internal void ExecuteProcedure(PostScriptArray proc)
  {
    if (proc == null)
      return;
    foreach (object obj in proc)
    {
      if (obj is PostScriptOperators postScriptOperators)
        postScriptOperators.Execute(this);
      else
        this.Operands.AddLast(obj);
    }
  }

  private void InitializeSystemDict()
  {
    this.systemDict["FontDirectory"] = (object) new PostScriptDict();
  }
}
