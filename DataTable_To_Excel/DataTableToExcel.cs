using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using OfficeOpenXml;

namespace VariationGeneration.DataTable_To_Excel
{
    public class DataTableToExcel
    {
        public static void DataTabletoExcel(System.Data.DataTable tmpDataTable, string strFileName)
        {
            if (tmpDataTable == null)
                return;
            int rowNum = tmpDataTable.Rows.Count;
            int columnNum = tmpDataTable.Columns.Count;
            int rowIndex = 1;
            int columnIndex = 0;
     
            FileInfo newFile = new FileInfo(strFileName);
			if (newFile.Exists)
			{
				newFile.Delete();  // ensures we create a new workbook
                newFile = new FileInfo(strFileName);
			}
            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                // this will cause the assembly to output the raw XML files in the outputDir
                // for debug purposes.  You will see to sub-folders called 'xl' and 'docProps'.
                //xlPackage.DebugMode = true;

                // add a new worksheet to the empty workbook
                ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets.Add("Cases");

                //将DataTable的列名导入Excel表第一行
                foreach (DataColumn dc in tmpDataTable.Columns)
                {
                    columnIndex++;
                    worksheet.Cell(rowIndex,columnIndex).Value = dc.ColumnName;
                }

                //将DataTable中的数据导入Excel中
                for (int i = 0; i < rowNum; i++)
                {
                    rowIndex++;
                    columnIndex = 0;
                    for (int j = 0; j < columnNum; j++)
                    {
                        columnIndex++;
                        worksheet.Cell(rowIndex, columnIndex).Value = tmpDataTable.Rows[i][j].ToString();
                    }

                }

                // save our new workbook and we are done!
                xlPackage.Save();
            }
         
     

         

          
        }




    }
}
