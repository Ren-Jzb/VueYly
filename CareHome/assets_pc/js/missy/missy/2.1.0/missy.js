// JavaScript Document
/*
 * 2.1.0更新日志：
 * 1.方法名称全部才采用小驼峰命名法
 * 2.方法前缀尽可能用如下前缀：
 * get（表获取），set（表设置），is（表判断）,
 * i(表弹出框和表系统全局)，format(表格式：字符，状态等显示内容格式)
 * trim（表去除）其它方法不带以上前缀的将不得调用和
 * 诺：要添加其它前缀将标注更新。
*/
/*
JavaScript脚本命名法规则如下
(1)变量采用小驼峰法命名法：除第一个单词之外，其他单词首字母大写。
(2)方法采用大驼峰法命名法：相比小驼峰法，大驼峰法把第一个单词的首字母也大写了。
(3)命名采用有效意义命名，尽可能采用全称。
*/
/*javascript中null,undefined,0,"",false作为if的条件的时候，被认为是flase*/
/*javascript除了数字，布尔，字符串这些原始值和null, undefined这些特殊的，其他都是对象*/
/*旨在解决常规性业务通用功能。 */
; !function (window,undefined) {
    "use strict";
    window.MISSY = {
        version: "2.1.0",
        formatJsonCharFilter: function (s) {
            s = s.replace(/\\b/g, '\\\b');
            s = s.replace(/\\t/g, '\\\t');
            s = s.replace(/\\n/g, '\\\n');
            s = s.replace(/\\f/g, '\\\f');
            s = s.replace(/\\r/g, '\\\r');
            return s;
        },
        getQueryString: function (name) //QueryString获取地址栏参数值
        {
            if (arguments.length < 1) //Params 数组
            {
                var url = location.search; //获取url中"?"符后的字串
                var requestArray = new Object();
                if (url.indexOf("?") !== -1) {
                    var str = url.substr(1);
                    var strs = str.split("&");
                    for (var i = 0; i < strs.length; i++) {
                        requestArray[strs[i].split("=")[0]] = unescape(strs[i].split("=")[1]);
                    }
                }
                return requestArray;
            } else if (arguments.length === 1) //Param 单个
            {
                var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
                var r = window.location.search.substr(1).match(reg);
                if (r != null) return unescape(r[2]);
                return null;
            }
            return null;
        },
        isIframe: function () //判断当前是否处在iframe中
        {
            return top.location !== self.location;
        },
        isFunc: function (fun) //判断是否是方法
        {
            return (fun != null && typeof fun == "function");
        },
        setDoNothing: function () //返回 true 且啥也不处理的回调函数
        {
            return true;
        },
        setUpdateUrl: function (url) //更新浏览器地址栏链接地址
        {
            if (window.history && window.history.pushState) {
                window.history.pushState(null, url, url);
            }
        },
        setOpenWin: function (url) //window.open时，在很多情况下，弹出的窗口会被浏览器阻止
        {
            var a = document.createElement("a");
            a.setAttribute("href", url);
            a.setAttribute("target", "_blank");
            a.setAttribute("id", "openwin");
            document.body.appendChild(a);
            a.click();
        },
        setOpenAllScreenWin: function (url) //满屏
        {
            window.open(url, "", "toolbar=no,menubar=no,scrollbars=yes,resizable=yes,location=no,status=no");
        },
        setFullScreen: function (obj, t) {
            if (!obj) {
                return;
            }
            var o = null || obj,
                el = document.documentElement,
                cl = document,
                rfs = el.requestFullScreen || el.webkitRequestFullScreen || el.mozRequestFullScreen,
                cfs = cl.cancelFullScreen || cl.webkitCancelFullScreen || cl.mozCancelFullScreen;
            if (t) {
                o.toggle(function () {
                    rfs.call(el);
                    t.text("退出");
                    return false;
                }, function () {
                    cfs.call(cl);
                    t.text("全屏");
                    return false;
                });
            } else {
                o.on("dblclick", function () {
                    rfs.call(el);
                    return false;
                });
            }
        },
        setHome: function (obj, url) //设置主页 eg:setHome(this,window.location);
        {
            if (this.browser() === 'chrome') {
                alert("您的浏览器不支持该操作，请使用浏览器菜单手动设置.");
                return true;
            }
            if (document.all) {
                document.body.style.behavior = 'url(#default#homepage)';
                document.body.setHomePage(url);
            } else if (window.sidebar) {
                if (window.netscape) {
                    try {
                        window.netscape.security.PrivilegeManager.enablePrivilege("UniversalXPConnect");
                    } catch (e) {
                        alert("您的浏览器不支持该操作，请使用浏览器菜单手动设置.");
                        return true;
                    }
                }
                if (window.confirm("你确定要设置" + url + "为首页吗？")) {
                    var prefs = window.Components.classes['@mozilla.org/preferences-service;1'].getService(window.Components.interfaces.nsIPrefBranch);
                    prefs.setCharPref('browser.startup.homepage', url);
                }
            }
            return true;
        },
        setAddFavorite: function (sUrl, sTitle) //添加收藏夹 eg:addFavorite(document.location.href,document.title)
        {
            if (document.all) {
                window.external.addFavorite(sUrl, sTitle); //IE 360 世界之窗
            } else if (window.sidebar) {
                window.sidebar.addPanel(sTitle, sUrl, ""); //FF
            } else {
                alert("你可以尝试通过快捷键Ctrl+D加入收藏夹~"); //谷歌 搜狗 猎豹
            }
        },
        setPlaceholder: function (element) //文本框placeholder 
        {
            //检测是否需要模拟placeholder  
            var placeholder = '';
            if (element && !("placeholder" in document.createElement("input")) && (placeholder = element.getAttribute("placeholder"))) {
                var idLabel = element.id; //当前文本控件是否有id, 没有则创建
                if (!idLabel) {
                    idLabel = "placeholder_" + new Date().getTime();
                    element.id = idLabel;
                }
                //创建label元素  
                var eleLabel = document.createElement("label");
                eleLabel.htmlFor = idLabel;
                eleLabel.style.position = "absolute";
                eleLabel.style.margin = "2px 0 0 3px";
                eleLabel.style.color = "graytext";
                eleLabel.style.cursor = "text";
                //插入创建的label元素节点  
                element.parentNode.insertBefore(eleLabel, element);
                element.onfocus = function () {
                    eleLabel.innerHTML = "";
                };
                element.onblur = function () {
                    if (this.value === "") {
                        eleLabel.innerHTML = placeholder;
                    }
                };
                if (element.value === "") {
                    eleLabel.innerHTML = placeholder;
                }
            }
        },
        getOutputDate: function (obj, type, isWeek) //输出日期-星期
        {
            var tempDate = new Date();
            var day = tempDate.getDate();
            var month = tempDate.getMonth() + 1;
            var year = tempDate.getFullYear();
            var tempHtml;
            switch (type) {
                case 1: //yyyy-MM-dd
                    tempHtml = year + "-" + month + "-" + day;
                    break;
                case 2: //dd-MM-yyyy
                    tempHtml = day + "-" + month + "-" + year;
                    break;
                case 3: //MM-dd-yyyy
                    tempHtml = month + "-" + day + "-" + year;
                    break;
                case 4: //yyyy/MM/dd
                    tempHtml = year + "/" + month + "/" + day;
                    break;
                case 5: //dd/MM/yyyy
                    tempHtml = day + "-" + month + "-" + year;
                    break;
                case 6: //MM/dd/yyyy
                    tempHtml = month + "-" + day + "-" + year;
                    break;
                default: //yyyy年MM月dd日
                    tempHtml = year + "年" + month + "月" + day + "日";
                    break;
            }
            if (isWeek) {
                var weeks = new Array("星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六");
                tempHtml += "  " + weeks[tempDate.getDay()];
            }
            obj.innerHTML = tempHtml;
        },
        setRefresh: function (url) //刷新页面
        {
            window.location.href = url;
        },
        setRefreshIframe: function (id) //刷新指定iframe
        {
            var iframe = this.getId(id);
            iframe.src = iframe.src;
        },
        setPreloadImages: function () //预加载图片
        {
            var d = document;
            if (d.images) {
                if (!d.p_i_a) d.p_i_a = new Array();
                var i, j = d.p_i_a.length, a = arguments;
                for (i = 0; i < a.length; i++) {
                    d.p_i_a[j] = new Image();
                    d.p_i_a[j++].src = a[i];
                }
            }
        },
        setImageAutoSize: function (obj, width) //处理图片自适应大小
        {
            var image = new Image();
            image.src = obj.src;
            var imagew = image.width;
            var imageh = image.height;
            if (imagew > width) {
                obj.height = (imageh * width) / imagew;
                obj.width = width;
            }
        },
        setReSizeImg: function (obj) //等比例缩放 img 
        {
            var w = 100;
            var h = 75;
            var tempImg = new Image();
            tempImg.src = obj.src;
            var tempH = tempImg.height;
            var tempW = tempImg.width;
            var tempIs = tempH / tempW;
            var tempR = w / h;
            //说明 要对高进行缩放 宽自适应
            if (tempH > h || tempW > w) {
                if (tempIs > tempR) {
                    obj.height = h;
                } else {
                    obj.width = w;
                }
            }
        },
        getFileSize: function (path) //得到文件大小KB
        {
            var fso = new ActiveXObject("Scripting.FileSystemObject");
            var f1 = fso.GetFile(path);
            return f1.Size / 1024.0;
        },

        /******checkbox复选框 Start******/
        getCheckboxCheckedValID: function (objId, separator) //获得id里面的CheckBoxList选中项的值
        {
            var tempVal = "";
            var checkboxlist = document.getElementById(objId);
            var chkinput = checkboxlist.getElementsByTagName("input");
            for (var i = 0; i < chkinput.length; i++) {
                if (chkinput[i].type == "checkbox") {
                    if (chkinput[i].checked) {
                        tempVal += chkinput[i].value + separator;
                    }
                }
            }
            if (tempVal !== "")
                tempVal = tempVal.substring(0, tempVal.length - separator.length);
            return tempVal;
        },
        getCheckboxCheckedVal: function (objName, separator) //获得CheckBoxList选中项的值
        {
            var checkboxlist = document.getElementsByName(objName);
            var array = new Array();
            var k = 0;
            for (var i = 0; i < checkboxlist.length; i++) {
                if (checkboxlist[i].type == "checkbox") {
                    if (checkboxlist[i].checked) {
                        array[k] = checkboxlist[i].value;
                        k++;
                    }
                }
            }
            return array.join(separator);
            /*方法2*/
            //    var tempVal = "";
            //    var checkboxlist = document.getElementsByName(objName);
            //    for (var i = 0; i < checkboxlist.length; i++) {
            //        if (checkboxlist[i].type === "checkbox") {
            //            if (checkboxlist[i].checked) {
            //                tempVal += checkboxlist[i].value + separator;
            //            }
            //        }
            //    }
            //    if (tempVal != "")
            //        tempVal = tempVal.substring(0, tempVal.length - separator.length);
            //    return tempVal;
        },
        setCheckboxChecked: function (objName, value, separator) //设置checkbox选中项
        {
            if (!value)
                return;
            var array = value.split(separator);
            var aggregate = document.getElementsByName(objName);
            for (var k = 0; k < array.length; k++) {
                for (var i = 0; i < aggregate.length; i++) {
                    if (aggregate[i].value == array[k]) {
                        aggregate[i].checked = true;
                        break;
                    }
                }
            }
        },
        setClickCheckAll: function (obj, objName) //checkbox全选与全不选
        {
            var checkboxs = document.getElementsByName(objName);
            if (checkboxs != null) {
                if (obj.checked) {
                    for (var i = 0; i < checkboxs.length; i++) {
                        if (checkboxs[i].type == "checkbox")
                            checkboxs[i].checked = true;
                    }
                } else {
                    for (var j = 0; j < checkboxs.length; j++) {
                        if (checkboxs[j].type === "checkbox")
                            checkboxs[j].checked = false;
                    }
                }
            }
        },
        setCheckboxSelectAll: function (objName) //全选
        {
            var objs = document.getElementsByName(objName);
            for (var i = 0; i < objs.length; i++) {
                if (objs[i].type == "checkbox") {
                    objs[i].checked = true;
                }
            }
        },
        setCheckboxUnselectAll: function (objName) //全不选 
        {
            var objs = document.getElementsByName(objName);
            for (var i = 0; i < objs.length; i++) {
                if (objs[i].type == "checkbox") {
                    if (objs[i].checked) {
                        objs[i].checked = false;
                    }
                }
            }
        },
        setCheckboxUnselect: function (objName) //反选 
        {
            var objs = document.getElementsByName(objName);
            for (var i = 0; i < objs.length; i++) {
                if (objs[i].type == "checkbox") {
                    if (objs[i].checked) {
                        objs[i].checked = false;
                    } else {
                        objs[i].checked = true;
                    }
                }
            }
        },
        /******checkbox End******/

        /******radio单选按钮 Start******/
        getRadioCheckedVal: function (objName) //获取radio选中项的值
        {
            var tempVal = "";
            var objs = document.getElementsByName(objName);
            for (var i = 0; i < objs.length; i++) {
                if (objs[i].checked) {
                    tempVal = objs[i].value;
                    break;
                }
            }
            return tempVal;
        },
        getRadioCheckedValID: function (objId, separator) //获取radio选中项的值
        {
            var tempVal = "";
            var rdolist = document.getElementById(objId);
            var objs = rdolist.getElementsByTagName("input");
            for (var i = 0; i < objs.length; i++) {
                if (objs[i].type == "radio") {
                    if (objs[i].checked)
                        tempVal += objs[i].value + separator;
                }
            }
            if (tempVal !== "")
                tempVal = tempVal.substring(0, tempVal.length - separator.length);
            return tempVal;
        },
        setRadioChecked: function (objName, value) //设置radio选中项
        {
            var aggregate = document.getElementsByName(objName);
            for (var i = 0; i < aggregate.length; i++) {
                if (aggregate[i].value == value) {
                    aggregate[i].checked = true;
                    break;
                }
            }
        },
        /******radio End******/

        /******select option下拉列表 Start******/
        setSelectOptionChecked: function (objId, value) //设置下拉列表选中的值
        {
            var obj = document.getElementById(objId);
            for (var i = 0; i < obj.options.length; i++) {
                if (obj.options[i].value == value) {
                    obj.options[i].selected = true;
                    break;
                }
            }
        },
        setSelectOptionAdd: function (objId, values, texts) //下拉列表添加选项option
        {
            var obj = document.getElementById(objId);
            for (var i = 0; i < values.length; i++) {
                obj.options.add(new Option(texts[i], values[i]));
            }
        },
        getSelectOptionVal: function (objId) //下拉列表选中值
        {
            var obj = document.getElementById(objId);
            return obj.options(obj.selectedIndex).value;
        },
        /******select option下拉列表 End******/

        /*Cookie Begin*/
        isEnableCookie: function () { //是否禁用cookie
            var result = false;
            if (navigator.cookiesEnabled) return true;
            document.cookie = "testcookie=yes;";
            var cookieSet = document.cookie;
            if (cookieSet.indexOf("testcookie=yes") > -1)
                result = true;
            document.cookie = "";
            return result;
        },
        setCookie: function (cookieName, value, days, domain) //设置cookie
        {
            if (!this.isEnableCookie()) {
                alert("您的浏览器的Cookie功能被禁用，请开启；推荐操作：浏览器工具-->INTERNET选项-->隐私-->Cookie安全级别设置为中!");
                return;
            }
            var expire = new Date();
            if (days == null || days == 0) days = 1;
            expire.setTime(expire.getTime() + days * 24 * 60 * 60 * 1000); //getTime():1970/01/01至今毫秒数
            var re = new RegExp("(?:;?" + cookieName + "\s*=)\w+[^;];*");
            var sCookie = document.cookie;
            if (re.test(sCookie)) {
                document.cookie = sCookie.replace(re, escape(value) + ";");
            } else {
                document.cookie = cookieName + "=" + escape(value) + ";path=/;" + (domain ? ("domain=" + domain) + ";" : '') + "expires=" + expire.toGMTString();
            }
        },
        getCookie: function (name) {
            if (!this.isEnableCookie()) {
                alert("您的浏览器的Cookie功能被禁用，请开启；推荐操作：浏览器工具-->INTERNET选项-->隐私-->Cookie安全级别设置为中!");
                return null;
            }
            var r = new RegExp("(^|;|\\s+)" + name + "=([^;]*)(;|$)");
            var m = document.cookie.match(r);
            return (!m ? "" : unescape(m[2]));
        },
        getChildCookie: function (cookieName, cookieChildName) { //获取cookie子键的值
            var cookieVal = this.getCookie(cookieName);
            if (cookieVal == null)
                return null;
            var childCookies = cookieVal.split('&');
            var szChildValue = "";
            for (var i = 0; i < childCookies.length; i++) {
                //substr 方法用于返回一个从指定位置开始的指定长度的子字符串。
                //语法：stringObject.substr(start [, length ])
                if (childCookies[i].replace(/(^\s+)|(\s+$)/g, "").substr(0, cookieChildName.length) === cookieChildName) {
                    szChildValue = childCookies[i].replace(/(^\s+)|(\s+$)/g, "").substr(cookieChildName.length + 1);
                    break;
                }
            }
            return szChildValue;
        },
        setDeleteCookie: function (name, domain, path) {
            if (!this.isEnableCookie()) {
                alert("您的浏览器的Cookie功能被禁用，请开启；推荐操作：浏览器工具-->INTERNET选项-->隐私-->Cookie安全级别设置为中!");
                return;
            }
            document.cookie = name + "=;" + (domain ? ("domain=" + domain + ";") : '') + "expires=" + (new Date(0)).toGMTString();
        },
        /*Cookie End*/

        /*正则表达式 Begin*/
        isNULL: function (obj) //判断当前对象是否为空
        {
            return (obj == null || typeof obj == "undefined" || obj.length === 0);
        },
        trim: function (s) //去处首尾空格
        {
            if (this.isEmpty(s))
                return "";
            return s.replace(/(^\s*)|(\s*$)/g, "");
        },
        isEmpty: function (v) //非空效验
        {
            switch (typeof v) {
                case 'undefined'://
                    return true;
                case 'string':
                    if (v.replace(/(^[ \t\n\r]*)|([ \t\n\r]*$)/g, '').length === 0) return true;
                    break;
                case 'boolean':
                    if (!v) return true;
                    break;
                case 'number':
                    if (0 === v || isNaN(v)) return true;
                    break;
                case 'object':
                    if (null === v || v.length === 0) return true;
                    for (var i in v) {
                        return false;
                    }
                    return true;
                case 'null':
                    return true;
                default:
                    return true;
            }
            return false;
        },
        trimLeft: function (s) //去左空格
        {
            return s.replace(/^\s*/, "");
        },
        trimRight: function (s) //去右空格
        {
            return s.replace(/\s*$/, "");
        },
        isExitsSpace: function (s) //是否存在空格
        {
            if (s.indexOf(" ") > -1)
                return true;
            else
                return false;
        },
        isGuid: function (s) //GUID
        {
            if (this.isEmpty(s))
                return false;
            s = this.trim(s);
            var p = /^[A-Fa-f0-9]{8}(-[A-Fa-f0-9]{4}){3}-[A-Fa-f0-9]{12}$/;
            return p.test(s);
        },
        isNumber: function (s) //数字
        {
            return !isNaN(s);
        },
        isNumber2: function (s, len) //数字（两位小数） 0.xx或(123456789)xxxx.xx
        {
            if (this.isEmpty(s))
                return false;
            s = this.trim(s);
            var l = len - 1;
            var regex = new RegExp("^((0(.\d{1,2})?)|([1-9][0-9]{0," + l + "}(.\\d{1,2})?))$");
            return regex.test(s);
        },
        isColor: function (s) //颜色值
        {
            if (this.isEmpty(s))
                return false;
            s = this.trim(s);
            if (s.length !== 7) return false;
            return s.search(/\#[a-fA-F0-9]{6}/) !== -1;
        },
        isDigit: function (s) //判断是否为整数
        {
            if (this.isEmpty(s))
                return false;
            s = this.trim(s);
            var p = /^[0-9]+$/;
            return p.test(s);
        },
        isPositiveInt: function (s) //判断是正整数,并且不能以0开头
        {
            if (this.isEmpty(s))
                return false;
            s = this.trim(s);
            var p = /^[1-9]+[0-9]*$/;
            return p.test(s);
        },
        isMobile: function (s) //验证11位手机号码
        {
            if (this.isEmpty(s))
                return false;
            s = this.trim(s);
            var p = /^1[3|4|5|7|8][0-9]\d{8}$/; // 11 位手机
            return p.test(s);
        },
        isPhone: function (s) //Phone
        {
            if (this.isEmpty(s))
                return false;
            s = this.trim(s);
            var p = /^((\(\d{3}\))|(\d{3}\-))?(\(0\d{2,3}\)|0\d{2,3}-)?[1-9]\d{6,7}$/;
            return p.test(s);
        },
        isEmail: function (s) //验证邮箱格式
        {
            if (this.isEmpty(s))
                return false;
            s = this.trim(s);
            //var p = /^[_\.0-9a-z-]+@([0-9a-z][0-9a-z-]+\.){1,4}[a-z]{2,3}$/i; 
            //var p = /^([a-zA-Z0-9_-])+@([a-zA-Z0-9_-])+(\.[a-zA-Z0-9_-])+/;
            var p = /^\w+((-\w+)|(\.\w+))*\@[A-Za-z0-9]+((\.|-)[A-Za-z0-9]+)*\.[A-Za-z0-9]+$/;
            return p.test(s);
        },
        isCard: function (s) //身份证
        {
            if (this.isEmpty(s))
                return false;
            s = this.trim(s);
            var p = /^\d{15}(\d{2}[xX0-9])?$/;
            return p.test(s);
        },
        isUrl: function (s) //URL
        {
            if (this.isEmpty(s))
                return false;
            s = this.trim(s);
            s = s.toLowerCase();
            var p = /^http:\/\/[A-Za-z0-9]+\.[A-Za-z0-9]+[\/=\?%\-&_~`@[\]\':+!]*([^<>\"\"])*$/;
            return p.test(s);
        },
        isZip: function (s) //Zip
        {
            if (this.isEmpty(s))
                return false;
            s = this.trim(s);
            var p = /^[1-9]\d{5}$/;
            return p.test(s);
        },
        isEnglish: function (s) //English
        {
            if (this.isEmpty(s))
                return false;
            s = this.trim(s);
            var p = /^[A-Za-z]+$/;
            return p.test(s);
        },
        isSimplifiedChinese: function (s) //简体中文
        {
            if (this.isEmpty(s))
                return false;
            s = this.trim(s);
            var p = /^[\u0391-\uFFE5]+$/;
            return p.test(s);
        },
        isChinese: function (s) //验证是否为中文（简体与繁体）
        {
            if (this.isEmpty(s))
                return false;
            s = this.trim(s);
            var p = /[\u4E00-\u9FA5]|[\uFE30-\uFFA0]/gi;
            if (!p.exec(s))
                return false;
            else
                return true;
        },
        stringValidate: function (s) {
            var reg = /['|\\|"]/g;
            var len = 0;
            try {
                len = s.match(reg).length;
            } catch (e) {
            }
            if (len > 0)
                return false;
            else
                return true;
        },
        isDoubleChar: function (s) //双字节 
        {
            var p = /^[^\x00-\xff]+$/;
            return p.test(s);
        },
        isHasChineseChar: function (s) //含有中文字符 
        {
            var p = /[^\x00-\xff]/;
            return p.test(s);
        },
        isLimitLen: function (s, minLength, maxLength) //字符串界线范围
        {
            if (this.isEmpty(s))
                return false;
            s = this.trim(s);
            if (s.toString().length > maxLength || s.toString().length < minLength)
                return false;
            else
                return true;
        },
        isLimitLenMax: function (s, maxLength) //字符串最大界线范围
        {
            if (this.isEmpty(s))
                return false;
            s = this.trim(s);
            if (s.toString().length > maxLength)
                return false;
            else
                return true;
        },
        isLimitLenMin: function (s, minLength) //字符串最小界线范围
        {
            if (this.isEmpty(s))
                return false;
            s = this.trim(s);
            if (s.toString().length < minLength)
                return false;
            else
                return true;
        },
        isIpAddress: function (s) //检查IP是否合法
        {
            if (this.isEmpty(s))
                return false;
            s = this.trim(s);
            var p = /^([1-9]\d{0,1}|1\d\d|2[0-2]\[0-3]|22[0-3])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$/;
            if (!p.exec(s)) {
                return false;
            } else
                return true;
        },
        isSmtp: function (s) //检查SMTP服务器地址//不要加smtp了  如：gmail.com
        {
            if (this.isEmpty(s))
                return false;
            s = this.trim(s);
            var p = /^((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-zA-Z]{2,6}(?:\.[a-zA-Z]{2})?)$/;
            if (!p.exec(s)) {
                return false;
            } else
                return true;
        },
        isWebAddress: function (s) //检查服务器WEB是否合法
        {
            if (this.isEmpty(s))
                return false;
            s = this.trim(s);
            var p = /^(http:\/\/([1-9]\d{0,1}|1\d\d|2[0-2]\[0-3]|22[0-3])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5]):\d{1,5})$/;
            if (!p.exec(s)) {
                return false;
            } else
                return true;
        },
        isPortValidate: function (s) //端口号效验
        {
            if (this.isEmpty(s))
                return false;
            s = this.trim(s);
            if (!this.isDigit(s)) {
                return false;
            } else if (parseInt(s, 10) <= 0 || parseInt(s, 10) > 65535) {
                return false;
            } else
                return true;
        },
        isDate: function (s) //日期
        {
            if (this.isEmpty(s))
                return false;
            s = this.trim(s);
            var result = s.match(/^(\d{4})(-|\/)(\d{1,2})\2(\d{1,2})$/);
            if (result == null) return false;
            var d = new Date(result[1], result[3] - 1, result[4]);
            return (d.getFullYear() === result[1] && d.getMonth() + 1 === result[3] && d.getDate() === result[4]);
        },
        isDatetime: function (s) //日期时间
        {
            if (this.isEmpty(s))
                return false;
            s = this.trim(s);
            var result = s.match(/^(\d{4})(-|\/)(\d{1,2})\2(\d{1,2}) (\d{1,2}):(\d{1,2}):(\d{1,2})$/);
            if (result == null) return false;
            var d = new Date(result[1], result[3] - 1, result[4], result[5], result[6], result[7]);
            return (d.getFullYear() === result[1] && (d.getMonth() + 1) === result[3] && d.getDate() === result[4] && d.getHours() === result[5] && d.getMinutes() === result[6] && d.getSeconds() === result[7]);
        },
        isGtTime: function (time1, time2) //时间比较大小 参数：格式必需是yyyy/MM/dd HH:mm:ss
        {
            if (this.isEmpty(time1) || this.isEmpty(time2))
                return false;
            return Date.parse(new Date(time1)) > Date.parse(new Date(time2));
        },
        /*正则表达式 End*/

        /*格式JSON数据 Begin*/
        formatString: function (s) //JSON数据格式化
        {
            if (this.isEmpty(s))
                return "";
            s = this.trim(s);
            return s;
        },
        formatStringByNoNull: function (s) //JSON数据格式化
        {
            if (this.isEmpty(s))
                return "";
            return s;
        },
        formatDate: function (s, type) //JSON日期类型数据格式化
        {
            if (this.isEmpty(s))
                return "";
            s = this.trim(s);
            /////s = s + "+08:00";//UTC字符串末尾加入北京标准时间
            //s = s.replace(/-/g, "/");
            //var d = new Date(s);//ISO8601格式
            var d = eval('new ' + s.replace('/', '', 'g').replace('/', '', 'g')); //微软
            var tempYear = d.getFullYear();
            var tempMonth = (d.getMonth() + 1) < 10 ? "0" + (d.getMonth() + 1) : (d.getMonth() + 1);
            var tempDay = d.getDate() < 10 ? "0" + d.getDate() : d.getDate();
            var tempHours = d.getHours() < 10 ? "0" + d.getHours() : d.getHours();
            var tempMinutes = d.getMinutes() < 10 ? "0" + d.getMinutes() : d.getMinutes();
            var tempSeconds = d.getSeconds() < 10 ? "0" + d.getSeconds() : d.getSeconds();
            switch (type) {
                case 1: //yyyy-MM-dd
                    return tempYear + "-" + tempMonth + "-" + tempDay;
                case 2: //yyyy/MM/dd HH:mm:ss
                    return tempYear + "/" + tempMonth + "/" + tempDay + " " + tempHours + ":" + tempMinutes + ":" + tempSeconds;
                case 3: //yyyy-MM-dd HH:mm:ss
                    return tempYear + "-" + tempMonth + "-" + tempDay + " " + tempHours + ":" + tempMinutes + ":" + tempSeconds;
                case 4: //yyyy-MM-dd HH:mm
                    return tempYear + "-" + tempMonth + "-" + tempDay + " " + tempHours + ":" + tempMinutes;
                case 5: //yyyy年MM月dd日
                    return tempYear + "年" + tempMonth + "月" + tempDay + "日";
                case 6: //yyyy/MM/dd HH:mm:ss
                    return tempYear + "." + tempMonth + "." + tempDay + " " + tempHours + ":" + tempMinutes + ":" + tempSeconds;
                case 7: //yyyy/MM/dd HH:mm
                    return tempYear + "." + tempMonth + "." + tempDay + " " + tempHours + ":" + tempMinutes;
                case 8: //MM/dd
                    return tempMonth + "/" + tempDay;
                case 9: //HH:mm
                    return tempHours + ":" + tempMinutes;
                case 10: //MM-dd
                    return tempMonth + "-" + tempDay;
                case 11: //MM-dd HH:mm
                    return tempMonth + "-" + tempDay + " " + tempHours + ":" + tempMinutes;
                case 12: //yyyy-MM
                    return tempYear + "-" + tempMonth;
                default:
                    return "";
            }
        },
        formatNowDate: function (s, type) //JSON日期类型数据格式化
        {
            var d = s;
            var tempYear = d.getFullYear();
            var tempMonth = (d.getMonth() + 1) < 10 ? "0" + (d.getMonth() + 1) : (d.getMonth() + 1);
            var tempDay = d.getDate() < 10 ? "0" + d.getDate() : d.getDate();
            var tempHours = d.getHours() < 10 ? "0" + d.getHours() : d.getHours();
            var tempMinutes = d.getMinutes() < 10 ? "0" + d.getMinutes() : d.getMinutes();
            var tempSeconds = d.getSeconds() < 10 ? "0" + d.getSeconds() : d.getSeconds();
            switch (type) {
                case 1: //yyyy-MM-dd
                    return tempYear + "-" + tempMonth + "-" + tempDay;
                case 2: //yyyy/MM/dd HH:mm:ss
                    return tempYear + "/" + tempMonth + "/" + tempDay + " " + tempHours + ":" + tempMinutes + ":" + tempSeconds;
                case 3: //yyyy-MM-dd HH:mm:ss
                    return tempYear + "-" + tempMonth + "-" + tempDay + " " + tempHours + ":" + tempMinutes + ":" + tempSeconds;
                case 4: //yyyy-MM-dd HH:mm
                    return tempYear + "-" + tempMonth + "-" + tempDay + " " + tempHours + ":" + tempMinutes;
                case 5: //yyyy年MM月dd日
                    return tempYear + "年" + tempMonth + "月" + tempDay + "日";
                case 6: //yyyy/MM/dd HH:mm:ss
                    return tempYear + "." + tempMonth + "." + tempDay + " " + tempHours + ":" + tempMinutes + ":" + tempSeconds;
                case 7: //yyyy/MM/dd HH:mm
                    return tempYear + "." + tempMonth + "." + tempDay + " " + tempHours + ":" + tempMinutes;
                case 8: //MM/dd
                    return tempMonth + "/" + tempDay;
                case 9: //HH:mm
                    return tempHours + ":" + tempMinutes;
                case 10: //MM-dd
                    return tempMonth + "-" + tempDay;
                case 11: //MM-dd HH:mm
                    return tempMonth + "-" + tempDay + " " + tempHours + ":" + tempMinutes;
                case 12: //yyyy-MM
                    return tempYear + "-" + tempMonth;
                default:
                    return "";
            }
        },
        formatDateDay: function (s) 
        {
            if (this.isEmpty(s))
                return "";
            s = this.trim(s);
            /////s = s + "+08:00";//UTC字符串末尾加入北京标准时间
            //s = s.replace(/-/g, "/");
            //var d = new Date(s);//ISO8601格式
            var d = eval('new ' + s.replace('/', '', 'g').replace('/', '', 'g')); //微软
            var tempYear = d.getFullYear();
            var tempMonth = (d.getMonth() + 1) < 10 ? "0" + (d.getMonth() + 1) : (d.getMonth() + 1);
            var tempDay = d.getDate() < 10 ?  d.getDate() : d.getDate();
            var tempHours = d.getHours() < 10 ? "0" + d.getHours() : d.getHours();
            var tempMinutes = d.getMinutes() < 10 ? "0" + d.getMinutes() : d.getMinutes();
            var tempSeconds = d.getSeconds() < 10 ? "0" + d.getSeconds() : d.getSeconds();
            return tempDay;
        },
        formatStatus: function (s) {
            switch (s) {
                case -1:
                    return "删除";
                case 0:
                    return "临时";
                case 1:
                    return "正常";
                case 2:
                    return "禁用";
                default:
                    return "其它";
            }
        },
        formatIsValid: function (s) {
            switch (s) {
                case 1:
                    return "有效";
                default:
                    return "无效";
            }
        },
        formatIsStage: function (s) {
            switch (s) {
                case 11:
                    return "入住申请";
                case 12:
                    return "入住调访";
                case 13:
                    return "入住审批";
                default:
                    return "入院";
            }
        },
        formatTreatmentResult: function (s) {
            switch (s) {
            case 1:
                    return "治愈";
            case 2:
                    return "好转";
            case 3:
                    return "未愈";
            case 4:
                    return "死亡";
            default:
                return "其它";
            }
        },
        /*格式JSON数据 End*/

        /***日期时间 Begin***/
        setRunTime: function (obj, isZhsxw) //eg:var objRunTime1=new setRunTime(obj,false);objRunTime1.StartClock();
        {
            var tempObj = {};
            tempObj._timerID = null;
            tempObj._timerRunning = false;
            tempObj.StopClock = function () {
                if (tempObj._timerRunning)
                    clearTimeout(tempObj._timerID);
                tempObj.timerRunning = false;
            };
            tempObj.StartClock = function () {
                tempObj.StopClock();
                tempObj.ShowTime();
            };
            tempObj.ShowTime = function () {
                var now = new Date();
                var hours = now.getHours();
                var minutes = now.getMinutes();
                var seconds = now.getSeconds();
                var tempTimeValue = "";
                if (isZhsxw) {
                    if (hours > 12)
                        tempTimeValue = "下午 ";
                    else if (hours === 12)
                        tempTimeValue = "中午 ";
                    else
                        tempTimeValue = "上午 ";
                }
                tempTimeValue += hours;
                tempTimeValue += ((minutes < 10) ? ":0" : ":") + minutes;
                tempTimeValue += ((seconds < 10) ? ":0" : ":") + seconds;
                obj.innerHTML = tempTimeValue;
                tempObj._timerID = setTimeout(tempObj._ShowTime(), 1000);
                tempObj._timerRunning = true;
            };
            tempObj._ShowTime = function () {
                return function () {
                    return tempObj.ShowTime();
                }
            }
            return tempObj;
        },
        /***日期时间 End***/


        /**
            * 功能:通用分页
            * 引用laypage.js库文件
            * @param {} sPageCount :总页数
            * @param {} sCurrentPage :初始化当前页
            * @param {} sPageSize :分页条数
            * @param {} divdom :容器->只支持id名、原生dom对象，jquery对象。
            * @param {} jumpMethod :回调分页方法
            * @returns {} 
            */
        setCommonPagination: function (sPageCount, sCurrentPage, sPageSize, divdom, jumpMethod)
        {
            laypage({
                cont: divdom, //容器->只支持id名、原生dom对象，jquery对象。
                pages: sPageCount, //通过后台拿到的总页数
                curr: sCurrentPage, //初始化当前页
                skip: true, //是否开启跳页
                skin: 'molv', //样式
                groups: 5, //连续显示分页数
                jump: function (e, first) { //触发分页后的回调                
                    if (!first) { //一定要加此判断，否则初始时会无限刷新                    
                        jumpMethod(e.curr, sPageSize);
                    }
                }
            });
        },
        log: function (msg, type) {
            //window.console &&(console[type || (type = "log")]) &&console[type](msg);
        },
        /*layer弹出应用 layer2.3 Begin*/
        iDebugAjaxError: function (xmlHttpRequest, textStatus, errorThrown) {
            this.log("responseText:" + xmlHttpRequest.responseText+ ",status:" + xmlHttpRequest.status+ ",readyState:" + xmlHttpRequest.readyState+ ",textStatus:" + textStatus+ ",errorThrown:" + errorThrown, "error");
        },
        iSystemAjaxError: function (msg) {
            if (arguments.length === 1) {
                layer.msg(msg, { icon: 2, shade: [0.3, '#000'] });
            } else {
                layer.msg("系统性错误，请稍候再试.", { icon: 2, time: 2000, shade: [0.3, '#000'] });
            }
        },
        iConfirmMessage: function (msg, yes, cancel) //弹出confirm
        {
            if (arguments.length === 3) {
                layer.confirm(msg, { icon: 3, title: "提示" }, yes, cancel);
            } else {
                layer.confirm(msg, { icon: 3, title: "提示" }, yes);
            }
        },
        iErrorReturnNull: function () //返回null
        {
            layer.msg("返回值错误，请联系系统管理员或稍后再试.", { icon: 2, time: 2000, shade: [0.3, '#000'] });
        },
        iErrorMessage: function (msg, callbackMethod) //错误
        {
            if (arguments.length === 1) {
                layer.msg(msg, { icon: 2, time: 2000, shade: [0.3, '#000'] });
            } else if (arguments.length === 2) {
                layer.msg(msg, { icon: 2, time: 2000, shade: [0.3, '#000'] }, function () {
                    callbackMethod();
                });
            } else {
                layer.msg("操作失败.", { icon: 2, time: 2000, shade: [0.3, '#000'] });
            }
        },
        iSuccessMessage: function (msg, callbackMethod) //返回正确数据
        {
            //构建重构方法
            if (arguments.length === 1) {
                layer.msg(msg, { icon: 1, time: 2000, shade: [0.3, '#000'] });
            } else if (arguments.length === 2) {
                layer.msg(msg, { icon: 1, time: 2000, shade: [0.3, '#000'] }, function () {
                    callbackMethod();
                });
            } else {
                layer.msg("操作成功.", { icon: 1, time: 2000, shade: [0.3, '#000'] });
            }
        },
        iSuccessMessageAlert: function (msg, callbackMethod) //返回正确数据
        {
            //构建重构方法
            if (arguments.length === 1) {
                layer.alert(msg, { icon: 1 });
            } else if (arguments.length === 2) {
                layer.alert(msg, { icon: 1 }, function () {
                    callbackMethod();
                });
            } else {
                layer.alert("操作成功.", { icon: 1 });
            }
        },
        iNoFound: function (msg, iUrl) //请求地址不正确
        {
            var pwin = window.self;
            if (window.top !== window.self)
                pwin = window.top;
            pwin.layer.msg("您请求地址不正确.", { icon: 2, time: 2000, shade: [0.3, '#000'] }, function () {
                if (arguments.length === 2)
                    location.href = iUrl + "?Msg=" + msg + "&ErrorRank=" + 1;
                else
                    location.href = "../Error/Index?Msg=" + msg + "&ErrorRank=" + 1;
            });
        },
        iNoLogin: function (msg, iLoginUrl) //未登录
        {
            var pwin = window.self;
            if (window.top !== window.self)
                pwin = window.top;
            pwin.layer.msg("您还未登录，请先登录.", { icon: 2, time: 2000, shade: [0.3, '#000'] }, function (index) {
                if (arguments.length === 2)
                    MISSY.iShowLoginDialog(iLoginUrl);
                else
                    MISSY.iShowLoginDialog();
            });
        },
        iNoPageAuth: function (msg, iUrl) //无页面权限
        {
            var pwin = window.self;
            if (window.top !== window.self)
                pwin = window.top;
            pwin.layer.msg("您无页面权限.", { icon: 2, time: 2000, shade: [0.3, '#000'] }, function () {
                if (arguments.length === 2)
                    location.href = iUrl + "?Msg=" + msg + "&ErrorRank=" + 1;
                else
                    location.href = "../Error/Index?Msg=" + msg + "&ErrorRank=" + 1;
            });
        },
        iNoOperateAuth: function (msg) //无操作权限
        {
            layer.msg(msg, { icon: 2, time: 2000, shade: [0.3, '#000'] });
        },
        iShowLoading: function (msg) {
            var index = layer.msg(msg, { icon: 16, time: 0, shade: [0.3, '#000'] });
            return index;
        },
        iHideLoading: function (index) {
            layer.close(index);
        },
        iShowLoginDialog: function (iLoginUrl) //显示登录弹出框
        {
            var pwin = window.self;
            if (window.top !== window.self)
                pwin = window.top;
            var index;
            if (arguments.length === 1) {
                index = pwin.layer.open({
                    type: 2,
                    title: false,
                    area: ['380px', '260px'],
                    closeBtn: 0,
                    shadeClose: true,
                    content: iLoginUrl
                });
            } else {
                index = pwin.layer.open({
                    type: 2,
                    title: false,
                    area: ['380px', '260px'],
                    closeBtn: 0,
                    shadeClose: true,
                    content: ['../Login/DIndex','no']
                });
            }
            return index;
        },
        iShowDialog: function (iDivId, iTitle, iWidth, iHeight) //显示弹出框-无遮罩层
        {
            var index;
            if (arguments.length === 4) {
                index = layer.open({
                    type: 1,
                    shadeClose: true,
                    area: [iWidth + 'px', iHeight + 'px'],
                    content: $("#" + iDivId),
                    title: iTitle
                });
            } else if (arguments.length === 2) {
                index = layer.open({
                    type: 1,
                    shadeClose: true,
                    area: ['auto', 'auto'],
                    content: $("#" + iDivId),
                    title: iTitle
                });
            } else {
                index = layer.open({
                    type: 1,
                    shadeClose: true,
                    area: ['auto', 'auto'],
                    content: $("#" + iDivId),
                    title: false
                });
            }
            return index;
        },
        iHideDialog: function (layerIndex) //隐藏弹出框
        {
            if (arguments.length === 1)
                layer.close(layerIndex);
            else
                layer.closeAll();
        },
        iShowModalDialogFrame: function (iTitle, iUrl, iWidth, iHeight,isScoll) //弹出模式iframe
        {
            var pwin = window.self;
                if (window.top !== window.self)
                    pwin = window.top;
            var index;
            if (isScoll) {
                index = pwin.layer.open({
                    type: 2,
                    title: iTitle,
                    shadeClose: true,
                    area: [iWidth + 'px', iHeight + 'px'],
                    closeBtn: 1,
                    content: iUrl
                });
            } else {
                index = pwin.layer.open({
                    type: 2,
                    title: iTitle,
                    shadeClose: true,
                    area: [iWidth + 'px', iHeight + 'px'],
                    closeBtn: 1,
                    content: [iUrl,"no"]
                });
            }
            return index;
        },
        iCloseModalDialogFrame: function (name) //关闭模式iframe
        {
            var index = top.layer.getFrameIndex(name); //先得到当前iframe层的索引
            top.layer.close(index); //再执行关闭   
        },
        iShowDialogFrame: function (iTitle, iUrl, iWidth, iHeight, isScoll) //弹出非模式iframe
        {
            var index;
            if (isScoll) {
                index = layer.open({
                    type: 2,
                    title: iTitle,
                    shadeClose: true,
                    area: [iWidth + 'px', iHeight + 'px'],
                    closeBtn: 1,
                    content: iUrl
                });
            } else {
                index = layer.open({
                    type: 2,
                    title: iTitle,
                    shadeClose: true,
                    area: [iWidth + 'px', iHeight + 'px'],
                    closeBtn: 1,
                    content: [iUrl, "no"]
                });
            }
            return index;
        },
        iCloseDialogFrame: function () //关闭非模式iframe
        {
            var index = parent.layer.getFrameIndex(window.name); //先得到当前iframe层的索引
            parent.layer.close(index); //再执行关闭
        }
        /*layer弹出应用 End*/


        /*zTree*/
        , zTreeExpandNode: function (treeName, isZk) {
            $.fn.zTree.getZTreeObj(treeName).expandAll(isZk);
        }
        , zTreeCheckedAll: function (treeName, isChecked) {
            $.fn.zTree.getZTreeObj(treeName).checkAllNodes(isChecked);
        }
        , zTreeCheckedVal: function (objName, separator, isOnlyLeaf) {
            var zTreeCheckedNodes = $.fn.zTree.getZTreeObj(objName).getCheckedNodes(true);
            var array = [];
            for (var i = 0; i < zTreeCheckedNodes.length; i++) {
                if (isOnlyLeaf !== true || (isOnlyLeaf === true && zTreeCheckedNodes[i].Pid !== 0))
                    array.push(zTreeCheckedNodes[i].Id);
            }
            return array.join(separator);
        }
        , zTreeCheckedName: function (objName, separator, isOnlyLeaf) {
            var zTreeCheckedNodes = $.fn.zTree.getZTreeObj(objName).getCheckedNodes(true);
            var array = [];
            for (var i = 0; i < zTreeCheckedNodes.length; i++) {
                if (isOnlyLeaf !== true || (isOnlyLeaf === true && zTreeCheckedNodes[i].Pid !== 0))
                    array.push(zTreeCheckedNodes[i].Title);
            }
            return array.join(separator);
        }
        // 针对Guid
        , zTreeCheckedValEx: function (objName, separator, isOnlyLeaf, Gpid) {
            var zTreeCheckedNodes = $.fn.zTree.getZTreeObj(objName).getCheckedNodes(true);
            var array = [];
            for (var i = 0; i < zTreeCheckedNodes.length; i++) {
                if (isOnlyLeaf !== true || (isOnlyLeaf === true && zTreeCheckedNodes[i].Pid !== Gpid))
                    array.push(zTreeCheckedNodes[i].Id);
            }
            return array.join(separator);
        }
        //针对Guid
        , zTreeCheckedNameEx: function (objName, separator, isOnlyLeaf,Gpid) {
            var zTreeCheckedNodes = $.fn.zTree.getZTreeObj(objName).getCheckedNodes(true);
            var array = [];
            for (var i = 0; i < zTreeCheckedNodes.length; i++) {
                if (isOnlyLeaf !== true || (isOnlyLeaf === true && zTreeCheckedNodes[i].Pid !== Gpid))
                    array.push(zTreeCheckedNodes[i].Title);
            }
            return array.join(separator);
        }
        , zTreeCheckNodes: function (treeObj, IdArray, flag) {
            if (treeObj) {
                treeObj.checkAllNodes(false);
                if (IdArray)
                    for (var i = 0, max = IdArray.length; i < max; i++) {
                        var node = treeObj.getNodeByParam("Id", IdArray[i], null);
                        if (node) {
                            treeObj.checkNode(node, true, false);
                            if (flag === true)
                                this.zTreeCheckParentNodes(treeObj, node.Pid);
                        }
                    }
            }
        }
        , zTreeCheckParentNodes: function (treeObj, Id) {
            if (treeObj) {
                var node = treeObj.getNodeByParam("Id", Id, null);
                if (node) {
                    treeObj.checkNode(node, true, false);
                    if (node.level)
                        this.zTreeCheckParentNodes(treeObj, node.Pid);
                }
            }
        }
    };

    //for 页面模块加载、Node.js运用、页面普通应用
    "function" === typeof define ? define(function () {
        return MISSY;
    }) : "undefined" != typeof exports ? module.exports = MISSY : window.MISSY = MISSY;
}(window);
