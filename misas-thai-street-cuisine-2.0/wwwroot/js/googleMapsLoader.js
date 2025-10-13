// Simple Google Maps initialization - fallback approach
window.initializeGoogleMaps = function(apiKey) {
    console.log('Initializing Google Maps with static script loading...');
    
    // Remove any existing Google Maps scripts
    const existingScripts = document.querySelectorAll('script[src*="maps.googleapis.com"]');
    existingScripts.forEach(script => script.remove());
    
    return new Promise((resolve, reject) => {
        const script = document.createElement('script');
        script.src = `https://maps.googleapis.com/maps/api/js?key=${apiKey}&libraries=geometry&callback=googleMapsCallback`;
        
        // Global callback function
        window.googleMapsCallback = function() {
            console.log('Google Maps loaded via callback');
            delete window.googleMapsCallback; // Clean up
            resolve();
        };
        
        script.onerror = function(error) {
            console.error('Google Maps script failed to load:', error);
            delete window.googleMapsCallback; // Clean up
            reject(new Error('Google Maps failed to load'));
        };
        
        document.head.appendChild(script);
    });
};