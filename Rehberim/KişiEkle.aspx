<%@ Page Title="Kişi Ekle" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="KişiEkle.aspx.cs" Inherits="Rehberim.KisiEkle" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Kişi Ekle</h2>
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

        .form-container {
            display: flex;
            justify-content: space-between;
        }

        .form-group {
            flex: 1;
            padding: 10px;
            border-radius: 10px; 
        }

            .form-group label {
                display: block;
                margin-bottom: 5px;
            }

            .form-group input {
                width: 100%;
                padding: 5px;
            }

            .form-group .error-space {
                margin-top: 5px;
            }
    </style>
    <div class="form-container">
        <div class="form-group">
            <label for="txtAd">Ad</label>
            <asp:TextBox ID="txtAd" runat="server" />
            <div class="error-space">
                <asp:RequiredFieldValidator ID="rfvAd" runat="server" ControlToValidate="txtAd"
                    InitialValue="" ErrorMessage="Ad alanı boş bırakılamaz." Display="Dynamic" ForeColor="Red" /><br />
            </div>
        </div>
        <div class="form-group">
            <label for="txtSoyad">Soyad</label>
            <asp:TextBox ID="txtSoyad" runat="server" />
            <div class="error-space">
                <asp:RequiredFieldValidator ID="rfvSoyad" runat="server" ControlToValidate="txtSoyad"
                    InitialValue="" ErrorMessage="Soyad alanı boş bırakılamaz." Display="Dynamic" ForeColor="Red" /><br />
            </div>
        </div>
        <div class="form-group">
            <label for="txtTelNo">Tel No</label>
            <asp:TextBox ID="txtTelNo" runat="server" />
            <div class="error-space">
                <asp:RequiredFieldValidator ID="rfvTelNo" runat="server" ControlToValidate="txtTelNo"
                    InitialValue="" ErrorMessage="Tel No alanı boş bırakılamaz." Display="Dynamic" ForeColor="Red" /><br />
            </div>
        </div>
    </div>

    <div class="form-container">
        <div class="form-group">
            <label for="txtMail">Mail</label>
            <asp:TextBox ID="txtMail" runat="server" />
            <div class="error-space">
                <asp:RequiredFieldValidator ID="rfvMail" runat="server" ControlToValidate="txtMail"
                    InitialValue="" ErrorMessage="Mail alanı boş bırakılamaz." Display="Dynamic" ForeColor="Red" /><br />
                <asp:RegularExpressionValidator ID="revMail" runat="server" ControlToValidate="txtMail"
                    ErrorMessage="Geçerli bir e-posta adresi girin." Display="Dynamic" ForeColor="Red"
                    ValidationExpression="^[a-zA-Z0-9ÇçĞğIıİiÖöŞşÜü._%+-]+@[a-zA-Z0-9ÇçĞğIıİiÖöŞşÜü.-]+\.[a-zA-ZÇçĞğIıİiÖöŞşÜü]{2,4}$" /><br />
            </div>
        </div>
        <div class="form-group">
            <label for="txtCalistigiYer">Çalıştığı Yer</label>
            <asp:TextBox ID="txtCalistigiYer" runat="server" />
            <div class="error-space">
            </div>
        </div>
        <div class="form-group">
            <label for="txtKimlikNo">Kimlik Numarası</label>
            <asp:TextBox ID="txtKimlikNo" runat="server"></asp:TextBox>
            <div class="error-space">
                <asp:CustomValidator ID="cvKimlikNo" runat="server" ControlToValidate="txtKimlikNo"
                    ClientValidationFunction="validateKimlikNo" Display="Dynamic" ForeColor="Red"
                    ErrorMessage="Geçerli bir T.C. Kimlik Numarası girin " />
            </div>

        </div>

    </div>

    <asp:Button ID="btnAddPerson" runat="server" Text="Kişi Ekle" CssClass="btn btn-primary" OnClick="btnAddPerson_Click" />

    <asp:Label ID="lblMessage" runat="server" EnableViewState="false" Visible="false" CssClass="text-success" />


    <script type="text/javascript">
        function validateKimlikNo(sender, args) {
            var kimlikNo = args.Value.trim();

            if (kimlikNo === "") {
                args.IsValid = true;
                return;
            }

            if (kimlikNo.length !== 11) {
                args.IsValid = false;
                return;
            }

            for (var i = 0; i < kimlikNo.length; i++) {
                if (isNaN(kimlikNo[i])) {
                    args.IsValid = false;
                    return;
                }
            }


        }
    </script>


</asp:Content>
