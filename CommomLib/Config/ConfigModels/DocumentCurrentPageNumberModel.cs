// Decompiled with JetBrains decompiler
// Type: CommomLib.Config.ConfigModels.DocumentCurrentPageNumberModel
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

using Newtonsoft.Json;

#nullable disable
namespace CommomLib.Config.ConfigModels;

internal class DocumentCurrentPageNumberModel
{
  [JsonProperty("file")]
  public string FilePath { get; set; }

  [JsonProperty("idx")]
  public int PageNumber { get; set; }
}
