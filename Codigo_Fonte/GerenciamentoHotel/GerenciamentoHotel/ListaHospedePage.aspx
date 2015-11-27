<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListaHospedePage.aspx.cs" Inherits="GerenciamentoHotel.ListaHospedePage" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
    <body>
        <form id="form1" runat="server">
            <div align="center">
                <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" EnablePartialRendering="False" ></asp:ScriptManager>
                <rsweb:ReportViewer  ID="relatorio" runat="server" Font-Names="Verdana" Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Width="100%" Height="700px" ShowPrintButton="True">
                </rsweb:ReportViewer>
                <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="Clients" TypeName="rpData"></asp:ObjectDataSource>
            </div>
        </form>
    </body>
</html>
