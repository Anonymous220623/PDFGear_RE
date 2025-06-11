// Decompiled with JetBrains decompiler
// Type: pdfeditor.Utils.AttachmentFileUtils
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Microsoft.Win32;
using Patagames.Pdf.Net.Annotations;
using Patagames.Pdf.Net.BasicTypes;
using Patagames.Pdf.Net.Wrappers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

#nullable disable
namespace pdfeditor.Utils;

internal static class AttachmentFileUtils
{
  private static List<AttachmentFileUtils.AttachmentFileCache> fileCache = new List<AttachmentFileUtils.AttachmentFileCache>();

  public static async Task<bool> AttachmentSaveAsFileFromAnnotation(
    PdfFileAttachmentAnnotation annot,
    string initialDirectory,
    bool openAfterSaved)
  {
    try
    {
      PdfFileSpecification file = annot?.FileSpecification;
      if (!AttachmentFileUtils.IsUrl(file))
      {
        string fileName1 = file.FileName;
        SaveFileDialog saveFileDialog1 = new SaveFileDialog();
        saveFileDialog1.Filter = "pdf|*.pdf";
        saveFileDialog1.CreatePrompt = false;
        saveFileDialog1.OverwritePrompt = true;
        saveFileDialog1.InitialDirectory = initialDirectory;
        saveFileDialog1.FileName = fileName1;
        SaveFileDialog saveFileDialog2 = saveFileDialog1;
        if (saveFileDialog2.ShowDialog(App.Current.MainWindow).GetValueOrDefault())
        {
          string fileName = saveFileDialog2.FileName;
          bool flag = false;
          using (Stream stream = await AttachmentFileUtils.ExtraAttachmentFromAnnotation(annot, fileName).ConfigureAwait(false))
          {
            if (stream != null)
            {
              flag = true;
              lock (AttachmentFileUtils.fileCache)
                AttachmentFileUtils.fileCache.Add(new AttachmentFileUtils.AttachmentFileCache(file.EmbeddedFile, fileName));
            }
          }
          if (flag & openAfterSaved)
          {
            int num = await AttachmentFileUtils.OpenFileAsync(fileName) ? 1 : 0;
          }
          return flag;
        }
      }
      file = (PdfFileSpecification) null;
    }
    catch
    {
    }
    return false;
  }

  public static async Task<bool> OpenFileSpecAsync(PdfFileSpecification fileSpec)
  {
    if ((PdfWrapper) fileSpec == (PdfWrapper) null)
      return false;
    try
    {
      if (AttachmentFileUtils.IsUrl(fileSpec))
      {
        Uri result;
        if (Uri.TryCreate(fileSpec.FileName, UriKind.RelativeOrAbsolute, out result))
        {
          ProcessStartInfo processStartInfo = new ProcessStartInfo()
          {
            FileName = result.ToString(),
            UseShellExecute = true
          };
          try
          {
            new Process() { StartInfo = processStartInfo }.Start();
          }
          catch (Exception ex)
          {
            Console.WriteLine(ex.Message);
          }
        }
      }
      else
      {
        if ((PdfWrapper) fileSpec.EmbeddedFile != (PdfWrapper) null)
        {
          string filePath = await AttachmentFileUtils.ExtraEmbeddedFileFromFileSpec(fileSpec);
          if (!string.IsNullOrEmpty(filePath))
          {
            int num = await AttachmentFileUtils.OpenFileAsync(filePath) ? 1 : 0;
          }
          return true;
        }
        if (fileSpec.FileName != null)
          return File.Exists(fileSpec.FileName) && await AttachmentFileUtils.OpenFileAsync(fileSpec.FileName);
      }
    }
    catch
    {
    }
    return false;
  }

