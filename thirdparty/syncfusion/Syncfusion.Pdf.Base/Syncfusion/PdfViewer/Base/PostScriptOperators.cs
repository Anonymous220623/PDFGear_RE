// Decompiled with JetBrains decompiler
// Type: Syncfusion.PdfViewer.Base.PostScriptOperators
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.PdfViewer.Base;

internal abstract class PostScriptOperators
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
  private static Dictionary<string, PostScriptOperators> operators;

  public static bool IsOperator(string str) => PostScriptOperators.operators.ContainsKey(str);

  public static PostScriptOperators FindOperator(string op)
  {
    PostScriptOperators postScriptOperators;
    return !PostScriptOperators.operators.TryGetValue(op, out postScriptOperators) ? (PostScriptOperators) null : postScriptOperators;
  }

  private static void InitializeOperators()
  {
    PostScriptOperators.operators = new Dictionary<string, PostScriptOperators>();
    PostScriptOperators.operators["array"] = (PostScriptOperators) new Syncfusion.PdfViewer.Base.Array();
    PostScriptOperators.operators["begin"] = (PostScriptOperators) new Syncfusion.PdfViewer.Base.Begin();
    PostScriptOperators.operators["cleartomark"] = (PostScriptOperators) new Syncfusion.PdfViewer.Base.ClearToMark();
    PostScriptOperators.operators["closefile"] = (PostScriptOperators) new Syncfusion.PdfViewer.Base.CloseFile();
    PostScriptOperators.operators["currentdict"] = (PostScriptOperators) new Syncfusion.PdfViewer.Base.CurrentDict();
    PostScriptOperators.operators["currentfile"] = (PostScriptOperators) new Syncfusion.PdfViewer.Base.CurrentFile();
    PostScriptOperators.operators["def"] = (PostScriptOperators) new Syncfusion.PdfViewer.Base.Def();
    PostScriptOperators.operators["definefont"] = (PostScriptOperators) new Syncfusion.PdfViewer.Base.DefineFont();
    PostScriptOperators.operators["dict"] = (PostScriptOperators) new Syncfusion.PdfViewer.Base.Dict();
    PostScriptOperators.operators["dup"] = (PostScriptOperators) new Syncfusion.PdfViewer.Base.Dup();
    PostScriptOperators.operators["eexec"] = (PostScriptOperators) new Syncfusion.PdfViewer.Base.EExec();
    PostScriptOperators.operators["end"] = (PostScriptOperators) new Syncfusion.PdfViewer.Base.End();
    PostScriptOperators.operators["exch"] = (PostScriptOperators) new Syncfusion.PdfViewer.Base.Exch();
    PostScriptOperators.operators["for"] = (PostScriptOperators) new Syncfusion.PdfViewer.Base.For();
    PostScriptOperators.operators["get"] = (PostScriptOperators) new Syncfusion.PdfViewer.Base.Get();
    PostScriptOperators.operators["index"] = (PostScriptOperators) new Syncfusion.PdfViewer.Base.Index();
    PostScriptOperators.operators["mark"] = (PostScriptOperators) new Syncfusion.PdfViewer.Base.Mark();
    PostScriptOperators.operators["ND"] = (PostScriptOperators) new Syncfusion.PdfViewer.Base.ND();
    PostScriptOperators.operators["|-"] = (PostScriptOperators) new Syncfusion.PdfViewer.Base.ND();
    PostScriptOperators.operators["noaccess"] = (PostScriptOperators) new Syncfusion.PdfViewer.Base.NoAccess();
    PostScriptOperators.operators["NP"] = (PostScriptOperators) new Syncfusion.PdfViewer.Base.NP();
    PostScriptOperators.operators["|"] = (PostScriptOperators) new Syncfusion.PdfViewer.Base.NP();
    PostScriptOperators.operators["pop"] = (PostScriptOperators) new Pops();
    PostScriptOperators.operators["put"] = (PostScriptOperators) new Syncfusion.PdfViewer.Base.Put();
    PostScriptOperators.operators["RD"] = (PostScriptOperators) new Syncfusion.PdfViewer.Base.RD();
    PostScriptOperators.operators["-|"] = (PostScriptOperators) new Syncfusion.PdfViewer.Base.RD();
    PostScriptOperators.operators["readstring"] = (PostScriptOperators) new Syncfusion.PdfViewer.Base.ReadString();
    PostScriptOperators.operators["string"] = (PostScriptOperators) new String();
    PostScriptOperators.operators["copy"] = (PostScriptOperators) new Syncfusion.PdfViewer.Base.Copy();
    PostScriptOperators.operators["systemdict"] = (PostScriptOperators) new Syncfusion.PdfViewer.Base.SystemDict();
    PostScriptOperators.operators["known"] = (PostScriptOperators) new Syncfusion.PdfViewer.Base.Known();
    PostScriptOperators.operators["if"] = (PostScriptOperators) new Syncfusion.PdfViewer.Base.If();
    PostScriptOperators.operators["ifelse"] = (PostScriptOperators) new Syncfusion.PdfViewer.Base.IfElse();
    PostScriptOperators.operators["FontDirectory"] = (PostScriptOperators) new Syncfusion.PdfViewer.Base.FontDirectory();
  }

  static PostScriptOperators() => PostScriptOperators.InitializeOperators();

  public abstract void Execute(FontInterpreter interpreter);
}
