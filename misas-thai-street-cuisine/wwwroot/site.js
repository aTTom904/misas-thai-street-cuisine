function disableNonThursdays(inputId) {
    const input = document.getElementById(inputId);
    input.addEventListener('input', function () {
        const date = new Date(this.value);
        if (date.getDay() !== 4) {
            this.value = '';
            alert('Only Thursdays are selectable.');
        }
    });

    input.addEventListener('focus', function () {
        const picker = this;
        setTimeout(function () {
            const days = picker.parentElement.querySelectorAll('.flatpickr-day');
            days.forEach(day => {
                const date = new Date(day.dateObj);
                if (date.getDay() !== 4) {
                    day.classList.add('disabled');
                }
            });
        }, 100);
    });
}
