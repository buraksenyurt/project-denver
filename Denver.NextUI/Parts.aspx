<%@ Page Title="" Language="C#" MasterPageFile="~/DenverUI.Master" AutoEventWireup="true" CodeBehind="Parts.aspx.cs" Inherits="Denver.NextUI.Parts" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   
    <div>
        <asp:GridView ID="grdPartList" runat="server" AllowPaging="True" BackColor="White" BorderColor="#CC9966" BorderStyle="None" BorderWidth="1px" CellPadding="4">
            <FooterStyle BackColor="#FFFFCC" ForeColor="#330099" />
            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="#FFFFCC" />
            <PagerStyle BackColor="#FFFFCC" ForeColor="#330099" HorizontalAlign="Center" />
            <RowStyle BackColor="White" ForeColor="#330099" />
            <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="#663399" />
            <SortedAscendingCellStyle BackColor="#FEFCEB" />
            <SortedAscendingHeaderStyle BackColor="#AF0101" />
            <SortedDescendingCellStyle BackColor="#F6F0C0" />
            <SortedDescendingHeaderStyle BackColor="#7E0000" />

        </asp:GridView>
    </div>
        <div>
            <table>
                <tr>
                    <td>
                         <asp:Label ID="Label1" runat="server" Text="Parça Kodu"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtCode" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtCode" ErrorMessage="Parça kodu girilmeli" ForeColor="Red">!</asp:RequiredFieldValidator>
                    </td>
                </tr>

                <tr>
                    <td>
                         Parça Numarası</td>
                    <td>
                        <asp:TextBox ID="txtNumber" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtNumber" ErrorMessage="Parça numarası girilmeli" ForeColor="Red">!</asp:RequiredFieldValidator>
                    </td>
                </tr>

                <tr>
                    <td>
                         Birim Fiyatı</td>
                    <td>
                        <asp:TextBox ID="txtPrice" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtPrice" ErrorMessage="Birim fiyat girilmeli" ForeColor="Red">!</asp:RequiredFieldValidator>
                    </td>
                </tr>

                <tr>
                    <td>
                         Parça Tanımı</td>
                    <td>
                        <asp:TextBox ID="txtName" MaxLength="30" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtName" ErrorMessage="Parça tanımı girilmeli" ForeColor="Red">!</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        Stok Miktarı
                    </td>
                    <td>
                        <asp:TextBox ID="txtQuantity" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtQuantity" ErrorMessage="Güncel stok miktarı girilmeli" ForeColor="Red">!</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        Tedarikçi
                    </td>
                    <td>
                         <asp:DropDownList ID="drpSupplier" runat="server" AutoPostBack="True">
                             <asp:ListItem>South Brasil Iron Giant Company</asp:ListItem>
                             <asp:ListItem>Mario Brothers Associated</asp:ListItem>
                             <asp:ListItem>Noname Business Solutions</asp:ListItem>
                             <asp:ListItem>Douglas Mac Corp</asp:ListItem>
                             <asp:ListItem>Spain Super Steal</asp:ListItem>
                         </asp:DropDownList>                        
                    </td>
                </tr>
                <tr>
                    <td>Açıklama</td>
                    <td>
                        <asp:TextBox ID="txtDescription" runat="server" Height="85px" MaxLength="200" TextMode="MultiLine" Width="292px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtDescription" ErrorMessage="Açıklama girilmeli" ForeColor="Red">!</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td><asp:Button ID="btnAdd" runat="server" Text="Stoğa Ekle" OnClick="btnAdd_Click" SkinID="btnSubmit" /></td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Label ID="lblSummary" runat="server"></asp:Label>
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ForeColor="Red" />
                    </td>
                </tr>
            </table>            
        </div>
</asp:Content>
