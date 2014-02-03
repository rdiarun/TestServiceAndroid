<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="DetectorService.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="Scripts/jquery-2.0.3.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("$btn").click(function () {
                __doPostBack("#btnHello", "");
            });

        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Button Text="text" ID="btn" runat="server" />
            <asp:LinkButton Text="Hello" OnClick="btnHello_Click" ID="btnHello" runat="server" />
        </div>
    </form>
</body>
</html>
