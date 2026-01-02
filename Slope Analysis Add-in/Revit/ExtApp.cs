using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Slope_Analysis_Add_in.Revit
{

    public class ExtApp : IExternalApplication
    {
        public Result OnShutdown(UIControlledApplication uicApp)
        {
            return Result.Succeeded;
        }

        public Result OnStartup(UIControlledApplication uicApp)
        {
            string tabName = "KAITECH-BD-R09";
            string panelName = "Architecture";

            try
            {
                uicApp.CreateRibbonTab(tabName);
            }
            catch (Exception) { }

            RibbonPanel panel = uicApp.GetRibbonPanels(tabName)
                                        .FirstOrDefault(p => p.Name == panelName);

            if (panel == null)
            {
                panel = uicApp.CreateRibbonPanel(tabName, panelName);
            }

            Assembly assembly = Assembly.GetExecutingAssembly();
            PushButtonData pb_Data = new PushButtonData(
                "Aanalysis",
                "Slope Analysis",
                assembly.Location,
                "Slope_Analysis_Add_in.ExtCmd"
            );

            PushButton pb = panel.AddItem(pb_Data) as PushButton;
            pb.ToolTip = "This is Slope Analysis Add-in Developed By Amr Khaled";

            Bitmap originalBitmap = Properties.Resources.icons8_analysis_96__1_;

            Bitmap resizedLarge = new Bitmap(originalBitmap, new Size(32, 32));
            pb.LargeImage = GetImageSource(resizedLarge);

            Bitmap resizedSmall = new Bitmap(originalBitmap, new Size(16, 16));
            pb.Image = GetImageSource(resizedSmall);

            return Result.Succeeded;
        }
        public ImageSource GetImageSource(System.Drawing.Bitmap bitmap) //gpt help
        {
            using (MemoryStream ms = new MemoryStream())
            {
                bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                ms.Position = 0;
                PngBitmapDecoder decoder = new PngBitmapDecoder(ms, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);
                return decoder.Frames[0];
            }
        }
    }
        
}
