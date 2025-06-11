// Decompiled with JetBrains decompiler
// Type: pdfeditor.Utils.TempFileUtils
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using CommomLib.Commom;
using Patagames.Pdf.Enums;
using Patagames.Pdf.Net;
using PDFKit.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting;
using System.Threading;
using System.Threading.Tasks;

#nullable disable
namespace pdfeditor.Utils;

public static class TempFileUtils
{
  private static string _rootTempFolder;
  private static Func<PdfDocument, Stream> documentPdfStreamGetter;
  private static object documentPdfStreamLocker = new object();

  private static string RootTempFolder
  {
    get
    {
      if (string.IsNullOrEmpty(TempFileUtils._rootTempFolder))
        TempFileUtils._rootTempFolder = AppDataHelper.TemporaryFolder;
      return TempFileUtils._rootTempFolder;
    }
  }

  private static string DocumentTempFolder
  {
    get
    {
      string path = Path.Combine(TempFileUtils.RootTempFolder, "Documents");
      if (!Directory.Exists(path))
        Directory.CreateDirectory(path);
      return path;
    }
  }

  private static string MergeDocumentTempFolder
  {
    get
    {
      string path = Path.Combine(TempFileUtils.DocumentTempFolder, "Merge");
      if (!Directory.Exists(path))
        Directory.CreateDirectory(path);
      return path;
    }
  }

  public static async Task<Stream> CreateStreamAsync(
    string fileFullName,
    FileStream fileStream,
    CancellationToken cancellationToken)
  {
    cancellationToken.ThrowIfCancellationRequested();
    MemoryStream memoryStream;
    if (fileStream.Length > 20971520L /*0x01400000*/)
    {
      memoryStream = new MemoryStream();
      fileStream.Seek(0L, SeekOrigin.Begin);
      await fileStream.CopyToAsync((Stream) memoryStream, (int) fileStream.Length, cancellationToken).ConfigureAwait(false);
      return (Stream) memoryStream;
    }
    FileInfo fileInfo = new FileInfo(fileFullName);
    string tmpFileFullName = "";
    do
    {
      tmpFileFullName = Path.Combine(TempFileUtils.DocumentTempFolder, TempFileUtils.GenerateRandomFilename(fileInfo));
    }
    while (File.Exists(tmpFileFullName));
    FileStream tmpFileStream = (FileStream) null;
    try
    {
      tmpFileStream = new FileStream(tmpFileFullName, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.None);
    }
    catch
    {
      tmpFileFullName = Path.Combine(TempFileUtils.DocumentTempFolder, TempFileUtils.GenerateShortRandomFilename(fileInfo));
      try
      {
        tmpFileStream = new FileStream(tmpFileFullName, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.None);
      }
      catch
      {
        memoryStream = new MemoryStream();
        fileStream.Seek(0L, SeekOrigin.Begin);
        await fileStream.CopyToAsync((Stream) memoryStream, (int) fileStream.Length, cancellationToken).ConfigureAwait(false);
        return (Stream) memoryStream;
      }
    }
    fileStream.Seek(0L, SeekOrigin.Begin);
    try
    {
      await fileStream.CopyToAsync((Stream) tmpFileStream, (int) fileStream.Length, cancellationToken).ConfigureAwait(false);
    }
    catch
    {
      try
      {
        tmpFileStream.Dispose();
        tmpFileStream = (FileStream) null;
        File.Delete(tmpFileFullName);
      }
      catch
      {
      }
      throw;
    }
    TempFileUtils.StreamWrapper streamAsync = new TempFileUtils.StreamWrapper(tmpFileStream);
    streamAsync.Disposed += (EventHandler) ((s, a) =>
    {
      try
      {
        File.Delete(tmpFileFullName);
      }
      catch
      {
      }
    });
    return (Stream) streamAsync;
  }

  public static async Task<string> SaveMergeSourceFile(PdfDocument document, int[] range)
  {
    if (document == null)
      throw new ArgumentNullException(nameof (document));
    if (range == null || range.Length == 0)
      throw new ArgumentNullException(nameof (range));
    return await Task.Run<string>(TaskExceptionHelper.ExceptionBoundary<string>((Func<string>) (() =>
    {
      try
      {
        string path;
        do
        {
          path = Path.Combine(TempFileUtils.MergeDocumentTempFolder, Guid.NewGuid().ToString("N").Substring(0, 8) ?? "");
        }
        while (File.Exists(path));
        if (range.Length != 0)
          PageDisposeHelper.TryFixResource(document, ((IEnumerable<int>) range).Min(), ((IEnumerable<int>) range).Max());
        using (FileStream fileStream = File.Create(path))
        {
          using (PdfDocument pdfDocument = PdfDocument.CreateNew())
          {
            pdfDocument.Pages.ImportPages(document, ((IEnumerable<int>) range).ConvertToRange(), 0);
            pdfDocument.Save((Stream) fileStream, SaveFlags.NoIncremental);
          }
          return path;
        }
      }
      catch
      {
        return string.Empty;
      }
    })));
  }

