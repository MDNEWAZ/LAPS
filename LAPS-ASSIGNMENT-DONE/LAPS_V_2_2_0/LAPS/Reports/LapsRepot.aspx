<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LapsRepot.aspx.cs" Inherits="LAPS.Reports.LapsRepot" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>LAPS Report</title>

    <script type="text/javascript">


        function Print() {
            var dvReport = document.getElementById("dvReport");
            var frame1 = dvReport.getElementsByTagName("iframe")[0];
            if (navigator.appName.indexOf("Internet Explorer") != -1 || navigator.appVersion.indexOf("Trident") != -1) {
                frame1.name = frame1.id;
                window.frames[frame1.id].focus();
                window.frames[frame1.id].print();
            }
            else {
                var frameDoc = frame1.contentWindow ? frame1.contentWindow : frame1.contentDocument.document ? frame1.contentDocument.document : frame1.contentDocument;
                frameDoc.print();
            }
        }
    </script>


</head>
<body>
    <form id="form1" runat="server">
        
      <%--  <asp:Button ID="btnPrint" runat="server" Text="Print Directly" OnClientClick="Print()"></asp:Button>--%>

        <div id="dvReport">
            <%-- <CR:CrystalReportViewer ID="lapsCReportViewer" runat="server" HasPrintButton="True" AutoDataBind="true" EnableParameterPrompt="False" EnableDatabaseLogonPrompt="False" EnableTheming="False" HasCrystalLogo="False" HasRefreshButton="True" HasToggleParameterPanelButton="False" Height="50px" ShowAllPageIds="True" ToolPanelView="None" Width="350px" HasDrillUpButton="False" HyperlinkTarget="" EnableDrillDown="False" HasDrilldownTabs="False" PrintMode="ActiveX" OnUnload="lapsCReportViewer_Unload" />--%>
            <CR:CrystalReportViewer ID="lapsCReportViewer" runat="server" HasPrintButton="True" AutoDataBind="true" EnableParameterPrompt="False" EnableDatabaseLogonPrompt="False" EnableTheming="False" HasCrystalLogo="False" HasRefreshButton="True" HasToggleParameterPanelButton="False" Height="50px" ShowAllPageIds="True" ToolPanelView="None" Width="350px" HasDrillUpButton="False" HyperlinkTarget="" EnableDrillDown="False" HasDrilldownTabs="False" PrintMode="ActiveX" OnUnload="lapsCReportViewer_Unload" />
        </div>
        
    </form>
</body>
</html>
