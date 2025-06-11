// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.IDataValidation
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Interfaces;
using System;

#nullable disable
namespace Syncfusion.XlsIO;

public interface IDataValidation : IParentApplication, IOptimizedUpdate
{
  string PromptBoxTitle { get; set; }

  string PromptBoxText { get; set; }

  string ErrorBoxTitle { get; set; }

  string ErrorBoxText { get; set; }

  string FirstFormula { get; set; }

  DateTime FirstDateTime { get; set; }

  string SecondFormula { get; set; }

  DateTime SecondDateTime { get; set; }

  ExcelDataType AllowType { get; set; }

  ExcelDataValidationComparisonOperator CompareOperator { get; set; }

  bool IsListInFormula { get; set; }

  bool IsEmptyCellAllowed { get; set; }

  bool IsSuppressDropDownArrow { get; set; }

  bool ShowPromptBox { get; set; }

  bool ShowErrorBox { get; set; }

  int PromptBoxHPosition { get; set; }

  int PromptBoxVPosition { get; set; }

  bool IsPromptBoxVisible { get; set; }

  bool IsPromptBoxPositionFixed { get; set; }

  ExcelErrorStyle ErrorStyle { get; set; }

  string[] ListOfValues { get; set; }

  IRange DataRange { get; set; }
}