  public static async Task<string> ExtraEmbeddedFileFromFileSpec(PdfFileSpecification fileSpec)
  {
    try
    {
      if ((PdfWrapper) fileSpec == (PdfWrapper) null || AttachmentFileUtils.IsUrl(fileSpec))
        return (string) null;
      string filePath;
      if (AttachmentFileUtils.TryGetFilePathFromCache(fileSpec.EmbeddedFile, out filePath) && File.Exists(filePath))
        return filePath;
      string tempFileName = Path.GetTempFileName();
      File.Delete(tempFileName);
      Directory.CreateDirectory(tempFileName);
      filePath = Path.Combine(tempFileName, fileSpec.FileName);
      bool flag = false;
      using (Stream stream = await AttachmentFileUtils.ExtraEmbeddedFileFromFileSpec(fileSpec, filePath).ConfigureAwait(false))
      {
        if (stream != null)
        {
          lock (AttachmentFileUtils.fileCache)
            AttachmentFileUtils.fileCache.Add(new AttachmentFileUtils.AttachmentFileCache(fileSpec.EmbeddedFile, filePath));
          flag = true;
        }
      }
      if (flag)
      {
        try
        {
          File.SetAttributes(filePath, File.GetAttributes(filePath) | FileAttributes.ReadOnly);
        }
        catch
        {
        }
        return filePath;
      }
      filePath = (string) null;
    }
    catch
    {
    }
    return (string) null;
  }

  public static async Task<Stream> ExtraEmbeddedFileFromFileSpec(
    PdfFileSpecification fileSpec,
    string targetFilePath)
  {
    try
    {
      return (PdfWrapper) fileSpec == (PdfWrapper) null || AttachmentFileUtils.IsUrl(fileSpec) ? (Stream) null : await AttachmentFileUtils.ExtraEmbeddedFileToFileAsync(fileSpec?.EmbeddedFile, targetFilePath).ConfigureAwait(false);
    }
    catch
    {
    }
    return (Stream) null;
  }

  public static async Task<Stream> ExtraEmbeddedFileToFileAsync(
    PdfFile pdfFile,
    string targetFilePath)
  {
    return (PdfWrapper) pdfFile == (PdfWrapper) null ? (Stream) null : await AttachmentFileUtils.WriteToFileAsync(pdfFile, targetFilePath).ConfigureAwait(false);
  }

  public static async Task<bool> OpenAttachmentFromAnnotation(PdfFileAttachmentAnnotation annot)
  {
    try
    {
      PdfFileSpecification fileSpecification = annot?.FileSpecification;
      if (AttachmentFileUtils.IsUrl(fileSpecification))
      {
        Uri result;
        if (Uri.TryCreate(fileSpecification.FileName, UriKind.RelativeOrAbsolute, out result))
          Process.Start(result.ToString());
      }
      else
      {
        string filePath = await AttachmentFileUtils.ExtraAttachmentFromAnnotation(annot);
        if (!string.IsNullOrEmpty(filePath))
        {
          int num = await AttachmentFileUtils.OpenFileAsync(filePath) ? 1 : 0;
        }
        return true;
      }
    }
    catch
    {
    }
    return false;
  }

  public static async Task<string> ExtraAttachmentFromAnnotation(PdfFileAttachmentAnnotation annot)
  {
    try
    {
      PdfFileSpecification file = annot?.FileSpecification;
      if ((PdfWrapper) file == (PdfWrapper) null || AttachmentFileUtils.IsUrl(file))
        return (string) null;
      string filePath;
      if (AttachmentFileUtils.TryGetFilePathFromCache(file.EmbeddedFile, out filePath) && File.Exists(filePath))
        return filePath;
      string tempFileName = Path.GetTempFileName();
      File.Delete(tempFileName);
      Directory.CreateDirectory(tempFileName);
      filePath = Path.Combine(tempFileName, file.FileName);
      bool flag = false;
      using (Stream stream = await AttachmentFileUtils.ExtraAttachmentFromAnnotation(annot, filePath).ConfigureAwait(false))
      {
        if (stream != null)
        {
          lock (AttachmentFileUtils.fileCache)
            AttachmentFileUtils.fileCache.Add(new AttachmentFileUtils.AttachmentFileCache(file.EmbeddedFile, filePath));
          flag = true;
        }
      }
      if (flag)
      {
        try
        {
          File.SetAttributes(filePath, File.GetAttributes(filePath) | FileAttributes.ReadOnly);
        }
        catch
        {
        }
        return filePath;
      }
      file = (PdfFileSpecification) null;
      filePath = (string) null;
    }
    catch
    {
    }
    return (string) null;
  }

