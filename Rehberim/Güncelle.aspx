<%@ Page Title="Kişi Güncelle" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Guncelle.aspx.cs" Inherits="Rehberim.Guncelle" %>

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

    <h2>Kişi Güncelle</h2>
    <div class="row">
        <div class="col-md-3">
            <div class="form-horizontal">
                <div class="form-group">
                    <label class="control-label">Ad</label>
                    <asp:TextBox ID="txtAd" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="form-group">
                    <label class="control-label">Soyad</label>
                    <asp:TextBox ID="txtSoyad" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="form-group">
                    <label class="control-label">Tel No</label>
                    <asp:TextBox ID="txtTelNo" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="form-group">
                    <label class="control-label">Açıklama</label>
                    <asp:TextBox ID="txtAciklama" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="5"></asp:TextBox>
                </div>
                <div class="col-md-offset-2 col-md-10" style="margin-top: 10px;">
                    <asp:Button ID="btnUpdate" runat="server" Text="Güncelle" CssClass="btn btn-primary" OnClick="btnUpdate_Click" />
                    <asp:Label ID="lblUpdateMessage" runat="server" CssClass="text-success" Visible="false" Style="margin-left: 10px;">Kaydedildi</asp:Label>
                </div>
            </div>
        </div>
        <div class="col-md-3">
            <div class="form-horizontal">
                <div class="form-group">
                    <label class="control-label">Mail</label>
                    <asp:TextBox ID="txtMail" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="form-group">
                    <label class="control-label">Çalıştığı Yer</label>
                    <asp:TextBox ID="txtCalistigiYer" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="form-group">
                    <label class="control-label">Kimlik Numarası</label>
                    <asp:TextBox ID="txtKimlikNo" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
        </div>

        <div class="col-md-6">
            <div style="margin-top: 5px;">
                <h4 style="margin-top: 0;">Fotoğraf Yükle</h4>
                <div style="border: 2px solid #ccc; padding: 10px; text-align: center;">
                    <asp:Image ID="uploadedImage" runat="server" ImageUrl="" AlternateText="Yüklenen Fotoğraf" Style="max-width: 100%;" />
                </div>
                <br />
                <asp:FileUpload ID="fileUpload" runat="server" />
                <div style="margin-top: 8px;">
                    <asp:Button ID="btnUpload" runat="server" Text="Yükle" CssClass="btn btn-primary" OnClick="btnUpload_Click" />
                </div>
            </div>


        </div>
    </div>





</asp:Content>


