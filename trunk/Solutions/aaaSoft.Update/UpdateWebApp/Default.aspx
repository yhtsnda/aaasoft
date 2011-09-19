<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="UpdateWebApp.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>软件更新服务器</title>
    <link href="css/css.css" type="text/css" rel="stylesheet" />
</head>
<body>
<form id="form1" runat="server">
    <div>
        <h1>软件更新服务器</h1>
    </div>
    <div>
        <h4>当前路径:<asp:Label ID="lblCurrentPath" runat="server" Text="/"></asp:Label>
        </h4>
    </div>
    <div>
        <table width="100%" cellspacing="1" cellpadding="3" class="list_table">
            <asp:Repeater ID="rptFolders" runat="server">
                <HeaderTemplate>
                    <tr class="list_table_Title">
                        <td align="center">
                            文件名
                        </td>
                        <td align="center">
                            版本
                        </td>
                        <td align="center">
                            时间
                        </td>
                        <td align="center">
                            大小
                        </td>
                    </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr onmouseover="this.className='list_moveOn'" onmouseout="this.className='list_moveOff'">
                        <td align="left">
                            <asp:ImageButton ID="imgGoto" runat="server"  OnClick="lklGoto_Click" ImageUrl="./images/folder.png" CommandName='<%# Eval("FileName")%>' />
                            <asp:LinkButton ID="lklGoto" runat="server" OnClick="lklGoto_Click" Text = '<%# Eval("FileName")%>' CommandName='<%# Eval("FileName")%>'></asp:LinkButton>
                        </td>
                        <td align="center">
                        </td>
                        <td align="center">
                            <%# Eval("LastWriteTime")%>
                        </td>
                        <td align="center">
                            <a>文件夹</a>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
            <asp:Repeater ID="rptFiles" runat="server">                
                <ItemTemplate>
                    <tr onmouseover="this.className='list_moveOn'" onmouseout="this.className='list_moveOff'">
                        <td align="left">
                            <a href="?Action=DownloadFile&FileName=<%# Eval("RelativeFilePath")%>">
                                <img src="./images/file.png" alt="" border="0" />
                            </a>
                            <a href="?Action=DownloadFile&FileName=<%# Eval("RelativeFilePath")%>">
                                <%# Eval("FileName")%>
                            </a>
                        </td>
                        <td align="center">
                            <%# Eval("FileVersion")%>
                        </td>
                        <td align="center">
                            <%# Eval("LastWriteTime")%>
                        </td>
                        <td align="center">
                            <%# Eval("FileLength")%>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
        <div style="text-align: center">
            <p runat ="server" id="pFoot" />
        </div>
    </div>
    </form>
</body>
</html>
