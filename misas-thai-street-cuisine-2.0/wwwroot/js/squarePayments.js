// Square Payments JavaScript Interop
let payments;
let card;

window.squarePayments = {
    // Initialize Square Payments
    async init(applicationId, locationId) {
        try {
            payments = Square.payments(applicationId, locationId);
            return { success: true };
        } catch (error) {
            console.error('Error initializing Square Payments:', error);
            return { success: false, error: error.message };
        }
    },

    // Initialize the card payment method
    async initCard(containerId) {
        try {
            card = await payments.card();
            await card.attach(`#${containerId}`);
            return { success: true };
        } catch (error) {
            console.error('Error initializing card:', error);
            return { success: false, error: error.message };
        }
    },

    // Tokenize the card
    async tokenizeCard() {
        try {
            const result = await card.tokenize();
            if (result.status === 'OK') {
                return {
                    success: true,
                    token: result.token,
                    details: result.details
                };
            } else {
                return {
                    success: false,
                    errors: result.errors
                };
            }
        } catch (error) {
            console.error('Error tokenizing card:', error);
            return { success: false, error: error.message };
        }
    },

    // Destroy the card instance
    destroy() {
        if (card) {
            card.destroy();
            card = null;
        }
    }
};