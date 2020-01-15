/*
FloatAd:1.0.0
功能：页面随机浮动广告
用法：$("#dd").FloatAd({ unit:1,speed:10,randPos:true });
参数解释：
unit:移动单位量，单位：px，默认为1。
speed:速度，单位：毫秒，默认为10。
randPos:是否随机初始化元素位置，默认为true。
样式：必须设定元素的position样式为absolute，并根据需要调整z-index。
*/
(function($){
    $.fn.FloatAd = function (options) {
        var defaults = { unit:1,speed:10,randPos:true }
        
        var options = $.extend(defaults,options);
        
        var element = $(this);
        if(options.randPos){
            //初始化元素位置
            var leftInit= $.fn.floatElement.getRandom($(window).width() - element.outerWidth());
            var topInit= $.fn.floatElement.getRandom($(window).height() - element.outerHeight());
            element.css({"top":topInit+"px","left":leftInit+"px"});
        }
        
        var left=true,top=true;
        
        var moveEvent=setInterval(move, options.speed);
        
        element.hover(function(){clearInterval(moveEvent);},function(){ moveEvent=setInterval(move, options.speed); });
        
        function move(){
            var winW= $(window).width();//获取屏幕可视宽度
            var winH= $(window).height();//获取屏幕可视高度
            var scorllW=$(document).scrollLeft();//获取滚动条滚动宽度
            var scorllH=$(document).scrollTop();//获取滚动条滚动高度
            var objW=element.outerWidth();//获取元素宽度
            var objH=element.outerHeight();//获取元素高度
            var offset = element.offset();//获取元素的偏移值对象
            //判断元素是下行还是上行
            if(offset.left + objW + options.unit > winW + scorllW && left)
            {
                left = false;
            }
            if(offset.left - options.unit < scorllW && !left)
            {
                left = true;
            }
            //判断元素是左行还是右行
            if(offset.top + objH + options.unit > winH + scorllH && top)
            {
                top = false;
            }
            if(offset.top -options.unit < scorllH && !top)
            {
                top = true;
            }
            //设置移动值
            var topSize = 0,leftSize = 0;
            if(top)
                topSize = offset.top + options.unit;
            else
                topSize = offset.top - options.unit;
            if(left)
                leftSize = offset.left + options.unit;
            else
                leftSize = offset.left - options.unit;
            //扣除滚动宽高后移动值
            if(topSize + objH > winH + scorllH)
                topSize -= topSize + objH - winH - scorllH - options.unit;
            if(topSize < scorllH)
                topSize = scorllH - options.unit;
            if(leftSize + objW > winW + scorllW)
                leftSize -= leftSize + objW - winW - scorllW - options.unit;
            if(leftSize < scorllW)
                leftSize = scorllW - options.unit;
            //移动元素
            element.css({"top":topSize+"px","left":leftSize+"px"});
            
        }
    }
    
    $.fn.floatElement.RandNum=function (n){
        if(arguments.length==0){
            n = 1;
        }
        var rnd="";
        for(var i = 0; i < n; i++)
           rnd +=  $.fn.floatElement.getRandom();
        return rnd;
    }
    
    $.fn.floatElement.getRandom=function (start,end){
        if(arguments.length == 2){
            return parseInt(Math.random() * (end-start) + start)
        }else if(arguments.length == 1){
            return parseInt(Math.random() * start);
        }else{
            return parseInt(Math.random() * 10);
        }
    }
})(jQuery);