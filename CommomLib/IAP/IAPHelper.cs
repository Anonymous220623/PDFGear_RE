// Decompiled with JetBrains decompiler
// Type: CommomLib.IAP.IAPHelper
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

using CommomLib.Commom;
using CommomLib.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

#nullable disable
namespace CommomLib.IAP;

public static class IAPHelper
{
  private const string BuyPlanUrl = "https://www.pdfgear.com/store/";
  private const string DownloadUrl = "https://www.pdfgear.com/download/pdfgear-download-instructions.htm";
  private static IAPHelper.InfoWrapper info;
  private static SemaphoreSlim infoLocker = new SemaphoreSlim(1, 1);

  private static IAPHelper.InfoWrapper Info
  {
    get => IAPHelper.GetInfoWrapperAsync().GetAwaiter().GetResult();
  }

  public static string AccessToken => IAPHelper.Info.AccessToken;

  private static UserInfo FakeUserInfo
  {
    get
    {
      return new UserInfo()
      {
        Email = "user@pdfgear.com",
        ExpireTime = new DateTime?(new DateTime(2099, 1, 1)),
        Premium = true,
        Id = "1123319B-9AE6-4B68-B067-DB03AD7F692E"
      };
    }
  }

  public static UserInfo UserInfo => IAPHelper.FakeUserInfo;

  public static bool IsPaidUser
  {
    get
    {
      UserInfo userInfo = IAPHelper.UserInfo;
      if (userInfo == null || !userInfo.Premium)
        return false;
      if (!userInfo.IsSubscription)
        return true;
      DateTime? expireTime = userInfo.ExpireTime;
      DateTime utcNow = DateTime.UtcNow;
      return expireTime.HasValue && expireTime.GetValueOrDefault() > utcNow;
    }
  }

