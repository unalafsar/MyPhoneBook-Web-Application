<%@ Page Title="Kişi Detayı" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="KisiDetay.aspx.cs" Inherits="Rehberim.KisiDetay" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

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

    <h2 style="margin-top: 50px;">Kişi Detayı</h2>
    <div class="row">
        <div class="col-md-12">
            <div class="row">
                <div class="col-md-6">
                    <ul class="list-group" style="height: 400px; overflow-y: auto;">
                        <li class="list-group-item">
                            <strong>Ad:</strong>
                            <asp:Label ID="lblAd" runat="server" Text=""></asp:Label>
                        </li>
                        <li class="list-group-item">
                            <strong>Soyad:</strong>
                            <asp:Label ID="lblSoyad" runat="server" Text=""></asp:Label>
                        </li>
                        <li class="list-group-item">
                            <strong>Tel No:</strong>
                            <asp:Label ID="lblTelNo" runat="server" Text=""></asp:Label>
                        </li>
                        <li class="list-group-item">
                            <strong>Mail:</strong>
                            <asp:Label ID="lblMail" runat="server" Text=""></asp:Label>
                        </li>
                        <li class="list-group-item">
                            <strong>Çalıştığı Yer:</strong>
                            <asp:Label ID="lblCalistigiYer" runat="server" Text=""></asp:Label>
                        </li>
                        <li class="list-group-item">
                            <strong>Kimlik Numarası:</strong>
                            <asp:Label ID="lblKimlikNo" runat="server" Text=""></asp:Label>
                        </li>
                        <li class="list-group-item">
                            <strong>Açıklama:</strong>
                            <asp:Label ID="lblAciklama" runat="server" Text=""></asp:Label>
                        </li>
                    </ul>
                </div>
                <div class="col-md-6">
                    <div class="horizontal">
                        <asp:Image ID="imgFoto" runat="server" Width="600" Height="500" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
