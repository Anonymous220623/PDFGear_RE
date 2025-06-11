// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.ImportFormSettings
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Parsing;

public class ImportFormSettings
{
  private DataFormat dataFormat;
  internal bool AsPerSpecification = true;
  private string formName = string.Empty;
  private bool ignoreErrors;

  public DataFormat DataFormat
  {
    get => this.dataFormat;
    set => this.dataFormat = value;
  }

  public string FormName
  {
    get => this.formName;
    set => this.formName = value;
  }

  public bool IgnoreErrors
  {
    get => this.ignoreErrors;
    set => this.ignoreErrors = value;
  }
}
