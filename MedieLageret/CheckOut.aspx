<%@ Page Title="" Language="C#" MasterPageFile="~/Layout/Site.Master" AutoEventWireup="true" CodeBehind="CheckOut.aspx.cs" Inherits="MedieLageret.CheckOut" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Main" runat="server">
    <asp:Literal ID="Literal_message" runat="server"></asp:Literal><div class="checkbox">
        <label>
            <asp:CheckBox ID="CheckBox_terms" runat="server" />
            Accepter betingelser for køb
        </label>
    </div>
    <asp:LinkButton ID="LinkButton_accept_and_buy" CssClass="btn btn-primary" runat="server" OnClick="LinkButton_accept_and_buy_Click">Accepter og gem ordre!</asp:LinkButton>
</asp:Content>
