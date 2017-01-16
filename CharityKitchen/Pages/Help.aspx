<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Help.aspx.cs" Inherits="CharityKitchen.Pages.Help" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <br />
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <!--Main Heading-->
            <h3>Help Page</h3>
            <br />
            
            <h4>How to save an Order</h4>
            <asp:BulletedList runat="server">
                <asp:ListItem>Go to Orders Page</asp:ListItem>
                <asp:ListItem>Either click on 'select' link in list of orders or click 'New' button to create a new order</asp:ListItem>
                <asp:ListItem>After getting to the Order Details page, enter the order details in the text-boxes</asp:ListItem>
                <asp:ListItem>Select the meals for this order (either add meals or remove meals from the order)</asp:ListItem>
                <asp:ListItem>Click the Save button and the order will be saved</asp:ListItem>
            </asp:BulletedList>
            <br />
            <h4>How to Delete an Order</h4>
            <asp:BulletedList runat="server">
                <asp:ListItem>Go to Orders Page</asp:ListItem>
                <asp:ListItem>Click on Delete link next to the order you wish to delete</asp:ListItem>
            </asp:BulletedList>
            <br />
            <h4>How to view all ingredients needed for an Order</h4>
            <asp:BulletedList runat="server">
                <asp:ListItem>Go to Orders Page</asp:ListItem>
                <asp:ListItem>Pick an Order for which you wish to see the ingredients</asp:ListItem>
                <asp:ListItem>In the Order Details page click on 'Ingredients' button and the ingredients will be displayed</asp:ListItem>
            </asp:BulletedList>
            <br />
            <h4>How to save a Meal</h4>
            <asp:BulletedList runat="server">
                <asp:ListItem>Go to Meals Page</asp:ListItem>
                <asp:ListItem>Either click on 'select' link in list of meals or click 'New' button to create a new meal</asp:ListItem>
                <asp:ListItem>After getting to the Meal Details page, enter the meal details in the text-boxes</asp:ListItem>
                <asp:ListItem>Select the ingredients for this meal (either add or remove ingredients from the meal)</asp:ListItem>
                <asp:ListItem>Click the Save button and the meal will be saved</asp:ListItem>
            </asp:BulletedList>
            <br />
            <h4>How to Delete a Meal</h4>
            <asp:BulletedList runat="server">
                <asp:ListItem>Go to Meals Page</asp:ListItem>
                <asp:ListItem>Click on Delete link next to the meal you wish to delete</asp:ListItem>
            </asp:BulletedList>
            <br />
            <h4>How to save an Ingredient</h4>
            <asp:BulletedList runat="server">
                <asp:ListItem>Go to Ingredients Page</asp:ListItem>
                <asp:ListItem>Either click on 'select' link in list of ingredients or click 'New' button to create a new ingredient</asp:ListItem>
                <asp:ListItem>After getting to the Ingredient Details page, enter the ingredient details in the text-boxes</asp:ListItem>
                <asp:ListItem>Click the Save button and the ingredient will be saved</asp:ListItem>
            </asp:BulletedList>
            <br />
            <h4>How to Delete an Ingredient</h4>
            <asp:BulletedList runat="server">
                <asp:ListItem>Go to Ingredients Page</asp:ListItem>
                <asp:ListItem>Click on Delete link next to the ingredient you wish to delete</asp:ListItem>
            </asp:BulletedList>
            <br />

        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>