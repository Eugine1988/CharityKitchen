<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="CharityKitchen._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <!--Main Heading-->
    <h3>Charity Kitchen</h3>
    <br />
            
    <!--Display Page Links-->
    <asp:Label ID="lblInfo" runat="server" ForeColor="LightGray" Text="Edit Record" />
    <h4>Navigation</h4>
    <table>
        <tr>
            <td>Orders:</td>
            <td><asp:LinkButton ID="btnOrders" runat="server" OnClick="btnOrders_Click">Orders</asp:LinkButton></td>
        </tr>
        <tr><td></td></tr> <!--Create row of space between entries-->
        <tr>
            <td>Meals:</td>
            <td><asp:LinkButton ID="btnMeals" runat="server" OnClick="btnMeals_Click">Meals</asp:LinkButton></td>
        </tr>
        <tr><td></td></tr> <!--Create row of space between entries-->
        <tr>
            <td>Ingredients:</td>
            <td><asp:LinkButton ID="btnIngredients" runat="server" OnClick="btnIngredients_Click">Ingredients</asp:LinkButton></td>
        </tr>
        <tr><td></td></tr> <!--Create row of space between entries-->
        <tr>
            <td>Log Out:</td>
            <td><asp:LinkButton ID="btnLogOut" runat="server" OnClick="btnLogOut_Click">Log Out</asp:LinkButton></td>
        </tr>
    </table>

</asp:Content>
