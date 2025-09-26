window.preloadImage = function (url) {
    var img = new Image();
    img.src = url;
};

window.scrollToMenuInfo = function() {
    var el = document.getElementById('menu-info');
    if (el) el.scrollIntoView({ behavior: 'smooth' });
};