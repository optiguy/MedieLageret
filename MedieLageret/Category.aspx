<%@ Page Title="" Language="C#" MasterPageFile="~/Layout/Site.Master" AutoEventWireup="true" CodeBehind="Category.aspx.cs" Inherits="MedieLageret.Layout.Category" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Main" runat="server">
    <div class="row">
        <div class="col-xs-12">
            <asp:LinkButton runat="server" OnClick="Linkbutton_Filter_Click"><i class="fa fa-sliders fa-2x"></i> Vis filtre</asp:LinkButton>
        </div>
        <asp:Panel ID="Panel_filters" runat="server" Visible="false">

            <div class="col-xs-8">
                <asp:TextBox runat="server" ID="Textbox_search" CssClass="form-control" placeholder="Søg på titel eller beskrivelse her..." />
            </div>
            <div class="col-xs-4">
                <asp:Button ID="Button_search" runat="server" Text="Søg" CssClass="btn btn-block" OnClick="Button_search_Click" />
            </div>
            <div class="col-xs-6">
                <asp:DropDownList ID="DropDownList_category" runat="server" AutoPostBack="true" CssClass="form-control" DataSourceID="SqlDataSource_categories" DataTextField="title" DataValueField="id" OnSelectedIndexChanged="Button_search_Click"></asp:DropDownList>
                <asp:SqlDataSource runat="server" ID="SqlDataSource_categories" ConnectionString='<%$ ConnectionStrings:ConnectionString %>' SelectCommand="SELECT [id], [title] FROM [Categories]"></asp:SqlDataSource>
            </div>
            <div class="col-xs-6">
                <asp:DropDownList ID="DropDownList_sorting" CssClass="form-control" runat="server" OnSelectedIndexChanged="Button_search_Click" AutoPostBack="true">
                    <asp:ListItem Value="priceAsc">Laveste pris</asp:ListItem>
                    <asp:ListItem Value="priceDesc">Højeste pris</asp:ListItem>
                    <asp:ListItem Value="titleAsc">Alfabetisk A-Z</asp:ListItem>
                    <asp:ListItem Value="titleDesc">Alfabetisk Z-A</asp:ListItem>
                    <asp:ListItem Value="yearAsc">Laveste årstal</asp:ListItem>
                    <asp:ListItem Value="yearDesc">Højeste årstal</asp:ListItem>
                </asp:DropDownList>
            </div>

            <div class="col-xs-3">
                Min. pris :
                <asp:TextBox ID="Textbox_minPris" CssClass="form-control" runat="server" />
                <br />
                Max. pris :
                <asp:TextBox ID="Textbox_maxPris" CssClass="form-control" runat="server" />
            </div>
            <div class="col-xs-3">
                Min. årstal :
                <asp:TextBox ID="Textbox_minYear" CssClass="form-control" runat="server" />
                <br />
                Max. årstal :
                <asp:TextBox ID="Textbox_maxYear" CssClass="form-control" runat="server" />
            </div>
            <div class="col-xs-6">
                <asp:CheckBoxList ID="CheckBoxList_genre" runat="server" DataSourceID="SqlDataSource_genre" DataTextField="title" DataValueField="id"></asp:CheckBoxList>
                <asp:SqlDataSource runat="server" ID="SqlDataSource_genre" ConnectionString='<%$ ConnectionStrings:ConnectionString %>' SelectCommand="SELECT [id], [title] FROM [Categories]"></asp:SqlDataSource>
            </div>

        </asp:Panel>
    </div>
    <div class="row">
        <asp:Repeater ID="Repeater_Products" runat="server">
            <ItemTemplate>
                <div class="col-xs-6 col-md-4">
                    <asp:HyperLink NavigateUrl='<%# "~/Product.aspx?produkt=" + Eval("id") %>' runat="server">
                        <h2><%#Eval("name")%></h2>
                    </asp:HyperLink>
                    Pris: <%#Eval("price")%> DKK<br />
                    På lager: <%#Eval("stock")%><br />
                    <asp:HyperLink NavigateUrl='<%# "~/Product.aspx?produkt=" + Eval("id") %>' runat="server">
                        <asp:Image CssClass="img-responsive" ImageUrl='<%#Eval("image")%>' runat="server" />
                    </asp:HyperLink>
                </div>
            </ItemTemplate>
        </asp:Repeater>
        <asp:Label Text="" ID="Label_noProducts" runat="server" />
    </div>
</asp:Content>
