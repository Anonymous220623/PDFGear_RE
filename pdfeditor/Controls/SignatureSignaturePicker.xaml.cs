// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Signature.SignaturePicker
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using CommomLib.Commom;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.WindowsAPICodePack.Shell;
using pdfeditor.Controls.Annotations;
using pdfeditor.Models.Menus;
using pdfeditor.ViewModels;
using System;
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
namespace pdfeditor.Controls.Signature;

public partial class SignaturePicker : Control
{
  public static readonly DependencyProperty ImageSignaturesTemplatesProperty = DependencyProperty.Register(" ImageSignaturesTemplates", typeof (ObservableCollection<ImageStampModel>), typeof (SignaturePicker), new PropertyMetadata((object) new ObservableCollection<ImageStampModel>()));
  public static readonly DependencyProperty IsExistTemplateProperty = DependencyProperty.Register(nameof (IsExistTemplate), typeof (bool), typeof (SignaturePicker), new PropertyMetadata((object) false));
  private Button delItemButton;
  private Button addNewButton;
  private ItemsControl pickerItemContainer;
  public static readonly RoutedEvent ItemClickEvent = EventManager.RegisterRoutedEvent("ItemClick", RoutingStrategy.Bubble, typeof (EventHandler<SignaturePickerItemClickEventArgs>), typeof (SignaturePicker));

