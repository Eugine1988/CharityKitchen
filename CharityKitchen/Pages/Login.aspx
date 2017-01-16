<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="CharityKitchen.Pages.Login1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager" runat="server" /> <!--Need to add this line because this is not master page-->
        <asp:UpdatePanel ID="pnlLogin" runat="server">
            <ContentTemplate>
                <br />
                <h3>Login Page</h3>
                <table>
                    <tr>
                        <td>Username:</td>
                        <td><asp:TextBox ID="txtUserName" runat="server" /></td>
                    </tr>
                    <tr>
                        <td>Password:</td>
                        <td><asp:TextBox ID="txtPassword" runat="server" TextMode="Password" /></td>
                    </tr>
                    <tr>
                        <td><asp:Button ID="btnLogin" runat="server" OnClick="btnLogin_Click" Text="Login" /></td>
                        <td><asp:Label ID="lblInfo" runat="server" Text="Enter login details" /></td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
