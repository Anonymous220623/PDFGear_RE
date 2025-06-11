// Decompiled with JetBrains decompiler
// Type: CommomLib.IAP.InternalActivateHelper
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

using CommomLib.Commom;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

#nullable disable
namespace CommomLib.IAP;

internal static class InternalActivateHelper
{
  private const string server = "https://api.pdfgear.com";
  private static readonly string sendLoginCodeUrl = "https://api.pdfgear.com/api/user/sendaccesscode";
  private static readonly string getTokenUrl = "https://api.pdfgear.com/api/user/gettoken";
  private static readonly string getUserInfo = "https://api.pdfgear.com/api/user/getuserinfo";
  private static readonly string renew = "https://api.pdfgear.com/api/user/renew";
  private static readonly string eos = "https://api.pdfgear.com/api/app/eos";
  private static readonly string update = "https://api.pdfgear.com/api/app/update";
  private static HttpClient client;

  public static async Task<bool?> Eos(Version ver)
  {
    HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, InternalActivateHelper.eos ?? "");
    try
    {
      ResultModel<string> resultModel = await InternalActivateHelper.SendAsync<ResultModel<string>>(message).ConfigureAwait(false);
      if (resultModel != null)
      {
        if (resultModel != null)
        {
          if (resultModel.Success)
          {
            Version result;
            if (Version.TryParse(resultModel.Content, out result))
              return new bool?(ver >= result);
          }
        }
      }
    }
    catch
    {
    }
    return new bool?();
  }

  public static async Task<UpdateHelper.UpdateResponse> CheckUpdate()
  {
    HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, InternalActivateHelper.update ?? "");
    try
    {
      CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(20.0));
      ResultModel<UpdateHelper.UpdateResponse> resultModel = await InternalActivateHelper.SendAsync<ResultModel<UpdateHelper.UpdateResponse>>(message, cancellationToken: cancellationTokenSource.Token).ConfigureAwait(false);
      if (resultModel != null)
      {
        if (resultModel.Success)
        {
          if (resultModel.Content != null)
            return resultModel.Content;
        }
      }
    }
    catch
    {
    }
    return (UpdateHelper.UpdateResponse) null;
  }

  public static async Task<bool> SendLoginCodeAsync(string email, bool force)
  {
    string sendLoginCodeUrl = InternalActivateHelper.sendLoginCodeUrl;
    StringContent stringContent = new StringContent(JsonConvert.SerializeObject((object) new
    {
      email = email,
      timestamp = $"{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}",
      force = force
    }), Encoding.UTF8, "application/json");
    ResultModel<object> resultModel = await InternalActivateHelper.SendAsync<ResultModel<object>>(new HttpRequestMessage(HttpMethod.Post, sendLoginCodeUrl)
    {
      Content = (HttpContent) stringContent
    }).ConfigureAwait(false);
    return resultModel != null && resultModel.Success;
  }

  public static async Task<AccessTokenInfo> GetTokenAsync(string email, string code)
  {
    if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(code))
      return (AccessTokenInfo) null;
    email = email.Trim();
    code = code.Trim();
    ResultModel<AccessTokenInfo> resultModel = await InternalActivateHelper.SendAsync<ResultModel<AccessTokenInfo>>(new HttpRequestMessage(HttpMethod.Get, $"{InternalActivateHelper.getTokenUrl}?email={WebUtility.UrlEncode(email)}&accesscode={WebUtility.UrlEncode(code)}")).ConfigureAwait(false);
    return resultModel == null || !resultModel.Success || string.IsNullOrEmpty(resultModel?.Content?.AccessToken) || string.IsNullOrEmpty(resultModel?.Content?.User.Id) ? (AccessTokenInfo) null : resultModel.Content;
  }

  public static async Task<UserInfo> GetUserInfoAsync(string accessToken)
  {
    if (string.IsNullOrWhiteSpace(accessToken))
      return (UserInfo) null;
    ResultModel<UserInfo> resultModel = await InternalActivateHelper.SendAsync<ResultModel<UserInfo>>(new HttpRequestMessage(HttpMethod.Get, InternalActivateHelper.getUserInfo), accessToken).ConfigureAwait(false);
    return resultModel == null || !resultModel.Success || string.IsNullOrEmpty(resultModel?.Content?.Id) ? (UserInfo) null : resultModel.Content;
  }

  public static async Task<AccessTokenInfo> RenewAsync(string accessToken)
  {
    if (string.IsNullOrWhiteSpace(accessToken))
      return (AccessTokenInfo) null;
    ResultModel<AccessTokenInfo> resultModel = await InternalActivateHelper.SendAsync<ResultModel<AccessTokenInfo>>(new HttpRequestMessage(HttpMethod.Get, InternalActivateHelper.renew), accessToken).ConfigureAwait(false);
    return resultModel != null ? (resultModel == null || !resultModel.Success || string.IsNullOrEmpty(resultModel?.Content?.AccessToken) || string.IsNullOrEmpty(resultModel?.Content?.User?.Id) ? new AccessTokenInfo() : resultModel.Content) : (AccessTokenInfo) null;
  }

  private static async Task<T> SendAsync<T>(
    HttpRequestMessage message,
    string auth = null,
    CancellationToken cancellationToken = default (CancellationToken))
  {
    if (message == null)
      throw new ArgumentException(nameof (message));
    if (InternalActivateHelper.client == null)
      InternalActivateHelper.client = new HttpClient();
    if (!string.IsNullOrEmpty(auth))
    {
      auth = auth.Trim();
      if (!auth.StartsWith("Bearer "))
        auth = "Bearer " + auth;
      AuthenticationHeaderValue parsedValue;
      if (AuthenticationHeaderValue.TryParse(auth, out parsedValue))
        message.Headers.Authorization = parsedValue;
    }
    try
    {
      HttpResponseMessage httpResponseMessage = await InternalActivateHelper.client.SendAsync(message, cancellationToken).ConfigureAwait(false);
      if (httpResponseMessage.IsSuccessStatusCode)
      {
        string str = await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
        if (!string.IsNullOrEmpty(str))
          return JsonConvert.DeserializeObject<T>(str);
      }
    }
    catch
    {
    }
    return default (T);
  }
}
