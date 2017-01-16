<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="OrderEdit.aspx.cs" Inherits="CharityKitchen.Pages.MealOrder" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <br />
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <!--Main Heading-->
            <h3>Order</h3>
            
            <!--Display OrderID and lblInfo (e.g. when saved, lblInfo displays operation successful)-->
            <table>
                <tr>
                    <td>Order ID: </td>
                    <td><asp:Label ID="lblID" runat="server" /></td>
                </tr>
            </table>
            <asp:Label ID="lblInfo" runat="server" ForeColor="LightGray" Text="Edit Record" />
            <br />

            <!--Order Details-->
            <h4>Meal Ordered By</h4>
            <table>
                <tr>
                    <td>Customer Name:</td>
                    <td><asp:TextBox ID="txtCustomerName" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Customer Address1:</td>
                    <td><asp:TextBox ID="txtC_Address1" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Customer Address2:</td>
                    <td><asp:TextBox ID="txtC_Address2" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Order Date:</td>
                    <td><asp:TextBox ID="txtOrderDate" runat="server"></asp:TextBox></td>
                </tr>
            </table>

            <!--Button that takes to page to see all ingredients for this order-->
            <table>
                <tr>
                    <td>Get all Ingredients for this Order:</td>
                    <td><asp:Button ID="btnGetIngredients" Width="110px" runat="server" Text="Ingredients" OnClick="btnGetIngredients_Click" /></td>
                </tr>
            </table>
            <br />

            <!--GridView displays the Meals that have been added to the Order-->
            <h4>Meals Selected</h4>
            <asp:GridView ID="gvMeals" runat="server" OnRowDeleting="gvMeals_RowDeleting" AutoGenerateColumns="False" >
                <Columns>
                    <asp:BoundField Visible="False" HeaderText="MealID" DataField="MealID" />
                    <asp:BoundField HeaderText="Meal Name" DataField="MealName" />
                    <asp:BoundField DataField="MealQuantity" HeaderText="Quantity" AccessibleHeaderText="quantity" />
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
                    <td>Add a Meal:</td>
                    <td><asp:DropDownList ID="cboMeal" runat="server" /></td>
                    <td><asp:Button ID="btnAddMeal" Width="60px" runat="server" Text="Add" OnClick="btnAddMeal_Click" /></td>
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
                    <td>Go back to all Orders:</td>
                    <td><asp:Button ID="Back" Width="110px" runat="server" Text="Back to Orders" OnClick="Back_Click" /></td>
                </tr>
            </table>

        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>