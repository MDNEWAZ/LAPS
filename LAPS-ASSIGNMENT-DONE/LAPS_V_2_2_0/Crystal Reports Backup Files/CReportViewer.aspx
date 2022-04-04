<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CReportViewer.aspx.cs" Inherits="LAPS.Reports.ReportViewer" %>

<%@ Register assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form" runat="server">
    <div>
        <CR:CrystalReportViewer ID="CReportViewer" runat="server" AutoDataBind="true" EnableParameterPrompt="False" ReuseParameterValuesOnRefresh="True" ToolPanelView="None" />
    </div>
    </form>
</body>
</html>
