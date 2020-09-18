<%@ Page Title="" Language="C#" MasterPageFile="~/DenverUI.Master" AutoEventWireup="true" CodeBehind="Parts.aspx.cs" Inherits="Denver.NextUI.Parts" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="form1" runat="server">
        <div>
            <table>
                <tr>
                    <td>
                         <asp:Label ID="Label1" runat="server" Text="Parça Kodu"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtCode" runat="server"></asp:TextBox>
                    </td>
                </tr>

                <tr>
                    <td>
                         Parça Numarası</td>
                    <td>
                        <asp:TextBox ID="txtNumber" runat="server"></asp:TextBox></td>
                </tr>

                <tr>
                    <td>
                         Birim Fiyatı</td>
                    <td>
                        <asp:TextBox ID="txtPrice" runat="server"></asp:TextBox></td>
                </tr>

                <tr>
                    <td>
                         Parça Tanımı</td>
                    <td>
                        <asp:TextBox ID="txtName" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>
                        Stok Miktarı
                    </td>
                    <td>
                        <asp:TextBox ID="txtQuantity" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Tedarikçi
                    </td>
                    <td>
                         <asp:TextBox ID="txtSupplier" runat="server" Width="289px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>Açıklama</td>
                    <td><asp:TextBox ID="txtDescription" runat="server" Height="85px" MaxLength="200" TextMode="MultiLine" Width="292px"></asp:TextBox></td>
                </tr>
                <tr>
                    <td></td>
                    <td><asp:Button ID="btnAdd" runat="server" Text="Stoğa Ekle" OnClick="btnAdd_Click" /></td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Label ID="lblSummary" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
            
        </div>
    </form>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
