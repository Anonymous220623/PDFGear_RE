// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontPostScriptOperator
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal abstract class SystemFontPostScriptOperator
{
  internal const string Array = "array";
  internal const string Begin = "begin";
  internal const string ClearToMark = "cleartomark";
  internal const string CloseFile = "closefile";
  internal const string CurrentDict = "currentdict";
  internal const string CurrentFile = "currentfile";
  internal const string Def = "def";
  internal const string DefineFont = "definefont";
  internal const string Dict = "dict";
  internal const string Dup = "dup";
  internal const string EExec = "eexec";
  internal const string End = "end";
  internal const string Exch = "exch";
  internal const string For = "for";
  internal const string Get = "get";
  internal const string Index = "index";
  internal const string Put = "put";
  internal const string RD = "RD";
  internal const string RDAlternate = "-|";
  internal const string Mark = "mark";
  internal const string ND = "ND";
  internal const string NDAlternate = "|-";
  internal const string NoAccess = "noaccess";
  internal const string NP = "NP";
  internal const string NPAlternate = "|";
  internal const string ReadString = "readstring";
  internal const string String = "string";
  internal const string Pop = "pop";
  internal const string Copy = "copy";
  internal const string SystemDict = "systemdict";
  internal const string Known = "known";
  internal const string If = "if";
  internal const string IfElse = "ifelse";
  internal const string FontDirectory = "FontDirectory";
  private static Dictionary<string, SystemFontPostScriptOperator> operators;

  public static bool IsOperator(string str)
  {
    return SystemFontPostScriptOperator.operators.ContainsKey(str);
  }

  public static SystemFontPostScriptOperator FindOperator(string op)
  {
    SystemFontPostScriptOperator postScriptOperator;
    return !SystemFontPostScriptOperator.operators.TryGetValue(op, out postScriptOperator) ? (SystemFontPostScriptOperator) null : postScriptOperator;
  }

  private static void InitializeOperators()
  {
    SystemFontPostScriptOperator.operators = new Dictionary<string, SystemFontPostScriptOperator>();
    SystemFontPostScriptOperator.operators["array"] = (SystemFontPostScriptOperator) new Syncfusion.Pdf.Parsing.Array();
    SystemFontPostScriptOperator.operators["begin"] = (SystemFontPostScriptOperator) new SystemFontBegin();
    SystemFontPostScriptOperator.operators["cleartomark"] = (SystemFontPostScriptOperator) new SystemFontClearToMark();
    SystemFontPostScriptOperator.operators["closefile"] = (SystemFontPostScriptOperator) new SystemFontCloseFile();
    SystemFontPostScriptOperator.operators["currentdict"] = (SystemFontPostScriptOperator) new SystemFontCurrentDict();
    SystemFontPostScriptOperator.operators["currentfile"] = (SystemFontPostScriptOperator) new SystemFontCurrentFile();
    SystemFontPostScriptOperator.operators["def"] = (SystemFontPostScriptOperator) new SystemFontDef();
    SystemFontPostScriptOperator.operators["definefont"] = (SystemFontPostScriptOperator) new SystemFontDefineFont();
    SystemFontPostScriptOperator.operators["dict"] = (SystemFontPostScriptOperator) new SystemFontPostScriptDict();
    SystemFontPostScriptOperator.operators["dup"] = (SystemFontPostScriptOperator) new SystemFontDup();
    SystemFontPostScriptOperator.operators["eexec"] = (SystemFontPostScriptOperator) new SystemFontEExec();
    SystemFontPostScriptOperator.operators["end"] = (SystemFontPostScriptOperator) new SystemFontEnd();
    SystemFontPostScriptOperator.operators["exch"] = (SystemFontPostScriptOperator) new SystemFontExch();
    SystemFontPostScriptOperator.operators["for"] = (SystemFontPostScriptOperator) new SystemFontFor();
    SystemFontPostScriptOperator.operators["get"] = (SystemFontPostScriptOperator) new SystemFontGet();
    SystemFontPostScriptOperator.operators["index"] = (SystemFontPostScriptOperator) new SystemFontPostScriptIndex();
    SystemFontPostScriptOperator.operators["mark"] = (SystemFontPostScriptOperator) new SystemFontMark();
    SystemFontPostScriptOperator.operators["ND"] = (SystemFontPostScriptOperator) new SystemFontND();
    SystemFontPostScriptOperator.operators["|-"] = (SystemFontPostScriptOperator) new SystemFontND();
    SystemFontPostScriptOperator.operators["noaccess"] = (SystemFontPostScriptOperator) new SystemFontNoAccess();
    SystemFontPostScriptOperator.operators["NP"] = (SystemFontPostScriptOperator) new SystemFontNP();
    SystemFontPostScriptOperator.operators["|"] = (SystemFontPostScriptOperator) new SystemFontNP();
    SystemFontPostScriptOperator.operators["pop"] = (SystemFontPostScriptOperator) new SystemFontPops();
    SystemFontPostScriptOperator.operators["put"] = (SystemFontPostScriptOperator) new SystemFontPut();
    SystemFontPostScriptOperator.operators["RD"] = (SystemFontPostScriptOperator) new SystemFontRD();
    SystemFontPostScriptOperator.operators["-|"] = (SystemFontPostScriptOperator) new SystemFontRD();
    SystemFontPostScriptOperator.operators["readstring"] = (SystemFontPostScriptOperator) new SystemFontReadString();
    SystemFontPostScriptOperator.operators["string"] = (SystemFontPostScriptOperator) new SystemFontString();
    SystemFontPostScriptOperator.operators["copy"] = (SystemFontPostScriptOperator) new SystemFontCopy();
    SystemFontPostScriptOperator.operators["systemdict"] = (SystemFontPostScriptOperator) new SystemFontSystemDict();
    SystemFontPostScriptOperator.operators["known"] = (SystemFontPostScriptOperator) new SystemFontKnown();
    SystemFontPostScriptOperator.operators["if"] = (SystemFontPostScriptOperator) new SystemFontIf();
    SystemFontPostScriptOperator.operators["ifelse"] = (SystemFontPostScriptOperator) new SystemFontIfElse();
    SystemFontPostScriptOperator.operators["FontDirectory"] = (SystemFontPostScriptOperator) new SystemFontFontDirectory();
  }

  static SystemFontPostScriptOperator() => SystemFontPostScriptOperator.InitializeOperators();

  public abstract void Execute(SystemFontInterpreter interpreter);
}
