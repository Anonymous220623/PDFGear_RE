// Decompiled with JetBrains decompiler
// Type: CommomLib.Commom.ConfigManager
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

using CommomLib.Config;
using CommomLib.Config.ConfigModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;

#nullable disable
namespace CommomLib.Commom;

public class ConfigManager
{
  private static ApplicationSettings settings = new ApplicationSettings();

  public static string GetUserUUID()
  {
    string key = "UUID";
    return ConfigManager.settings != null && ConfigManager.settings.Values.ContainsKey(key) ? ConfigManager.settings.Values[key].ToString() : "";
  }

  public static void GenerateUserUUID()
  {
    string key = "UUID";
    ConfigManager.settings.Values[key] = Guid.NewGuid().ToString();
  }

  public static string GetOriginVersion()
  {
    string key = "OriginVersion";
    if (!ConfigManager.settings.Values.ContainsKey(key))
      ConfigManager.settings.Values[key] = UtilManager.GetAppVersion();
    return ConfigManager.settings.Values[key].ToString();
  }

  public static bool IsFirstVist()
  {
    string key = "first_vist";
    if (ConfigManager.settings.Values.ContainsKey(key))
      return false;
    ConfigManager.settings.Values[key] = "true";
    return true;
  }

  public static void SetIsNewUser(bool value)
  {
    ConfigManager.settings.Values["IsNewUser"] = value.ToString();
  }

  public static bool GetIsNewUser()
  {
    try
    {
      string str;
      if (ConfigManager.settings.Values.TryGetValue("IsNewUser", out str))
      {
        bool result;
        if (bool.TryParse(str, out result))
          return result;
      }
    }
    catch
    {
    }
    return true;
  }

  public static double GetInstallDate()
  {
    string key = "InstallDate";
    if (!ConfigManager.settings.Values.ContainsKey(key))
      ConfigManager.settings.Values[key] = (DateTime.Now - new DateTime(2000, 1, 1, 0, 0, 0)).TotalDays.ToString();
    double result;
    return double.TryParse(ConfigManager.settings.Values[key], out result) ? result : 0.0;
  }

  public static long getAppLaunchCount()
  {
    string key = "AppLaunchCount";
    long result;
    return long.TryParse(ConfigManager.settings.Values[key], out result) ? result : 0L;
  }

  public static void setAppLaunchCount(long count)
  {
    string key = "AppLaunchCount";
    ConfigManager.settings.Values[key] = count.ToString();
  }

  public static int GetOnlineRequestCount()
  {
    string key = "OnlineRequestCount";
    if (!ConfigManager.settings.Values.ContainsKey(key))
    {
      ConfigManager.settings.Values[key] = DateTime.Now.ToString("yyyyMMdd") + "_0";
      return 0;
    }
    string[] strArray = ConfigManager.settings.Values[key].ToString().Split('_');
    if (DateTime.Now.ToString("yyyyMMdd") != strArray[0])
    {
      ConfigManager.settings.Values[key] = DateTime.Now.ToString("yyyyMMdd") + "_0";
      return 0;
    }
    int result;
    return int.TryParse(strArray[1], out result) ? result : 0;
  }

  public static void AddOnlineRequestCount()
  {
    string key = "OnlineRequestCount";
    if (!ConfigManager.settings.Values.ContainsKey(key))
    {
      ConfigManager.settings.Values[key] = DateTime.Now.ToString("yyyyMMdd") + "_1";
    }
    else
    {
      string[] strArray = ConfigManager.settings.Values[key].ToString().Split('_');
      if (DateTime.Now.ToString("yyyyMMdd") != strArray[0])
      {
        ConfigManager.settings.Values[key] = DateTime.Now.ToString("yyyyMMdd") + "_1";
      }
      else
      {
        int int32 = Convert.ToInt32(strArray[1]);
        ConfigManager.settings.Values[key] = $"{DateTime.Now.ToString("yyyyMMdd")}_{int32 + 1}";
      }
    }
  }

  public static int GetWindowState(string source = null)
  {
    string key = "WindowState";
    if (!string.IsNullOrEmpty(source))
      key = $"{key}.{source}";
    int result;
    return ConfigManager.settings != null && !ConfigManager.settings.Values.ContainsKey(key) ? (!string.IsNullOrEmpty(source) && source.Equals("Launcher") ? 1 : 0) : (int.TryParse(ConfigManager.settings.Values[key], out result) ? result : 0);
  }

  public static void SetWindowState(int value, string source = null)
  {
    string key = "WindowState";
    if (!string.IsNullOrEmpty(source))
      key = $"{key}.{source}";
    ConfigManager.settings.Values[key] = value.ToString();
  }

  public static string GetWindowSize(string source = null)
  {
    string key = "WindowSize";
    if (!string.IsNullOrEmpty(source))
      key = $"{key}.{source}";
    return ConfigManager.settings != null && !ConfigManager.settings.Values.ContainsKey(key) ? "" : ConfigManager.settings.Values[key].ToString();
  }

  public static void SetWindowSize(string value, string source = null)
  {
    string key = "WindowSize";
    if (!string.IsNullOrEmpty(source))
      key = $"{key}.{source}";
    ConfigManager.settings.Values[key] = value;
  }

  public static bool IsRatingFinished()
  {
    string key = "RatingFinished";
    bool result;
    return bool.TryParse(ConfigManager.settings.Values[key], out result) && result;
  }

  public static void FinishRating()
  {
    string key = "RatingFinished";
    ConfigManager.settings.Values[key] = "true";
  }

  public static void SetCouldRateFlag(bool value)
  {
    string key = "CouldRate";
    ConfigManager.settings.Values[key] = value.ToString();
  }

  public static bool GetCouldRateFlag()
  {
    string key = "CouldRate";
    bool result;
    return bool.TryParse(ConfigManager.settings.Values[key], out result) && result;
  }

  public static long GetLastShowIAPTimestamp()
  {
    string key = "LastShowIAPTimestamp";
    long result;
    return long.TryParse(ConfigManager.settings.Values[key], out result) ? result : 0L;
  }

  public static void SetLastShowIAPTimestamp(long timestamp)
  {
    string key = "LastShowIAPTimestamp";
    ConfigManager.settings.Values[key] = timestamp.ToString();
  }

