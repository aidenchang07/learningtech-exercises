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
        string htmlFileName = "USPTO-html.html";
        string txtFileName = "USPTO-text.txt";

        WebClient client = new WebClient();
        Byte[] htmlData = client.DownloadData(url);
        string html = Encoding.UTF8.GetString(htmlData);
        //下載到 "文件" 資料夾
        StreamWriter html_sw = new StreamWriter(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), htmlFileName));
        StreamWriter txt_sw = new StreamWriter(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), txtFileName));
        html_sw.Write(html);
        html_sw.Close();
        txt_sw.Write(html);
        txt_sw.Close();

        //顯示下載完成
        dl_lab.Text = "Download OK...";
    }

    void pas_btn_Click(Object sender,EventArgs e)
    {
        Button clickedButton = (Button)sender;
        clickedButton.Text = "...button clicked...";
        clickedButton.Enabled = false;

        string fileName = "USPTO-text.txt";
        string htmlString = File.ReadAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), fileName));

        //使用 XDocument ---str---
        XDocument xDoc;
        var basePath = AppDomain.CurrentDomain.BaseDirectory;
        xDoc = XDocument.Load(basePath + "res/06001234.xml");
        //使用 XDocument ---end---

        //取得 PN 碼 ---str---
        string patternPN = @"<TITLE>.*?</TITLE>";

        Regex rulepattern = new Regex(patternPN, RegexOptions.Singleline);
        MatchCollection matchRules = rulepattern.Matches(htmlString);
        //Match matchRules = rulepattern.Match(htmlString);
        string result = null;
        int i = 0;
        foreach (Match m in matchRules)
        {
            string rule = Regex.Replace(m.ToString(), @"<TITLE>United States Patent: ", String.Empty);
            rule = Regex.Replace(rule, @"</TITLE>", String.Empty);

            result = rule;

        }
        //pas_text.Text = result;
        //取得 PN 碼 ---end---





        var node = xDoc.Root.Element("PN");
        node.SetValue(result);

        //存檔 ---str---
        xDoc.Save(basePath + "res/06001234-test.xml");
            //.FirstOrDefault(pn => pn.Element("PN").Value == result);
        //存檔 ---end---

    }
}