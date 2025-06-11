// Decompiled with JetBrains decompiler
// Type: CommomLib.Commom.HttpHelper
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

using System;
using System.Buffers;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

#nullable disable
namespace CommomLib.Commom;

public static class HttpHelper
{
  private static HttpClient httpClient;
  private static object locker = new object();

  public static async Task DownloadAsync(
    string url,
    Stream stream,
    Action<HttpHelperDownloadResponse> progressReporter,
    CancellationToken cancellationToken)
  {
    HttpClient client = HttpHelper.EnsureHttpClient();
    await Task.Run((Func<Task>) (async () =>
    {
      HttpResponseMessage async = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
      long? contentLength = (long?) async.Content?.Headers?.ContentLength;
      using (Stream responseStream = await async.Content.ReadAsStreamAsync().ConfigureAwait(false))
      {
        byte[] buffer = ArrayPool<byte>.Shared.Rent(4096 /*0x1000*/);
        long position = 0;
        if (contentLength.HasValue)
        {
          Action<HttpHelperDownloadResponse> action = progressReporter;
          if (action != null)
            action(new HttpHelperDownloadResponse(contentLength, position, new double?(0.0)));
        }
        CancellationTokenRegistration? cancellationTokenRegistration = new CancellationTokenRegistration?();
        try
        {
          int count = 0;
          do
          {
            CancellationTokenSource linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            linkedTokenSource.CancelAfter(TimeSpan.FromSeconds(10.0));
            cancellationTokenRegistration = new CancellationTokenRegistration?(linkedTokenSource.Token.Register((Action) (() =>
            {
              try
              {
                responseStream.Dispose();
              }
              catch
              {
              }
            })));
            count = await responseStream.ReadAsync(buffer, 0, 4096 /*0x1000*/, linkedTokenSource.Token).ConfigureAwait(false);
            ref CancellationTokenRegistration? local = ref cancellationTokenRegistration;
            if (local.HasValue)
              local.GetValueOrDefault().Dispose();
            cancellationTokenRegistration = new CancellationTokenRegistration?();
            if (count > 0)
            {
              position += (long) count;
              if (contentLength.HasValue)
              {
                Action<HttpHelperDownloadResponse> action = progressReporter;
                if (action != null)
                  action(new HttpHelperDownloadResponse(contentLength, position, new double?()));
              }
              await stream.WriteAsync(buffer, 0, count);
            }
          }
          while (count > 0);
          await stream.FlushAsync(cancellationToken);
          Action<HttpHelperDownloadResponse> action1 = progressReporter;
          if (action1 != null)
            action1(new HttpHelperDownloadResponse(contentLength, position, new double?(1.0)));
        }
        finally
        {
          ref CancellationTokenRegistration? local = ref cancellationTokenRegistration;
          if (local.HasValue)
            local.GetValueOrDefault().Dispose();
          cancellationTokenRegistration = new CancellationTokenRegistration?();
          ArrayPool<byte>.Shared.Return(buffer);
        }
        buffer = (byte[]) null;
        cancellationTokenRegistration = new CancellationTokenRegistration?();
      }
    }), cancellationToken);
  }

  private static HttpClient EnsureHttpClient()
  {
    if (HttpHelper.httpClient == null)
    {
      lock (HttpHelper.locker)
      {
        if (HttpHelper.httpClient == null)
          HttpHelper.httpClient = new HttpClient();
      }
    }
    return HttpHelper.httpClient;
  }
}