  private static async Task<bool> OpenFileAsync(string filePath)
  {
    if (!string.IsNullOrEmpty(filePath))
    {
      try
      {
        if (new FileInfo(filePath).Extension.ToLowerInvariant() == ".pdf")
          Process.Start(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "pdfeditor.exe"), $"\"{filePath}\"");
        else
          Process.Start(filePath);
        return true;
      }
      catch
      {
      }
    }
    return false;
  }

  private static bool TryGetFilePathFromCache(PdfFile pdfFile, out string fileName)
  {
    fileName = (string) null;
    if ((PdfWrapper) pdfFile == (PdfWrapper) null)
      return false;
    lock (AttachmentFileUtils.fileCache)
    {
      for (int index = AttachmentFileUtils.fileCache.Count - 1; index >= 0; --index)
      {
        AttachmentFileUtils.AttachmentFileCache attachmentFileCache = AttachmentFileUtils.fileCache[index];
        PdfFile pdfFile1 = attachmentFileCache.PdfFile;
        if ((PdfWrapper) pdfFile1 == (PdfWrapper) null)
          AttachmentFileUtils.fileCache.RemoveAt(index);
        else if ((PdfWrapper) pdfFile1 == (PdfWrapper) pdfFile)
        {
          fileName = attachmentFileCache.Path;
          return true;
        }
      }
    }
    return false;
  }

  public static async Task<Stream> ExtraAttachmentFromAnnotation(
    PdfFileAttachmentAnnotation annot,
    string targetFilePath)
  {
    try
    {
      PdfFileSpecification fileSpecification = annot?.FileSpecification;
      return (PdfWrapper) fileSpecification == (PdfWrapper) null || AttachmentFileUtils.IsUrl(fileSpecification) ? (Stream) null : await AttachmentFileUtils.ExtraAttachmentToFileAsync(annot?.FileSpecification?.EmbeddedFile, targetFilePath).ConfigureAwait(false);
    }
    catch
    {
    }
    return (Stream) null;
  }

  public static async Task<Stream> ExtraAttachmentToFileAsync(
    PdfFile pdfFile,
    string targetFilePath)
  {
    return (PdfWrapper) pdfFile == (PdfWrapper) null ? (Stream) null : await AttachmentFileUtils.WriteToFileAsync(pdfFile, targetFilePath).ConfigureAwait(false);
  }

  private static async Task<Stream> WriteToFileAsync(PdfFile pdfFile, string targetFilePath)
  {
    Stream stream = (Stream) null;
    try
    {
      if ((PdfWrapper) pdfFile == (PdfWrapper) null)
        return (Stream) null;
      byte[] decodedData = pdfFile.Stream.DecodedData;
      stream = (Stream) new FileStream(targetFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
      await stream.WriteAsync(decodedData, 0, decodedData.Length).ConfigureAwait(false);
      await stream.FlushAsync().ConfigureAwait(false);
      stream.SetLength(stream.Position);
      return stream;
    }
    catch
    {
      stream?.Dispose();
    }
    return (Stream) null;
  }

  public static bool IsUrl(PdfFileSpecification fileSpec)
  {
    return !((PdfWrapper) fileSpec == (PdfWrapper) null) && fileSpec.IsExists("FS") && string.Equals(fileSpec.Dictionary["FS"].As<PdfTypeName>().Value, "URL", StringComparison.OrdinalIgnoreCase);
  }

  private class AttachmentFileCache
  {
    private WeakReference<PdfFile> weakPdfFile;

    public AttachmentFileCache(PdfFile pdfFile, string path)
    {
      this.weakPdfFile = new WeakReference<PdfFile>(pdfFile);
      this.Path = path;
    }

    public PdfFile PdfFile
    {
      get
      {
        PdfFile target;
        return this.weakPdfFile != null && this.weakPdfFile.TryGetTarget(out target) ? target : (PdfFile) null;
      }
    }

    public string Path { get; }
  }
}