  public static long GetStartupShowIAPOneDayCount()
  {
    string key = "StartupShowIAPOneDayCount";
    long result;
    return long.TryParse(ConfigManager.settings.Values[key], out result) ? result : 0L;
  }

  public static void SetStartupShowIAPOneDayCount(long timestamp)
  {
    string key = "StartupShowIAPOneDayCount";
    ConfigManager.settings.Values[key] = timestamp.ToString();
  }

  public static long GetLastConfigTimestamp()
  {
    string key = "LastConfigTimeStamp";
    long result;
    return long.TryParse(ConfigManager.settings.Values[key], out result) ? result : 0L;
  }

  public static void SetEraserSize(int size)
  {
    string key = "EraserSize";
    ConfigManager.settings.Values[key] = size.ToString();
  }

  public static int GetEraserSize()
  {
    string key = "EraserSize";
    int result;
    return int.TryParse(ConfigManager.settings.Values[key], out result) ? result : 8;
  }

  public static void SetEraserMode(string type)
  {
    string key = "EraserMode";
    ConfigManager.settings.Values[key] = type.ToString();
  }

  public static string GetEraserMode()
  {
    string key = "EraserMode";
    return string.IsNullOrEmpty(ConfigManager.settings.Values[key]) ? "Partial" : ConfigManager.settings.Values[key];
  }

  public static void SetLastConfigTimestamp(long timestamp)
  {
    string key = "LastConfigTimeStamp";
    ConfigManager.settings.Values[key] = timestamp.ToString();
  }

  public static uint getAdID()
  {
    string key = "AdID";
    uint result;
    return uint.TryParse(ConfigManager.settings.Values[key], out result) ? result : 0U;
  }

  public static void setAdID(uint id)
  {
    string key = "AdID";
    ConfigManager.settings.Values[key] = id.ToString();
  }

  public static uint getAdShowCount()
  {
    string key = "AdShowCount";
    uint result;
    return uint.TryParse(ConfigManager.settings.Values[key], out result) ? result : 0U;
  }

  public static void setAdShowCount(uint id)
  {
    string key = "AdShowCount";
    ConfigManager.settings.Values[key] = id.ToString();
  }

  public static Version getNotShowVersion()
  {
    string key = "DontShowVersion";
    Version result;
    return Version.TryParse(ConfigManager.settings.Values[key], out result) ? result : (Version) null;
  }

  public static void setNotShowVersion(Version version)
  {
    string key = "DontShowVersion";
    ConfigManager.settings.Values[key] = version.ToString();
  }

  public static DateTimeOffset GetLastUpdateDialogShowTime()
  {
    string key = "LastUpdateDialogShowTime";
    string s;
    long result;
    return ConfigManager.settings.Values.TryGetValue(key, out s) && long.TryParse(s, out result) ? DateTimeOffset.FromUnixTimeSeconds(result) : new DateTimeOffset();
  }

  public static void SetLastUpdateDialogShowTime(DateTimeOffset time)
  {
    string key = "LastUpdateDialogShowTime";
    ConfigManager.settings.Values[key] = $"{time.ToUnixTimeSeconds()}";
  }

  public static void SetPromoteShowFlag(bool flag)
  {
    string key = "NotShowPromote";
    ConfigManager.settings.Values[key] = flag.ToString();
  }

  public static bool GetPromoteShowFlag()
  {
    string key = "NotShowPromote";
    bool result;
    return bool.TryParse(ConfigManager.settings.Values[key], out result) && result;
  }

  public static string GetPreferPDFAppVer()
  {
    string key = "PreferPDFAppVer";
    return !ConfigManager.settings.Values.ContainsKey(key) ? string.Empty : ConfigManager.settings.Values[key].ToString();
  }

  public static void SetPreferPDFAppVer(string ver)
  {
    string key = "PreferPDFAppVer";
    ConfigManager.settings.Values[key] = ver;
  }

  public static string GetConvertPath()
  {
    string key = "ConvertPath";
    return !ConfigManager.settings.Values.ContainsKey(key) ? string.Empty : ConfigManager.settings.Values[key].ToString();
  }

  public static void SetConvertPath(string path)
  {
    string key = "ConvertPath";
    ConfigManager.settings.Values[key] = path;
  }

  public static int GetOCRLanguageID()
  {
    string key = "OCRLanguageID";
    if (!ConfigManager.settings.Values.ContainsKey(key))
      return -1;
    int result;
    return int.TryParse(ConfigManager.settings.Values[key], out result) ? result : 0;
  }

  public static void SetOCRLanguageID(int id)
  {
    string key = "OCRLanguageID";
    ConfigManager.settings.Values[key] = id.ToString();
  }

  public static long GetPaidExpireTimestamp()
  {
    string key = "PaidExpire";
    long result;
    return long.TryParse(ConfigManager.settings.Values[key], out result) ? result : 0L;
  }

  public static void SetPaidExpireTimestamp(long timestamp)
  {
    string key = "PaidExpire";
    ConfigManager.settings.Values[key] = timestamp.ToString();
  }

  public static void SetIAPFlag(uint flag)
  {
    string key = "IAPFlag";
    ConfigManager.settings.Values[key] = flag.ToString();
  }

  public static uint GetIAPFlag()
  {
    string key = "IAPFlag";
    uint result;
    return uint.TryParse(ConfigManager.settings.Values[key], out result) ? result : 0U;
  }

  public static bool IsPaidExpired()
  {
    return (long) (DateTime.UtcNow - new DateTime(2000, 1, 1, 0, 0, 0)).TotalSeconds - ConfigManager.GetPaidExpireTimestamp() > 0L;
  }

  public static void SetConverterAppPaidUserFlag(bool flag)
  {
    string key = "ConverterAppPaidUser";
    ConfigManager.settings.Values[key] = flag.ToString();
  }

  public static bool GetConverterAppPaidUserFlag()
  {
    string key = "ConverterAppPaidUser";
    bool result;
    return bool.TryParse(ConfigManager.settings.Values[key], out result) && result;
  }

  public static void SetPasswordSaveNoMorePromptFlag(bool flag)
  {
    string key = "PasswordSavePromptFlag";
    ConfigManager.settings.Values[key] = flag.ToString();
  }

