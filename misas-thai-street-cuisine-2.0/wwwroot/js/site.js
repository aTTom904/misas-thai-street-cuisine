window.preloadImage = function (url) {
    var img = new Image();
    img.src = url;
};

function scrollToMenuInfo() {
    const header = document.querySelector('.navbar');
    const menuInfo = document.getElementById('menu-info');
    if (menuInfo) {
        const headerHeight = header ? header.offsetHeight : 0;
        const y = menuInfo.getBoundingClientRect().top + window.pageYOffset - headerHeight - 16; // 16px extra space
        window.scrollTo({ top: y, behavior: 'smooth' });
    }
}
window.scrollToMenuInfo = scrollToMenuInfo;