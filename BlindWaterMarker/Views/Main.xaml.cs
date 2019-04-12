using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace BlindWaterMarker.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Main : Page
    {
        public Main()
        {
            this.InitializeComponent();

            
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await PickFile();
            
        }

        
        // 选取图片
        private async Task PickFile()
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker
            {
                ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail,
                SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary
            }; //新建文件选取器
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg"); //联合图像专家组（JPEG）
            picker.FileTypeFilter.Add(".png"); //便携式网络图形（PNG）
            picker.FileTypeFilter.Add(".gif"); //图形交换格式（GIF）
            picker.FileTypeFilter.Add(".tiff"); //标记图像文件格式（TIFF）
            picker.FileTypeFilter.Add(".bmp"); //位图（BMP）
            picker.FileTypeFilter.Add(".ico"); //图标（ICO）

            

            // Windows.Storage.StorageFile file = await picker.PickSingleFileAsync();
            StorageFile file = await picker.PickSingleFileAsync();

            if (file != null)
            {
                // IRandomAccessStream stream = await file.OpenReadAsync();
                // <BitmapImage有关文档：https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.media.imaging.bitmapimage>
                // BitmapImage bitmap = new BitmapImage(); //新建Bitmap图像
                // await bitmap.SetSourceAsync(stream);
                // originImage.Source = bitmap;
                // Application now has read/write access to the picked file

                SoftwareBitmap inputBitmap;
                using (IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.Read))
                {
                    // Create the decoder from the stream
                    BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream);
                    // Get the SoftwareBitmap representation of the file
                    inputBitmap = await decoder.GetSoftwareBitmapAsync();
                }
                if (inputBitmap.BitmapPixelFormat != BitmapPixelFormat.Bgra8
                            || inputBitmap.BitmapAlphaMode != BitmapAlphaMode.Premultiplied)
                {
                    inputBitmap = SoftwareBitmap.Convert(inputBitmap, BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied);
                }
                SoftwareBitmap outputBitmap = new SoftwareBitmap(inputBitmap.BitmapPixelFormat, inputBitmap.PixelWidth, inputBitmap.PixelHeight, BitmapAlphaMode.Premultiplied);

                var helper = new OpenCVBridge.OpenCVHelper();
                helper.Blur(inputBitmap, outputBitmap);

                var bitmapSource = new SoftwareBitmapSource();
                await bitmapSource.SetBitmapAsync(outputBitmap);
                originImage.Source = bitmapSource;

                this.textBlock.Text = "Picked photo: " + file.Name;
            }
            else
            {
                this.textBlock.Text = "Operation cancelled.";
            }

        }
    }
}
