﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Net;
using System.IO;
using System.Text;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string url = "http://patft.uspto.gov/netacgi/nph-Parser?Sect1=PTO1&Sect2=HITOFF&d=PALL&p=1&u=%2Fnetahtml%2FPTO%2Fsrchnum.htm&r=1&f=G&l=50&s1=6001234.PN.&OS=PN/6001234&RS=PN/6001234";
        string fileName = "USPTO-text.html";

        WebClient client = new WebClient();
        Byte[] htmlData = client.DownloadData(url);
        string html = Encoding.UTF8.GetString(htmlData);
        StreamWriter sw = new StreamWriter(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),fileName));
        sw.Write(html);
        sw.Close();
    }
}