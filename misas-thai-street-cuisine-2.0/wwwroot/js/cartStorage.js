window.cartStorage = {
    saveCart: function (cart) {
        localStorage.setItem('shoppingCart', JSON.stringify(cart));
    },
    loadCart: function () {
        return JSON.parse(localStorage.getItem('shoppingCart') || '[]');
    }
};
