<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MealEdit.aspx.cs" Inherits="CharityKitchen.Pages.MealEdit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <br />
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <!--Main Heading-->
            <h3>Meal</h3>
            
            <!--Display OrderID and lblInfo (e.g. when saved, lblInfo displays operation successful)-->
            <table>
                <tr>
                    <td>Meal ID: </td>
                    <td><asp:Label ID="lblID" runat="server" /></td>
                </tr>
            </table>
            <asp:Label ID="lblInfo" runat="server" ForeColor="LightGray" Text="Edit Record" />
            <br />

            <!--Order Details-->
            <h4>Meal Details</h4>
            <table>
                <tr>
                    <td>Meal Name:</td>
                    <td><asp:TextBox ID="txtMealName" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Description:</td>
                    <td><asp:TextBox ID="txtDescription" runat="server"></asp:TextBox></td>
                </tr>
            </table>
            <br />

            <!--GridView displays the Meals that have been added to the Order-->
            <h4>Recipe</h4>
            <asp:GridView ID="gvRecipe" runat="server" OnRowDeleting="gvIngredients_RowDeleting" AutoGenerateColumns="False" >
                <Columns>
                    <asp:BoundField Visible="False" HeaderText="MealID" DataField="MealID" />
                    <asp:BoundField HeaderText="Ingredient Name" DataField="IngredientName" />
                    <asp:BoundField DataField="IngredientCost" HeaderText="Cost" />
                    <asp:BoundField DataField="IngredientUnit" HeaderText="Unit" />
                    <asp:BoundField DataField="IngredientQuantity" HeaderText="Quantity" />
                    <asp:CommandField DeleteText="Remove" ShowDeleteButton="True" />
                </Columns>
                <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                <HeaderStyle BackColor="#333333" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Right" />
                <SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
                <SortedAscendingCellStyle BackColor="#F7F7F7" />
                <SortedAscendingHeaderStyle BackColor="#4B4B4B" />
                <SortedDescendingCellStyle BackColor="#E5E5E5" />
                <SortedDescendingHeaderStyle BackColor="#242121" />
            </asp:GridView>
            <table>
                <tr>
                    <td>Add an Ingredient:</td>
                    <td><asp:DropDownList ID="cboIngredients" runat="server" /></td>
                </tr>
                <tr>
                    <td>Quantity:</td>
                    <td><asp:TextBox ID="txtIngredintQuantity" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td></td>
                    <td><asp:Button ID="btnAddIngredient" Width="60px" runat="server" Text="Add" OnClick="btnAddIngredient_Click" /></td>
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
                    <td>Go back to all Meals:</td>
                    <td><asp:Button ID="Back" Width="110px" runat="server" Text="Back to Meals" OnClick="Back_Click" /></td>
                </tr>
            </table>

        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>