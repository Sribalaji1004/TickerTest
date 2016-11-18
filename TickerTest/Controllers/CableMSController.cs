using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using TickerTest;

namespace TrialApplication.Controllers
{
    public class CableMSController : Controller
    {
        NeoCableMsEntities db = new NeoCableMsEntities();
        SportsDataQAEntities dbs = new SportsDataQAEntities();
        // GET: CableMS
        public ActionResult Index(HttpPostedFileBase file)
        {
            //string username = Session["name"].ToString();
            //string email = Session["email"].ToString();
            //if (file.FileName.Contains(".csv") || file.FileName.Contains(".txt"))
            //{
            //    var fileName = Path.GetFileName(file.FileName);
            //    var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
            //    file.SaveAs(path);

            //    new Thread(() =>
            //    {
            //        CSVProcessor csvp = new CSVProcessor();
            //        csvp.Process(file.InputStream, username, email);
            //    }).Start();
            //}

            var sr = new StreamReader(System.IO.File.OpenRead(@"C:\Users\Rinsoft\Documents\Balaji\NFL tricodes.csv"));
            string[] strArr = null;
            string line = string.Empty;
            string Name;
            string Cities;
            char[] splitchar = { '-' };
            DataRow row;
            DataTable dt = new DataTable();
            Regex r = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
            while (!sr.EndOfStream)
            {                
                string[] strArray;               
                line = sr.ReadLine();
                strArray = r.Split(line);
                Array.ForEach(strArray, s => dt.Columns.Add(new DataColumn()));
                while ((line = sr.ReadLine()) != null)
                {
                    row = dt.NewRow();
                    row.ItemArray = r.Split(line);
                    dt.Rows.Add(row);
                }                        
            }           
            
            for (int j = 0; j < dt.Rows.Count; j++)
            {
                Name = dt.Rows[j][1].ToString();
                Cities = dt.Rows[j][0].ToString();
                strArr = dt.Rows[j][2].ToString().Split(splitchar);
                string code = strArr[0];
               // string cntcode = strArr[1];

                var exist = dbs.Teams.Where(t => t.SportID == 4 && t.CountryCode == code && ((t.Abbreviation == code && t.Name.Contains(Name))
                    || (t.Abbreviation == Name && t.DisplayName.Contains(Name))
                    || (t.DisplayAbbreviation == Cities && t.Name.Contains(Name)))).ToList();
                if (exist.Count == 1)
                {
                    //Update tricode from excel to SDM DB                    
                    //exist[0].StatsIncAbbreviation = dt.Rows[j][1].ToString();
                    //dbs.SaveChanges();
                    //LogResult(exist.Count, dt.Rows[j][1].ToString());
                }
                else
                {
                    LogResult(exist.Count, dt.Rows[j][2].ToString());
                }
                //string code = strArr[0];
                //string cntcode = strArr[1];
                //var exist = dbs.Teams.Where(t => t.SportID == 5 && t.CountryCode == cntcode && ((t.Abbreviation == code && t.Name.Contains(Name))
                //    || (t.Abbreviation == code && t.DisplayName.Contains(Name))
                //    || (t.DisplayAbbreviation == code && t.Name.Contains(Name)))).ToList();
                //if(exist.Count == 1)
                //{
                //    //Update tricode from excel to SDM DB                    
                //    //exist[0].StatsIncAbbreviation = dt.Rows[j][1].ToString();
                //    //dbs.SaveChanges();
                //    //LogResult(exist.Count, dt.Rows[j][1].ToString());
                //}
                //else{
                //    LogResult(exist.Count, dt.Rows[j][1].ToString());
                //}
            }
            return View();
        }
        private void LogResult(int RecordCount, string Tricode)
        {
            if(RecordCount==1)
            {

            }
            StringBuilder sb = new StringBuilder();
            System.Text.StringBuilder messageBuilder = new System.Text.StringBuilder();
            if (RecordCount == 1)
            {
                messageBuilder.Append("Tricode " + Tricode + " updated successfully\n");
                sb.Append(messageBuilder.ToString());
            }
            else
            {
                messageBuilder.Append("Error while getting tricode " + Tricode + " and the count is " + Convert.ToString(RecordCount) + "\n");
                sb.Append(messageBuilder.ToString());
            }
            StreamWriter log;
            FileStream fileStream = null;
            DirectoryInfo logDirInfo = null;
            FileInfo logFileInfo;

            string logFilePath = "E:\\Balaji\\";
            logFilePath = logFilePath + "SDMLog-" + System.DateTime.Today.ToString("MM-dd-yyyy") + "." + "txt";
            logFileInfo = new FileInfo(logFilePath);
            logDirInfo = new DirectoryInfo(logFileInfo.DirectoryName);
            if (!logDirInfo.Exists) logDirInfo.Create();
            if (!logFileInfo.Exists)
            {
                fileStream = logFileInfo.Create();
            }
            else
            {
                fileStream = new FileStream(logFilePath, FileMode.Append);
            }
            log = new StreamWriter(fileStream);
            log.WriteLine(sb);
            log.Close();
        }
    }
}