<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="IngredientEdit.aspx.cs" Inherits="CharityKitchen.Pages.IngredientEdit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <br />
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <!--Main Heading-->
            <h3>Ingredient</h3>
            
            <!--Display IngredientID and lblInfo (e.g. when saved, lblInfo displays operation successful)-->
            <table>
                <tr>
                    <td>Ingredient ID: </td>
                    <td><asp:Label ID="lblID" runat="server" /></td>
                </tr>
            </table>
            <asp:Label ID="lblInfo" runat="server" ForeColor="LightGray" Text="Edit Record" />
            <br />

            <!--Order Details-->
            <h4>Ingredient Details</h4>
            <table>
                <tr>
                    <td>Name:</td>
                    <td><asp:TextBox ID="txtName" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Cost:</td>
                    <td><asp:TextBox ID="txtCost" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Unit:</td>
                    <td><asp:TextBox ID="txtUnit" runat="server"></asp:TextBox></td>
                </tr>
            </table>
            <br />

            <!--Navigation & Data Buttons-->
            <table>
                <tr>
                    <td>Save the changes:</td>
                    <td><asp:Button ID="btnSave" Width="110px" runat="server" Text="Save" OnClick="btnSave_Click" /></td>
                </tr>
                <tr>
                    <td>Go back to all Ingredients:</td>
                    <td><asp:Button ID="btnBack" Width="110px" runat="server" Text="Back to Meals" OnClick="Back_Click" /></td>
                </tr>
            </table>

        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>