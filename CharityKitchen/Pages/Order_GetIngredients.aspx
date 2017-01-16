<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Order_GetIngredients.aspx.cs" Inherits="CharityKitchen.Pages.Order_GetIngredients" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <br />
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <!--Main Heading-->
            <h3>Ingredients for Order:</h3>
            <asp:Label ID="lblInfo" runat="server" Text="" ForeColor="LightGray" />

            <!--Display some details about the Order-->
            <h4>Details</h4>
            <table>
                <tr>
                    <td>Order ID:</td>
                    <td><asp:Label ID="lblOrderID" runat="server" Text="" /></td>
                </tr>
                <tr>
                    <td>Customer Name:</td>
                    <td><asp:Label ID="lblCustomerName" runat="server" Text="" /></td>
                </tr>
                <tr>
                    <td>Order Date:</td>
                    <td><asp:Label ID="lblOrderDate" runat="server" Text="" /></td>
                </tr>
                <tr>
                    <td>Meals Ordered:</td>
                    <td><asp:Label ID="lblMealsOrdered" runat="server" Text="" /></td>
                </tr>
                <tr>
                    <td>Total Cost:</td>
                    <td><asp:Label ID="lblTotalCost" runat="server" Text="$" /></td>
                </tr>
            </table>
            <br />

            <!--GridView displays the Orders-->
            <h4>Ingredients</h4>
            <asp:GridView ID="gvIngredients" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="Black" GridLines="Horizontal" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" >
                <Columns>
                    <asp:BoundField HeaderText="Ingredient Id" DataField="IngredientID" />
                    <asp:BoundField HeaderText="Ingredient Name" DataField="IngredientName" />
                    <asp:BoundField DataField="Cost" HeaderText="Cost per Unit $" />
                    <asp:BoundField DataField="Unit" HeaderText="Unit" />
                    <asp:BoundField DataField="IngredientQuantity" HeaderText="Quantity" />
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
            <br />

            <!--New Button-->
            <table>
                <tr>
                    <td><asp:Button ID="btnBack" Width="90px" runat="server" Text="Back" OnClick="btnBack_Click" /></td>
                    <td>Go back to the Order page</td>
                </tr>
            </table>

        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>