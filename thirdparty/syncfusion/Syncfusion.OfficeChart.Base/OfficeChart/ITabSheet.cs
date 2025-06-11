// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.ITabSheet
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System.Drawing;

#nullable disable
namespace Syncfusion.OfficeChart;

internal interface ITabSheet : IParentApplication
{
  OfficeKnownColors TabColor { get; set; }

  Color TabColorRGB { get; set; }

  IOfficeChartShapes Charts { get; }

  IWorkbook Workbook { get; }

  IShapes Shapes { get; }

  bool IsRightToLeft { get; set; }

  bool IsSelected { get; }

  int TabIndex { get; }

  string Name { get; set; }

  OfficeWorksheetVisibility Visibility { get; set; }

  ITextBoxes TextBoxes { get; }

  string CodeName { get; }

  bool ProtectContents { get; }

  bool ProtectDrawingObjects { get; }

  bool ProtectScenarios { get; }

  OfficeSheetProtection Protection { get; }

  bool IsPasswordProtected { get; }

  void Activate();

  void Select();

  void Unselect();

  void Protect(string password);

  void Protect(string password, OfficeSheetProtection options);

  void Unprotect(string password);
}
