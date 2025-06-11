// Decompiled with JetBrains decompiler
// Type: CommomLib.Commom.ProcessManager
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

#nullable disable
namespace CommomLib.Commom;

public static class ProcessManager
{
  public static void RunProcess(string cmd, string Parameters)
  {
    try
    {
      new Process()
      {
        StartInfo = {
          FileName = cmd,
          Arguments = Parameters,
          UseShellExecute = false
        }
      }.Start();
    }
    catch
    {
    }
  }

  public static string RunProcess(
    string cmd,
    string Parameters,
    string taskID = "id",
    bool needScreenOutput = true)
  {
    ProcessStartInfo startInfo = new ProcessStartInfo(cmd, Parameters);
    startInfo.UseShellExecute = false;
    startInfo.CreateNoWindow = true;
    startInfo.RedirectStandardOutput = false;
    startInfo.RedirectStandardError = needScreenOutput;
    string str1 = (string) null;
    StreamReader srOutput = (StreamReader) null;
    try
    {
      Process process = Process.Start(startInfo);
      string.IsNullOrEmpty(taskID);
      StringBuilder sbOutputInfo = new StringBuilder();
      srOutput = process.StandardError;
      Task.Factory.StartNew((Action) (() =>
      {
        while (true)
        {
          string str2 = srOutput.ReadLine();
          if (str2 != null)
            sbOutputInfo.AppendLine(str2);
          else
            break;
        }
      }))?.Wait();
      str1 = sbOutputInfo.ToString();
      process.WaitForExit();
      process.Close();
    }
    catch (Exception ex)
    {
      str1 = string.Empty;
    }
    finally
    {
      if (srOutput != null)
      {
        srOutput.Close();
        srOutput.Dispose();
      }
    }
    return str1;
  }
}