  static SignaturePicker()
  {
    FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (SignaturePicker), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (SignaturePicker)));
  }

  public SignaturePicker()
  {
    this.Loaded += new RoutedEventHandler(this.SignatureTemplatePreviewControl_Loaded);
  }

  public ObservableCollection<ImageStampModel> ImageSignaturesTemplates
  {
    get
    {
      return (ObservableCollection<ImageStampModel>) this.GetValue(SignaturePicker.ImageSignaturesTemplatesProperty);
    }
    set => this.SetValue(SignaturePicker.ImageSignaturesTemplatesProperty, (object) value);
  }

  public bool IsExistTemplate
  {
    get => (bool) this.GetValue(SignaturePicker.IsExistTemplateProperty);
    set => this.SetValue(SignaturePicker.IsExistTemplateProperty, (object) value);
  }

  public MainViewModel VM => Ioc.Default.GetRequiredService<MainViewModel>();

  private void SignatureTemplatePreviewControl_Loaded(object sender, RoutedEventArgs e)
  {
    string path = Path.Combine(AppDataHelper.LocalFolder, "Signature");
    if (!Directory.Exists(path))
      Directory.CreateDirectory(path);
    List<string> list = ((IEnumerable<string>) Directory.GetFiles(path)).ToList<string>();
    this.IsExistTemplate = list.Count<string>() > 0;
    if (list.Count == 0)
      return;
    this.ImageSignaturesTemplates?.Clear();
    List<FileInfo> fileInfos = new List<FileInfo>();
    list.ForEach((Action<string>) (f => fileInfos.Add(new FileInfo(f))));
    foreach (FileInfo fileInfo in fileInfos.OrderByDescending<FileInfo, DateTime>((Func<FileInfo, DateTime>) (f => f.CreationTime)).ToList<FileInfo>().Take<FileInfo>(50))
    {
      ImageStampModel imageStampModel = new ImageStampModel()
      {
        ImageFilePath = fileInfo.FullName
      };
      try
      {
        ShellFile shellFile = ShellFile.FromFilePath(fileInfo.FullName);
        imageStampModel.StampImageSource = shellFile.Thumbnail.LargeBitmapSource;
        this.ImageSignaturesTemplates.Add(imageStampModel);
      }
      catch
      {
      }
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
      this.pickerItemContainer = value;
    }
  }

  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();
    this.AddNewButton = this.GetTemplateChild("AddNewButton") as Button;
    this.PickerItemContainer = this.GetTemplateChild("PickerItemContainer") as ItemsControl;
    this.PickerItemContainer.ItemContainerGenerator.StatusChanged -= new EventHandler(this.ItemContainerGenerator_StatusChanged);
    this.PickerItemContainer.ItemContainerGenerator.StatusChanged += new EventHandler(this.ItemContainerGenerator_StatusChanged);
  }

  private void ItemContainerGenerator_StatusChanged(object sender, EventArgs e)
  {
    if (this.PickerItemContainer.ItemContainerGenerator.Status != GeneratorStatus.ContainersGenerated)
      return;
    this.PickerItemContainer.ItemContainerGenerator.StatusChanged -= new EventHandler(this.ItemContainerGenerator_StatusChanged);
    this.Dispatcher.BeginInvoke(DispatcherPriority.DataBind, (Delegate) new Action(this.DelayedAction));
  }

  private void DelayedAction()
  {
    for (int index = 0; index < this.PickerItemContainer.Items.Count; ++index)
    {
      DependencyObject parentObj = this.PickerItemContainer.ItemContainerGenerator.ContainerFromIndex(index);
      Grid singleVisualChildren1 = SignaturePicker.FindSingleVisualChildren<Grid>(parentObj);
      Button singleVisualChildren2 = SignaturePicker.FindSingleVisualChildren<Button>(parentObj);
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
    if (this.ImageSignaturesTemplates != null && this.ImageSignaturesTemplates.Count >= 50)
      new SignatureSaveNumTipWin().ShowDialog();
    else
      this.VM.AnnotationToolbar.DoSignatureMenuCmd((ContextMenuItemModel) null);
  }

  private void DelItemButton_Click(object sender, RoutedEventArgs e)
  {
    if (!(e.OriginalSource is FrameworkElement originalSource) || !(originalSource.DataContext is ImageStampModel dataContext))
      return;
    string path = Path.Combine(AppDataHelper.LocalFolder, "Signature");
    if (!Directory.Exists(path))
      Directory.CreateDirectory(path);
    if (!((IEnumerable<string>) Directory.GetFiles(path)).ToList<string>().Contains(dataContext.ImageFilePath))
      return;
    try
    {
      this.ImageSignaturesTemplates.Remove(dataContext);
      string imageFilePath = dataContext.ImageFilePath;
      File.Delete(imageFilePath);
      this.IsExistTemplate = ((IEnumerable<string>) Directory.GetFiles(path)).ToList<string>().Count<string>() > 0;
      ConfigManager.RemoveSignatureRemoveBg(imageFilePath);
    }
    catch (Exception ex)
    {
    }
  }

  private async void PickerItem_Click(object sender, RoutedEventArgs e)
  {
    if (this.VM.Document == null || !(e.OriginalSource is FrameworkElement originalSource) || !(originalSource.DataContext is ImageStampModel dataContext))
      return;
    this.RaiseItemClickEvent(dataContext);
    e.Handled = true;
    this.VM.AnnotationMode = AnnotationMode.Signature;
    await this.VM.AnnotationToolbar.ProcessStampImageModelAsync(new StampImageModel()
    {
      ImageFilePath = dataContext.ImageFilePath,
      IsSignature = true
    });
  }

  private void UpdateItemsSource()
  {
  }

  private void RaiseItemClickEvent(ImageStampModel item)
  {
    if (item == null)
      return;
    this.RaiseEvent((RoutedEventArgs) new SignaturePickerItemClickEventArgs((object) this, item));
  }

  public event EventHandler<SignaturePickerItemClickEventArgs> ItemClick
  {
    add => this.AddHandler(SignaturePicker.ItemClickEvent, (Delegate) value);
    remove => this.RemoveHandler(SignaturePicker.ItemClickEvent, (Delegate) value);
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
        singleVisualChildren = SignaturePicker.FindSingleVisualChildren<T>(child);
        if ((object) singleVisualChildren != null)
          break;
      }
    }
    return singleVisualChildren;
  }
}