  public static bool ShowActivationWindow(string source, string ext)
  {
    AccessTokenInfo info = ActivationWindow.CreateDialog(source, ext, false);
    DispatcherFrame frame = new DispatcherFrame()
    {
      Continue = true
    };
    bool flag = false;
    Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background, (Delegate) (async () =>
    {
      await IAPHelper.UpdateCachedInfoAsync(info?.AccessToken, info?.User, true).ConfigureAwait(false);
      flag = !string.IsNullOrEmpty(info?.AccessToken);
      frame.Continue = false;
    }));
    Dispatcher.PushFrame(frame);
    return flag;
  }

  public static async Task<bool> RenewAccessTokenAsync()
  {
    if (IAPHelper.AccessToken == null)
      return false;
    AccessTokenInfo info = await InternalActivateHelper.RenewAsync(IAPHelper.AccessToken).ConfigureAwait(false);
    if (info == null)
      return false;
    await IAPHelper.UpdateCachedInfoAsync(info?.AccessToken, info?.User, true).ConfigureAwait(false);
    return !string.IsNullOrEmpty(info?.AccessToken);
  }

  public static async Task<bool> UpdateUserInfoAsync()
  {
    string accessToken = IAPHelper.AccessToken;
    if (accessToken == null)
      return false;
    UserInfo info = await InternalActivateHelper.GetUserInfoAsync(accessToken).ConfigureAwait(false);
    if (info != null)
      await IAPHelper.UpdateCachedInfoAsync(accessToken, info, false).ConfigureAwait(false);
    return info != null;
  }

  public static async Task<bool> ShouldRenewAsync()
  {
    DateTimeOffset now = new DateTimeOffset(DateTimeOffset.UtcNow.Date, TimeSpan.Zero);
    string s = (await ConfigUtils.TryGetAsync<string>(ConfigureKeys.UserInfoUpdateDate, new CancellationToken())).Item2;
    long result;
    return !string.IsNullOrEmpty(s) && long.TryParse(s, out result) && (DateTimeOffset.FromUnixTimeSeconds(result) - now).TotalDays >= 1.0;
  }

  public static async Task LogoutAsync()
  {
    await IAPHelper.UpdateCachedInfoAsync((string) null, (UserInfo) null, true);
  }

  public static void LaunchBuyPlanUri(string type = null, string email = null)
  {
    Dictionary<string, string> source = new Dictionary<string, string>();
    if (!string.IsNullOrEmpty(type))
      source[nameof (type)] = type;
    if (!string.IsNullOrEmpty(email))
      source[nameof (email)] = email.Trim();
    Uri uri;
    if (source.Count > 0)
    {
      StringBuilder stringBuilder = source.Aggregate<KeyValuePair<string, string>, StringBuilder>(new StringBuilder("https://www.pdfgear.com/store/").Append('?'), (Func<StringBuilder, KeyValuePair<string, string>, StringBuilder>) ((s, c) => s.Append(c.Key).Append('=').Append(Uri.EscapeDataString(c.Value)).Append('&')));
      string linkArgs = ChannelHelper.GetLinkArgs();
      if (!string.IsNullOrEmpty(linkArgs))
        stringBuilder.Append(linkArgs);
      else
        --stringBuilder.Length;
      uri = new Uri(stringBuilder.ToString());
    }
    else
      uri = new Uri("https://www.pdfgear.com/store/");
    IAPHelper.ShellExecute(IntPtr.Zero, "open", uri.AbsoluteUri, "", "", 1);
  }

  public static void LaunchDownloadUri()
  {
    Uri uri = new Uri("https://www.pdfgear.com/download/pdfgear-download-instructions.htm?h=" + Guid.NewGuid().ToString("n").Substring(0, 8));
    IAPHelper.ShellExecute(IntPtr.Zero, "open", uri.AbsoluteUri, "", "", 1);
  }

  internal static Task UpdateCachedInfoAsync(string accessToken, UserInfo userInfo)
  {
    return IAPHelper.UpdateCachedInfoAsync(accessToken, userInfo, true);
  }

  private static async Task UpdateCachedInfoAsync(
    string accessToken,
    UserInfo userInfo,
    bool updateDate)
  {
    try
    {
      await IAPHelper.infoLocker.WaitAsync().ConfigureAwait(false);
      IAPHelper.InfoWrapper info = new IAPHelper.InfoWrapper()
      {
        AccessToken = accessToken,
        UserInfo = userInfo
      };
      if (string.IsNullOrEmpty(accessToken))
      {
        ConfiguredTaskAwaitable<bool> configuredTaskAwaitable = ConfigUtils.TrySetAsync<IAPHelper.InfoWrapper>(ConfigureKeys.UserInfo, (IAPHelper.InfoWrapper) null, new CancellationToken()).ConfigureAwait(false);
        int num1 = await configuredTaskAwaitable ? 1 : 0;
        configuredTaskAwaitable = ConfigUtils.TrySetAsync<string>(ConfigureKeys.UserInfoUpdateDate, (string) null, new CancellationToken()).ConfigureAwait(false);
        int num2 = await configuredTaskAwaitable ? 1 : 0;
      }
      else
      {
        int num3 = await ConfigUtils.TrySetAsync<IAPHelper.InfoWrapper>(ConfigureKeys.UserInfo, info, new CancellationToken()).ConfigureAwait(false) ? 1 : 0;
        if (updateDate)
        {
          int num4 = await ConfigUtils.TrySetAsync<string>(ConfigureKeys.UserInfoUpdateDate, new DateTimeOffset(DateTime.UtcNow.Date, TimeSpan.Zero).ToUnixTimeSeconds().ToString(), new CancellationToken()).ConfigureAwait(false) ? 1 : 0;
        }
      }
      IAPHelper.info = info;
      info = (IAPHelper.InfoWrapper) null;
    }
    finally
    {
      IAPHelper.infoLocker.Release();
    }
    IAPHelper.OnUserInfoChanged();
  }

  private static async Task<IAPHelper.InfoWrapper> GetInfoWrapperAsync()
  {
    bool flag = false;
    if (IAPHelper.info == null)
    {
      try
      {
        await IAPHelper.infoLocker.WaitAsync().ConfigureAwait(false);
        if (IAPHelper.info == null)
        {
          flag = true;
          bool result = false;
          IAPHelper.InfoWrapper wrapper = (IAPHelper.InfoWrapper) null;
          try
          {
            (result, wrapper) = await ConfigUtils.TryGetAsync<IAPHelper.InfoWrapper>(ConfigureKeys.UserInfo, new CancellationToken()).ConfigureAwait(false);
          }
          catch
          {
          }
          if (result && wrapper != null && wrapper.UserInfo != null)
            IAPHelper.info = wrapper;
          if (IAPHelper.info == null)
            IAPHelper.info = new IAPHelper.InfoWrapper();
          wrapper = (IAPHelper.InfoWrapper) null;
        }
      }
      finally
      {
        IAPHelper.infoLocker.Release();
      }
    }
    if (flag)
      IAPHelper.OnUserInfoChanged();
    return IAPHelper.info;
  }

  public static event UserInfoChangedEventHandler UserInfoChanged;

  private static void OnUserInfoChanged()
  {
    UserInfoChangedEventHandler userInfoChanged = IAPHelper.UserInfoChanged;
    if (userInfoChanged == null)
      return;
    userInfoChanged(IAPHelper.AccessToken, IAPHelper.UserInfo);
  }

  [DllImport("Shell32.dll", CharSet = CharSet.Auto, SetLastError = true)]
  private static extern IntPtr ShellExecute(
    IntPtr hwnd,
    string lpOperation,
    string lpFile,
    string lpParameters,
    string lpDirectory,
    int nShowCmd);

  private class InfoWrapper
  {
    public string AccessToken { get; set; }

    public UserInfo UserInfo { get; set; }

    public DateTime CreateTime { get; set; }
  }
}
