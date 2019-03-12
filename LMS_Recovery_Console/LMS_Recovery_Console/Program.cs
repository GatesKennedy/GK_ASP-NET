using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS_Recovery_Console
{
    class Program
    {
        static void Main(string[] args)
        {            
            //==========================
            //  Prelim Processing
            //==========================

            //const string txt1_Numbered = @"C:\Users\gates\Documents\GatesKennedy\LMS_Recovery_Console\aolcc_versioncontrol\AOLCC_VC_TxtNum.txt";
            //const string txt2_Bulleted = @"C:\Users\gates\Documents\GatesKennedy\LMS_Recovery_Console\aolcc_versioncontrol\AOLCC_VC_TxtBull.txt";
            const string txt_Target = @"C:\Users\gates\Documents\GatesKennedy\LMS_Recovery_Console\aolcc_5_CSS-BootStrap\AOLCC_CSS_Preform_Mar11.txt";

            Parse_Pop ObjTarget = new Parse_Pop();
                ObjTarget.Read_InputDoc(txt_Target);


            //==========================
            //  Target Processing
            //==========================
            ObjTarget.Read_Clean();
                Display.ExportCollection(ObjTarget.LinesList, "LinesList_Clean");

            ObjTarget.SeparatePages();
                Display.ExportCollection(ObjTarget.PageList, "PageList_SeparatePages");
                Display.ExportCollection(ObjTarget.IsolatedList, "IsolatedList_SeparatePages");

            //ObjTarget.SeparateModules();
            //    Display.ExportCollection(ObjTarget.ModuleList, "ModuleList_SeparateModules");

            ObjTarget.RoutePageTypes();
            Display.ExportCollection(ObjTarget.PageList, "Handled_Pages");


            //ObjTarget.SeparateTests();
            //    Display.ExportCollection(ObjTarget.PageList, "PageList_SepTests");
            //    Display.ExportCollection(ObjTarget.ModuleList, "ModuleList_SeparateTests");

            //ObjTarget.GroupLessons();
            //    Display.ExportCollection(ObjTarget.FormatAOLCC, "FormatAOLCC");

            //ObjTarget.CompileQuestions_AOLCC();

            //var temp = ObjTarget.TestsList;
            //ObjTarget.PageList.Add(temp);

            ////ObjTarget.CompileText_AOLCC();

            //ObjTarget.CompTextClean();

            //ObjTarget.compText_LessonsLeft();

            //ObjTarget.CountAddKnowledge();

            Console.WriteLine("DONE.");
            Console.ReadLine();
        }
    }
}
