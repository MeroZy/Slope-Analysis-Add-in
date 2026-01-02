using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Slope_Analysis_Add_in.Revit;
using Slope_Analysis_Add_in.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Slope_Analysis_Add_in.Methods;


namespace Slope_Analysis_Add_in
{
    [TransactionAttribute(TransactionMode.Manual)]
    public class ExtCmd : IExternalCommand
    {
        public static UIDocument uidoc { get; set; }
        public static Document doc { get; set; }
        public static Slope_Analysis Mainform { get; set; }
        public static ExtEventHan ExtEventHan { get; set; }
        public static ExternalEvent ExtEvent { get; set; }


        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            uidoc = commandData.Application.ActiveUIDocument;
            doc = uidoc.Document;
            //StringBuilder sb = new StringBuilder();

            ExtEventHan = new ExtEventHan();
            ExtEvent = ExternalEvent.Create(ExtEventHan);

            //MAIN
            Mainform = new Slope_Analysis();
            Mainform.Show();
            

                //RVTUtlis.Run();
                //TaskDialog.Show("Test",RVTUtlis.sb.ToString()); //for test

                return Result.Succeeded;
        }

        // filter selection in revit from gpt 
        
        

    }

}
