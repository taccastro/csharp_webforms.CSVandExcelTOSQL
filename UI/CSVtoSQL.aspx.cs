﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace UI
{
    public partial class CSVtoSQL : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

       

        protected void Upload(object sender, EventArgs e)
        {
            string constr = ConfigurationManager.ConnectionStrings["ConnLocal"].ConnectionString;


            //Upload and save the file
            string csvPath = Server.MapPath("~/Files/") + Path.GetFileName(FileUpload1.PostedFile.FileName);
            FileUpload1.SaveAs(csvPath);

            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[3] { new DataColumn("Id", typeof(int)),
            new DataColumn("Name", typeof(string)),
            new DataColumn("Country",typeof(string)) });


            string csvData = File.ReadAllText(csvPath);
            foreach (string row in csvData.Split('\n'))
            {
                if (!string.IsNullOrEmpty(row))
                {
                    dt.Rows.Add();
                    int i = 0;
                    foreach (string cell in row.Split(','))
                    {
                        dt.Rows[dt.Rows.Count - 1][i] = cell;
                        i++;
                    }
                }
            }

            string consString = ConfigurationManager.ConnectionStrings["ConnLocal"].ConnectionString;
            using (SqlConnection con = new SqlConnection(consString))
            {
                using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                {
                    //Set the database table name
                    sqlBulkCopy.DestinationTableName = "dbo.Customers";
                    con.Open();
                    sqlBulkCopy.WriteToServer(dt);
                    con.Close();
                }
            }
        }
    }




}