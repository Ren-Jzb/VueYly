(function () {
  function setFontSize() {
    document.documentElement.style.fontSize = window.innerWidth / 16.66 + 'px';
    //根据浏览器窗口大小设置rem大小，设计图中内容宽度为1666px，1666宽度下，1rem=100px，其他宽度等比缩放
  }
  setFontSize();

  window.onresize = function () {
    setTimeout(setFontSize(),200)
  }

})();