  public static bool GetPasswordSaveNoMorePromptFlag()
  {
    string key = "PasswordSavePromptFlag";
    bool result;
    return ConfigManager.settings.Values.ContainsKey(key) && bool.TryParse(ConfigManager.settings.Values[key], out result) && result;
  }

  public static void SetSignatureExistFlag(bool flag)
  {
    string key = "SignatureExist";
    ConfigManager.settings.Values[key] = flag.ToString();
  }

  public static bool GetSignatureExistFlag()
  {
    string key = "SignatureExist";
    bool result;
    return ConfigManager.settings.Values.ContainsKey(key) && bool.TryParse(ConfigManager.settings.Values[key], out result) && result;
  }

  public static void SetEditTextToolTipMsgFlag(bool flag)
  {
    string key = "EditTextToolTipMsgFlag";
    ConfigManager.settings.Values[key] = flag.ToString();
  }

  public static bool GetEditTextToolTipMsgFlag()
  {
    string key = "EditTextToolTipMsgFlag";
    bool result;
    return bool.TryParse(ConfigManager.settings.Values[key], out result) && result;
  }

  public static void SetWelcomeOpenBtnTipsFlag(bool flag)
  {
    string key = "WelcomeOpenBtnTips";
    ConfigManager.settings.Values[key] = flag.ToString();
  }

  public static bool GetWelcomeOpenBtnTipsFlag()
  {
    string key = "WelcomeOpenBtnTips";
    return ConfigManager.settings.Values.ContainsKey(key) && Convert.ToBoolean(ConfigManager.settings.Values[key]);
  }

  public static void SetWelcomeOpenBtnShowTipsCount(int value)
  {
    string key = "WelcomeOpenBtnShowTipsCount";
    ConfigManager.settings.Values[key] = value.ToString();
  }

  public static int GetWelcomeOpenBtnShowTipsCount()
  {
    string key = "WelcomeOpenBtnShowTipsCount";
    return !ConfigManager.settings.Values.ContainsKey(key) ? 0 : Convert.ToInt32(ConfigManager.settings.Values[key]);
  }

