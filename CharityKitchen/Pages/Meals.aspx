<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Meals.aspx.cs" Inherits="CharityKitchen.Pages.Meals" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <br />
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <!--Main Heading-->
            <h3>Meals</h3>
            
            <!--Display lblInfo (e.g. when saved, lblInfo displays operation successful)-->
            <asp:Label ID="lblInfo" runat="server" ForeColor="LightGray" />
            <br />

            <!--GridView displays the Orders-->
            <asp:GridView ID="gvMeals" runat="server" OnSelectedIndexChanged="gvMeals_SelectedIndexChanged" OnRowDeleting="gvMeals_RowDeleting" AutoGenerateColumns="False" CellPadding="4" ForeColor="Black" GridLines="Horizontal" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" >
                <Columns>
                    <asp:CommandField ShowSelectButton="True" />
                    <asp:BoundField HeaderText="Meal Id" DataField="MealID" />
                    <asp:BoundField HeaderText="Meal Name" DataField="MealName" />
                    <asp:CommandField ShowDeleteButton="True" />
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
                    <td><asp:Button ID="btnNew" Width="90px" runat="server" Text="New" OnClick="btnNew_Click" /></td>
                    <td>Create a new Meal</td>
                </tr>
            </table>

        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>