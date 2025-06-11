// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.IComboBoxShape
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

#nullable disable
namespace Syncfusion.XlsIO;

public interface IComboBoxShape : IShape, IParentApplication
{
  IRange ListFillRange { get; set; }

  IRange LinkedCell { get; set; }

  int SelectedIndex { get; set; }

  int DropDownLines { get; set; }

  bool Display3DShading { get; set; }

  string SelectedValue { get; }
}
