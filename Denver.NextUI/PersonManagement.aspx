<%@ Page Title="" Language="C#" MasterPageFile="~/DenverUI.Master" AutoEventWireup="true" CodeBehind="PersonManagement.aspx.cs" Inherits="Denver.NextUI.PersonManagement" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <p>
        <table style="width:100%;">
            <tr>
                <td colspan="2">Yeni Personel Ekleme</td>
            </tr>
            <tr>
                <td>Ad</td>
                <td>
                    <asp:TextBox ID="txtFirstName" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>İkinci Ad</td>
                <td>
                    <asp:TextBox ID="txtMiddleName" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>Soyad</td>
                <td>
                    <asp:TextBox ID="txtSurname" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>Mail</td>
                <td>
                    <asp:TextBox ID="txtMail" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>İşe Giriş Tarihi</td>
                <td>
                    <asp:Calendar ID="calendarWorkStartDateOfPerson" runat="server"></asp:Calendar>
                </td>
            </tr>
            <tr>
                <td>Çalışma Lokasyonu</td>
                <td>
                    <asp:DropDownList ID="drpDownListWorkLocation" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="btnSaveNewPersonToTheDatabase" runat="server" OnClick="btnSaveNewPersonToTheDatabase_Click" Text="Veritabanına Kaydet" />
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblProcessResult" runat="server" Text="Label"></asp:Label>
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
        </table>
    </p>
</asp:Content>
