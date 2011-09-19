using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Data;
using System.IO;

namespace aaaSoft.Helpers
{
    public class DataHelper
    {
        /// <summary>
        /// 将数据集转换为XML字符串
        /// </summary>
        /// <param name="ds">数据集</param>
        /// <returns></returns>
        public static String ConvertDataSet2Xml(DataSet ds)
        {
            StringBuilder sb = new StringBuilder();
            XmlWriter writer = XmlWriter.Create(sb);
            ds.WriteXml(writer, XmlWriteMode.WriteSchema);
            return sb.ToString();
        }

        /// <summary>
        /// 将XML字符串转换为数据集
        /// </summary>
        /// <param name="xml">XML字符串</param>
        /// <returns></returns>
        public static DataSet ConvertXml2DataSet(String xml)
        {
            DataSet ds = new DataSet();
            ds.ReadXml(new StringReader(xml), XmlReadMode.ReadSchema);
            return ds;
        }

        /// <summary>
        /// 将数据表转换为XML字符串
        /// </summary>
        /// <param name="dt">数据表</param>
        /// <returns></returns>
        public static String ConvertDataTable2Xml(DataTable dt)
        {
            StringBuilder sb = new StringBuilder();
            XmlWriter writer = XmlWriter.Create(sb);
            dt.WriteXml(writer, XmlWriteMode.WriteSchema);
            return sb.ToString();
        }

        /// <summary>
        /// 将XML字符串转换为数据表
        /// </summary>
        /// <param name="xml">XML字符串</param>
        /// <returns></returns>
        public static DataTable ConvertXml2DataTable(String xml)
        {
            DataTable dt = new DataTable();
            dt.ReadXml(new StringReader(xml));
            return dt;
        }

        /// <summary>
        /// 数据类型转换委托
        /// </summary>
        /// <typeparam name="TSrc">源类型</typeparam>
        /// <typeparam name="TDes">目的类型</typeparam>
        /// <param name="srcData">源泉数据</param>
        /// <returns></returns>
        public delegate TDes ConvertDataDelegate<TSrc, TDes>(TSrc srcData);

        /// <summary>
        /// 将DataTable中指定列的数据类型转换为指定的数据类型
        /// </summary>
        /// <typeparam name="TSrc">源类型</typeparam>
        /// <typeparam name="TDes">目的类型</typeparam>
        /// <param name="SrcDataTable">源DataTable</param>
        /// <param name="ConvertColumnNameArray">要转换数据类型的列名数组</param>
        /// <param name="DataConvertor">数据类型转换器</param>
        /// <returns></returns>
        public static DataTable ConvertColumnDataType<TSrc, TDes>(DataTable SrcDataTable, String[] ConvertColumnNameArray, ConvertDataDelegate<TSrc, TDes> DataConvertor)
        {
            try
            {
                List<String> ConvertColumnNameList = new List<string>();
                ConvertColumnNameList.AddRange(ConvertColumnNameArray);
                List<Int32> ConvertColumnIndexList = new List<int>();
                //列数据
                Int32 ColumnCount = SrcDataTable.Columns.Count;


                DataTable rtnDt = new DataTable();
                //添加列到目的数据表
                foreach (DataColumn dc in SrcDataTable.Columns)
                {
                    if (ConvertColumnNameList.Contains(dc.ColumnName))
                    {
                        ConvertColumnIndexList.Add(dc.Ordinal);
                        rtnDt.Columns.Add(new DataColumn(dc.ColumnName, typeof(TDes)));
                    }
                    else
                    {
                        rtnDt.Columns.Add(new DataColumn(dc.ColumnName, dc.DataType));
                    }
                }

                //添加数据到目的数据表
                foreach (DataRow dr in SrcDataTable.Rows)
                {
                    var objArray = dr.ItemArray;
                    for (Int32 i = 0; i <= ColumnCount - 1; i++)
                    {
                        if (ConvertColumnIndexList.Contains(i))
                        {
                            objArray[i] = DataConvertor((TSrc)objArray[i]);
                        }
                    }
                    DataRow newDr = rtnDt.Rows.Add(objArray);
                }

                return rtnDt;
            }
            catch
            {
                return null;
            }
        }
    }
}
