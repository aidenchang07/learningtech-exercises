using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Net;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

public partial class _Default : System.Web.UI.Page
{
    //專案目錄的路徑
    static string basePath = AppDomain.CurrentDomain.BaseDirectory + @"res\";
    
    //html檔名
    static string htmlFileName = "USPTO-html.html";

    //XML參考檔名
    static string xmlReferenceName = "06001234.xml";

    //XML存檔名稱
    static string xmlSaveName = "06001234-new.xml";

    protected void Page_Load(object sender, EventArgs e)
    {
        dl_btn.Click += new EventHandler(this.dl_btn_Click);
        pas_btn.Click += new EventHandler(this.pas_btn_Click);
    }

    void dl_btn_Click(Object sender,EventArgs e)
    {
        Button clickedButton = (Button)sender;
        clickedButton.Text = "...button clicked...";
        clickedButton.Enabled = false;

        string url = "http://patft.uspto.gov/netacgi/nph-Parser?Sect1=PTO1&Sect2=HITOFF&d=PALL&p=1&u=%2Fnetahtml%2FPTO%2Fsrchnum.htm&r=1&f=G&l=50&s1=6001234.PN.&OS=PN/6001234&RS=PN/6001234";

        WebClient client = new WebClient();
        Byte[] htmlData = client.DownloadData(url);
        string html = Encoding.UTF8.GetString(htmlData);
        
        //下載到 專案/res 目錄下，而目前此專案名稱為"WebSite-v1"
        StreamWriter html_sw = new StreamWriter(basePath + htmlFileName);
        html_sw.Write(html);
        html_sw.Close();

        //顯示下載完成
        dl_lab.Text = "Download OK...";
    }

    void pas_btn_Click(Object sender,EventArgs e)
    {
        Button clickedButton = (Button)sender;
        clickedButton.Text = "...button clicked...";
        clickedButton.Enabled = false;

        //讀取html檔
        string htmlString = File.ReadAllText(basePath + htmlFileName);
        Regex rulePattern = null;
        
        //接成功 match 後的 Groups 值
        string matchResult = null;
        
        //使用 XDocument ---str---
        XDocument xDoc;
        xDoc = XDocument.Load(basePath + xmlReferenceName);
        //使用 XDocument ---end---

        //取得 PN ---str---
        string pattern = @"<BODY.*?>.*?United States Patent.*?</B>([\d\,]+)</b>.*?Abstract";

        rulePattern = new Regex(pattern, RegexOptions.Singleline);
        Match matchRule = rulePattern.Match(htmlString);

        matchResult = null;
        if (matchRule.Success)
        {
            matchResult = Regex.Replace(matchRule.Groups[1].Value.ToString(), @"\,", String.Empty);
        }

        var node = xDoc.Root.Element("PN");
        node.SetValue(matchResult);
        //取得 PN ---end---
        
        //取得 APN ---str---
        pattern = @"Abstract.*?Appl.\sNo.:.*?<b>(.*?\/([\d\,]+))</b>.*?U.S. Patent Documents";
        rulePattern = new Regex(pattern, RegexOptions.Singleline);
        matchRule = rulePattern.Match(htmlString);

        matchResult = null;
        if (matchRule.Success)
        {
            //去掉逗號
            matchResult = Regex.Replace(matchRule.Groups[2].Value.ToString(), @"\,", String.Empty);
        }
        node = xDoc.Root.Element("APN");
        node.SetValue(matchResult);
        //設完值把值清空
        matchResult = null;
        //取得 APN ---end---

        //取得 APD ---str---
        pattern = @"Abstract.*?Filed:.*?<b>(([a-zA-Z]+)\s([\d]+)\,\s([\d]+))</b>.*?U.S. Patent Documents";
        /*
         * Group 1 => 完整年月日，ex.September 30, 1997
         * Group 2 => 月，ex.September
         * Group 3 => 日，ex.30
         * Group 4 => 年，ex.1997
         */
        rulePattern = new Regex(pattern, RegexOptions.Singleline);
        matchRule = rulePattern.Match(htmlString);

        matchResult = null;
        if (matchRule.Success)
        {

            matchResult = matchRule.Groups[4].Value.ToString() + "/";

            //月份對照表
            var xmlEntityReplacements = new Dictionary<string, string>
            {
                { "January"  , "01" },
                { "February" , "02" },
                { "March"    , "03" },
                { "April"    , "04" },
                { "May"      , "05" },
                { "June"     , "06" },
                { "July"     , "07" },
                { "August"   , "08" },
                { "September", "09" },
                { "October"  , "10" },
                { "November" , "11" },
                { "December" , "12" }
            };

            //換數字月份
            matchResult += Regex.Replace(matchRule.Groups[2].Value.ToString(), string.Join("|", xmlEntityReplacements.Keys
            .Select(k => k.ToString()).ToArray()), m => xmlEntityReplacements[m.Value]);

            matchResult += "/" + matchRule.Groups[3].Value.ToString();

        }
        node = xDoc.Root.Element("APD");
        node.SetValue(matchResult);
        //設完值把值清空
        matchResult = null;
        //取得 APD ---end---

        //存檔為XML檔
        xDoc.Save(basePath + xmlSaveName);

    }
}