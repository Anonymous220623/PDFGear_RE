// Decompiled with JetBrains decompiler
// Type: CommomLib.Config.ConfigModels.AutoSaveFileModel
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

#nullable disable
namespace CommomLib.Config.ConfigModels;

public class AutoSaveFileModel
{
  public string Guid { get; set; }

  public string SoruceFileFullName { get; set; }

  public string FileName { get; set; }

  public int CreatePid { get; set; }

  public string TempFileName { get; set; }

  public string LastSaveTime { get; set; }
}
