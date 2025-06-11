// Decompiled with JetBrains decompiler
// Type: CommomLib.Commom.FilesArgsModel
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

using Newtonsoft.Json;
using System;
using System.Collections.Generic;

#nullable disable
namespace CommomLib.Commom;

public class FilesArgsModel
{
  [JsonConstructor]
  public FilesArgsModel()
  {
  }

  public string ConvertType { get; set; }

  public List<OpenedInfo> FilesPath { get; set; } = new List<OpenedInfo>();

  public void AddOneFile(string path, string password = null)
  {
    OpenedInfo openedInfo = this.FilesPath.Find((Predicate<OpenedInfo>) (e => e.FilePath == path));
    if (openedInfo != null)
      this.FilesPath.Remove(openedInfo);
    string str = ((DateTimeOffset) DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc)).UtcTicks.ToString();
    this.FilesPath.Insert(0, new OpenedInfo()
    {
      FilePath = path,
      OpenDate = str,
      Password = password
    });
  }

  public void RemoveOneFile(string path)
  {
    OpenedInfo openedInfo = this.FilesPath.Find((Predicate<OpenedInfo>) (e => e.FilePath == path));
    if (openedInfo == null)
      return;
    this.FilesPath.Remove(openedInfo);
  }

  public FilesArgsModel(string convertType) => this.ConvertType = convertType;
}
