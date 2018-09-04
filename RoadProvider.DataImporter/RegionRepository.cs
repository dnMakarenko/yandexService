using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoadProvider.Core.Entities;
using System.IO;
using System.Reflection;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using System.Configuration;

namespace RoadProvider.DataImporter
{
    public class RegionRepository
    {
        private static List<Region> regionCodes
        {
            get;set;
        }

        private static List<Region> GetAllRegions()
        {
            
            var regions = new List<Region>();

            string binPath = Path.GetDirectoryName((new System.Uri(Assembly.GetExecutingAssembly().CodeBase)).LocalPath);

            string regionsResourceFileName = ConfigurationManager.AppSettings["regionsResourceFileName"];
            var excelFilePath = Path.Combine(binPath, regionsResourceFileName);
            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlWorkBook = xlApp.Workbooks.Open(excelFilePath);
            Excel.Worksheet xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

            Excel.Range xlRange = xlWorkSheet.UsedRange;
            int totalRows = xlRange.Rows.Count;
            int totalColumns = xlRange.Columns.Count;

            string firstValue, secondValue;

            for (int rowCount = 2; rowCount <= totalRows; rowCount++)
            {

                firstValue = Convert.ToString((xlRange.Cells[rowCount, 1] as Excel.Range).Text);
                secondValue = Convert.ToString((xlRange.Cells[rowCount, 2] as Excel.Range).Text);

                regions.Add(new Region { Code = firstValue, Name = secondValue });

            }

            xlWorkBook.Close();
            xlApp.Quit();

            Marshal.ReleaseComObject(xlWorkSheet);
            Marshal.ReleaseComObject(xlWorkBook);
            Marshal.ReleaseComObject(xlApp);

            return regions.OrderBy(q => q.Code).ToList();
        }


        public static List<Region> RegionCodes
        {
            get
            {
                if (regionCodes == null)
                {
                    regionCodes = GetAllRegions();
                }
                return regionCodes;
            }
            set { RegionCodes = value; }
        }
    }
}
