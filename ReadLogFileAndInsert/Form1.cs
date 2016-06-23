using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReadLogFileAndInsert
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            
            try
            {
                string fileName = @"F:\Practice\ReadLogFileAndInsert\Access.log";

                using (FileStream fs = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (BufferedStream bs = new BufferedStream(fs))
                using (StreamReader sr = new StreamReader(bs))
                {
                    string line;
                    string[] lineArray;
                    string timeStamp;
                    string message;
                    string status;
                 
                    StringBuilder insertStatement = new StringBuilder();
                    DbHandler dbHandler = new DbHandler();

                    while ((line = sr.ReadLine()) != null)
                    {
                        lineArray = line.Split('\t');
                        if (!Regex.IsMatch(lineArray[0], "<*>", RegexOptions.None))
                            continue;

                        lineArray[0] = lineArray[0].Trim(new char[] { '<', '>', ' ' });
                        timeStamp = Regex.Replace(lineArray[0], "-|T|:", string.Empty);
                        message = lineArray[5].Substring(0, lineArray[5].Length - 8);
                        status = lineArray[5].Substring(message.Length + 1);

                        //string insertSql = string.Format("Insert into loginfo values('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}');",
                            //timeStamp, lineArray[0], lineArray[1], lineArray[2], lineArray[3], lineArray[4], message, status);

                        string insertSql = string.Format("Insert into loginfo select '{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}' where not exists (select id from loginfo where date_time='{8}' and id='{9}');",
                            timeStamp, lineArray[0], lineArray[1], lineArray[2], lineArray[3], lineArray[4], message, status, lineArray[0], lineArray[1]);

                        //insertStatement.AppendLine(insertSql);

                        
                        dbHandler.InsertData(insertSql);
                        
                    }

                    dbHandler.Close();                  

                }

                watch.Stop();

                MessageBox.Show((watch.ElapsedMilliseconds).ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
