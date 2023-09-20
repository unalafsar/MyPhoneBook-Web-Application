<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RolAyarlama.aspx.cs" Inherits="Rehberim.RolAyarlama" %>

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
        <asp:GridView ID="GridViewKullanicilar" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-striped">
            <Columns>
                <asp:BoundField DataField="ID" HeaderText="ID" />
                <asp:BoundField DataField="KullaniciAdi" HeaderText="Kullanıcı Adı" />
                <asp:BoundField DataField="Rol" HeaderText="Rol" />
                <asp:TemplateField HeaderText="Yeni Rol">
                    <ItemTemplate>
                        <asp:DropDownList ID="DropDownList1" runat="server">
                        </asp:DropDownList>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="İşlemler">
                    <ItemTemplate>
                        <asp:Button ID="btnKaydet" runat="server" Text="Kaydet" OnClick="btnKaydet_Click" CommandArgument='<%# Eval("ID") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>

        <asp:GridView ID="GridViewRolSayfaAtama" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-striped">
            <Columns>
                <asp:BoundField DataField="RolAdi" HeaderText="Rol Adı" />
                <asp:TemplateField HeaderText="Sayfalar">
                    <ItemTemplate>
                        <asp:DropDownList ID="DropDownListSayfalar" runat="server" DataTextField="SayfaAdi" DataValueField="SayfaID">
                        </asp:DropDownList>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Kaydet">
                    <ItemTemplate>
                        <asp:Button ID="btnKaydetRolSayfa" runat="server" Text="Kaydet" OnClick="btnKaydetRolSayfa_Click" CommandArgument='<%# Eval("RolAdi") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Geri Al">
                    <ItemTemplate>
                        <asp:Button ID="btnGeriAlRol" runat="server" Text="Geri Al" OnClick="btnGeriAlRol_Click" CommandArgument='<%# Eval("RolAdi") %>' />
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Mevcut İzinli Sayfalar">
                    <ItemTemplate>
                        <%# GetMevcutIzinliSayfalar(Eval("RolAdi").ToString()) %>
                    </ItemTemplate>
                </asp:TemplateField>


            </Columns>
        </asp:GridView>


    </div>
</asp:Content>
