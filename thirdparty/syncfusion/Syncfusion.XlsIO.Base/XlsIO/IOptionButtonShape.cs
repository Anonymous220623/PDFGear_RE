// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.IOptionButtonShape
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

#nullable disable
namespace Syncfusion.XlsIO;

public interface IOptionButtonShape : ITextBoxShape, ITextBox, IShape, IParentApplication
{
  ExcelCheckState CheckState { get; set; }

  bool IsFirstButton { get; }

  bool Display3DShading { get; set; }

  IRange LinkedCell { get; set; }
}
