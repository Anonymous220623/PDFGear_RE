// Decompiled with JetBrains decompiler
// Type: Tesseract.EngineConfig
// Assembly: Tesseract, Version=4.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: C5D5562D-D917-402B-968F-9F8B28C3D951
// Assembly location: D:\PDFGear\bin\Tesseract.dll

#nullable disable
namespace Tesseract;

public class EngineConfig
{
  private string _dataPath;
  private string _language;

  public string DataPath
  {
    get => this._dataPath;
    set => this._dataPath = value;
  }

  public string Language
  {
    get => this._language;
    set => this._language = value;
  }
}
