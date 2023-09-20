<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="KullaniciEkle.aspx.cs" Inherits="Rehberim.KullaniciEkle" %>

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

    <div class="row">
        <div class="col-md-5">
            <h2>Kullanıcılar</h2>
            <asp:GridView ID="GridView1" runat="server" CssClass="table table-bordered table-striped" AutoGenerateColumns="False" OnRowCommand="GridView1_RowCommand">
                <Columns>
                    <asp:BoundField DataField="KullaniciAdi" HeaderText="Kullanıcı Adı" />
                    <asp:TemplateField HeaderText="İşlemler">
                        <ItemTemplate>
                            <asp:Button ID="btnSil" runat="server" Text="Sil" CommandName="Sil" CommandArgument='<%# Eval("KullaniciAdi") %>' OnClientClick='<%# "return OnaylaSilme(\"" + Eval("KullaniciAdi") + "\");" %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <div class="form-group">
                <asp:Label ID="lblMesaj2" runat="server" ForeColor="Red"></asp:Label>
            </div>
        </div>
        <div class="col-md-2">
        </div>
        <div class="col-md-5">
            <h2>Kullanıcı Ekle</h2>
            <div class="form-group">
                <label for="txtKullaniciAdi">Kullanıcı Adı</label>
                <asp:TextBox ID="TextKullaniciAdi" runat="server" CssClass="form-control" placeholder="Kullanıcı Adı"></asp:TextBox>
            </div>
            <div class="form-group">
                <label for="txtSifre">Şifre</label>
                <asp:TextBox ID="txtSifre" runat="server" CssClass="form-control" TextMode="Password" placeholder="Şifre"></asp:TextBox>
            </div>
            <div class="form-group" style="margin-top: 8px;">
                <asp:Button ID="btnKaydet" runat="server" CssClass="btn-kaydet" Text="Kaydet Yap" OnClick="btnKaydet_Click" />
            </div>
            <div class="form-group">
                <asp:Label ID="lblMesaj" runat="server" ForeColor="Red"></asp:Label>
            </div>
        </div>
    </div>

    <script>
        function OnaylaSilme(kullaniciAdi) {
            if (confirm("Kullanıcıyı silmek istediğinizden emin misiniz?")) {
                SilKullanici(kullaniciAdi);
            } else {
                return false;
            }
        }
    </script>
</asp:Content>
