// Cart localStorage operations
window.cartStorage = {
    // Get item from localStorage
    getItem: function(key) {
        try {
            return localStorage.getItem(key);
        } catch (error) {
            console.error('Error getting item from localStorage:', error);
            return null;
        }
    },

    // Set item in localStorage
    setItem: function(key, value) {
        try {
            localStorage.setItem(key, value);
            return true;
        } catch (error) {
            console.error('Error setting item in localStorage:', error);
            return false;
        }
    },

    // Remove item from localStorage
    removeItem: function(key) {
        try {
            localStorage.removeItem(key);
            return true;
        } catch (error) {
            console.error('Error removing item from localStorage:', error);
            return false;
        }
    },

    // Legacy methods for backward compatibility
    saveCart: function (cart) {
        try {
            localStorage.setItem('misas-cart', JSON.stringify(cart));
        } catch (error) {
            console.error('Error saving cart:', error);
        }
    },
    
    loadCart: function () {
        try {
            return JSON.parse(localStorage.getItem('misas-cart') || '[]');
        } catch (error) {
            console.error('Error loading cart:', error);
            return [];
        }
    }
};
