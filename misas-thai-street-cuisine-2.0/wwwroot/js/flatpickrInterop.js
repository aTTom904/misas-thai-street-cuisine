// Flatpickr initialization for catering date picker
window.initializeFlatpickr = function(element) {
    if (!element) return;
    
    // Calculate minimum date (7 days from now)
    const minDate = new Date();
    minDate.setDate(minDate.getDate() + 7);
    
    flatpickr(element, {
        minDate: minDate,
        dateFormat: "m/d/Y",
        altInput: true,
        altFormat: "F j, Y",
        defaultDate: null,
        theme: "light",
        disableMobile: true,
        onChange: function(selectedDates, dateStr, instance) {
            // Trigger Blazor binding update
            element.value = dateStr;
            element.dispatchEvent(new Event('change', { bubbles: true }));
        }
    });
};

window.destroyFlatpickr = (element) => {
    if (element && element._flatpickr) {
        element._flatpickr.destroy();
    }
};
