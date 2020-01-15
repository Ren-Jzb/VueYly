require.config({
    baseUrl: "/assets_pc/js",//baseUrl + paths
    map: {
        '*': {
            'css': 'require/require-css/0.1.10/css'//配置require-css插件路径
        }
    },
    paths: {
        "$": "jquery/jquery/1.11.3/jquery.js",
        "jquery": "jquery/jquery/1.11.3/jquery",
        "$-migrate": "jquery/migrate/1.3.0/jquery-migrate",
        "jquery-migrate": "jquery/migrate/1.3.0/jquery-migrate",
        "jquery.md5": "jquery/md5/1.2.1/jquery.md5",
        "layer": "layer/layer/2.3.0/layer",
        "laypage": "layer/laypage/1.3.0/laypage",
        "MISSY": "MISSY/MISSY/2.1.0/MISSY",
        "jquery.form": "jquery/form/3.51.0/jquery.form",
        "jquery.scrollUp": "jquery/scrollup/2.4.1/jquery.scrollUp",
        "jquery.SuperSlide": "jquery/SuperSlide/2.1.1/jquery.SuperSlide",
        "jquery.autocomplete": "jquery/autocomplete/1.2.26/jquery.autocomplete.min",
        "jquery.ztree": "zTree/zTree/3.5.28/jquery.ztree.all.min",
        "i18n": "require/require-i18n/2.0.6/i18n",
        "jquery.lazyload": "jquery/lazyload/1.9.7/jquery.lazyload.min",
        "webuploader": "webuploader/0.1.5/webuploader",
        "echars": "echarts/3.3.2/echarts",
        "jquery.jqprint": "jquery/jqprint/0.3.0/jquery.jqprint",
        "jquery.layui":"layer/layui2.4.5/layui"
    },
    shim: {
        "jquery-migrate": { deps: ["jquery"] },
        "jquery.md5": { deps: ["jquery"] },
        "layer": { deps: ["jquery", "css!/assets_pc/js/layer/layer/2.3.0/skin/layer.css"] },
        "laypage": { deps: ["jquery", "css!/assets_pc/js/layer/laypage/1.3.0/skin/laypage.css"] },
        "MISSY": { deps: ["jquery", "layer", "laypage"]},
        "jquery.form": { deps: ["jquery"] },
        "jquery.scrollUp": { deps: ["jquery",  "css!/assets_pc/js/jquery/scrollup/2.4.1/skin/default.css"] },
        "jquery.SuperSlide": { deps: ["jquery"] },
        "jquery.autocomplete": { deps: ["jquery","css!/assets_pc/js/jquery/autocomplete/1.2.26/skin/default.css"]  },
        "jquery.ztree": { deps: ["jquery","css!/assets_pc/js/zTree/zTree/3.5.28/skin/zTreeStyle/zTreeStyle.css"] },
        "jquery.lazyload": { deps: ["jquery"] },
        "webuploader": { deps: ["css!/assets_pc/js/webuploader/0.1.5/skin/default/css/webuploader.css?v=1.1.3"] },
        "jquery.jqprint": { deps: ["jquery"] },
        "jquery.layui": { deps: ["jquery"] }//, "css!/assets_pc/js/layer/layui2.4.5/css/layui.css"
    },
    config: {
        i18n: { locale: "zh-cn" }
    },
    waitSeconds: 90
});