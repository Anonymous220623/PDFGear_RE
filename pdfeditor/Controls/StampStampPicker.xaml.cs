// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Stamp.StampPicker
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using CommomLib.Commom;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.WindowsAPICodePack.Shell;
using Newtonsoft.Json;
using pdfeditor.Controls.Annotations;
using pdfeditor.Controls.Signature;
using pdfeditor.Models.Menus;
using pdfeditor.Utils;
using pdfeditor.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Threading;

#nullable disable
namespace pdfeditor.Controls.Stamp;

public partial class StampPicker : Control
{
  private ObservableCollection<CustStampModel> stampModels;
  private Button delItemButton;
  private Button addNewButton;
  private ItemsControl pickerItemContainer;
  public static readonly RoutedEvent ItemClickEvent = EventManager.RegisterRoutedEvent("ItemClick", RoutingStrategy.Bubble, typeof (EventHandler<StampPickerItemClickEventArgs>), typeof (StampPicker));

  static StampPicker()
  {
    FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (StampPicker), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (StampPicker)));
  }

  public StampPicker()
  {
    this.Loaded += new RoutedEventHandler(this.SignatureTemplatePreviewControl_Loaded);
    this.stampModels = new ObservableCollection<CustStampModel>();
  }

  public MainViewModel VM => Ioc.Default.GetRequiredService<MainViewModel>();

  private void SignatureTemplatePreviewControl_Loaded(object sender, RoutedEventArgs e)
  {
    try
    {
      string path = Path.Combine(AppDataHelper.LocalFolder, "Stamp");
      if (!Directory.Exists(path))
        Directory.CreateDirectory(path);
      List<string> list = ((IEnumerable<string>) Directory.GetFiles(path)).ToList<string>();
      List<StampTextModel> stampTextModelList = ToolbarContextMenuHelper.ReadStamp();
      if (list.Count == 0 && stampTextModelList == null)
        return;
      this.stampModels?.Clear();
      List<CustStampModel> source = new List<CustStampModel>();
      if (list.Count > 0)
      {
        List<FileInfo> fileInfos = new List<FileInfo>();
        list.ForEach((Action<string>) (f => fileInfos.Add(new FileInfo(f))));
        foreach (FileInfo fileInfo in fileInfos)
        {
          CustStampModel custStampModel = new CustStampModel()
          {
            ImageFilePath = fileInfo.FullName,
            Image = "Visible",
            Text = "Collapsed",
            dateTime = fileInfo.CreationTime
          };
          try
          {
            ShellFile shellFile = ShellFile.FromFilePath(fileInfo.FullName);
            custStampModel.StampImageSource = shellFile.Thumbnail.LargeBitmapSource;
            source.Add(custStampModel);
          }
          catch
          {
          }
        }
      }
      if (stampTextModelList != null)
      {
        foreach (StampTextModel stampTextModel in stampTextModelList)
        {
          CustStampModel custStampModel = new CustStampModel()
          {
            TextContent = stampTextModel.TextContent,
            FontColor = stampTextModel.FontColor,
            GroupId = stampTextModel.GroupId,
            Image = "Collapsed",
            Text = "Visible",
            dateTime = stampTextModel.dateTime
          };
          source.Add(custStampModel);
        }
      }
      foreach (CustStampModel custStampModel in source.OrderByDescending<CustStampModel, DateTime>((Func<CustStampModel, DateTime>) (x => x.dateTime)).Take<CustStampModel>(50))
        this.stampModels.Add(custStampModel);
    }
    catch
    {
    }
    this.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, (Delegate) new Action(this.DelayedAction));
  }

  private Button AddNewButton
  {
    get => this.addNewButton;
    set
    {
      if (this.addNewButton == value)
        return;
      if (this.addNewButton != null)
        this.addNewButton.Click -= new RoutedEventHandler(this.AddNewButton_Click);
      this.addNewButton = value;
      if (this.addNewButton == null)
        return;
      this.addNewButton.Click += new RoutedEventHandler(this.AddNewButton_Click);
    }
  }

  private ItemsControl PickerItemContainer
  {
    get => this.pickerItemContainer;
    set
    {
      if (this.pickerItemContainer == value)
        return;
      if (this.pickerItemContainer != null)
      {
        this.pickerItemContainer.ItemsSource = (IEnumerable) null;
        this.pickerItemContainer.ItemContainerGenerator.StatusChanged -= new EventHandler(this.ItemContainerGenerator_StatusChanged);
      }
      this.pickerItemContainer = value;
      if (this.pickerItemContainer == null)
        return;
      this.pickerItemContainer.ItemsSource = (IEnumerable) this.stampModels;
      this.pickerItemContainer.ItemContainerGenerator.StatusChanged += new EventHandler(this.ItemContainerGenerator_StatusChanged);
    }
  }

  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();
    this.AddNewButton = this.GetTemplateChild("AddNewButton") as Button;
    if (this.PickerItemContainer != null)
    {
      this.PickerItemContainer.ItemsSource = (IEnumerable) null;
      this.PickerItemContainer.ItemContainerGenerator.StatusChanged -= new EventHandler(this.ItemContainerGenerator_StatusChanged);
    }
    this.PickerItemContainer = this.GetTemplateChild("PickerItemContainer") as ItemsControl;
    if (this.PickerItemContainer == null)
      return;
    this.PickerItemContainer.ItemsSource = (IEnumerable) this.stampModels;
    this.PickerItemContainer.ItemContainerGenerator.StatusChanged += new EventHandler(this.ItemContainerGenerator_StatusChanged);
  }

  private void ItemContainerGenerator_StatusChanged(object sender, EventArgs e)
  {
    if (this.PickerItemContainer.ItemContainerGenerator.Status != GeneratorStatus.ContainersGenerated)
      return;
    this.PickerItemContainer.ItemContainerGenerator.StatusChanged -= new EventHandler(this.ItemContainerGenerator_StatusChanged);
    this.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, (Delegate) new Action(this.DelayedAction));
  }

  private void DelayedAction()
  {
    for (int index = 0; index < this.PickerItemContainer.Items.Count; ++index)
    {
      DependencyObject parentObj = this.PickerItemContainer.ItemContainerGenerator.ContainerFromIndex(index);
      Grid singleVisualChildren1 = StampPicker.FindSingleVisualChildren<Grid>(parentObj);
      Button singleVisualChildren2 = StampPicker.FindSingleVisualChildren<Button>(parentObj);
      if (singleVisualChildren1 != null)
      {
        singleVisualChildren1.RemoveHandler(UIElement.MouseLeftButtonUpEvent, (Delegate) new RoutedEventHandler(this.PickerItem_Click));
        singleVisualChildren1.AddHandler(UIElement.MouseLeftButtonUpEvent, (Delegate) new RoutedEventHandler(this.PickerItem_Click));
      }
      if (singleVisualChildren2 != null)
      {
        singleVisualChildren2.RemoveHandler(ButtonBase.ClickEvent, (Delegate) new RoutedEventHandler(this.DelItemButton_Click));
        singleVisualChildren2.AddHandler(ButtonBase.ClickEvent, (Delegate) new RoutedEventHandler(this.DelItemButton_Click));
      }
    }
  }

  private void AddNewButton_Click(object sender, RoutedEventArgs e)
  {
    if (this.stampModels.Count >= 50)
      new SignatureSaveNumTipWin().ShowDialog();
    else
      this.VM.AnnotationToolbar.DoStampCmd((ContextMenuItemModel) null);
  }

  private void DelItemButton_Click(object sender, RoutedEventArgs e)
  {
    if (!(e.OriginalSource is FrameworkElement originalSource))
      return;
    CustStampModel item = originalSource.DataContext as CustStampModel;
    if (item == null)
      return;
    if (item.Image == "Visible")
    {
      string path = Path.Combine(AppDataHelper.LocalFolder, "Stamp");
      if (!Directory.Exists(path))
        Directory.CreateDirectory(path);
      if (!((IEnumerable<string>) Directory.GetFiles(path)).ToList<string>().Contains(item.ImageFilePath))
        return;
      try
      {
        this.stampModels.Remove(item);
        string imageFilePath = item.ImageFilePath;
        item = (CustStampModel) null;
        File.Delete(imageFilePath);
        ((IEnumerable<string>) Directory.GetFiles(path)).ToList<string>();
        ConfigManager.RemoveSignatureRemoveBg(imageFilePath);
      }
      catch (Exception ex)
      {
      }
    }
    else
    {
      List<StampTextModel> stampTextModelList = ToolbarContextMenuHelper.ReadStamp();
      stampTextModelList.Remove(stampTextModelList.Find((Predicate<StampTextModel>) (x => x.GroupId == item.GroupId)));
      string str1 = Path.Combine(AppDataHelper.LocalCacheFolder, "Config");
      if (!Directory.Exists(str1))
        Directory.CreateDirectory(str1);
      string path = Path.Combine(str1, "Stamp.json");
      try
      {
        this.stampModels.Remove(item);
        using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
        {
          using (StreamWriter streamWriter = new StreamWriter((Stream) fileStream))
          {
            string str2 = JsonConvert.SerializeObject((object) stampTextModelList, Formatting.Indented, new JsonSerializerSettings()
            {
              TypeNameHandling = TypeNameHandling.Auto
            });
            streamWriter.Write(str2);
            streamWriter.Close();
          }
          fileStream.Close();
        }
      }
      catch (Exception ex)
      {
      }
    }
  }

  private async void PickerItem_Click(object sender, RoutedEventArgs e)
  {
    if (this.VM.Document != null && e.Source is FrameworkElement source && source.DataContext is CustStampModel dataContext)
    {
      GAManager.SendEvent("PdfStampAnnotation", "DoStamp", "Custom", 1L);
      if (dataContext.Image == "Visible")
      {
        this.RaiseItemClickEvent(dataContext);
        e.Handled = true;
        this.VM.AnnotationMode = AnnotationMode.Stamp;
        await this.VM.AnnotationToolbar.ProcessStampImageModelAsync(new StampImageModel()
        {
          ImageFilePath = dataContext.ImageFilePath
        });
      }
      else
      {
        e.Handled = true;
        this.VM.AnnotationMode = AnnotationMode.Stamp;
        await this.VM.AnnotationToolbar.ProcessStampTextModelAsync((IStampTextModel) new StampTextModel()
        {
          FontColor = dataContext.FontColor,
          TextContent = dataContext.TextContent,
          GroupId = dataContext.GroupId
        });
      }
    }
    this.VM.AnnotationMode = AnnotationMode.None;
  }

  private void UpdateItemsSource()
  {
  }

  private void RaiseItemClickEvent(CustStampModel item)
  {
    if (item == null)
      return;
    this.RaiseEvent((RoutedEventArgs) new StampPickerItemClickEventArgs((object) this, new ImageStampModel()
    {
      ImageFilePath = item.ImageFilePath,
      StampImageSource = item.StampImageSource,
      ImageHeight = item.ImageHeight,
      ImageWidth = item.ImageWidth,
      PageSize = item.PageSize
    }));
  }

  public event EventHandler<StampPickerItemClickEventArgs> ItemClick
  {
    add => this.AddHandler(StampPicker.ItemClickEvent, (Delegate) value);
    remove => this.RemoveHandler(StampPicker.ItemClickEvent, (Delegate) value);
  }

  public static T FindSingleVisualChildren<T>(DependencyObject parentObj) where T : DependencyObject
  {
    T singleVisualChildren = default (T);
    if (parentObj != null)
    {
      for (int childIndex = 0; childIndex < VisualTreeHelper.GetChildrenCount(parentObj); ++childIndex)
      {
        DependencyObject child = VisualTreeHelper.GetChild(parentObj, childIndex);
        if (child != null && child is T)
        {
          singleVisualChildren = child as T;
          break;
        }
        singleVisualChildren = StampPicker.FindSingleVisualChildren<T>(child);
        if ((object) singleVisualChildren != null)
          break;
      }
    }
    return singleVisualChildren;
  }
}
