<%@ Page Title="" Language="C#" MasterPageFile="~/Layout/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="MedieLageret.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Main" runat="server">
    <h1>Velkommen til MedieLageret.</h1>
    <h2>Kategorier</h2>
    <div class="row">
        <asp:Repeater ID="Repeater_categories" runat="server">
            <ItemTemplate>
                <div class="col-xs-6 col-md-4">
                    <asp:HyperLink NavigateUrl='<%# "~/Category.aspx?kategori=" + Eval("id") %>' runat="server">
                        <h3><%# Eval("title") %></h3>
                        <asp:Image CssClass="img-responsive" ImageUrl='<%# Eval("image") %>' runat="server" />
                    </asp:HyperLink>
                    <p><%# Eval("description") %></p>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
</asp:Content>
