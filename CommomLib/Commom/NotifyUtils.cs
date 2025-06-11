// Decompiled with JetBrains decompiler
// Type: CommomLib.Commom.NotifyUtils
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

#nullable disable
namespace CommomLib.Commom;

public static class NotifyUtils
{
  public static async Task UpdateChannelAsync(bool force = false)
  {
    string oldUrl = ConfigManager.GetPushChannelUri() ?? "";
    ConfigManager.GetPushChannelExpired();
    string newUrl = "";
    DateTimeOffset newExpired = new DateTimeOffset();
    NotifyUtils.PushChannel channelUriAsync = await NotifyUtils.GetChannelUriAsync();
    if (channelUriAsync != null)
    {
      newUrl = channelUriAsync.Uri ?? "";
      newExpired = channelUriAsync.ExpirationTime;
    }
    if (!force && !(newUrl != oldUrl))
    {
      oldUrl = (string) null;
      newUrl = (string) null;
    }
    else if (!string.IsNullOrEmpty(newUrl))
    {
      if (!await NotifyUtils.NotifyServerUtils.RegisterAsync(newUrl, newExpired))
      {
        oldUrl = (string) null;
        newUrl = (string) null;
      }
      else
      {
        ConfigManager.SetPushChannelUri(newUrl);
        ConfigManager.SetPushChannelExpired(new DateTimeOffset?(newExpired));
        oldUrl = (string) null;
        newUrl = (string) null;
      }
    }
    else if (!await NotifyUtils.NotifyServerUtils.UnregisterAsync())
    {
      oldUrl = (string) null;
      newUrl = (string) null;
    }
    else
    {
      ConfigManager.SetPushChannelUri("");
      ConfigManager.SetPushChannelExpired(new DateTimeOffset?());
      oldUrl = (string) null;
      newUrl = (string) null;
    }
  }

  private static async Task<NotifyUtils.PushChannel> GetChannelUriAsync()
  {
    return (NotifyUtils.PushChannel) null;
  }

  private class PushChannel
  {
    public string Uri { get; set; }

    public DateTimeOffset ExpirationTime { get; set; }
  }

  internal static class NotifyServerUtils
  {
    private const string Server = "";
    private const string SignKey = "";
    private static HttpClient client;

    public static async Task<bool> RegisterAsync(string uri, DateTimeOffset expired)
    {
      if (NotifyUtils.NotifyServerUtils.client == null)
        NotifyUtils.NotifyServerUtils.client = new HttpClient();
      try
      {
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "/api/toast/register");
        SortedDictionary<string, string> dict = new SortedDictionary<string, string>()
        {
          [nameof (uri)] = uri,
          ["lang"] = CultureInfo.CurrentUICulture?.Name,
          [nameof (expired)] = $"{expired.ToUnixTimeMilliseconds()}",
          ["guid"] = UtilManager.GetUUID(),
          ["version"] = UtilManager.GetAppVersion(),
          ["isPaid"] = "false"
        };
        dict["sign"] = CryptHelper.GetSign(dict, "");
        request.Content = (HttpContent) new StringContent(JsonConvert.SerializeObject((object) dict), Encoding.UTF8, "application/json");
        HttpResponseMessage httpResponseMessage = await NotifyUtils.NotifyServerUtils.client.SendAsync(request).ConfigureAwait(false);
        httpResponseMessage.EnsureSuccessStatusCode();
        return JObject.Parse(await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false)).Value<bool?>((object) "success").GetValueOrDefault();
      }
      catch
      {
      }
      return false;
    }

    public static async Task<bool> UnregisterAsync()
    {
      if (NotifyUtils.NotifyServerUtils.client == null)
        NotifyUtils.NotifyServerUtils.client = new HttpClient();
      try
      {
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "/api/toast/unregister");
        SortedDictionary<string, string> dict = new SortedDictionary<string, string>()
        {
          ["guid"] = UtilManager.GetUUID(),
          ["test"] = "true"
        };
        dict["sign"] = CryptHelper.GetSign(dict, "");
        request.Content = (HttpContent) new StringContent(JsonConvert.SerializeObject((object) dict), Encoding.UTF8, "application/json");
        HttpResponseMessage httpResponseMessage = await NotifyUtils.NotifyServerUtils.client.SendAsync(request);
        httpResponseMessage.EnsureSuccessStatusCode();
        return JObject.Parse(await httpResponseMessage.Content.ReadAsStringAsync()).Value<bool?>((object) "success").GetValueOrDefault();
      }
      catch
      {
      }
      return false;
    }

    public static async Task<bool> SubmitEmailAsync(string email)
    {
      if (NotifyUtils.NotifyServerUtils.client == null)
        NotifyUtils.NotifyServerUtils.client = new HttpClient();
      try
      {
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "/api/email/addemail");
        var data = new
        {
          paid = 0,
          exp = $"{ConfigManager.GetPaidExpireTimestamp()}"
        };
        SortedDictionary<string, string> dict = new SortedDictionary<string, string>()
        {
          [nameof (email)] = email,
          ["lang"] = CultureInfo.CurrentUICulture?.Name,
          ["guid"] = UtilManager.GetUUID(),
          ["customFilter"] = JsonConvert.SerializeObject((object) data, Formatting.None)
        };
        dict["sign"] = CryptHelper.GetSign(dict, "");
        request.Content = (HttpContent) new StringContent(JsonConvert.SerializeObject((object) dict), Encoding.UTF8, "application/json");
        HttpResponseMessage httpResponseMessage = await NotifyUtils.NotifyServerUtils.client.SendAsync(request);
        httpResponseMessage.EnsureSuccessStatusCode();
        JObject jobject = JObject.Parse(await httpResponseMessage.Content.ReadAsStringAsync());
        GAManager.SendEvent("UserCommunityWindow", "EmailSubmit", "Succ", 1L);
        return jobject.Value<bool?>((object) "success").GetValueOrDefault();
      }
      catch
      {
      }
      return false;
    }

    private class PushChannel
    {
      public string Uri { get; set; }

      public DateTimeOffset ExpirationTime { get; set; }
    }
  }
}
