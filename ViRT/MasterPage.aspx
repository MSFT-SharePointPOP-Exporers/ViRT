<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MasterPage.aspx.cs" Inherits="MasterPage" %> 

<!DOCTYPE html>
<html>
<head runat="server">
    <meta charset ="utf-8" />
    <title>MasterPage</title>
    <link href="Content/foundation/foundation.css" rel="stylesheet" />
    <link href="Content/stylesheet.css" rel="stylesheet" />
    <script src="Scripts/foundation/foundation.js"></script>
    <script src="Scripts/jquery-2.1.1.min.js"></script>
   <script type="text/javascript">
        $(document).ready(function () {
            $('#pipelines').click(function () {
                $.ajax({
                    contentType: "text/html; charset=utf-8",
                    data: "Tag=" + $('#Tags').val(),
                    url: "FetchCustomer.aspx",
                    dataType: "html",
                    success: function (data) {
                        $("#rendering").html(data);
                    }
                });
            });
        }); 
   </script>
</head>
<body>
   <div id="pipelines">
        <ul class="side-nav">
            <li><a href="#">Link 1</a></li>
            <li><a href="#">Link 2</a></li>
            <li><a href="#">Link 3</a></li>
            <li><a href="#">Link 4</a></li>
        </ul>
    </div>
    <div id ="overviewbar">
    </div>
    <div id="rendering">
    </div>
</body>
</html>