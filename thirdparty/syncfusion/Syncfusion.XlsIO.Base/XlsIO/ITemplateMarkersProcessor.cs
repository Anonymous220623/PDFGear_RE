// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.ITemplateMarkersProcessor
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

#nullable disable
namespace Syncfusion.XlsIO;

public interface ITemplateMarkersProcessor
{
  void ApplyMarkers();

  void ApplyMarkers(UnknownVariableAction action);

  void AddVariable(string strName, object variable);

  void AddVariable(string strName, object variable, VariableTypeAction variableTypeAction);

  void RemoveVariable(string strName);

  bool ContainsVariable(string strName);

  IConditionalFormats CreateConditionalFormats(IRange range);

  string MarkerPrefix { get; set; }

  char ArgumentSeparator { get; set; }
}
