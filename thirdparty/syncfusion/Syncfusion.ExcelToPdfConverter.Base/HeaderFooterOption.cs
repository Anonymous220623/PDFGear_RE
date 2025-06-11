// Decompiled with JetBrains decompiler
// Type: Syncfusion.ExcelToPdfConverter.HeaderFooterOption
// Assembly: Syncfusion.ExcelToPDFConverter.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 4304B189-CB46-46CF-B5C1-2287263DCC93
// Assembly location: C:\Program Files\PDFgear\Syncfusion.ExcelToPdfConverter.Base.dll

#nullable disable
namespace Syncfusion.ExcelToPdfConverter;

public class HeaderFooterOption
{
  private bool _showFooter;
  private bool _showHeader;

  public HeaderFooterOption()
  {
    this._showFooter = true;
    this._showHeader = true;
  }

  public bool ShowHeader
  {
    get => this._showHeader;
    set => this._showHeader = value;
  }

  public bool ShowFooter
  {
    get => this._showFooter;
    set => this._showFooter = value;
  }
}
