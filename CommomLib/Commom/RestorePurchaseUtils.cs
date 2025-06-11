// Decompiled with JetBrains decompiler
// Type: CommomLib.Commom.RestorePurchaseUtils
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

#nullable disable
namespace CommomLib.Commom;

public static class RestorePurchaseUtils
{
  private static string CollectionTokenUrl = "";
  private static string QueryProductsUrl = "";

  public static async Task<string> GetCollectionToken()
  {
    try
    {
      HttpResponseMessage httpResponseMessage = await new HttpClient().SendAsync(new HttpRequestMessage(HttpMethod.Get, RestorePurchaseUtils.CollectionTokenUrl)).ConfigureAwait(false);
      if (httpResponseMessage.IsSuccessStatusCode)
        return await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
    }
    catch
    {
    }
    return string.Empty;
  }

  public static async Task<string> QueryProducts(string storeKey, string userId)
  {
    try
    {
      string str = "----------------------------" + DateTime.Now.Ticks.ToString("x");
      FormUrlEncodedContent urlEncodedContent = new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>) new Dictionary<string, string>()
      {
        [nameof (storeKey)] = storeKey,
        [nameof (userId)] = userId
      });
      HttpResponseMessage httpResponseMessage = await new HttpClient().SendAsync(new HttpRequestMessage(HttpMethod.Post, RestorePurchaseUtils.QueryProductsUrl)
      {
        Content = (HttpContent) urlEncodedContent
      }).ConfigureAwait(false);
      if (httpResponseMessage.IsSuccessStatusCode)
        return await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
    }
    catch
    {
    }
    return string.Empty;
  }
}
