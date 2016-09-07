<%@ Page Title="" Language="C#" MasterPageFile="~/Layout/Site.Master" AutoEventWireup="true" CodeBehind="Product.aspx.cs" Inherits="MedieLageret.Product" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Main" runat="server">
    <div class="row">
        <asp:FormView ID="FormView_product" runat="server">
            <ItemTemplate>
                <div class="col-xs-12">
                    <asp:Image runat="server" CssClass="img-responsive" ImageUrl='<%# Eval("image") %>' />
                </div> 
                <div class="col-xs-12">
                    <h2><%# Eval("name") %></h2>
                    Pris: <%# Eval("price") %> DKK<br />
                    På lager: <%# Eval("stock") %><br />
                    <p><%# Eval("description") %></p>
                    <asp:LinkButton ID="LinkButton_buy" CssClass="btn btn-info" runat="server" OnCommand="LinkButton_buy_Command" CommandArgument='<%#Eval("id") %>'>
                        <i class="glyphicon glyphicon-shopping-cart"></i> <%# Eval("price") %> DKK
                    </asp:LinkButton>
                </div>
            </ItemTemplate>
        </asp:FormView>
    </div>
</asp:Content>
