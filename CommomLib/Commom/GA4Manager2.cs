// Decompiled with JetBrains decompiler
// Type: CommomLib.Commom.GA4Manager2
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

#nullable disable
namespace CommomLib.Commom;

public static class GA4Manager2
{
  public static string Measurement_ID = "G-02BJ88WY12";
  public static string User_Language = CultureInfo.CurrentUICulture.Name;
  private static string Measurement_ProtocolVersion = "2";
  private static bool IsSendDebugEvent = false;
  private static string Client_ID = "";
  private static string App_Version = "";
  private static HttpClient client = (HttpClient) null;
  private static string GADomainUrl = "https://analytics.google.com/g/collect";
  private static string baseRequestParas = "";

  static GA4Manager2() => GA4Manager2.Init();

  public static void SendEvent(string strCategory, string strAction, string strLabel, long value)
  {
    Task.Factory.StartNew((Action) (() => GA4Manager2.TrackAsync("UAEvents", (IDictionary<string, string>) new Dictionary<string, string>()
    {
      ["UserType"] = (UtilManager.IsNewUser() ? "NewUser" : "OldUser"),
      ["UserCID"] = UtilManager.GetUserCID(),
      ["UAEventCategory"] = strCategory,
      ["UAEventAction"] = strAction,
      ["UAEventLabel"] = strLabel,
      ["UAEventValue"] = $"{value}"
    })));
  }

  public static void SendEvent(string eventName, IDictionary<string, string> eventParas)
  {
    Task.Factory.StartNew((Action) (() => GA4Manager2.TrackAsync(eventName, eventParas)));
  }

  private static async void TrackAsync(string eventName, IDictionary<string, string> eventParas)
  {
    try
    {
      string requestUrl = GA4Manager2.GenerateRequestUrl(eventName, eventParas);
      if (string.IsNullOrEmpty(requestUrl))
        return;
      int num = await GA4Manager2.RequestUrlAsync(requestUrl, "") ? 1 : 0;
    }
    catch
    {
    }
  }

  private static bool Init()
  {
    GA4Manager2.Client_ID = UtilManager.GetUUID();
    GA4Manager2.App_Version = UtilManager.GetAppVersion();
    if (string.IsNullOrEmpty(GA4Manager2.Client_ID) || string.IsNullOrEmpty(GA4Manager2.App_Version))
      return false;
    if (string.IsNullOrEmpty(GA4Manager2.User_Language))
      GA4Manager2.User_Language = "en-US";
    GA4Manager2.baseRequestParas = $"v={GA4Manager2.Measurement_ProtocolVersion}&tid={GA4Manager2.Measurement_ID}&cid={GA4Manager2.Client_ID}&ul={GA4Manager2.User_Language}&_et=1";
    if (!string.IsNullOrEmpty(GA4Manager2.App_Version))
      GA4Manager2.baseRequestParas = $"{GA4Manager2.baseRequestParas}&ep.app_version_2={GA4Manager2.App_Version}";
    return true;
  }

  private static string GenerateRequestUrl(string eventName, IDictionary<string, string> eventParas)
  {
    string requestUrl1 = "";
    if (string.IsNullOrEmpty(eventName) || string.IsNullOrEmpty(GA4Manager2.baseRequestParas))
      return requestUrl1;
    string str1 = "en=" + eventName;
    string str2 = "";
    if (eventParas != null && eventParas.Count > 0)
      str2 = string.Join("&", eventParas.Select<KeyValuePair<string, string>, string>((Func<KeyValuePair<string, string>, string>) (p => $"ep.{p.Key}={Uri.EscapeDataString(p.Value)}")));
    string requestUrl2 = $"{GA4Manager2.GADomainUrl}?{GA4Manager2.baseRequestParas}";
    if (GA4Manager2.IsFirstVist)
      requestUrl2 += "&_fv=1";
    if (!string.IsNullOrEmpty(str1))
      requestUrl2 = $"{requestUrl2}&{str1}";
    if (!string.IsNullOrEmpty(str2))
      requestUrl2 = $"{requestUrl2}&{str2}";
    return requestUrl2;
  }

  private static async Task<bool> RequestUrlAsync(string url, string userAgent)
  {
    int num;
    if (num != 0 && string.IsNullOrEmpty(url))
      return false;
    try
    {
      if (GA4Manager2.client == null)
        GA4Manager2.client = new HttpClient();
      HttpResponseMessage httpResponseMessage = await GA4Manager2.client.PostAsync(url, (HttpContent) null);
      return true;
    }
    catch (Exception ex)
    {
    }
    return false;
  }

  private static bool IsFirstVist => false;
}
