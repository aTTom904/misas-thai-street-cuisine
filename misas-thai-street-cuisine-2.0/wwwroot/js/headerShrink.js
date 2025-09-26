window._misasHeaderShrinkDotNetRef = null;

window.misasHeaderScrollInit = function () {
    if (window._misasHeaderScrollInit) return;
    window._misasHeaderScrollInit = true;
    var navbar = document.querySelector('.navbar');
    var maxShrink = 90; // px, from 150px to 60px
    var lastShrunk = null;
    var ticking = false;

    function updateShrink() {
        var scrollY = window.scrollY || window.pageYOffset;
        var shrunk = scrollY > 0;
        if (shrunk !== lastShrunk) {
            var shrinkValue = shrunk ? maxShrink : 0;
            if (navbar) {
                navbar.style.setProperty('--header-shrink', shrinkValue + 'px');
            }
            if (window._misasHeaderShrinkDotNetRef) {
                window._misasHeaderShrinkDotNetRef.invokeMethodAsync('SetHeaderShrunk', shrunk);
            }
            lastShrunk = shrunk;
        }
        ticking = false;
    }

    function onScroll() {
        if (!ticking) {
            window.requestAnimationFrame(updateShrink);
            ticking = true;
        }
    }

    updateShrink();
    window.addEventListener('scroll', onScroll, { passive: true });
};

window.misasSetHeaderShrinkCallback = function (dotNetObjRef) {
    window._misasHeaderShrinkDotNetRef = dotNetObjRef;
};
