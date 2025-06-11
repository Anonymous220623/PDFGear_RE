// Decompiled with JetBrains decompiler
// Type: RegExt.FileAssociations.RegistryBatchHelper
// Assembly: RegExt, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: DBF16820-DB7E-4C29-8C11-DD0B94851B7F
// Assembly location: C:\Program Files\PDFgear\RegExt.exe

using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

#nullable disable
namespace RegExt.FileAssociations;

internal static class RegistryBatchHelper
{
  private static Encoding UTF8WithoutBOMEncoding = (Encoding) new UTF8Encoding(false);

  public static void DeleteRegistry(string[] paths)
  {
    string tempFileName = Path.GetTempFileName();
    try
    {
      using (FileStream fileStream = new FileStream(tempFileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
      {
        using (StreamWriter streamWriter = new StreamWriter((Stream) fileStream, RegistryBatchHelper.UTF8WithoutBOMEncoding, 4096 /*0x1000*/, true))
        {
          foreach (string path in paths)
          {
            if (!string.IsNullOrEmpty(path))
            {
              streamWriter.WriteLine(path + " [1 7 17]");
              streamWriter.WriteLine(path + " [DELETE]");
            }
          }
        }
      }
      RegistryBatchHelper.RunProcess("regini.exe", $"\"{tempFileName}\"");
    }
    catch
    {
    }
    finally
    {
      try
      {
        File.Delete(tempFileName);
      }
      catch
      {
      }
    }
  }

  public static void SetRegistryKeyValue(
    string path,
    Dictionary<string, string> keyValues,
    string role = "")
  {
    string tempFileName = Path.GetTempFileName();
    try
    {
      using (FileStream fileStream = new FileStream(tempFileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
      {
        using (StreamWriter streamWriter = new StreamWriter((Stream) fileStream, RegistryBatchHelper.UTF8WithoutBOMEncoding, 4096 /*0x1000*/, true))
        {
          streamWriter.WriteLine(path + " [1 7 17]");
          foreach (KeyValuePair<string, string> keyValue in keyValues)
            streamWriter.WriteLine($"{keyValue.Key} = \"{keyValue.Value}\"");
          if (!string.IsNullOrEmpty(role))
            streamWriter.WriteLine($"{path} [ {role} ]");
        }
      }
      RegistryBatchHelper.RunProcess("regini.exe", $"\"{tempFileName}\"");
    }
    catch
    {
    }
    finally
    {
      try
      {
        File.Delete(tempFileName);
      }
      catch
      {
      }
    }
  }

  private static void RunProcess(string name, string args)
  {
    try
    {
      Process.Start(new ProcessStartInfo(name, args)
      {
        UseShellExecute = false,
        WindowStyle = ProcessWindowStyle.Hidden,
        CreateNoWindow = true
      }).WaitForExit(1000);
    }
    catch
    {
    }
  }
}
