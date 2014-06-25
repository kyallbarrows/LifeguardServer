<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<Parichay.MVC.Models.UsersModel>>" %>
 
 <fieldset>
 <legend><h2>Users</h2> </legend>
 <table class="tblAllBorders">
    <tr>
    <th>
    Id
    </th>
        <th>
            E-mail
        </th>
        <th>
            Info
        </th>
                <th>
            isApproved
        </th>
        <th>
            isLockedOut
        </th>
    </tr>

    <% foreach (var item in Model)
       { %>
    
        <tr>
            <td>
                <%= Html.ActionLink(item.name, "UserDetails", new { id=item.id }) %>
            </td>
            <td>
                <%= Html.Encode(item.email) %>
            </td>
            <td>
                <b>Created:</b> <%= Html.Encode(String.Format("{0:g}",item.creationDate)) %><br />
                <b>lastActivity:</b> <%= Html.Encode(String.Format("{0:g}", item.lastActivityDate)) %><br />
                <b>lastLogin:</b> <%= Html.Encode(String.Format("{0:g}",item.lastLoginDate)) %><br />
                <b>lastLockedOut:</b> <%= Html.Encode(String.Format("{0:g}", item.lastLockedOutDate)) %><br />
            </td>
            <td>
                <%= Html.Encode(item.isApproved) %>
            </td>
            <td>
                <%= Html.Encode(item.isLockedOut) %>
            </td>

        </tr>
    
    <% } %>

    </table>
 </fieldset>

 
    
