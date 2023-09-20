<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="LogKayıtları.aspx.cs" Inherits="Rehberim.LogKayıtları" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <style>
        body {
            background-image: url('Images/kişiler5.jpg'); 
            background-size: cover;
            background-repeat: no-repeat;
            background-attachment: fixed;
            margin: 0;
            padding: 0;
            background-position: center center; 
            overflow-x: hidden; 
        }
    </style>

    <div class="table-responsive" style="margin-top: 20px;">
        <div style="clear: both; margin-bottom: 10px;"></div>
        <div style="float: right;">
            <asp:Label ID="Label1" runat="server" Text="Sıralama Seçin: "></asp:Label>
            <asp:DropDownList ID="DropDownListSiralama" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DropDownListSiralama_SelectedIndexChanged">
                <asp:ListItem Text="Tarihe Göre (Önce En Eski)" Value="Artan" />
                <asp:ListItem Text="Tarihe Göre (Önce En Yeni)" Value="Azalan" />
            </asp:DropDownList>
        </div>

        <div>
            <asp:Label ID="Label2" runat="server" Text="Kullanıcı Adı ile Filtrele: "></asp:Label>
            <asp:TextBox ID="TextBoxKullaniciAdi" runat="server"></asp:TextBox>
            <asp:Button ID="ButtonFiltrele" runat="server" Text="Filtrele" OnClick="ButtonFiltrele_Click" />
        </div>

        <div style="margin-top: 20px;">
            <asp:GridView ID="GridViewLog" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-striped">
                <Columns>
                    <asp:BoundField DataField="KullaniciAdi" HeaderText="Kullanıcı Adı" />
                    <asp:BoundField DataField="IslemTuru" HeaderText="İşlem Türü" />
                    <asp:BoundField DataField="IslemDetayi" HeaderText="İşlem Detayı" />
                    <asp:BoundField DataField="IslemZamani" HeaderText="İşlem Zamanı" />
                </Columns>
            </asp:GridView>
        </div>
    </div>


</asp:Content>


