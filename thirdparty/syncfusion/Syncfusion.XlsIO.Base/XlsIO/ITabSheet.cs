// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.ITabSheet
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System.Drawing;

#nullable disable
namespace Syncfusion.XlsIO;

public interface ITabSheet : IParentApplication
{
  ExcelKnownColors TabColor { get; set; }

  Color TabColorRGB { get; set; }

  IChartShapes Charts { get; }

  IPictures Pictures { get; }

  IWorkbook Workbook { get; }

  IShapes Shapes { get; }

  bool IsRightToLeft { get; set; }

  bool IsSelected { get; }

  int TabIndex { get; }

  string Name { get; set; }

  WorksheetVisibility Visibility { get; set; }

  ITextBoxes TextBoxes { get; }

  ICheckBoxes CheckBoxes { get; }

  IOptionButtons OptionButtons { get; }

  IComboBoxes ComboBoxes { get; }

  string CodeName { get; }

  bool ProtectContents { get; }

  bool ProtectDrawingObjects { get; }

  bool ProtectScenarios { get; }

  ExcelSheetProtection Protection { get; }

  bool IsPasswordProtected { get; }

  int Zoom { get; set; }

  void Activate();

  void Select();

  void Unselect();

  void Protect(string password);

  void Protect(string password, ExcelSheetProtection options);

  void Unprotect(string password);
}
