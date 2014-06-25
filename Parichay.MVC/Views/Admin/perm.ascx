<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Parichay.MVC.Models.Permissions>" %>
  
<% using (Html.BeginForm("Permissions", "Admin", FormMethod.Post))
{
    Html.ValidationSummary(true); %>
<table class="tblAllBorders">
    <tr>
    <th colspan="6">
    Create Role Permission
    </th>
    </tr>
    <tr>
        <th>
            Role Name
        </th>
        <th>
            Controller
            -
            Action Name
        </th>
        <th>
            PermissionType
        </th>
        <th></th>
        </tr>
        <tr>
        <td>
        <%=Html.DropDownListFor(model => model.selectedRoleName, Model.rolesList, "--Select--")%>
        </td>
            <td><%=Html.DropDownListFor(model => Model.selectedActionId, new SelectList(Model.actionsList, "Id", "FullName", Model.selectedActionId), "--Select--")%></td>
    <td><%=Html.DropDownListFor(m => m.PTypeR, Model.PermTypes, "--Select--")%> </td>
        <td>
        <button name="button" value="RolePermission">Create</button>
        </td>
    </tr>
    </table>
    <br />
    <%}
        using (Html.BeginForm("UserPermission", "Admin", FormMethod.Post))
      {
       %>
    <h3>OR</h3>
    <br />
    <table class="tblAllBorders">
    <tr>
    <th colspan="6">
    Create User Permission
    </th>
    </tr>
    <tr>
        <th>
            User Name
        </th>
        <th>
            Controller
            -
            Action Name
        </th>
        <th>
            PermissionType
        </th>
        <th>
        
        </th>
    </tr>
    <tr>
    <td><%=Html.TextBoxFor(model => model.selectedUserId)%> <%=Html.HiddenFor(m => m.selectedUserKey)%></td>
    <td><%=Html.DropDownListFor(model => model.selectedActionIdUser, new SelectList(Model.actionsList, "Id", "FullName", Model.selectedActionIdUser), "--Select--")%></td>
    <td><%=Html.DropDownListFor(m => m.PTypeU, Model.PermTypes, "--Select--")%></td>
    <td>
    <button name="button" value="UserPermission">Create</button>
    </td>
    </tr>
    </table>
<%} %>
    <br />
    <h2>
    Existing Permission Rules
    </h2>
<table class="tblAllBorders">
    <tr>
    <th>
            Id
        </th>
        <th>
            Role Name
        </th>
        <th>
            User Name
        </th>
        <th>
            Controller
        </th>
        <th>
            Action
        </th>
        <th>
            PermissionType
        </th>
        <th></th>
    </tr>

<%foreach (var item in Model.permissionsList)
{%>
    <tr>
            <td>
            <%=Html.DisplayFor(modelItem => item.Id)%>
        </td>
                <td>
            <%=Html.DisplayFor(modelItem => item.Role)%>
        </td>
                <td>
            <%=Html.DisplayFor(modelItem => item.Users)%>
        </td>
                <td>
            <%=Html.DisplayFor(modelItem => item.Controller)%>
        </td>
        <td>
            <%=Html.DisplayFor(modelItem => item.Action)%>
        </td>
        <td>
            <%=Html.DisplayFor(modelItem => item.PermissionType)%>
        </td>
        <td>
        <%using (Html.BeginForm("DeletePermission", "Admin", FormMethod.Post, new { id = item.Id }))
        { %><%=Html.Hidden("hdnPermId", item.Id) %><button name="DeletePerm" value="DeletePerm"  onclick="if(!confirm('This Action will be deleted. Are you sure you wish to proceed for deletion?')){return false;}">Delete</button>
         <%} %>
           
            
        </td>
    </tr>
<%} %>


</table>

<br />
<br />
<table class="tblAllBorders" style="background-color:#ffffff">
<tr><th colspan="4">Existing Controllers</th></tr>
    <tr>
    <th>
            Id
        </th>
        <th>
            Controller
        </th>
        <th>
            Action
        </th>
        <th></th>
        </tr>

        <%foreach (var item in Model.actionsList)
        {%>
         <tr>
         <td><%=Html.DisplayFor(modelItem => item.Id)%></td>
         <td><%=Html.DisplayFor(modelItem => item.ControllerName)%></td>
         <td><%=Html.DisplayFor(modelItem => item.ActionName)%></td>
          
         <td>
         <%using (Html.BeginForm("DeleteAction", "Admin", FormMethod.Post, new { id = item.Id }))
         { %><%=Html.Hidden("hdnActionId", item.Id)%> <button name="DeleteAction" value="DeleteAction" onclick="if(!confirm('This Action will be deleted. Are you sure you wish to proceed for deletion?')){return false;}">Delete</button></td>
        <% }%></tr>
       <%} %> 
        </table>


<br /><br />
        <table class="tblAllBorders">
        <tr><th colspan="4">Add New Controller Action</th></tr>
            <tr>
        <th>
            Controller
        </th>
        <th>
            Action
        </th>
        <th></th>
        </tr>
        <%using (Html.BeginForm("AddAction", "Admin", FormMethod.Post))
        {Html.ValidationSummary(true);%>
        <tr>
        <td><%=Html.TextBox("ControllerName")%></td>
        <td><%=Html.TextBox("ActionName")%></td>
        <td><input type="submit" value="Add New" /></td>
        </tr>
       <%} %> 
        </table>




