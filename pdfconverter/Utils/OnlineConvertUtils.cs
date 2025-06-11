// Decompiled with JetBrains decompiler
// Type: pdfconverter.Utils.OnlineConvertUtils
// Assembly: pdfconverter, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 691C26A2-7651-46C3-AA3C-2839EDA9722B
// Assembly location: C:\Program Files\PDFgear\pdfconverter.exe

using CommomLib.Commom;
using Newtonsoft.Json;
using pdfconverter.Models;
using System;
using System.Buffers;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

#nullable disable
namespace pdfconverter.Utils;

public static class OnlineConvertUtils
{
  private static readonly string ServiceURLForAuth = "https://apiw.pdfgear.com/v1/auth";
  private static readonly string ServiceURLConvert = "https://apiw.pdfgear.com/v1/convert";

  public static async Task<OnlineAuthResponsModel> IsServiceOnline(ConnectModel model)
  {
    return await OnlineConvertUtils.IsServiceOnline(JsonConvert.SerializeObject((object) model), OnlineConvertUtils.ServiceURLForAuth).ConfigureAwait(false);
  }

  public static async Task<OnlineAuthResponsModel> IsServiceOnline(ConnectModel model, string url)
  {
    return await OnlineConvertUtils.IsServiceOnline(JsonConvert.SerializeObject((object) model), url).ConfigureAwait(false);
  }

  public static async Task<OnlineAuthResponsModel> IsServiceOnline(string strPara, string url)
  {
    try
    {
      string base64Aes = EncryptUtils.EncryptStringToBase64_Aes(strPara);
      return JsonConvert.DeserializeObject<OnlineAuthResponsModel>(await (await new HttpClient().SendAsync(new HttpRequestMessage(HttpMethod.Post, url)
      {
        Content = (HttpContent) new MultipartFormDataContent("----------------------------" + DateTime.Now.Ticks.ToString("x"))
        {
          {
            (HttpContent) new StringContent(base64Aes),
            "parameter"
          }
        }
      })).Content.ReadAsStringAsync());
    }
    catch
    {
      return new OnlineAuthResponsModel()
      {
        Success = false
      };
    }
  }

  public static async Task<bool> RequestConvertFile(
    string SrcFilePath,
    string DesFilePath,
    string _token,
    ConnectModel model,
    string fileNameWithoutExtension)
  {
    try
    {
      string base64Aes = EncryptUtils.EncryptStringToBase64_Aes(JsonConvert.SerializeObject((object) new ConvertPara()
      {
        converttype = model.convertType,
        token = _token
      }), EncryptUtils.key, EncryptUtils.iv);
      if (!File.Exists(SrcFilePath))
        return false;
      bool flag;
      using (FileStream stream = new FileStream(SrcFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
      {
        MultipartFormDataContent multipartFormDataContent = new MultipartFormDataContent("----------------------------" + DateTime.Now.Ticks.ToString("x"));
        if (stream.CanSeek)
          stream.Seek(0L, SeekOrigin.Begin);
        multipartFormDataContent.Add((HttpContent) new StreamContent((Stream) stream), "attachment", model.fileName);
        multipartFormDataContent.Add((HttpContent) new StringContent(base64Aes), "parameter");
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, OnlineConvertUtils.ServiceURLConvert);
        request.Content = (HttpContent) multipartFormDataContent;
        request.Headers.TryAddWithoutValidation("Authorization", "Bearer " + _token);
        flag = await OnlineConvertUtils.DownLoadFile(await (await new HttpClient().SendAsync(request)).Content.ReadAsStreamAsync(), DesFilePath, model, fileNameWithoutExtension);
      }
      return flag;
    }
    catch
    {
      return false;
    }
  }

  public static string GetRandomFileName() => Guid.NewGuid().ToString("N");

  private static async Task<bool> DownLoadFile(
    Stream stream,
    string OutPath,
    ConnectModel model,
    string fileNameWithoutExtension)
  {
    try
    {
      string extension = MapOnlineConverttype.GetOnlineExtensionStr(model.convertType);
      string DownLoadPath = OutPath;
      if (extension == ".zip")
        DownLoadPath = Path.Combine(OnlineConvertUtils.GetDataPath(), OnlineConvertUtils.GetRandomFileName() + ".zip");
      await OnlineConvertUtils.SaveFile(stream, DownLoadPath);
      return !(extension == ".zip") || OnlineConvertUtils.UnPackFileToDes(DownLoadPath, OutPath, model, fileNameWithoutExtension);
    }
    catch
    {
      return false;
    }
  }

  private static bool UnPackFileToDes(
    string sourceFile,
    string OutFolder,
    ConnectModel model,
    string fileNameWithoutExtension)
  {
    try
    {
      string str = Path.Combine(Directory.GetParent(sourceFile).FullName, Path.GetFileNameWithoutExtension(sourceFile));
      ZipFile.ExtractToDirectory(sourceFile, str);
      FileInfo[] files = new DirectoryInfo(str).GetFiles();
      Array.Sort<FileInfo>(files, (Comparison<FileInfo>) ((x, y) => OnlineConvertUtils.GetNum(x.Name) > OnlineConvertUtils.GetNum(y.Name) ? 1 : -1));
      foreach (FileInfo fileInfo in files)
      {
        string extension = Path.GetExtension(fileInfo.FullName);
        if (string.IsNullOrWhiteSpace(extension))
          return false;
        string newPath = OnlineConvertUtils.GetNewPath(fileNameWithoutExtension, extension, OutFolder, model.pageFrom, model.pageTo);
        fileInfo.CopyTo(newPath);
      }
      Directory.Delete(str, true);
      File.Delete(sourceFile);
      return true;
    }
    catch
    {
      return false;
    }
  }

  private static int GetNum(string name)
  {
    try
    {
      string[] strArray = name.Split('_');
      return int.Parse(strArray[strArray.Length - 1].Split('.')[0]);
    }
    catch
    {
      return 0;
    }
  }

  private static string GetNewPath(
    string fileName,
    string extention,
    string OutFolder,
    int from,
    int to)
  {
    string path2_1 = $"{fileName} {from}{extention}";
    string path;
    string path2_2;
    for (path = Path.Combine(OutFolder, path2_1); File.Exists(path); path = Path.Combine(OutFolder, path2_2))
    {
      ++from;
      path2_2 = $"{fileName} {from}{extention}";
    }
    return path;
  }

  private static async Task SaveFile(Stream stream, string LocalPath)
  {
    using (FileStream fileStream = File.Create(LocalPath))
    {
      if (stream.CanSeek)
        stream.Seek(0L, SeekOrigin.Begin);
      byte[] buffer = ArrayPool<byte>.Shared.Rent(16384 /*0x4000*/);
      try
      {
        while (true)
        {
          int count;
          if ((count = await stream.ReadAsync(buffer, 0, 4096 /*0x1000*/, new CancellationToken())) != 0)
            await fileStream.WriteAsync(buffer, 0, count, new CancellationToken());
          else
            break;
        }
      }
      catch (Exception ex) when (!(ex is OperationCanceledException))
      {
      }
      finally
      {
        ArrayPool<byte>.Shared.Return(buffer);
      }
      buffer = (byte[]) null;
    }
  }

  private static string GetDataPath()
  {
    string path = Path.Combine(AppDataHelper.TemporaryFolder, "Online");
    if (!Directory.Exists(path))
      Directory.CreateDirectory(path);
    return path;
  }
}
