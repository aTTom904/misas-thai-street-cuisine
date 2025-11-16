// Square Payments JavaScript Interop
let payments;
let card;
let applePay;
let currentPaymentRequest;

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

    // Initialize Apple Pay
    async initApplePay(containerId, amount, label = 'Total') {
        try {
            currentPaymentRequest = payments.paymentRequest({
                countryCode: 'US',
                currencyCode: 'USD',
                total: {
                    amount: '1.00',
                    label: label,
                },
            });

            applePay = await payments.applePay(currentPaymentRequest);
            await applePay.attach(`#${containerId}`);
            return { success: true };
        } catch (error) {
            console.error('Error initializing Apple Pay:', error);
            return { success: false, error: error.message };
        }
    },

    // Check if Apple Pay is available
    async isApplePayAvailable() {
        try {
            if (!payments) {
                return false;
            }
            // Use minimal payment request just for availability check
            const testPaymentRequest = payments.paymentRequest({
                countryCode: 'US',
                currencyCode: 'USD',
                total: {
                    amount: '1.00',
                    label: 'Test',
                },
            });
            const applePayInstance = await payments.applePay(testPaymentRequest);
            return true;
        } catch (error) {
            console.log('Apple Pay not available:', error);
            return false;
        }
    },

    // Tokenize Apple Pay
    async tokenizeApplePay() {
        try {
            const result = await applePay.tokenize();
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
            console.error('Error tokenizing Apple Pay:', error);
            return { success: false, error: error.message };
        }
    },

    // Destroy all instances
    destroy() {
        if (card) {
            card.destroy();
            card = null;
        }
        if (applePay) {
            applePay.destroy();
            applePay = null;
        }
        currentPaymentRequest = null;
    }
};