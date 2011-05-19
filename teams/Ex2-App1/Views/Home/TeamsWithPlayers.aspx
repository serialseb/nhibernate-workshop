<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<System.Collections.Generic.List<Eq1.App1.Model.Team>>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
<ul>
    <% foreach(var team in Model){ %>
       <li>Id <%= team.Id %>, name: <%= team.Name %>, manager: 
       <%= team.Manager != null ? team.Manager.FullName : "(none)" %>, <%= team.CreationTime %>, players: <%= team.Players.Count() %></li> 
    <%} %>
    </ul>
</body>
</html>
