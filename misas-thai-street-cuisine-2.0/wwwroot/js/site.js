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

// Initialize Bootstrap carousel
window.initializeCarousel = function (carouselId) {
    
    const carouselElement = document.getElementById(carouselId);
    
    if (carouselElement && window.bootstrap) {
        // Dispose any existing carousel instance
        const existingCarousel = bootstrap.Carousel.getInstance(carouselElement);
        if (existingCarousel) {
            existingCarousel.dispose();
        }
        
        // Create new carousel instance
        const carousel = new bootstrap.Carousel(carouselElement, {
            interval: 5000,
            wrap: true,
            pause: 'hover'
        });
        
        // Start the carousel cycling
        carousel.cycle();
    } else {
        console.error('Failed to initialize carousel - element or bootstrap not found');
    }
};