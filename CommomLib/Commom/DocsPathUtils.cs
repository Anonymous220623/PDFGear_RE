// Decompiled with JetBrains decompiler
// Type: CommomLib.Commom.DocsPathUtils
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

#nullable disable
namespace CommomLib.Commom;

public static class DocsPathUtils
{
  public static string GetLocalJsonPath()
  {
    string str = Path.Combine(UtilManager.GetLocalCachePath(), "Config");
    if (!Directory.Exists(str))
      Directory.CreateDirectory(str);
    return Path.Combine(str, "conv_docs.json");
  }

  public static bool WriteFilesPathJson(string oprateType, object arg, object password = null)
  {
    try
    {
      FilesArgsModel filesArgsModel = new FilesArgsModel(oprateType);
      switch (arg)
      {
        case string[] strArray when strArray.Length == 1:
          if (string.IsNullOrWhiteSpace(strArray[0]))
            return false;
          filesArgsModel.AddOneFile(strArray[0], (string) password);
          break;
        case string[] _:
          foreach (string path in arg as string[])
          {
            if (!string.IsNullOrWhiteSpace(path))
              filesArgsModel.AddOneFile(path, (string) password);
          }
          break;
        case IEnumerable<string> source:
          List<string> list = source.ToList<string>();
          int count = list.Count;
          using (List<string>.Enumerator enumerator = list.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              string current = enumerator.Current;
              filesArgsModel.AddOneFile(current);
            }
            break;
          }
      }
      using (FileStream fileStream = new FileStream(DocsPathUtils.GetLocalJsonPath(), FileMode.Create, FileAccess.ReadWrite))
      {
        using (StreamWriter streamWriter = new StreamWriter((Stream) fileStream))
        {
          string str = JsonConvert.SerializeObject((object) filesArgsModel, Formatting.Indented);
          streamWriter.Write(str);
          streamWriter.Close();
        }
        fileStream.Close();
      }
    }
    catch
    {
      return false;
    }
    return true;
  }

  public static string GetFilesPathJson(string oprateType, object arg)
  {
    FilesArgsModel filesArgsModel = new FilesArgsModel(oprateType);
    switch (arg)
    {
      case string _:
        if (string.IsNullOrWhiteSpace((string) arg))
          return "";
        filesArgsModel.AddOneFile((string) arg);
        break;
      case List<string> _:
        List<string> stringList = arg as List<string>;
        if (stringList.Count == 0)
          return "";
        using (List<string>.Enumerator enumerator = stringList.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            string current = enumerator.Current;
            filesArgsModel.AddOneFile(current);
          }
          break;
        }
    }
    return JsonConvert.SerializeObject((object) filesArgsModel, Formatting.Indented);
  }

  public static FilesArgsModel ReadFilesPathJson(string jsonpath)
  {
    using (StreamReader streamReader = new StreamReader(jsonpath))
      return JsonConvert.DeserializeObject<FilesArgsModel>(streamReader.ReadToEnd());
  }
}
