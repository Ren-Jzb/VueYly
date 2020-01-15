<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminWeb.aspx.cs" Inherits="CareHome.AdminWeb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="renderer" content="webkit" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,user-scalable=no" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge, chrome=1" />
    <title>开发：快捷账号</title>
    <style type="text/css">
        .button {
            border: 1px solid gray;
            width: 150px;
            display: inline-block;
            margin-right: 20px;
            margin-bottom: 10px;
            text-align: center;
            text-decoration: none;
            color: #000;
            line-height: 24px;
            font-weight: bolder;
            background: #fff;
            cursor: pointer;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <fieldset style="margin-top: 20px;">
            <legend>后台：</legend>
            <div>
                <a href="/VueText/List" target="_blank" class="button">Vue测试页面</a>
                <asp:Button ID="btnsuper" class="button" runat="server" Text="超级管理员[super]" OnClick="btnsuper_Click" />
            </div>
        </fieldset>
        <fieldset style="margin-top: 20px;">
            <legend>接口：</legend>
            <div>
                <a href="Ws/CareOperate.asmx" target="_blank" class="button">WebApi</a>
                <a href="Ws/CareOperate.asmx" target="_blank" class="button">WebService</a>
            </div>
        </fieldset>
        <fieldset style="margin-top: 20px;">
            <legend>新增业务：</legend>
            <div>
                <a href="/NuringMobile/Index" target="_blank" class="button">小程序测试Index</a>
            </div>
        </fieldset>
    </form>
</body>
</html>
