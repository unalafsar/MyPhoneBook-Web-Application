<%@ Page Title="Kişileri Görüntüle" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="KisileriGoruntule.aspx.cs" Inherits="Rehberim.KisileriGoruntule" %>

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

    <div class="row">
        <div class="col-md-8">
            <h2>Kişileri Görüntüle</h2>
        </div>
        <div class="col-md-4">
            <div class="input-group">
                <asp:TextBox ID="txtSearch" runat="server" placeholder="Ad veya Soyad girin" CssClass="form-control"></asp:TextBox>
                <span class="input-group-btn" style="margin-left: 5px;">
                    <asp:Button ID="btnSearch" runat="server" Text="Ara" CssClass="btn btn-primary" OnClick="btnSearch_Click" />
                </span>
                <span class="input-group-btn" style="margin-left: 5px;">
                    <asp:Button ID="btnExportToExcel" runat="server" Text="Kişileri Excel'e Aktar" CssClass="btn btn-success" OnClick="btnExportToExcel_Click" />
                </span>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-12">
            <asp:Button ID="btnSelectAll" runat="server" Text="Hepsini Seç" CssClass="btn btn-secondary" OnClick="btnSelectAll_Click" />
        </div>
    </div>

    <div class="table-responsive" style="margin-top: 20px;">
        <asp:GridView ID="gvKisiler" runat="server" DataKeyNames="ID" CssClass="table table-bordered table-striped" AutoGenerateColumns="false" OnRowDeleting="gvKisiler_RowDeleting" OnRowCommand="gvKisiler_RowCommand">
            <Columns>
                <asp:TemplateField HeaderText="">
                    <ItemTemplate>
                        <asp:CheckBox ID="chkSelect" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Ad" HeaderText="Ad" />
                <asp:BoundField DataField="Soyad" HeaderText="Soyad" />
                <asp:BoundField DataField="TelNo" HeaderText="Tel No" />
                <asp:TemplateField HeaderText="Mail">
                    <ItemTemplate>
                        <asp:HyperLink ID="gvMailLink" runat="server" NavigateUrl='<%# "mailto:" + Eval("Mail") %>' Text='<%# Eval("Mail") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="CalistigiYer" HeaderText="Çalıştığı Yer" />
                <asp:BoundField DataField="KimlikNo" HeaderText="Kimlik Numarası" />

                <asp:TemplateField HeaderText="İşlemler" ItemStyle-Width="150px" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <div class="btn-group">
                            <asp:Button ID="btnDelete" runat="server" Text="Sil" CssClass="btn btn-danger btn-sm btn-rounded" CommandName="Delete" CommandArgument='<%# Container.DataItemIndex %>'
                                OnClientClick="return confirmDelete();" />
                            <asp:Button ID="btnEdit" runat="server" Text="Güncelle" CssClass="btn btn-primary btn-sm btn-rounded" CommandName="EditRow" CommandArgument='<%# Container.DataItemIndex %>' Style="margin-left: 5px; margin-right: 5px;" />
                            <asp:Button ID="btnDetails" runat="server" Text="Detay" CssClass="btn btn-info btn-sm btn-rounded" CommandName="Details" CommandArgument='<%# Container.DataItemIndex %>' />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>

    <script>
        function confirmDelete() {
            return confirm("Bu kişiyi silmek istediğinizden emin misiniz?");
        }
    </script>
</asp:Content>
