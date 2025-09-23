// Returns true if point (lat, lng) is within bufferMiles of the route between origin and destination
// Requires geometry library: &libraries=geometry in Google Maps script
window.isPointNearRoute = function (origin, destination, lat, lng, bufferMiles) {
    return new Promise(function (resolve, reject) {
        if (!window.google || !window.google.maps || !window.google.maps.geometry) {
            reject('Google Maps JS API or geometry library not loaded.');
            return;
        }
        var directionsService = new google.maps.DirectionsService();
        directionsService.route({
            origin: origin,
            destination: destination,
            travelMode: 'DRIVING'
        }, function (result, status) {
            if (status === 'OK' && result.routes[0]) {
                var path = google.maps.geometry.encoding.decodePath(result.routes[0].overview_polyline);
                var point = new google.maps.LatLng(lat, lng);
                var minDistMeters = Number.POSITIVE_INFINITY;
                for (var i = 0; i < path.length - 1; i++) {
                    var segStart = path[i];
                    var segEnd = path[i + 1];
                    var dist = window._distanceToSegment(point, segStart, segEnd);
                    if (dist < minDistMeters) minDistMeters = dist;
                }
                var bufferMeters = bufferMiles * 1609.34;
                resolve(minDistMeters <= bufferMeters);
            } else {
                resolve(false);
            }
        });
    });
};

// Helper: shortest distance from point to segment
window._distanceToSegment = function (p, v, w) {
    // p, v, w are google.maps.LatLng
    var l2 = window.google.maps.geometry.spherical.computeDistanceBetween(v, w) ** 2;
    if (l2 === 0) return window.google.maps.geometry.spherical.computeDistanceBetween(p, v);
    var t = ((p.lat() - v.lat()) * (w.lat() - v.lat()) + (p.lng() - v.lng()) * (w.lng() - v.lng())) / ((w.lat() - v.lat()) ** 2 + (w.lng() - v.lng()) ** 2);
    t = Math.max(0, Math.min(1, t));
    var proj = new window.google.maps.LatLng(v.lat() + t * (w.lat() - v.lat()), v.lng() + t * (w.lng() - v.lng()));
    return window.google.maps.geometry.spherical.computeDistanceBetween(p, proj);
};
// Update map center and marker after initial render
window.updateGoogleMap = function (mapId, lat, lng) {
    if (!window.google || !window.google.maps) {
        console.error('Google Maps JS API not loaded.');
        return;
    }
    var mapDiv = document.getElementById(mapId);
    if (!mapDiv || !mapDiv._blazorMapInstance) {
        // Try to find the map instance
        var maps = google.maps && google.maps.Map ? google.maps.Map.instances : undefined;
        // fallback: re-initialize
        window.initGoogleMap(mapId, lat, lng, 10);
        return;
    }
    var map = mapDiv._blazorMapInstance;
    map.setCenter({ lat: lat, lng: lng });
    if (mapDiv._blazorMarkerInstance) {
        mapDiv._blazorMarkerInstance.setPosition({ lat: lat, lng: lng });
    } else {
        mapDiv._blazorMarkerInstance = new google.maps.Marker({
            position: { lat: lat, lng: lng },
            map: map
        });
    }
};
window.initGoogleMap = function (mapId, lat, lng, zoom) {
    if (!window.google || !window.google.maps) {
        console.error('Google Maps JS API not loaded.');
        return;
    }
    var mapDiv = document.getElementById(mapId);
    var map = new google.maps.Map(mapDiv, {
        center: { lat: lat, lng: lng },
        zoom: zoom
    });
    var marker = new google.maps.Marker({
        position: { lat: lat, lng: lng },
        map: map
    });
    // Store map and marker instances for later updates
    mapDiv._blazorMapInstance = map;
    mapDiv._blazorMarkerInstance = marker;
};

window.geocodeAddress = function (address) {
    return new Promise(function (resolve, reject) {
        if (!window.google || !window.google.maps) {
            reject('Google Maps JS API not loaded.');
            return;
        }
        var geocoder = new google.maps.Geocoder();
        geocoder.geocode({ address: address }, function (results, status) {
            if (status === 'OK' && results[0]) {
                var location = results[0].geometry.location;
                resolve({ lat: location.lat(), lng: location.lng() });
            } else {
                resolve(null);
            }
        });
    });
};

// Returns driving distance in miles between two addresses
window.getDrivingDistance = function (origin, destination) {
    return new Promise(function (resolve, reject) {
        if (!window.google || !window.google.maps) {
            reject('Google Maps JS API not loaded.');
            return;
        }
        var service = new google.maps.DistanceMatrixService();
        service.getDistanceMatrix({
            origins: [origin],
            destinations: [destination],
            travelMode: 'DRIVING',
            unitSystem: google.maps.UnitSystem.IMPERIAL
        }, function (response, status) {
            if (status === 'OK' && response.rows[0] && response.rows[0].elements[0].status === 'OK') {
                var distanceText = response.rows[0].elements[0].distance.text; // e.g. "24.3 mi"
                var miles = parseFloat(distanceText.replace(/[^0-9.]/g, ''));
                resolve(miles);
            } else {
                resolve(null);
            }
        });
    });
};