  private static string GenerateRandomFilename(FileInfo fileInfo)
  {
    if (fileInfo == null)
      throw new ArgumentException(nameof (fileInfo));
    return $"{Guid.NewGuid().ToString("N").Substring(0, 8)}_{fileInfo.Name}";
  }

  private static string GenerateShortRandomFilename(FileInfo fileInfo)
  {
    if (fileInfo == null)
      throw new ArgumentException(nameof (fileInfo));
    return Guid.NewGuid().ToString("N").Substring(0, 8) + fileInfo.Extension;
  }

  public static void ClearDocuments()
  {
    try
    {
      string[] files = Directory.GetFiles(TempFileUtils.DocumentTempFolder);
      if (files != null)
      {
        foreach (string filename in files)
          TryDeleteFileCore(filename);
      }
      if (Directory.GetFiles(TempFileUtils.MergeDocumentTempFolder) == null)
        return;
      foreach (string filename in files)
        TryDeleteFileCore(filename);
    }
    catch
    {
    }

    static void TryDeleteFileCore(string filename)
    {
      try
      {
        File.Delete(filename);
      }
      catch
      {
      }
    }
  }

  public static async Task<Stream> CloneToTempStream(PdfDocument sourceDocument)
  {
    long? nullable = sourceDocument != null ? TempFileUtils.TryGetDocumentStreamLength(sourceDocument) : throw new ArgumentNullException(nameof (sourceDocument));
    return nullable.HasValue && nullable.Value < 20971520L /*0x01400000*/ ? await TempFileUtils.CloneToTempStreamCore(sourceDocument, (Func<Task<Stream>>) (() => Task.FromResult<Stream>((Stream) new MemoryStream()))) : await TempFileUtils.CloneToTempStreamCore(sourceDocument, (Func<Task<Stream>>) (() =>
    {
      string tmpFileFullName = "";
      do
      {
        tmpFileFullName = Path.Combine(TempFileUtils.DocumentTempFolder, Guid.NewGuid().ToString("N").Substring(0, 8) + "_TEMPDOC");
      }
      while (File.Exists(tmpFileFullName));
      TempFileUtils.StreamWrapper result = new TempFileUtils.StreamWrapper(new FileStream(tmpFileFullName, FileMode.Create, FileAccess.ReadWrite));
      result.Disposed += (EventHandler) ((s, a) =>
      {
        try
        {
          File.Delete(tmpFileFullName);
        }
        catch
        {
        }
      });
      return Task.FromResult<Stream>((Stream) result);
    }));
  }

  private static async Task<Stream> CloneToTempStreamCore(
    PdfDocument sourceDocument,
    Func<Task<Stream>> tempStream)
  {
    if (sourceDocument == null)
      throw new ArgumentNullException(nameof (sourceDocument));
    if (tempStream == null)
      throw new ArgumentNullException(nameof (tempStream));
    Stream tempStreamCore;
    using (Stream stream1 = await tempStream())
    {
      if (!stream1.CanRead || !stream1.CanWrite || !stream1.CanSeek)
        throw new ArgumentException((string) null, nameof (tempStream));
      sourceDocument.Save(stream1, SaveFlags.RemoveSecurity | SaveFlags.RemoveUnusedObjects);
      stream1.Seek(0L, SeekOrigin.Begin);
      using (PdfDocument tmpDocument = PdfDocument.Load(stream1, new PdfForms()))
      {
        PageDisposeHelper.TryFixResource(tmpDocument, 0, tmpDocument.Pages.Count - 1);
        for (int pageIndex = 0; pageIndex < tmpDocument.Pages.Count; ++pageIndex)
          PageDisposeHelper.TryFixPageAnnotations(tmpDocument, pageIndex);
        PdfDocumentUtils.RemoveUnusedObjects(tmpDocument);
        Stream stream = await tempStream();
        if (!stream.CanRead || !stream.CanWrite || !stream.CanSeek)
          throw new ArgumentException((string) null, nameof (tempStream));
        tmpDocument.Save(stream, SaveFlags.RemoveSecurity | SaveFlags.RemoveUnusedObjects);
        stream.Seek(0L, SeekOrigin.Begin);
        tempStreamCore = stream;
      }
    }
    return tempStreamCore;
  }

