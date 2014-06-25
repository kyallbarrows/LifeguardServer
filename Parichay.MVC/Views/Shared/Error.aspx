<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<System.Web.Mvc.HandleErrorInfo>" %>

<asp:Content ID="errorTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Error
</asp:Content>

<asp:Content ID="errorContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Sorry, an error occurred while processing your request.
    </h2>
    <%
        Exception exc = (Exception)HttpContext.Current.Server.GetLastError();
        
        if (exc != null)
        {
         %>
         <table class="tblAllBorders">
         <tr><th>Message:</th><td><%= exc.Message %></td></tr>
         <tr><th>Target Site:</th><%= exc.TargetSite %></tr>
         <tr><th>Stack Trace:</th><%= exc.StackTrace %></tr>
         </table>
         <%} %>
</asp:Content>
