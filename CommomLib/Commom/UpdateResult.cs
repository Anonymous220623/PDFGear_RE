// Decompiled with JetBrains decompiler
// Type: CommomLib.Commom.UpdateResult
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

#nullable disable
namespace CommomLib.Commom;

public class UpdateResult
{
  public bool HasUpdate { get; set; }

  public bool UpdateSuccess { get; set; }

  public string SetupFilePath { get; set; }

  public bool ShouldExit => this.HasUpdate && this.UpdateSuccess;

  public bool DownloadSuccess => !string.IsNullOrEmpty(this.SetupFilePath);
}
