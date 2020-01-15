$.extend($.validator.messages, {
    required: "必选字段",
    remote: "请修正该字段",
    email: "请输入正确格式的电子邮件",
    url: "请输入合法的网址",
    date: "请输入合法的日期",
    dateISO: "请输入合法的日期 (ISO).",
    number: "请输入合法的数字",
    digits: "只能输入整数",
    creditcard: "请输入合法的信用卡号",
    equalTo: "请再次输入相同的值",
    accept: "请输入拥有合法后缀名的字符串",
    maxlength: $.validator.format("请输入一个长度最多是 {0} 的字符串"),
    minlength: $.validator.format("请输入一个长度最少是 {0} 的字符串"),
    rangelength: $.validator.format("请输入一个长度介于 {0} 和 {1} 之间的字符串"),
    range: $.validator.format("请输入一个介于 {0} 和 {1} 之间的值"),
    max: $.validator.format("请输入一个最大为 {0} 的值"),
    min: $.validator.format("请输入一个最小为 {0} 的值")
});

jQuery.validator.addMethod("moreThanNumber", function(value, element, params)
{
    var v = parseFloat(value);
    var result = (v >= params[0]);
    if (result)
        $(params[1]).css("background-color", "");
    else
        $(params[1]).css("background-color", "red");
    return this.optional(element) || result
}, "必须大于{0}");

jQuery.validator.addMethod("specialChar", function(value, element)
{
    //var pattern = new RegExp("[`~!@%#$^&*()=|{}':;',　\\[\\]<>/? \\.；：%……+￥（）【】‘”“'。，、？]");    
    var result = value.search(/['"]/);
    return this.optional(element) || (result == -1)
}, "不能输入标点符号等特殊字符");

jQuery.validator.addMethod("isPhone", function(value, element)
{
    var tel = /^(\d{3,4}-?)?\d{7,9}$/g;
    return this.optional(element) || (tel.test(value));
}, "请正确填写您的电话号码!");

jQuery.validator.addMethod("isIDCard", function(value, element)
{
    var tel = /^[0-9XY]{18}$/g;
    return this.optional(element) || (tel.test(value));
}, "请输入正确的身份证号码!");

jQuery.validator.addMethod("isIDCard2", function(value, element)
{
    var aCity = { 11: "北京", 12: "天津", 13: "河北", 14: "山西", 15: "内蒙古", 21: "辽宁", 22: "吉林", 23: "黑龙江 ", 31: "上海", 32: "江苏", 33: "浙江", 34: "安徽", 35: "福建", 36: "江西", 37: "山东", 41: "河南", 42: "湖北 ", 43: "湖南", 44: "广东", 45: "广西", 46: "海南", 50: "重庆", 51: "四川", 52: "贵州", 53: "云南", 54: "西藏 ", 61: "陕西", 62: "甘肃", 63: "青海", 64: "宁夏", 65: "新疆", 71: "台湾", 81: "香港", 82: "澳门", 91: "国外 " }
    var code = /^[0-9]{17}[0-9X]$/g;
    if (code.test(value) == false)
        return false;
    if (aCity[parseInt(value.substr(0, 2))] == null)
        return false;
    var birthday = /^((19[0-9]{2})|(20[0-9]{2}))((0[1-9])|(1[012]))((0[1-9])|([12][0-9])|(3[01]))$/;
    if (birthday.test(value.substr(6, 8)) == false)
        return false;
    return true;
}, "请输入正确的身份证号码!");

jQuery.validator.addMethod("itemNameSelected", function(value, element)
{
    var selectValue = $("#ddl_name_ajax").val();
    return this.optional(element) || (selectValue == value);
}, "请选择培训项目!");