  private static long? TryGetDocumentStreamLength(PdfDocument document)
  {
    if (document == null)
      return new long?();
    if (TempFileUtils.documentPdfStreamGetter == null)
    {
      lock (TempFileUtils.documentPdfStreamLocker)
      {
        if (TempFileUtils.documentPdfStreamGetter == null)
        {
          try
          {
            TempFileUtils.documentPdfStreamGetter = CommomLib.Commom.TypeHelper.CreateFieldOrPropertyGetter<PdfDocument, Stream>("_streamPdf", BindingFlags.Instance | BindingFlags.NonPublic);
          }
          catch
          {
          }
          if (TempFileUtils.documentPdfStreamGetter == null)
            TempFileUtils.documentPdfStreamGetter = (Func<PdfDocument, Stream>) (c => (Stream) null);
        }
      }
    }
    try
    {
      return new long?(TempFileUtils.documentPdfStreamGetter(document).Length);
    }
    catch
    {
    }
    return new long?();
  }

  private class StreamWrapper : Stream
  {
    private readonly FileStream stream;

    public StreamWrapper(FileStream stream) => this.stream = stream;

    public override bool CanRead => this.stream.CanRead;

    public override bool CanSeek => this.stream.CanSeek;

    public override bool CanWrite => this.stream.CanWrite;

    public override long Length => this.stream.Length;

    public override long Position
    {
      get => this.stream.Position;
      set => this.stream.Position = value;
    }

    public override void Flush() => this.stream.Flush();

    public override int Read(byte[] buffer, int offset, int count)
    {
      return this.stream.Read(buffer, offset, count);
    }

    public override long Seek(long offset, SeekOrigin origin) => this.stream.Seek(offset, origin);

    public override void SetLength(long value) => this.stream.SetLength(value);

    public override void Write(byte[] buffer, int offset, int count)
    {
      this.stream.Write(buffer, offset, count);
    }

    public override IAsyncResult BeginRead(
      byte[] buffer,
      int offset,
      int count,
      AsyncCallback callback,
      object state)
    {
      return this.stream.BeginRead(buffer, offset, count, callback, state);
    }

    public override IAsyncResult BeginWrite(
      byte[] buffer,
      int offset,
      int count,
      AsyncCallback callback,
      object state)
    {
      return this.stream.BeginWrite(buffer, offset, count, callback, state);
    }

    public override bool CanTimeout => this.stream.CanTimeout;

    public override Task CopyToAsync(
      Stream destination,
      int bufferSize,
      CancellationToken cancellationToken)
    {
      return this.stream.CopyToAsync(destination, bufferSize, cancellationToken);
    }

    public override ObjRef CreateObjRef(Type requestedType)
    {
      return this.stream.CreateObjRef(requestedType);
    }

    public override int EndRead(IAsyncResult asyncResult) => this.stream.EndRead(asyncResult);

    public override void EndWrite(IAsyncResult asyncResult) => this.stream.EndWrite(asyncResult);

    public override Task FlushAsync(CancellationToken cancellationToken)
    {
      return this.stream.FlushAsync(cancellationToken);
    }

    public override Task<int> ReadAsync(
      byte[] buffer,
      int offset,
      int count,
      CancellationToken cancellationToken)
    {
      return this.stream.ReadAsync(buffer, offset, count, cancellationToken);
    }

    public override int ReadByte() => this.stream.ReadByte();

    public override Task WriteAsync(
      byte[] buffer,
      int offset,
      int count,
      CancellationToken cancellationToken)
    {
      return this.stream.WriteAsync(buffer, offset, count, cancellationToken);
    }

    public override void WriteByte(byte value) => this.stream.WriteByte(value);

    public override int WriteTimeout
    {
      get => this.stream.WriteTimeout;
      set => this.stream.WriteTimeout = value;
    }

    public override int ReadTimeout
    {
      get => this.stream.ReadTimeout;
      set => this.stream.ReadTimeout = value;
    }

    protected override void Dispose(bool disposing)
    {
      base.Dispose(disposing);
      this.stream.Dispose();
      EventHandler disposed = this.Disposed;
      if (disposed != null)
        disposed((object) this, EventArgs.Empty);
      int num = disposing ? 1 : 0;
    }

    ~StreamWrapper()
    {
      GC.SuppressFinalize((object) this);
      this.Dispose(false);
    }

    public event EventHandler Disposed;
  }
}