  public static async Task<int> GetDocumentCurrentPageNumberAsync(
    string filepath,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrEmpty(filepath))
      return -1;
    (bool flag, List<DocumentCurrentPageNumberModel> source) = await ConfigUtils.TryGetAsync<List<DocumentCurrentPageNumberModel>>(ConfigureKeys.DocumentCurrentPageNumber, cancellationToken);
    if (!flag)
      return -1;
    string _filepath = filepath.Trim().ToUpperInvariant();
    return (source != null ? source.FirstOrDefault<DocumentCurrentPageNumberModel>((Func<DocumentCurrentPageNumberModel, bool>) (c => c.FilePath == _filepath))?.PageNumber : new int?()) ?? -1;
  }

  public static async Task<BackgroundModel> GetBackgroundModelAsync(
    CancellationToken cancellationToken)
  {
    (bool flag, BackgroundModel backgroundModel) = await ConfigUtils.TryGetAsync<BackgroundModel>(ConfigureKeys.Background, cancellationToken);
    return flag ? backgroundModel : (BackgroundModel) null;
  }

  public static async Task<PageDisplayModel> GetPageDisplayModelAsync(
    CancellationToken cancellationToken)
  {
    (bool flag, PageDisplayModel pageDisplayModel) = await ConfigUtils.TryGetAsync<PageDisplayModel>(ConfigureKeys.PageDisplay, cancellationToken);
    return flag ? pageDisplayModel : (PageDisplayModel) null;
  }

  public static async Task<PageSizeModel> GetPageSizeModelAsync(CancellationToken cancellationToken)
  {
    (bool flag, PageSizeModel pageSizeModel) = await ConfigUtils.TryGetAsync<PageSizeModel>(ConfigureKeys.PageSize, cancellationToken);
    return flag ? pageSizeModel : (PageSizeModel) null;
  }

  public static async Task<AutoSaveModel> GetAutoSaveAsync(CancellationToken cancellationToken)
  {
    (bool flag, AutoSaveModel autoSaveModel) = await ConfigUtils.TryGetAsync<AutoSaveModel>(ConfigureKeys.AutoSave, cancellationToken);
    return flag ? autoSaveModel : (AutoSaveModel) null;
  }

  public static async Task<AutoSaveFileModel> GetAutoSaveFileModelAsync(
    CancellationToken cancellationToken,
    string fileguid)
  {
    if (string.IsNullOrEmpty(fileguid))
      return (AutoSaveFileModel) null;
    (bool flag, List<AutoSaveFileModel> source) = await ConfigUtils.TryGetAsync<List<AutoSaveFileModel>>(ConfigureKeys.AutoSaveFile, cancellationToken);
    return flag ? (source != null ? source.FirstOrDefault<AutoSaveFileModel>((Func<AutoSaveFileModel, bool>) (a => a.Guid == fileguid)) : (AutoSaveFileModel) null) ?? (AutoSaveFileModel) null : (AutoSaveFileModel) null;
  }

  public static async Task<List<AutoSaveFileModel>> GetAutoSaveFilesAsync(
    CancellationToken cancellationToken)
  {
    (bool flag, List<AutoSaveFileModel> autoSaveFileModelList) = await ConfigUtils.TryGetAsync<List<AutoSaveFileModel>>(ConfigureKeys.AutoSaveFile, cancellationToken);
    return flag ? autoSaveFileModelList : (List<AutoSaveFileModel>) null;
  }

  public static async Task<bool> SetDocumentCurrentPageNumberAsync(string filepath, int pageNumber)
  {
    if (string.IsNullOrEmpty(filepath))
      return false;
    if (pageNumber < 0)
      pageNumber = 0;
    List<DocumentCurrentPageNumberModel> currentPageNumberModelList = (await ConfigUtils.TryGetAsync<List<DocumentCurrentPageNumberModel>>(ConfigureKeys.DocumentCurrentPageNumber, new CancellationToken()).ConfigureAwait(false)).Item2;
    string upperInvariant = filepath.Trim().ToUpperInvariant();
    if (currentPageNumberModelList != null)
    {
      for (int index = currentPageNumberModelList.Count - 1; index >= 0; --index)
      {
        if (string.IsNullOrEmpty(currentPageNumberModelList[index].FilePath) || currentPageNumberModelList[index].FilePath == upperInvariant)
          currentPageNumberModelList.RemoveAt(index);
      }
    }
    else
      currentPageNumberModelList = new List<DocumentCurrentPageNumberModel>();
    currentPageNumberModelList.Insert(0, new DocumentCurrentPageNumberModel()
    {
      FilePath = upperInvariant,
      PageNumber = pageNumber
    });
    return await ConfigUtils.TrySetAsync<List<DocumentCurrentPageNumberModel>>(ConfigureKeys.DocumentCurrentPageNumber, currentPageNumberModelList, new CancellationToken()).ConfigureAwait(false);
  }

  public static async Task<bool> SetBackgroundAsync(
    string backgroundName,
    string pageBackground,
    string viewerBackground)
  {
    if (string.IsNullOrEmpty(pageBackground) && string.IsNullOrEmpty(viewerBackground))
      return false;
    return await ConfigUtils.TrySetAsync<BackgroundModel>(ConfigureKeys.Background, new BackgroundModel()
    {
      BackgroundName = backgroundName,
      PageBackground = pageBackground,
      ViewerBackground = viewerBackground
    }, new CancellationToken()).ConfigureAwait(false);
  }

  public static async Task<bool> SetPageDisplayModeAsync(
    string displayMode,
    string continuousDisplayMode)
  {
    if (string.IsNullOrEmpty(displayMode) && string.IsNullOrEmpty(continuousDisplayMode))
      return false;
    return await ConfigUtils.TrySetAsync<PageDisplayModel>(ConfigureKeys.PageDisplay, new PageDisplayModel()
    {
      DisplayMode = displayMode,
      ContinuousDisplayMode = continuousDisplayMode
    }, new CancellationToken()).ConfigureAwait(false);
  }

  public static async Task<bool> SetPageSizeModeAsync(string pagesizeMode)
  {
    if (string.IsNullOrEmpty(pagesizeMode))
      return false;
    return await ConfigUtils.TrySetAsync<PageSizeModel>(ConfigureKeys.PageSize, new PageSizeModel()
    {
      SizeMode = pagesizeMode
    }, new CancellationToken()).ConfigureAwait(false);
  }

  public static async Task<bool> SetAutoSaveAsync(bool isAuto, int freMins)
  {
    return await ConfigUtils.TrySetAsync<AutoSaveModel>(ConfigureKeys.AutoSave, new AutoSaveModel()
    {
      IsAutoSave = isAuto,
      FrequencyMins = freMins
    }, new CancellationToken()).ConfigureAwait(false);
  }

  public static async Task<bool> SetAutoSaveFileAsync(AutoSaveFileModel model)
  {
    if (string.IsNullOrEmpty(model.Guid))
      return false;
    List<AutoSaveFileModel> autoSaveFileModelList = (await ConfigUtils.TryGetAsync<List<AutoSaveFileModel>>(ConfigureKeys.AutoSaveFile, new CancellationToken()).ConfigureAwait(false)).Item2;
    if (autoSaveFileModelList != null)
    {
      for (int index = autoSaveFileModelList.Count - 1; index >= 0; --index)
      {
        if (!string.IsNullOrEmpty(autoSaveFileModelList[index].Guid) && model.Guid == autoSaveFileModelList[index].Guid)
          autoSaveFileModelList.RemoveAt(index);
      }
    }
    else
      autoSaveFileModelList = new List<AutoSaveFileModel>();
    autoSaveFileModelList.Insert(0, model);
    return await ConfigUtils.TrySetAsync<List<AutoSaveFileModel>>(ConfigureKeys.AutoSaveFile, autoSaveFileModelList, new CancellationToken()).ConfigureAwait(false);
  }

  public static async Task<bool> DelAutoSaveFileAsync(string fileGuid, int? Pid)
  {
    if (string.IsNullOrEmpty(fileGuid))
      return false;
    List<AutoSaveFileModel> autoSaveFileModelList = (await ConfigUtils.TryGetAsync<List<AutoSaveFileModel>>(ConfigureKeys.AutoSaveFile, new CancellationToken()).ConfigureAwait(false)).Item2;
    if (autoSaveFileModelList == null)
      return false;
    for (int index = autoSaveFileModelList.Count - 1; index >= 0; --index)
    {
      if (fileGuid == autoSaveFileModelList[index].Guid)
      {
        if (Pid.HasValue && Pid.Value == autoSaveFileModelList[index].CreatePid)
          autoSaveFileModelList.RemoveAt(index);
        else
          autoSaveFileModelList.RemoveAt(index);
      }
    }
    return await ConfigUtils.TrySetAsync<List<AutoSaveFileModel>>(ConfigureKeys.AutoSaveFile, autoSaveFileModelList, new CancellationToken()).ConfigureAwait(false);
  }

  public static async Task<IReadOnlyList<string>> GetColorPickerRecentColorsAsync(
    string recentKey,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(recentKey))
      return (IReadOnlyList<string>) Array.Empty<string>();
    recentKey = recentKey.Trim();
    (bool flag, string[] strArray) = await ConfigUtils.TryGetAsync<string[]>(string.Format(ConfigureKeys.ColorPickerRecentTemplate, (object) recentKey), cancellationToken);
    return !flag ? (IReadOnlyList<string>) Array.Empty<string>() : (IReadOnlyList<string>) (strArray ?? Array.Empty<string>());
  }

  public static async Task<bool> AddColorPickerRecentColorsAsync(string recentKey, string color)
  {
    if (string.IsNullOrWhiteSpace(recentKey) || string.IsNullOrWhiteSpace(color))
      return false;
    recentKey = recentKey.Trim();
    IReadOnlyList<string> recentColorsAsync = await ConfigManager.GetColorPickerRecentColorsAsync(recentKey, new CancellationToken());
    List<string> source = (recentColorsAsync != null ? recentColorsAsync.ToList<string>() : (List<string>) null) ?? new List<string>();
    source.Remove(color);
    source.Insert(0, color);
    List<string> list = source.Take<string>(10).ToList<string>();
    return await ConfigUtils.TrySetAsync<List<string>>(string.Format(ConfigureKeys.ColorPickerRecentTemplate, (object) recentKey), list, new CancellationToken());
  }

  public static async Task<bool> ClearColorPickerRecentColorsAsync(string recentKey)
  {
    if (string.IsNullOrWhiteSpace(recentKey))
      return false;
    recentKey = recentKey.Trim();
    return await ConfigUtils.TrySetAsync<string[]>(string.Format(ConfigureKeys.ColorPickerRecentTemplate, (object) recentKey), (string[]) null, new CancellationToken());
  }

  public static async Task<bool> AddSignatureRemoveBg(string fileName)
  {
    if (string.IsNullOrEmpty(fileName))
      return false;
    List<SignatureRemoveBackgroundModel> removeBackgroundModelList = (await ConfigUtils.TryGetAsync<List<SignatureRemoveBackgroundModel>>(ConfigureKeys.SignatureRemoveBg, new CancellationToken()).ConfigureAwait(false)).Item2;
    if (removeBackgroundModelList != null)
    {
      removeBackgroundModelList.Insert(0, new SignatureRemoveBackgroundModel()
      {
        FileName = fileName
      });
    }
    else
    {
      removeBackgroundModelList = new List<SignatureRemoveBackgroundModel>();
      removeBackgroundModelList.Insert(0, new SignatureRemoveBackgroundModel()
      {
        FileName = fileName
      });
    }
    return await ConfigUtils.TrySetAsync<List<SignatureRemoveBackgroundModel>>(ConfigureKeys.SignatureRemoveBg, removeBackgroundModelList, new CancellationToken()).ConfigureAwait(false);
  }

  public static async Task<bool> RemoveSignatureRemoveBg(string fileName)
  {
    if (string.IsNullOrEmpty(fileName))
      return false;
    List<SignatureRemoveBackgroundModel> removeBackgroundModelList = (await ConfigUtils.TryGetAsync<List<SignatureRemoveBackgroundModel>>(ConfigureKeys.SignatureRemoveBg, new CancellationToken()).ConfigureAwait(false)).Item2;
    if (removeBackgroundModelList == null)
      return false;
    for (int index = removeBackgroundModelList.Count - 1; index >= 0; --index)
    {
      if (fileName == removeBackgroundModelList[index].FileName)
        removeBackgroundModelList.RemoveAt(index);
    }
    return await ConfigUtils.TrySetAsync<List<SignatureRemoveBackgroundModel>>(ConfigureKeys.SignatureRemoveBg, removeBackgroundModelList, new CancellationToken()).ConfigureAwait(false);
  }

  public static async Task<bool> IsRemoveSignatureBg(string fileName)
  {
    if (string.IsNullOrEmpty(fileName))
      return false;
    List<SignatureRemoveBackgroundModel> removeBackgroundModelList = (await ConfigUtils.TryGetAsync<List<SignatureRemoveBackgroundModel>>(ConfigureKeys.SignatureRemoveBg, new CancellationToken()).ConfigureAwait(false)).Item2;
    if (removeBackgroundModelList == null)
      return false;
    for (int index = removeBackgroundModelList.Count - 1; index >= 0; --index)
    {
      if (fileName == removeBackgroundModelList[index].FileName)
        return true;
    }
    return false;
  }

  public static async Task<bool> AddDocumentSignatures(string fileName, int pageIdx)
  {
    if (string.IsNullOrEmpty(fileName) || pageIdx < 0)
      return false;
    List<SignatureDocModel> signatureDocModelList = (await ConfigUtils.TryGetAsync<List<SignatureDocModel>>(ConfigureKeys.DocumentSignatures, new CancellationToken()).ConfigureAwait(false)).Item2;
    if (signatureDocModelList != null)
    {
      int num1 = 0;
      for (int index = signatureDocModelList.Count - 1; index >= 0; --index)
      {
        SignatureDocModel signatureDocModel = signatureDocModelList[index];
        if (signatureDocModel.FileName == fileName && signatureDocModel.PageIdx == pageIdx)
        {
          num1 = signatureDocModel.SignatureCounts;
          signatureDocModelList.RemoveAt(index);
          break;
        }
      }
      int num2 = num1 + 1;
      signatureDocModelList.Insert(0, new SignatureDocModel()
      {
        FileName = fileName,
        PageIdx = pageIdx,
        SignatureCounts = num2
      });
    }
    else
    {
      signatureDocModelList = new List<SignatureDocModel>();
      signatureDocModelList.Insert(0, new SignatureDocModel()
      {
        FileName = fileName,
        PageIdx = pageIdx,
        SignatureCounts = 1
      });
    }
    return await ConfigUtils.TrySetAsync<List<SignatureDocModel>>(ConfigureKeys.DocumentSignatures, signatureDocModelList, new CancellationToken()).ConfigureAwait(false);
  }

  public static async Task<bool> RemoveDocumentSignatures(string fileName, int pageIdx)
  {
    if (string.IsNullOrEmpty(fileName) || pageIdx < 0)
      return false;
    List<SignatureDocModel> signatureDocModelList = (await ConfigUtils.TryGetAsync<List<SignatureDocModel>>(ConfigureKeys.DocumentSignatures, new CancellationToken()).ConfigureAwait(false)).Item2;
    if (signatureDocModelList == null)
      return false;
    int num1 = 0;
    for (int index = signatureDocModelList.Count - 1; index >= 0; --index)
    {
      SignatureDocModel signatureDocModel = signatureDocModelList[index];
      if (signatureDocModel.FileName == fileName && signatureDocModel.PageIdx == pageIdx)
      {
        num1 = signatureDocModel.SignatureCounts;
        signatureDocModelList.RemoveAt(index);
        break;
      }
    }
    int num2 = num1 - 1;
    if (num2 < 0)
      num2 = 0;
    signatureDocModelList.Insert(0, new SignatureDocModel()
    {
      FileName = fileName,
      SignatureCounts = num2,
      PageIdx = pageIdx
    });
    return await ConfigUtils.TrySetAsync<List<SignatureDocModel>>(ConfigureKeys.DocumentSignatures, signatureDocModelList, new CancellationToken()).ConfigureAwait(false);
  }

  public static async Task RemoveAllSignatures(string fileName)
  {
    if (string.IsNullOrEmpty(fileName))
      return;
    List<SignatureDocModel> signatureDocModelList = (await ConfigUtils.TryGetAsync<List<SignatureDocModel>>(ConfigureKeys.DocumentSignatures, new CancellationToken()).ConfigureAwait(false)).Item2;
    if (signatureDocModelList == null)
      return;
    for (int index = signatureDocModelList.Count - 1; index >= 0; --index)
    {
      if (signatureDocModelList[index].FileName == fileName)
        signatureDocModelList.RemoveAt(index);
    }
    ConfigUtils.TrySetAsync<List<SignatureDocModel>>(ConfigureKeys.DocumentSignatures, signatureDocModelList, new CancellationToken()).ConfigureAwait(false);
  }

  public static async Task<bool> IsExistSignatures(string fileName)
  {
    if (string.IsNullOrEmpty(fileName))
      return false;
    List<SignatureDocModel> signatureDocModelList = (await ConfigUtils.TryGetAsync<List<SignatureDocModel>>(ConfigureKeys.DocumentSignatures, new CancellationToken()).ConfigureAwait(false)).Item2;
    return signatureDocModelList != null && signatureDocModelList.FindAll((Predicate<SignatureDocModel>) (s => s.FileName == fileName)).Sum<SignatureDocModel>((Func<SignatureDocModel, int>) (x => x.SignatureCounts)) > 0;
  }

  public static async Task<IReadOnlyList<SignatureDocModel>> GetDocumentSignaturesAsync(
    string fileName,
    CancellationToken cancellationToken)
  {
    (bool flag, List<SignatureDocModel> signatureDocModelList) = await ConfigUtils.TryGetAsync<List<SignatureDocModel>>(ConfigureKeys.DocumentSignatures, cancellationToken);
    return flag ? (IReadOnlyList<SignatureDocModel>) signatureDocModelList.FindAll((Predicate<SignatureDocModel>) (f => f.FileName == fileName)) : (IReadOnlyList<SignatureDocModel>) null;
  }

  public static async Task<IReadOnlyList<SignatureDocModel>> GetAppSignaturesAsync(
    CancellationToken cancellationToken)
  {
    (bool flag, List<SignatureDocModel> signatureDocModelList) = await ConfigUtils.TryGetAsync<List<SignatureDocModel>>(ConfigureKeys.DocumentSignatures, cancellationToken);
    return flag ? (IReadOnlyList<SignatureDocModel>) signatureDocModelList : (IReadOnlyList<SignatureDocModel>) null;
  }

  public static void SetDebugMode(bool flag)
  {
    ConfigManager.settings.Values["PDF_DEBUG_MODE"] = flag.ToString();
  }

  public static bool GetDebugMode()
  {
    try
    {
      string str;
      if (ConfigManager.settings.Values.TryGetValue("PDF_DEBUG_MODE", out str))
      {
        bool result;
        if (bool.TryParse(str, out result))
          return result;
      }
    }
    catch
    {
    }
    return false;
  }

  public static async Task<int> GetAutoScrollSpeedAsync(int defaultValue)
  {
    (bool flag, int num) = await ConfigUtils.TryGetAsync<int>(ConfigureKeys.AutoScrollSpeed, new CancellationToken());
    return !flag ? defaultValue : num;
  }

  public static async Task SetAutoScrollSpeedAsync(int speed)
  {
    int num = await ConfigUtils.TrySetAsync<int>(ConfigureKeys.AutoScrollSpeed, speed, new CancellationToken()) ? 1 : 0;
  }

  public static async Task<string> GetScreenshotOcrLanguage(string defaultValue)
  {
    (bool flag, string str) = await ConfigUtils.TryGetAsync<string>(ConfigureKeys.ScreenshotOcrLanguage, new CancellationToken());
    return !flag ? defaultValue : str;
  }

  public static async Task SetScreenshotOcrLanguageAsync(string cultureInfoName)
  {
    int num = await ConfigUtils.TrySetAsync<string>(ConfigureKeys.ScreenshotOcrLanguage, cultureInfoName, new CancellationToken()) ? 1 : 0;
  }

  public static async Task<PageStyleModel> GetPageStyleAsync(CancellationToken cancellationToken)
  {
    (bool flag, PageStyleModel pageStyleModel) = await ConfigUtils.TryGetAsync<PageStyleModel>(ConfigureKeys.PageStyle, cancellationToken);
    return flag ? pageStyleModel : (PageStyleModel) null;
  }

  public static async Task<bool> SetPageStyleAsync(
    double leftViewWidth,
    bool leftViewIsExpand,
    string selectItem,
    double rightViewWidth)
  {
    return await ConfigUtils.TrySetAsync<PageStyleModel>(ConfigureKeys.PageStyle, new PageStyleModel()
    {
      LeftNavigationViewWidth = leftViewWidth,
      LeftNavigationViewIsExpand = leftViewIsExpand,
      LeftNavigationViewSelectItem = selectItem,
      RightNavigationViewWidth = rightViewWidth
    }, new CancellationToken()).ConfigureAwait(false);
  }

  public static async Task<PageSizeZoomModel> GetPageSizeZoomModelAsync(
    string filePath,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrEmpty(filePath))
      return (PageSizeZoomModel) null;
    (bool flag, List<PageSizeZoomModel> source) = await ConfigUtils.TryGetAsync<List<PageSizeZoomModel>>(ConfigureKeys.PageSizeZoom, cancellationToken);
    if (!flag)
      return (PageSizeZoomModel) null;
    string _filepath = filePath.Trim().ToUpperInvariant();
    return source != null ? source.FirstOrDefault<PageSizeZoomModel>((Func<PageSizeZoomModel, bool>) (c => c.FilePath == _filepath)) : (PageSizeZoomModel) null;
  }

  public static async Task<bool> SetPageSizeZoomModelAsync(
    string filePath,
    string sizeModel,
    float pageZoom)
  {
    if (string.IsNullOrEmpty(filePath))
      return false;
    List<PageSizeZoomModel> pageSizeZoomModelList = (await ConfigUtils.TryGetAsync<List<PageSizeZoomModel>>(ConfigureKeys.PageSizeZoom, new CancellationToken()).ConfigureAwait(false)).Item2;
    string upperInvariant = filePath.Trim().ToUpperInvariant();
    if (pageSizeZoomModelList != null)
    {
      for (int index = pageSizeZoomModelList.Count - 1; index >= 0; --index)
      {
        if (string.IsNullOrEmpty(pageSizeZoomModelList[index].FilePath) || pageSizeZoomModelList[index].FilePath == upperInvariant)
          pageSizeZoomModelList.RemoveAt(index);
      }
    }
    else
      pageSizeZoomModelList = new List<PageSizeZoomModel>();
    pageSizeZoomModelList.Insert(0, new PageSizeZoomModel()
    {
      FilePath = upperInvariant,
      PageZoom = pageZoom,
      SizeMode = sizeModel
    });
    return await ConfigUtils.TrySetAsync<List<PageSizeZoomModel>>(ConfigureKeys.PageSizeZoom, pageSizeZoomModelList, new CancellationToken()).ConfigureAwait(false);
  }

  public static string GetApplicationLanugageName()
  {
    string str;
    return ConfigManager.settings.Values.TryGetValue("ApplicationLanugageName", out str) && str != null ? str : string.Empty;
  }

  public static void SetApplicationLanugageName(string name)
  {
    ConfigManager.settings.Values["ApplicationLanugageName"] = name ?? string.Empty;
  }

  public static string GetPageDefaultSize()
  {
    string str;
    return ConfigManager.settings.Values.TryGetValue("DefaultSize", out str) && str != null ? str : "FitToWidth";
  }

  public static void SetPageDefaultSize(string sizemode)
  {
    ConfigManager.settings.Values["DefaultSize"] = sizemode ?? "FitToWidth";
  }

  public static void SetAnnotationAuthorName(string authorName)
  {
    ConfigManager.settings.Values["AnnotationAuthorName"] = authorName.Trim();
  }

  public static string GetAnnotationAuthorName()
  {
    string str;
    return ConfigManager.settings.Values.TryGetValue("AnnotationAuthorName", out str) && str != null ? str : string.Empty;
  }

  public static void SetCreateShortcutFlag(bool flag)
  {
    ConfigManager.settings.Values["CreateShortcutFlag"] = flag.ToString();
  }

  public static bool GetCreateShortcutFlag()
  {
    try
    {
      string str;
      if (ConfigManager.settings.Values.TryGetValue("CreateShortcutFlag", out str))
      {
        bool result;
        if (bool.TryParse(str, out result))
          return result;
      }
    }
    catch
    {
    }
    return false;
  }

  public static void SetCIDEventFlag(bool flag)
  {
    ConfigManager.settings.Values["CIDEventFlag"] = flag.ToString();
  }

  public static bool GetCIDEventFlag()
  {
    try
    {
      string str;
      if (ConfigManager.settings.Values.TryGetValue("CIDEventFlag", out str))
      {
        bool result;
        if (bool.TryParse(str, out result))
          return result;
      }
    }
    catch
    {
    }
    return false;
  }

  public static void SetUserCommunityFlag(bool flag)
  {
    ConfigManager.settings.Values["UserCommunityFlag"] = flag.ToString();
  }

  public static bool GetUserCommunityFlag()
  {
    try
    {
      string str;
      if (ConfigManager.settings.Values.TryGetValue("UserCommunityFlag", out str))
      {
        bool result;
        if (bool.TryParse(str, out result))
          return result;
      }
    }
    catch
    {
    }
    return false;
  }

  public static string GetPushChannelUri()
  {
    try
    {
      string pushChannelUri;
      if (ConfigManager.settings.Values.TryGetValue("PushChannelUri", out pushChannelUri))
      {
        if (pushChannelUri != null)
          return pushChannelUri;
      }
    }
    catch
    {
    }
    return (string) null;
  }

  public static DateTimeOffset GetPushChannelExpired()
  {
    try
    {
      string s;
      if (ConfigManager.settings.Values.TryGetValue("PushChannelExpired", out s))
      {
        if (s != null)
        {
          long result;
          if (long.TryParse(s, out result))
            return DateTimeOffset.FromUnixTimeMilliseconds(result);
        }
      }
    }
    catch
    {
    }
    return new DateTimeOffset();
  }

  public static void SetPushChannelUri(string channelUri)
  {
    ConfigManager.settings.Values["PushChannelUri"] = channelUri;
  }

  public static void SetPushChannelExpired(DateTimeOffset? expired)
  {
    if (expired.HasValue)
      ConfigManager.settings.Values["PushChannelExpired"] = $"{expired.Value.ToUnixTimeMilliseconds()}";
    else
      ConfigManager.settings.Values["PushChannelExpired"] = "";
  }

  public static async Task SetDefaultAppActionAsync(string action)
  {
    int num = await ConfigUtils.TrySetAsync<string>(ConfigureKeys.DefaultAppAction, action ?? "", new CancellationToken()) ? 1 : 0;
  }

  public static async Task<string> GetDefaultAppActionAsync()
  {
    (bool flag, string str) = await ConfigUtils.TryGetAsync<string>(ConfigureKeys.DefaultAppAction, new CancellationToken());
    return !flag ? string.Empty : str;
  }

  public static bool GetTest() => false;

  public static void SetTest(bool value)
  {
    ConfigUtils.TrySet<string>("ID_A54C4E30729A469DA8F0864FDB400881", (string) null);
  }

  public static void SetSubscriptionFlag(bool value)
  {
    ConfigManager.settings.Values["SubscriptionFlag2"] = value.ToString();
  }

  public static bool GetSubscriptionFlag()
  {
    try
    {
      string str;
      if (ConfigManager.settings.Values.TryGetValue("SubscriptionFlag2", out str))
      {
        bool result;
        if (bool.TryParse(str, out result))
          return result;
      }
    }
    catch
    {
    }
    return false;
  }

  public static void SetSurveyFlag(bool value)
  {
    ConfigManager.settings.Values["SurveyFlag"] = value.ToString();
  }

  public static bool GetSurveyFlag()
  {
    try
    {
      string str;
      if (ConfigManager.settings.Values.TryGetValue("SurveyFlag", out str))
      {
        bool result;
        if (bool.TryParse(str, out result))
          return result;
      }
    }
    catch
    {
    }
    return false;
  }

  public static void SetPhoneQRCodeFlag(bool value)
  {
    ConfigManager.settings.Values["PhoneQRCodeFlag"] = value.ToString();
  }

  public static bool GetPhoneQRCodeFlag()
  {
    try
    {
      string str;
      if (ConfigManager.settings.Values.TryGetValue("PhoneQRCodeFlag", out str))
      {
        bool result;
        if (bool.TryParse(str, out result))
          return result;
      }
    }
    catch
    {
    }
    return false;
  }

  public static void SetChatPanelFirstClose(bool value)
  {
    ConfigManager.settings.Values["ChatPanelFirstClose"] = value.ToString();
  }

  public static bool GetChatPanelFirstClose()
  {
    try
    {
      string str;
      if (ConfigManager.settings.Values.TryGetValue("ChatPanelFirstClose", out str))
      {
        bool result;
        if (bool.TryParse(str, out result))
          return result;
      }
    }
    catch
    {
    }
    return false;
  }

  public static void SetCurrentAppTheme(string name)
  {
    if (name != "Auto" && name != "Dark" && name != "Light")
      return;
    ConfigManager.settings.Values["CurrentAppTheme"] = name ?? "Light";
  }

  public static string GetCurrentAppTheme()
  {
    string str;
    return ConfigManager.settings.Values.TryGetValue("CurrentAppTheme", out str) && str != null ? str : "Light";
  }

  public static void SetChatPanelClosed(bool value)
  {
    ConfigManager.settings.Values["ChatPanelClosed"] = value.ToString();
  }

  public static bool GetChatPanelClosed()
  {
    try
    {
      string str;
      if (ConfigManager.settings.Values.TryGetValue("ChatPanelClosed", out str))
      {
        bool result;
        if (bool.TryParse(str, out result))
          return result;
      }
    }
    catch
    {
    }
    return false;
  }

  public static void SetBookmarkWrapFlag(bool flag)
  {
    ConfigManager.settings.Values["BookmarkWrapFlag"] = flag ? "1" : "0";
  }

  public static bool GetBookmarkWrapFlag()
  {
    string str;
    return ConfigManager.settings.Values.TryGetValue("BookmarkWrapFlag", out str) && str == "1";
  }

  public static void SetShowcaseChatButtonFlag(bool flag)
  {
    string key = "ShowcaseChatButtonFlag";
    ConfigManager.settings.Values[key] = flag.ToString();
  }

  public static bool GetShowcaseChatButtonFlag()
  {
    try
    {
      string str;
      if (ConfigManager.settings.Values.TryGetValue("ShowcaseChatButtonFlag", out str))
      {
        bool result;
        if (bool.TryParse(str, out result))
          return result;
      }
    }
    catch
    {
    }
    return true;
  }

  public static void SetIsFillFormHighlightedFlag(bool flag)
  {
    string key = "IsFillFormHighlightedFlag";
    ConfigManager.settings.Values[key] = flag.ToString();
  }

  public static bool GetIsFillFormHighlightedFlag()
  {
    try
    {
      string str;
      if (ConfigManager.settings.Values.TryGetValue("IsFillFormHighlightedFlag", out str))
      {
        bool result;
        if (bool.TryParse(str, out result))
          return result;
      }
    }
    catch
    {
    }
    return true;
  }

  public static string GetPrintSetting([CallerMemberName] string key = "")
  {
    string str;
    return ConfigManager.settings.Values.TryGetValue(key, out str) ? str.ToString() : string.Empty;
  }

  public static void SetPrintSetting(string value, [CallerMemberName] string key = "")
  {
    ConfigManager.settings.Values[key] = value;
  }

  public static bool GetDoNotShowFlag(string name, bool defaultValue = false)
  {
    name = name?.Trim();
    if (string.IsNullOrEmpty(name))
      return false;
    string str;
    return ConfigManager.settings.Values.TryGetValue(name, out str) ? str == "1" : defaultValue;
  }

  public static void SetDoNotShowFlag(string name, bool flag)
  {
    name = name?.Trim();
    if (string.IsNullOrEmpty(name))
      return;
    ConfigManager.settings.Values[name] = flag ? "1" : "0";
  }

  public static void SetConvertViewFileInExplore(bool flag)
  {
    string key = "ConvertViewFileInExplore";
    ConfigManager.settings.Values[key] = flag.ToString();
  }

  public static bool GetConvertViewFileInExplore()
  {
    try
    {
      string str;
      if (ConfigManager.settings.Values.TryGetValue("ConvertViewFileInExplore", out str))
      {
        bool result;
        if (bool.TryParse(str, out result))
          return result;
      }
    }
    catch
    {
    }
    return true;
  }

  public static double GetScreenshotThickness()
  {
    try
    {
      string s;
      if (ConfigManager.settings.Values.TryGetValue("ScreenshotThickness", out s))
      {
        double result;
        if (double.TryParse(s, out result))
          return result;
      }
    }
    catch
    {
    }
    return 3.0;
  }

  public static void SetScreenshotThickness(double value)
  {
    string key = "ScreenshotThickness";
    ConfigManager.settings.Values[key] = value.ToString();
  }

  public static Color GetScreenshotColor()
  {
    try
    {
      string str;
      if (ConfigManager.settings.Values.TryGetValue("ScreenshotColor", out str))
        return (Color) ColorConverter.ConvertFromString(str);
    }
    catch
    {
    }
    return Color.FromRgb((byte) 251, (byte) 48 /*0x30*/, (byte) 47);
  }

  public static void SetScreenshotColor(Color value)
  {
    string key = "ScreenshotColor";
    ConfigManager.settings.Values[key] = value.ToString();
  }

  public static double GetScreenshotFontSize()
  {
    try
    {
      string s;
      if (ConfigManager.settings.Values.TryGetValue("ScreenshotFontSize", out s))
      {
        double result;
        if (double.TryParse(s, out result))
          return result;
      }
    }
    catch
    {
    }
    return 12.0;
  }

  public static void SetScreenshotFontSize(double value)
  {
    string key = "ScreenshotFontSize";
    ConfigManager.settings.Values[key] = value.ToString();
  }

  public static void SetDocumentPropertiesUnit(int index)
  {
    string key = "DocumentPropertiesUnit";
    ConfigManager.settings.Values[key] = index.ToString();
  }

  public static int GetDocumentPropertiesUnit()
  {
    string key = "DocumentPropertiesUnit";
    int result;
    return !ConfigManager.settings.Values.ContainsKey(key) || !int.TryParse(ConfigManager.settings.Values[key], out result) ? 0 : result;
  }
}
