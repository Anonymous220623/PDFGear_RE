// Decompiled with JetBrains decompiler
// Type: CommomLib.Commom.HistoryManager
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

using CommomLib.Config;

#nullable disable
namespace CommomLib.Commom;

public static class HistoryManager
{
  public static FilesArgsModel ReadHistory()
  {
    FilesArgsModel filesArgsModel;
    ConfigUtils.TryGet<FilesArgsModel>("HistoryFiles", out filesArgsModel);
    return filesArgsModel;
  }

  public static void UpdateHistory(FilesArgsModel content)
  {
    ConfigUtils.TrySet<FilesArgsModel>("HistoryFiles", content);
  }

  public static void UpdateHistory(string path, bool delete = false)
  {
    FilesArgsModel filesArgsModel = HistoryManager.ReadHistory();
    if (filesArgsModel == null)
    {
      filesArgsModel = new FilesArgsModel("unknow");
      filesArgsModel.AddOneFile(path);
    }
    else if (delete)
      filesArgsModel.RemoveOneFile(path);
    else
      filesArgsModel.AddOneFile(path);
    ConfigUtils.TrySet<FilesArgsModel>("HistoryFiles", filesArgsModel);
  }
}
