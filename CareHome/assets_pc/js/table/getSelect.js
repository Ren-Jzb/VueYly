
//获取单位下拉框的值
function getSelectUnitValue(paramUnit) {
    switch (paramUnit) {
        case "斤":
            return "<td class=\"Unit\" editstate=\"false\">斤</td>"

        case "公斤":
            return "<td class=\"Unit\" editstate=\"false\">公斤</td>";

        case "瓶":
            return "<td class=\"Unit\" editstate=\"false\">瓶</td>";

        case "盒":
            return "<td class=\"Unit\" editstate=\"false\">盒</td>";

        case "块":
            return "<td class=\"Unit\" editstate=\"false\">块</td>";

        case "卷":
            return "<td class=\"Unit\" editstate=\"false\">卷</td>";

        case "只":
            return "<td class=\"Unit\" editstate=\"false\">只</td>";

        case "包":
            return "<td class=\"Unit\" editstate=\"false\">包</td>";

        case "刀":
            return "<td class=\"Unit\" editstate=\"false\">刀</td>";

        case "箱":
            return "<td class=\"Unit\" editstate=\"false\">箱</td>";

        case "袋":
            return "<td class=\"Unit\" editstate=\"false\">袋</td>";

        case "听":
            return "<td class=\"Unit\" editstate=\"false\">听</td>";

        case "根":
            return "<td class=\"Unit\" editstate=\"false\">根</td>";

        case "条":
            return "<td class=\"Unit\" editstate=\"false\">条</td>";

        default: "<td class=\"Unit\" editstate=\"false\">无</td>";
    }


    ////获取下拉框的值
    //if (Unit == "斤") {
    //    buf1.push("<td editstate=\"false\"><select class=\"Unit\"><option value=\"斤\" selected=\"selected\">斤</option ><option value=\"公斤\">公斤</option><option value=\"瓶\">瓶</option><option value=\"盒\">盒</option><option value=\"块\">块</option><option value=\"卷\">卷</option><option value=\"只\">只</option><option value=\"包\">包</option><option value=\"刀\">刀</option><option value=\"箱\">箱</option><option value=\"袋\">袋</option><option value=\"听\">听</option><option value=\"根\">根</option><option value=\"条\">条</option></select></td>");
    //}
    //else if (Unit == "公斤") {
    //    buf1.push("<td editstate=\"false\"><select class=\"Unit\"><option value=\"斤\">斤</option ><option value=\"公斤\" selected=\"selected\">公斤</option><option value=\"瓶\">瓶</option><option value=\"盒\">盒</option><option value=\"块\">块</option><option value=\"卷\">卷</option><option value=\"只\">只</option><option value=\"包\">包</option><option value=\"刀\">刀</option><option value=\"箱\">箱</option><option value=\"袋\">袋</option><option value=\"听\">听</option><option value=\"根\">根</option><option value=\"条\">条</option></select></td>");
    //}
    //else if (Unit == "瓶") {
    //    buf1.push("<td editstate=\"false\"><select class=\"Unit\"><option value=\"斤\">斤</option ><option value=\"公斤\">公斤</option><option value=\"瓶\" selected=\"selected\">瓶</option><option value=\"盒\">盒</option><option value=\"块\">块</option><option value=\"卷\">卷</option><option value=\"只\">只</option><option value=\"包\">包</option><option value=\"刀\">刀</option><option value=\"箱\">箱</option><option value=\"袋\">袋</option><option value=\"听\">听</option><option value=\"根\">根</option><option value=\"条\">条</option></select></td>");
    //}
    //else if (Unit == "盒") {
    //    buf1.push("<td editstate=\"false\"><select class=\"Unit\"><option value=\"斤\">斤</option ><option value=\"公斤\">公斤</option><option value=\"瓶\">瓶</option><option value=\"盒\" selected=\"selected\">盒</option><option value=\"块\">块</option><option value=\"卷\">卷</option><option value=\"只\">只</option><option value=\"包\">包</option><option value=\"刀\">刀</option><option value=\"箱\">箱</option><option value=\"袋\">袋</option><option value=\"听\">听</option><option value=\"根\">根</option><option value=\"条\">条</option></select></td>");
    //}
    //else if (Unit == "块") {
    //    buf1.push("<td editstate=\"false\"><select class=\"Unit\"><option value=\"斤\">斤</option ><option value=\"公斤\">公斤</option><option value=\"瓶\">瓶</option><option value=\"盒\">盒</option><option value=\"块\" selected=\"selected\">块</option><option value=\"卷\">卷</option><option value=\"只\">只</option><option value=\"包\">包</option><option value=\"刀\">刀</option><option value=\"箱\">箱</option><option value=\"袋\">袋</option><option value=\"听\">听</option><option value=\"根\">根</option><option value=\"条\">条</option></select></td>");
    //}
    //else if (Unit == "卷") {
    //    buf1.push("<td editstate=\"false\"><select class=\"Unit\"><option value=\"斤\">斤</option ><option value=\"公斤\">公斤</option><option value=\"瓶\">瓶</option><option value=\"盒\">盒</option><option value=\"块\">块</option><option value=\"卷\" selected=\"selected\">卷</option><option value=\"只\">只</option><option value=\"包\">包</option><option value=\"刀\">刀</option><option value=\"箱\">箱</option><option value=\"袋\">袋</option><option value=\"听\">听</option><option value=\"根\">根</option><option value=\"条\">条</option></select></td>");
    //}
    //else if (Unit == "只") {
    //    buf1.push("<td editstate=\"false\"><select class=\"Unit\"><option value=\"斤\">斤</option ><option value=\"公斤\">公斤</option><option value=\"瓶\">瓶</option><option value=\"盒\">盒</option><option value=\"块\">块</option><option value=\"卷\">卷</option><option value=\"只\" selected=\"selected\">只</option><option value=\"包\">包</option><option value=\"刀\">刀</option><option value=\"箱\">箱</option><option value=\"袋\">袋</option><option value=\"听\">听</option><option value=\"根\">根</option><option value=\"条\">条</option></select></td>");
    //}
    //else if (Unit == "包") {
    //    buf1.push("<td editstate=\"false\"><select class=\"Unit\"><option value=\"斤\">斤</option ><option value=\"公斤\">公斤</option><option value=\"瓶\">瓶</option><option value=\"盒\">盒</option><option value=\"块\">块</option><option value=\"卷\">卷</option><option value=\"只\">只</option><option value=\"包\" selected=\"selected\">包</option><option value=\"刀\">刀</option><option value=\"箱\">箱</option><option value=\"袋\">袋</option><option value=\"听\">听</option><option value=\"根\">根</option><option value=\"条\">条</option></select></td>");
    //}
    //else if (Unit == "刀") {
    //    buf1.push("<td editstate=\"false\"><select class=\"Unit\"><option value=\"斤\">斤</option ><option value=\"公斤\">公斤</option><option value=\"瓶\">瓶</option><option value=\"盒\">盒</option><option value=\"块\">块</option><option value=\"卷\">卷</option><option value=\"只\">只</option><option value=\"包\">包</option><option value=\"刀\" selected=\"selected\">刀</option><option value=\"箱\">箱</option><option value=\"袋\">袋</option><option value=\"听\">听</option><option value=\"根\">根</option><option value=\"条\">条</option></select></td>");
    //}
    //else if (Unit == "箱") {
    //    buf1.push("<td editstate=\"false\"><select class=\"Unit\"><option value=\"斤\">斤</option ><option value=\"公斤\">公斤</option><option value=\"瓶\">瓶</option><option value=\"盒\">盒</option><option value=\"块\">块</option><option value=\"卷\">卷</option><option value=\"只\">只</option><option value=\"包\">包</option><option value=\"刀\">刀</option><option value=\"箱\" selected=\"selected\">箱</option><option value=\"袋\">袋</option><option value=\"听\">听</option><option value=\"根\">根</option><option value=\"条\">条</option></select></td>");
    //}
    //else if (Unit == "袋") {
    //    buf1.push("<td editstate=\"false\"><select class=\"Unit\"><option value=\"斤\">斤</option ><option value=\"公斤\">公斤</option><option value=\"瓶\">瓶</option><option value=\"盒\">盒</option><option value=\"块\">块</option><option value=\"卷\">卷</option><option value=\"只\">只</option><option value=\"包\">包</option><option value=\"刀\">刀</option><option value=\"箱\">箱</option><option value=\"袋\" selected=\"selected\">袋</option><option value=\"听\">听</option><option value=\"根\">根</option><option value=\"条\">条</option></select></td>");
    //}
    //else if (Unit == "听") {
    //    buf1.push("<td editstate=\"false\"><select class=\"Unit\"><option value=\"斤\">斤</option ><option value=\"公斤\">公斤</option><option value=\"瓶\">瓶</option><option value=\"盒\">盒</option><option value=\"块\">块</option><option value=\"卷\">卷</option><option value=\"只\">只</option><option value=\"包\">包</option><option value=\"刀\">刀</option><option value=\"箱\">箱</option><option value=\"袋\">袋</option><option value=\"听\" selected=\"selected\">听</option><option value=\"根\">根</option><option value=\"条\">条</option></select></td>");
    //}
    //else if (Unit == "根") {
    //    buf1.push("<td editstate=\"false\"><select class=\"Unit\"><option value=\"斤\">斤</option ><option value=\"公斤\">公斤</option><option value=\"瓶\">瓶</option><option value=\"盒\">盒</option><option value=\"块\">块</option><option value=\"卷\">卷</option><option value=\"只\">只</option><option value=\"包\">包</option><option value=\"刀\">刀</option><option value=\"箱\">箱</option><option value=\"袋\">袋</option><option value=\"听\">听</option><option value=\"根\" selected=\"selected\">根</option><option value=\"条\">条</option></select></td>");
    //}
    //else if (Unit == "条") {
    //    buf1.push("<td editstate=\"false\"><select class=\"Unit\"><option value=\"斤\">斤</option ><option value=\"公斤\">公斤</option><option value=\"瓶\">瓶</option><option value=\"盒\">盒</option><option value=\"块\">块</option><option value=\"卷\">卷</option><option value=\"只\">只</option><option value=\"包\">包</option><option value=\"刀\">刀</option><option value=\"箱\">箱</option><option value=\"袋\">袋</option><option value=\"听\">听</option><option value=\"根\">根</option><option value=\"条\" selected=\"selected\">条</option></select></td>");
    //}


}