<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GirişYap.aspx.cs" Inherits="Rehberim.GirişYap" %>



<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .container.body-content {
            padding-bottom: 10px; 
        }

        footer {
            position: absolute;
            bottom: 0;
            width: 100%;
            background-color: transparent; 
        }

        .navbar-dark .navbar-toggler-icon {
            background-color: white; 
        }

        .navbar {
            background-color: transparent !important; 
            border-bottom: 1px solid #ccc; 
            border-bottom-color: white;
        }

        .navbar-brand,
        .navbar-nav .nav-link {
            color: white !important; 
        }

        .container.body-content footer {
            
            color: white; 
        }

        body {
            font-family: Arial, sans-serif;
            background-image: url('Images/login2.jpg'); 
            background-size: cover;
            background-repeat: no-repeat;
            background-attachment: fixed;
            margin: 0;
            padding: 0;
            background-position: center center; 
            overflow-x: hidden; 
        }

        h2 {
            color: #fff; 
        }

        .login-container {
            background-color: rgba(255, 255, 255, 0); 
            color: #fff; 
            border-radius: 5px;
            box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
            width: 300px;
        }

        .form-group {
            margin-bottom: 15px;
        }

            .form-group label {
                display: block;
                margin-bottom: 5px;
            }

        .form-control {
            width: 100%;
            padding: 10px;
            border: 1px solid #ccc;
            border-radius: 3px;
            color: cadetblue;
        }

        .btn {
            background-color: #007bff;
            color: #fff;
            border: none;
            padding: 10px 20px;
            border-radius: 3px;
            cursor: pointer;
        }

            .btn:hover {
                background-color: #0056b3;
            }

        .form-horizontal {
            color: #fff;
        }

        .custom-textbox {
            background-color:mediumturquoise; 
            color: #000; 
            border: 5px solid #000; 
            border-radius: 10px; 
            padding: 5px; 
            width: 100%; 
        }
    </style>


    <div class="row">

        <div class="col-md-6">
            <h2>Giriş Yap</h2>
            <div class="form-horizontal">
                <div class="form-group">
                    <label for="txtKullaniciAdi">Kullanıcı Adı</label>
                    <asp:TextBox ID="txtKullaniciAdi" runat="server" CssClass="custom-textbox" placeholder="Kullanıcı Adı"></asp:TextBox>
                </div>
                <div class="form-group">
                    <label for="txtPassword">Şifre</label>
                    <asp:TextBox ID="txtPassword" runat="server" CssClass="custom-textbox" TextMode="Password" placeholder="Şifre"></asp:TextBox>
                </div>
                <div class="form-group">
                    <asp:Button ID="btnLogin" runat="server" CssClass="btn" Text="Giriş Yap" OnClick="btnLogin_Click" />
                </div>
                <div class="form-group">
                    <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                </div>
            </div>
        </div>




        <div class="col-md-6">
            <h2>Şifremi Değiştir</h2>
            <div class="form-horizontal">
                <div class="form-group">
                    <label for="txtKullaniciAdii">Kullanıcı Adı</label>
                    <asp:TextBox ID="txtKullaniciAdii" runat="server" CssClass="custom-textbox" placeholder="Kullanıcı Adı"></asp:TextBox>
                </div>
                <div class="form-group">
                    <label for="txtMevcutSifre">Mevcut Şifre</label>
                    <asp:TextBox ID="txtMevcutSifre" runat="server" CssClass="custom-textbox" TextMode="Password" placeholder="Mevcut Şifre"></asp:TextBox>
                </div>
                <div class="form-group">
                    <label for="txtYeniSifre">Yeni Şifre</label>
                    <asp:TextBox ID="txtYeniSifre" runat="server" CssClass="custom-textbox" TextMode="Password" placeholder="Yeni Şifre"></asp:TextBox>
                </div>
                <div class="form-group">
                    <label for="txtYeniSifreTekrar">Yeni Şifre (Tekrar)</label>
                    <asp:TextBox ID="txtYeniSifreTekrar" runat="server" CssClass="custom-textbox" TextMode="Password" placeholder="Yeni Şifre (Tekrar)"></asp:TextBox>
                </div>
                <div class="form-group">
                    <asp:Button ID="btnSifreDegistir" runat="server" CssClass="btn" Text="Şifremi Değiştir" OnClick="btnSifreDegistir_Click" />
                </div>
                <div class="form-group">
                    <asp:Label ID="lblMessage2" runat="server" ForeColor="Red"></asp:Label>
                </div>
            </div>
        </div>
    </div>










</asp:Content>

