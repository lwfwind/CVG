using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;

namespace VariationGeneration.DataTable_To_Html
{
    public static class DataTableToHtml
    {
        public static void ToHtmlTable(this DataTable table, string destination)
        {
            string style = @"
            table.stats 
            {text-align: center;
            font-family: Verdana, Geneva, Arial, Helvetica, sans-serif ;
            font-weight: normal;
            font-size: 11px;
            color: #fff;
            width: 280px;
            background-color: #4F81BD;
            border: 0px;
            border-collapse: collapse;
            border-spacing: 0px;}

            table.stats td.first
            {background-color: #D3DFEE;
            color: #000;
            padding: 4px;
            text-align: left;
            border: 1px #7BA0CD solid;}

            table.stats td.second
            {background-color: #FFFFFF;
            color: #000;
            padding: 4px;
            text-align: left;
            border: 1px #7BA0CD solid;}

            table.stats td.hed
            {background-color: #4F81BD;
            color: #fff;
            padding: 4px;
            text-align: left;
            border-bottom: 2px #7BA0CD solid;
            font-size: 12px;
            font-weight: bold;} ";

            StringBuilder Html = new StringBuilder();
            Html.AppendLine("<html>");
            Html.AppendLine("<head>");
            Html.AppendLine("<style type='text/css'>");
            Html.AppendLine(style);
            Html.AppendLine("</style>");
            Html.AppendLine("</head>");
            Html.AppendLine("<body>");
            Html.AppendLine("<table class='stats' cellspacing='0'>");

            Html.AppendLine("<tr>");
            for (int i = 0; i <= table.Columns.Count - 1; i++)
            {
                Html.AppendLine("<td class='hed'>" + table.Columns[i].ColumnName + "</td>");
            }
            Html.AppendLine("</tr>");

            for (int i = 0; i <= table.Rows.Count - 1; i++)
            {
                Html.AppendLine("<tr>");
                for (int j = 0; j <= table.Columns.Count - 1; j++)
                {
                    if (i % 2 == 0)
                        Html.AppendLine("<td class='second'>" + table.Rows[i][j].ToString() + "</td>");
                    else
                        Html.AppendLine("<td class='first'>" + table.Rows[i][j].ToString() + "</td>");
                }

                Html.AppendLine("</tr>");
            }

            Html.AppendLine("</table>");
            Html.AppendLine("</body>");
            Html.AppendLine("</html>");
            using (StreamWriter sw = new StreamWriter(destination))
            {
                sw.Write(Html.ToString());
            }
        }
    }
}
