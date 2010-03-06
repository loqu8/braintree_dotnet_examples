<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Payment Result</h1>

    <div>Thank you for your payment.</div>

    <h2>Transaction Details</h2>

    <table>
        <tr><td>Amount</td><td>$<%= ViewData["Amount"] %></td></tr>
        <tr><td>Transaction ID:</td><td><%= ViewData["TransactionId"] %></td></tr>
        <tr><td>First Name:</td><td><%= Html.Encode(ViewData["FirstName"]) %></td></tr>
        <tr><td>Last Name:</td><td><%= Html.Encode(ViewData["LastName"]) %></td></tr>
        <tr><td>Email:</td><td><%= Html.Encode(ViewData["Email"]) %></td></tr>
        <tr><td>Credit Card:</td><td><%= Html.Encode(ViewData["MaskedNumber"]) %></td></tr>
        <tr><td>Card Type:</td><td><%= Html.Encode(ViewData["CardType"]) %></td></tr>
    </table>
</asp:Content>