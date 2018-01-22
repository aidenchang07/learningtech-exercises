<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:Label  ID="dl_lab"
                    Text="Download USPTO PatentNumber/6001234 之 HTML" runat="server"/>
        <br />
        <asp:Button ID="dl_btn"
                    Text="Click Here" runat="server"/>
        <br />
        <br />
        <asp:Label  ID="pas_lab"
                    Text="將 HTML Parse 成 XML" runat="server"/>
        <br />
        <asp:Button ID="pas_btn"
                    Text="Click Here" runat="server"/>
    </form>
    </body>
</html>
