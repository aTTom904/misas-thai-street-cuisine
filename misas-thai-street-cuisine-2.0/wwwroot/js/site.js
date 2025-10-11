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

// Dynamic Google Maps script loading
window.loadGoogleMapsScript = function (apiKey) {
    console.log('loadGoogleMapsScript called with API key:', apiKey ? 'PROVIDED' : 'NOT PROVIDED');
    
    return new Promise((resolve, reject) => {
        try {
            // Check if Google Maps is already loaded
            if (window.google && window.google.maps) {
                console.log('Google Maps already loaded');
                resolve();
                return;
            }

            // Check if script is already being loaded
            const existingScript = document.querySelector('script[src*="maps.googleapis.com"]');
            if (existingScript) {
                console.log('Google Maps script already exists, waiting for load...');
                // Wait for it to load
                const checkLoaded = setInterval(() => {
                    if (window.google && window.google.maps) {
                        console.log('Existing Google Maps script loaded');
                        clearInterval(checkLoaded);
                        resolve();
                    }
                }, 100);
                
                // Timeout after 10 seconds
                setTimeout(() => {
                    clearInterval(checkLoaded);
                    reject(new Error('Timeout waiting for existing Google Maps script to load'));
                }, 10000);
                return;
            }

            // Create and load the script
            console.log('Creating new Google Maps script tag');
            const script = document.createElement('script');
            script.src = `https://maps.googleapis.com/maps/api/js?key=${apiKey}&libraries=geometry`;
            script.async = true;
            script.defer = true;
            
            script.onload = () => {
                console.log('Google Maps script loaded successfully');
                resolve();
            };
            
            script.onerror = (error) => {
                console.error('Failed to load Google Maps script:', error);
                reject(new Error('Failed to load Google Maps script'));
            };
            
            document.head.appendChild(script);
            console.log('Google Maps script tag added to head');
        } catch (error) {
            console.error('Error in loadGoogleMapsScript:', error);
            reject(error);
        }
    });
};

// Make sure the function is available
console.log('loadGoogleMapsScript function defined:', typeof window.loadGoogleMapsScript);