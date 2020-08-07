<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Part.aspx.cs" Inherits="Denver.NextUI.Part" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="Label1" runat="server" Text="Parça Kodu"></asp:Label>
&nbsp;<asp:TextBox ID="txtCode" runat="server"></asp:TextBox>
&nbsp;<br />
            <br />
            Parça Numarası
            <asp:TextBox ID="txtNumber" runat="server"></asp:TextBox>
            <br />
            <br />
            Birim Fiyatı
            <asp:TextBox ID="txtPrice" runat="server"></asp:TextBox>
            <br />
            <br />
            Parça Tanımı
            <asp:TextBox ID="txtName" runat="server"></asp:TextBox>
            <br />
            <br />
            Stok Miktarı
            <asp:TextBox ID="txtQuantity" runat="server"></asp:TextBox>
            <br />
            <br />
            Tedarikçi
            <asp:TextBox ID="txtSupplier" runat="server" Width="289px"></asp:TextBox>
            <br />
            <br />
            Açıklama <asp:TextBox ID="txtDescription" runat="server" Height="85px" MaxLength="200" TextMode="MultiLine" Width="292px"></asp:TextBox>
            <br />
            <br />
            <asp:Button ID="btnAdd" runat="server" Text="Stoğa Ekle" OnClick="btnAdd_Click" />
            <br />
            <br />
            <asp:Label ID="lblSummary" runat="server"></asp:Label>
        </div>
    </form>
</body>
</html>
