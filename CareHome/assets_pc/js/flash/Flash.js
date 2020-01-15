IncludeFlashJS("/js/Flash/swfobject_modified.js");

function LoadFlash(url, wmode, w, h)//eg：LoadFlash("../js/Flash/top.swf", "", 1000, 180);
{
    switch (wmode) {
        case "opaque"://设置成这个后FLASh可以被调整层深。
        case "transparent"://即把FLASH背景设成透明，在网页上就可以把FLASH放到图片或者文字之上。
        case "Window"://就是FLASH是单独的一层，和浏览的网页上的内容是不相干的，总是在所有东西的上面。
            break;
        default:
            wmode = "opaque";
            break;
    }
    expressinstall = "/js/Flash/expressInstall.swf";
    var flashcode = "<object classid=\"clsid:D27CDB6E-AE6D-11cf-96B8-444553540000\" width=\"" + w + "\" height=\"" + h + "\">"
          + "<param name=\"movie\" value=\"" + url + "\" />"
          + "<param name=\"quality\" value=\"high\" />"
          + "<param name=\"wmode\" value=\"" + wmode + "\" />"
          + "<param name=\"swfversion\" value=\"7.0.70.0\" />"
          + "<param name=\"expressinstall\" value=\"" + expressinstall + "\" />"
          + "<!--[if !IE]>-->"
          + "<object type=\"application/x-shockwave-flash\" data=\"" + url + "\" width=\"" + w + "\" height=\"" + h + "\">"
            + "<!--<![endif]-->"
            + "<param name=\"quality\" value=\"high\" />"
            + "<param name=\"wmode\" value=\"" + wmode + "\" />"
            + "<param name=\"swfversion\" value=\"7.0.70.0\" />"
            + "<param name=\"expressinstall\" value=\"" + expressinstall + "\" />"
            + "<div>"
              + "<h4>此页面上的内容需要较新版本的 Adobe Flash Player。</h4>"
              + "<p><a href=\"http://www.adobe.com/go/getflashplayer\"><img src=\"http://www.adobe.com/images/shared/download_buttons/get_flash_player.gif\" alt=\"获取 Adobe Flash Player\" width=\"112\" height=\"33\" /></a></p>"
            + "</div>"
            + "<!--[if !IE]>-->"
          + "</object>"
          + "<!--<![endif]-->"
        + "</object>";
    document.write(flashcode);
}

function IncludeFlashJS(fileUrl)//加载引用FlashJS
{
    var oHead = document.getElementsByTagName('HEAD').item(0);
    var oScript = document.createElement("script");
    oScript.language = "javascript";
    oScript.type = "text/javascript";
    oScript.src = fileUrl;
    oHead.appendChild(oScript);
} 

//脚本输出flash，免flash激活
//参数：url :flash路径	obj:输出对象 	w：宽度		h：高度
function PutFlash(url, obj, w, h) {
    var flash;
    flash = '<object classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" codebase="http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=6,0,29,0" width="' + w + '" height="' + h + '">';
    flash = flash + '<param name="movie" value="' + url + '">';
    flash = flash + '<param name="quality" value="high"> ';
    flash = flash + '<param name=quality value=transparent>';
    flash = flash + '<param name=wmode value=transparent>';
    flash = flash + '<param name="allowScriptAccess" value="sameDomain" />';
    flash = flash + '<embed src="' + url + '" quality="high" quality="transparent" wmode="transparent" pluginspage="http://www.macromedia.com/go/getflashplayer" align="middle" allowScriptAccess="sameDomain"  type="application/x-shockwave-flash" width="' + w + '" height="' + h + '"></embed>';
    flash = flash + '</object> ';
    obj.innerHTML = flash;
}